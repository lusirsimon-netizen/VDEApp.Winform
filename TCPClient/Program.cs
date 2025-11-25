using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpClientDemo
{
    class Program
    {
        // 服务器配置（可根据实际情况修改）
        private const string ServerIp = "127.0.0.1"; // 服务器IP
        private const int ServerPort = 8888;         // 服务器端口
        private const int BufferSize = 1024;         // 接收缓冲区大小
        private static TcpClient _tcpClient;         // 全局TCP客户端（保持长连接）
        private static NetworkStream _networkStream; // 全局网络流
        private static bool _isRunning = true;       // 程序运行状态标识

        static void Main(string[] args)
        {
            Console.WriteLine("============= TCP客户端（长连接模式）===========");
            Console.WriteLine($"服务器地址：{ServerIp}:{ServerPort}");
            Console.WriteLine("输入要发送的字符串（输入 'exit' 退出程序）");
            Console.WriteLine("提示：连接建立后会持续接收服务器消息，无需重复连接");
            Console.WriteLine("----------------------------------------");

            try
            {
                // 1. 创建TCP客户端并建立长连接（只连接一次）
                _tcpClient = new TcpClient();
                Console.Write("正在连接服务器...");
                _tcpClient.Connect(ServerIp, ServerPort); // 同步连接
                _networkStream = _tcpClient.GetStream();

                // 修复：长连接模式使用 Timeout.Infinite 表示无限等待（不超时）
                _networkStream.ReadTimeout = Timeout.Infinite;
                _networkStream.WriteTimeout = 10000; // 发送超时10秒
                Console.WriteLine("连接成功！");

                // 2. 启动独立线程监听服务器消息（避免阻塞用户输入）
                var receiveThread = new Thread(ReceiveServerMessages);
                receiveThread.IsBackground = true; // 设为后台线程，主程序退出时自动结束
                receiveThread.Start();

                // 3. 主线程处理用户输入和发送数据
                while (_isRunning)
                {
                    Console.Write("\n请输入发送内容：");
                    string sendMsg = Console.ReadLine();

                    // 退出条件判断
                    if (string.Equals(sendMsg, "exit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("正在退出程序...");
                        _isRunning = false;
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(sendMsg))
                    {
                        Console.WriteLine("错误：发送内容不能为空！");
                        continue;
                    }

                    try
                    {
                        // 检查连接状态
                        if (!_tcpClient.Connected)
                        {
                            Console.WriteLine("连接已断开，尝试重新连接...");
                            Reconnect();
                            if (!_tcpClient.Connected)
                            {
                                Console.WriteLine("重新连接失败，无法发送消息");
                                continue;
                            }
                        }

                        // 发送数据（UTF-8编码，末尾添加换行符便于服务器解析）
                        byte[] sendBytes = Encoding.UTF8.GetBytes(sendMsg + Environment.NewLine);
                        _networkStream.Write(sendBytes, 0, sendBytes.Length);
                        Console.WriteLine($"已发送：{sendMsg}");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"\n发送失败：{ex.Message}");
                        Console.WriteLine("可能原因：服务器已断开连接");
                        _isRunning = false;
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"\n发送错误：{ex.Message}");
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"\n网络错误：{ex.Message}（错误码：{ex.ErrorCode}）");
                Console.WriteLine("可能原因：服务器未启动、IP/端口错误、网络不通");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n初始化错误：{ex.Message}");
            }
            finally
            {
                // 释放资源（关闭连接）
                CleanupResources();
                Console.WriteLine("----------------------------------------");
            }

            // 程序退出提示
            Console.WriteLine("\n程序已退出，按任意键关闭窗口...");
            Console.ReadKey();
        }

        /// <summary>
        /// 接收服务器消息的线程方法（持续监听）
        /// </summary>
        private static void ReceiveServerMessages()
        {
            var receiveBuffer = new byte[BufferSize];

            while (_isRunning && _tcpClient != null && _tcpClient.Connected)
            {
                try
                {
                    // 持续读取服务器数据（长连接阻塞读取，有数据才返回）
                    int bytesRead = _networkStream.Read(receiveBuffer, 0, receiveBuffer.Length);

                    if (bytesRead > 0)
                    {
                        // 解析接收的数据（UTF-8编码）
                        string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

                        // 按行分割消息（处理粘包/拆包，假设服务器每条消息以换行符结束）
                        ProcessReceivedMessages(receivedData);
                    }
                    else
                    {
                        // bytesRead=0 表示服务器关闭了连接
                        Console.WriteLine("\n【通知】服务器已断开连接");
                        _isRunning = false;
                        break;
                    }
                }
                catch (IOException ex)
                {
                    // 长连接模式下，服务器正常断开会抛出此异常
                    if (_isRunning)
                    {
                        Console.WriteLine($"\n接收错误：{ex.Message}");
                    }
                    break;
                }
                catch (ObjectDisposedException)
                {
                    // 流已被释放（程序退出时正常触发）
                    break;
                }
                catch (Exception ex)
                {
                    if (_isRunning)
                    {
                        Console.WriteLine($"\n接收异常：{ex.Message}");
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        /// <param name="recivedata">接收消息</param>
        private static void ProcessReceivedMessages(string recivedata)
        {
            string allData = recivedata;
            lock (Console.Out)
            {
                Console.WriteLine($"\n【服务器回复】：{allData}\r\n");
                Console.Write("请输入发送内容："); // 重新显示输入提示
            }

        }

        /// <summary>
        /// 重新连接服务器
        /// </summary>
        private static void Reconnect()
        {
            try
            {
                CleanupResources(); // 清理旧连接资源
                _tcpClient = new TcpClient();
                _tcpClient.Connect(ServerIp, ServerPort);
                _networkStream = _tcpClient.GetStream();
                _networkStream.ReadTimeout = Timeout.Infinite;
                _networkStream.WriteTimeout = 10000;
                Console.WriteLine("重新连接成功！");

                // 重启接收线程
                var receiveThread = new Thread(ReceiveServerMessages);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"重新连接失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 清理资源（关闭连接和流）
        /// </summary>
        private static void CleanupResources()
        {
            try
            {
                if (_networkStream != null)
                {
                    _networkStream.Dispose(); // 使用 Dispose 更规范
                    _networkStream = null;
                }
                if (_tcpClient != null)
                {
                    _tcpClient.Close();
                    _tcpClient = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"资源清理错误：{ex.Message}");
            }
        }
    }
}
