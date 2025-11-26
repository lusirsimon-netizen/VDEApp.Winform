using AntdUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VDEApp.Configs;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Models;
using VDEApp.Utils.Communication;

namespace VDEApp.Controllers
{
    public sealed class TcpCommunicationController
    {
        private InsTcpServer _server;
        private int _port {
            get => ServiceLocator.ProjectController.CurrentProject.Port;
            set => ServiceLocator.ProjectController.CurrentProject.Port = value;
        }
        private int[] _ip = new int[] { 127, 0, 0, 1 };
        private Localizer _ => ServiceLocator.GlobalConfig.GlobalLocalizer;

        // 客户端连接信息
        private readonly ConcurrentDictionary<string, TcpClient> _connectedClients = new ConcurrentDictionary<string, TcpClient>();

        #region 发送队列+防粘包相关字段
        // 每个客户端的独立发送队列
        private readonly ConcurrentDictionary<string, ConcurrentQueue<byte[]>> _clientSendQueues = new ConcurrentDictionary<string, ConcurrentQueue<byte[]>>();
        // 每个客户端的队列消费任务
        private readonly ConcurrentDictionary<string, Task> _queueConsumeTasks = new ConcurrentDictionary<string, Task>();
        // 取消令牌源
        private readonly ConcurrentDictionary<string, CancellationTokenSource> _consumeCts = new ConcurrentDictionary<string, CancellationTokenSource>();
        // 【关键】默认编码（可外部修改，匹配你的原始字符串编码）
        public Encoding DefaultMessageEncoding { get; set; } = Encoding.UTF8;
        // 长度前缀字节数（仅用于防粘包，不修改字符串内容）
        private const int LENGTH_PREFIX_BYTES = 4;
        #endregion
        public TcpCommunicationController()
        {
            Init();
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public int[] Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        public InsTcpServer Server
        {
            get { return _server; }
        }

        /// <summary>
        /// 监听状态
        /// </summary>
        public bool IsListening { get; set; } = false;

        /// <summary>
        /// Initialize the TCP server
        /// </summary>
        private void Init()
        {
            _server = new InsTcpServer();
            _ip = GetLocalIPAddressAsIntArray();
            Log.Info(string.Format(_.GetString("log_tcp_server_init_ip"), string.Join(".", _ip)));
        }

        /// <summary>
        /// starts the TCP server if it is not already running.
        /// </summary>
        public void StartServer()
        {
            if (_server != null)
            {
                _server.LogMessage += (s, msg) => Log.Info($"[TCP SERVER] {msg}");
                _server.ClientConnected += OnClientConnected;
                _server.ClientDisconnected += OnClientDisconnected;
                _server.DataReceived += ServiceLocator.TaskController.ReciveDataHandler;
                _server.Start(_port);
                IsListening = true;
                Log.Info($"[TCP SERVER] Started, listening on {string.Join(".", _ip)}:{_port}");
            }
            else
            {
                Log.Error(_.GetString("log_tcp_server_not_initialized"));
            }
        }

        /// <summary>
        /// Stops the TCP server if it is currently running.
        /// </summary>
        public void StopServer()
        {
            IsListening = false;
            _server?.Stop();
            foreach (KeyValuePair<string, CancellationTokenSource> kvp in _consumeCts)
            {
                string clientKey = kvp.Key;
                CancellationTokenSource cts = kvp.Value;
                try
                {
                    cts.Cancel();
                    cts.Dispose();
                }
                catch (Exception ex)
                {
                    Log.Error($"[TCP SERVER] Failed to cancel consume task for client {clientKey}: {ex.Message}");
                }
            }

            _consumeCts.Clear();
            _queueConsumeTasks.Clear();
            _clientSendQueues.Clear();
            _connectedClients.Clear();
            Log.Info("[TCP SERVER] Stopped, all resources cleaned up");
        }

        #region 定向发送消息（不改变原始字符串格式）
        /// <summary>
        /// 定向发送消息（保持原始字符串内容不变，仅添加防粘包长度前缀）
        /// </summary>
        /// <param name="clientKey">客户端标识</param>
        /// <param name="message">原始字符串（不做任何转义/修改）</param>
        /// <param name="encoding">编码方式（可选，默认用 DefaultMessageEncoding，匹配你的原始字符串编码）</param>
        /// <returns></returns>
        public async Task SendToClientAsync(string clientKey, string message, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentNullException(nameof(clientKey), "Client key cannot be null or empty");
            }
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null or empty");
            }

            // 【关键】使用指定编码（未指定则用默认，不强制转换编码）
            Encoding useEncoding = encoding ?? DefaultMessageEncoding;
            // 编码字符串为字节数组（仅转格式，不修改内容）+ 加防粘包长度前缀（不影响字符串本身）
            byte[] messageFrame = EncodeMessageToFrame(message, useEncoding);

            // GetOrAdd的lambda带1个参数（key），匹配Func委托
            var sendQueue = _clientSendQueues.GetOrAdd(clientKey, key => new ConcurrentQueue<byte[]>());
            sendQueue.Enqueue(messageFrame);

            // 确保队列消费任务正在运行（单例，避免重复创建）
            if (!_queueConsumeTasks.ContainsKey(clientKey) ||
                _queueConsumeTasks[clientKey].IsCompleted ||
                _queueConsumeTasks[clientKey].IsFaulted ||
                _queueConsumeTasks[clientKey].IsCanceled)
            {
                var cts = _consumeCts.GetOrAdd(clientKey, key => new CancellationTokenSource());
                var consumeTask = ConsumeSendQueueAsync(clientKey, cts.Token);
                _queueConsumeTasks[clientKey] = consumeTask;
                // 不等待消费完成（异步后台发送），避免阻塞调用方
                consumeTask.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Log.Error($"[TCP SEND QUEUE] Client {clientKey} - Consume task failed: {t.Exception?.InnerException?.Message}");
                    }
                    else if (t.IsCanceled)
                    {
                        Log.Info($"[TCP SEND QUEUE] Client {clientKey} - Consume task canceled");
                    }
                }, TaskContinuationOptions.ExecuteSynchronously);
            }

            await Task.FromResult(0); // .NET 4.7.2中Task.CompletedTask替代方案
        }
        #endregion

        #region 广播消息
        /// <summary>
        /// 广播消息（保持原始字符串内容不变，仅添加防粘包长度前缀）
        /// </summary>
        /// <param name="message">原始字符串（不做任何转义/修改）</param>
        /// <param name="encoding">编码方式（可选，默认用 DefaultMessageEncoding）</param>
        /// <returns></returns>
        public async Task BroadcastMessage(string message, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(nameof(message), "Broadcast message cannot be null or empty");
            }

            // 【关键】使用指定编码，不强制转换
            Encoding useEncoding = encoding ?? DefaultMessageEncoding;
            byte[] messageFrame = EncodeMessageToFrame(message, useEncoding);

            // 遍历所有在线客户端，加入各自队列
            foreach (KeyValuePair<string, TcpClient> kvp in _connectedClients)
            {
                string clientKey = kvp.Key;
                try
                {
                    var sendQueue = _clientSendQueues.GetOrAdd(clientKey, key => new ConcurrentQueue<byte[]>());
                    sendQueue.Enqueue(messageFrame);

                    // 确保消费任务运行
                    if (!_queueConsumeTasks.ContainsKey(clientKey) ||
                        _queueConsumeTasks[clientKey].IsCompleted ||
                        _queueConsumeTasks[clientKey].IsFaulted ||
                        _queueConsumeTasks[clientKey].IsCanceled)
                    {
                        var cts = _consumeCts.GetOrAdd(clientKey, key => new CancellationTokenSource());
                        var consumeTask = ConsumeSendQueueAsync(clientKey, cts.Token);
                        _queueConsumeTasks[clientKey] = consumeTask;
                        consumeTask.ContinueWith(t =>
                        {
                            if (t.IsFaulted)
                            {
                                Log.Error($"[TCP BROADCAST] Client {clientKey} consume task failed: {t.Exception?.InnerException?.Message}");
                            }
                        }, TaskContinuationOptions.ExecuteSynchronously);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"[TCP BROADCAST] Failed to enqueue message for client {clientKey}: {ex.Message}");
                }
            }

            await Task.FromResult(0); // .NET 4.7.2兼容：替代Task.CompletedTask
        }
        #endregion

        #region 队列消费逻辑
        private async Task ConsumeSendQueueAsync(string clientKey, CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    TcpClient tcpClient; // 显式声明类型
                    if (!_connectedClients.TryGetValue(clientKey, out tcpClient) || !tcpClient.Connected)
                    {
                        Log.Warn($"[TCP SEND QUEUE] Client {clientKey} - Disconnected, exit consume task");
                        break;
                    }

                    ConcurrentQueue<byte[]> sendQueue; 
                    if (!_clientSendQueues.TryGetValue(clientKey, out sendQueue) || sendQueue.IsEmpty)
                    {
                        // 队列空时短暂等待
                        await Task.Delay(10, cancellationToken);
                        continue;
                    }

                    byte[] messageFrame; // 显式声明类型
                    if (sendQueue.TryDequeue(out messageFrame))
                    {

                        // 发送消息
                        var networkStream = tcpClient.GetStream();
                        if (!networkStream.CanWrite)
                        {
                            Log.Error($"[TCP SEND QUEUE] Client {clientKey} - Cannot write to stream");
                            break;
                        }

                        // 发送完整帧
                        await networkStream.WriteAsync(messageFrame, 0, messageFrame.Length, cancellationToken);
                        await networkStream.FlushAsync(cancellationToken); // 强制刷新，减少粘包概率
                    }
                }
                catch (OperationCanceledException)
                {
                    Log.Info($"[TCP SEND QUEUE] Client {clientKey} - Consume task canceled");
                    break;
                }
                catch (SocketException ex)
                {
                    Log.Error($"[TCP SEND QUEUE] Client {clientKey} - Socket error: {ex.SocketErrorCode} - {ex.Message}");
                    break; // 网络错误，退出消费
                }
                catch (Exception ex)
                {
                    Log.Error($"[TCP SEND QUEUE] Client {clientKey} - Unexpected error: {ex.Message}", ex);
                    // 非致命错误，短暂重试
                    await Task.Delay(100, cancellationToken);
                }
            }
            // 移除消费任务和取消令牌
            _queueConsumeTasks.TryRemove(clientKey, out Task _);
            if (_consumeCts.TryRemove(clientKey, out var cts))
            {
                cts.Dispose();
            }

            // 清理队列（可选：保留未发送消息或清空）
            if (_clientSendQueues.TryRemove(clientKey, out var remainingQueue))
            {
                Log.Warn($"[TCP SEND QUEUE] Client {clientKey} - Consume task exited, {remainingQueue.Count} messages not sent");
            }
        }
        #endregion

        #region 消息编码为防粘包帧格式
        /// <summary>
        /// 编码逻辑：仅将字符串转为字节数组（指定编码）+ 拼接4字节长度前缀（防粘包用）
        /// 不做任何字符串转义、替换、修改，原始内容完全保留
        /// </summary>
        /// <param name="message">原始字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>帧格式：[4字节大端序长度] + [原始字符串的字节数组]</returns>
        private byte[] EncodeMessageToFrame(string message, Encoding encoding)
        {
            // 1. 将原始字符串转为字节数组（仅编码格式转换，内容不变）
            byte[] contentBytes = encoding.GetBytes(message);

            // 2. 生成4字节大端序长度前缀
            byte[] lengthPrefix = BitConverter.GetBytes(contentBytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(lengthPrefix); // 转为大端序
            }

            // 3. 拼接帧：长度前缀 + 原始字符串的字节数组（无任何额外修改）
            byte[] frame = new byte[LENGTH_PREFIX_BYTES + contentBytes.Length];
            Buffer.BlockCopy(lengthPrefix, 0, frame, 0, LENGTH_PREFIX_BYTES);
            Buffer.BlockCopy(contentBytes, 0, frame, LENGTH_PREFIX_BYTES, contentBytes.Length);

            return frame;
        }
        #endregion

        /// <summary>
        /// When Client Connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClientConnected(object sender, TcpClientConnectedEventArgs e)
        {
            // 客户端标识：用 RemoteEndPoint.ToString() 唯一标识（与 TaskController 中 clientKey 一致）
            string clientKey = e.Client.Client.RemoteEndPoint.ToString();

            // 记录已连接的客户端（用于后续校验连接状态、强制断开等）
            if (_connectedClients.TryAdd(clientKey, e.Client))
            {
                Log.Info($"[TCP SERVER] Client connected: {clientKey}");
                // 初始化发送队列（提前创建，避免首次发送时延迟）
                _clientSendQueues.TryAdd(clientKey, new ConcurrentQueue<byte[]>());
            }
            else
            {
                Log.Warn(string.Format(_.GetString("log_tcp_server_client_already_exists"), clientKey));
            }
        }

        /// <summary>
        /// When Client Disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClientDisconnected(object sender, TcpClientDisconnectedEventArgs e)
        {
            string clientKey = e.ClientEndPoint.ToString();

            // 1. 从在线客户端列表中移除
            if (_connectedClients.TryRemove(clientKey, out var _))
            {
                Log.Info($"[TCP SERVER] Client disconnected: {clientKey}");

                // 2. 触发 CommandParser 的解绑逻辑（仅一次，无重复）
                CommandParser.OnClientDisconnected(clientKey);

                // 3. 解绑任务相关事件
                ServiceLocator.TaskController.UnbindClientRelatedEvents(clientKey);

                #region 清理客户端发送队列和消费任务
                // 取消消费任务
                if (_consumeCts.TryGetValue(clientKey, out var cts))
                {
                    try
                    {
                        cts.Cancel();
                        cts.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"[TCP SERVER] Failed to cancel consume task for client {clientKey}: {ex.Message}");
                    }
                    _consumeCts.TryRemove(clientKey, out var _);
                }

                // 移除消费任务和队列
                _queueConsumeTasks.TryRemove(clientKey, out var _);
                if (_clientSendQueues.TryRemove(clientKey, out var remainingQueue))
                {
                    Log.Info($"[TCP SERVER] Client {clientKey} - Cleared send queue with {remainingQueue.Count} unsent messages");
                }
                #endregion
            }
            else
            {
                Log.Warn(string.Format(_.GetString("log_tcp_server_client_not_exists"), clientKey));
            }
        }

        /// <summary>
        /// reinitializes the TCP server by stopping it and then re-initializing it.
        /// </summary>
        public void Reinitialize()
        {
            StopServer();
            Init();
        }

        #region 智能获取IP地址
        public List<string> GetLocalIPAddresses()
        {
            var ipAddresses = new List<string>();

            try
            {
                // 方式1：通过主机名获取
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);

                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork) // IPv4地址
                    {
                        ipAddresses.Add(address.ToString());
                    }
                }

                // 如果没有找到IPv4地址，尝试其他方式
                if (!ipAddresses.Any())
                {
                    ipAddresses.AddRange(GetLocalIPAddressesAlternative());
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(_.GetString("log_tcp_server_get_ip_error"), ex.Message));
                // 备用方案
                ipAddresses.AddRange(GetLocalIPAddressesAlternative());
            }

            return ipAddresses.Distinct().ToList();
        }

        public string GetPreferredLocalIPAddress()
        {
            try
            {
                // 优先获取非回环地址且可用的网络接口
                var addresses = GetLocalIPAddresses()
                    .Where(ip => ip != "127.0.0.1" && !ip.StartsWith("169.254.")) // 排除回环和APIPA地址
                    .ToList();

                return addresses.FirstOrDefault() ?? "127.0.0.1";
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(_.GetString("log_tcp_server_get_preferred_ip_error"), ex.Message));
                return "127.0.0.1";
            }
        }

        private List<string> GetLocalIPAddressesAlternative()
        {
            var ipAddresses = new List<string>();

            try
            {
                // 获取所有网络接口
                NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface networkInterface in networkInterfaces)
                {
                    // 只获取已启动且不是回环接口的网络接口
                    if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                        networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        IPInterfaceProperties properties = networkInterface.GetIPProperties();

                        foreach (UnicastIPAddressInformation unicastAddress in properties.UnicastAddresses)
                        {
                            if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                ipAddresses.Add(unicastAddress.Address.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(_.GetString("log_tcp_server_get_ip_by_interface_error"), ex.Message));
            }

            // 如果还是没有找到，返回回环地址
            if (!ipAddresses.Any())
            {
                ipAddresses.Add("127.0.0.1");
            }

            return ipAddresses;
        }

        public int[] GetLocalIPAddressAsIntArray()
        {
            string ipString = GetPreferredLocalIPAddress();

            try
            {
                return ipString.Split('.')
                              .Select(part => int.Parse(part))
                              .ToArray();
            }
            catch
            {
                // 如果解析失败，返回默认的127.0.0.1
                return new int[] { 127, 0, 0, 1 };
            }
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 外部校验客户端发送队列长度
        /// </summary>
        /// <param name="clientKey"></param>
        /// <returns></returns>
        public int GetClientQueueLength(string clientKey)
        {
            if (_clientSendQueues.TryGetValue(clientKey, out var queue))
            {
                return queue.Count;
            }
            return -1;
        }

        public void ClearClientQueue(string clientKey)
        {
            if (_clientSendQueues.TryGetValue(clientKey, out var queue))
            {
                while (queue.TryDequeue(out var _))
                { }
                Log.Info($"[TCP SEND QUEUE] Client {clientKey} - Queue cleared");
            }
        }
        #endregion
    }
}
