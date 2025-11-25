using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VDEApp.Utils.Communication
{
    public class InsTcpServer : IDisposable
    {
        private TcpListener _listener;
        private readonly List<TcpClient> _connectedClients;
        private readonly object _clientsLock = new object();
        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;

        public event EventHandler<string> LogMessage;
        public event EventHandler<TcpClientConnectedEventArgs> ClientConnected;
        public event EventHandler<TcpClientDisconnectedEventArgs> ClientDisconnected;
        public event EventHandler<TcpDataReceivedEventArgs> DataReceived;
        /// <summary>
        /// Returns whether the server is running
        /// </summary>
        public bool IsRunning => _isRunning;
        public int Port { get; private set; }
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        /// <summary>
        /// new instance of CustomTcpServer
        /// </summary>
        public InsTcpServer()
        {
            _connectedClients = new List<TcpClient>();
        }
        /// <summary>
        /// start TCP server
        /// </summary>
        /// <param name="port"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Start(int port)
        {
            if (_isRunning)
                throw new InvalidOperationException("Server is already running");

            Port = port;
            _cancellationTokenSource = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Any, port);
            try
            {
                _listener.Start();
            }
            catch (Exception ex)
            {
                Log($"Error starting TCP server,Please restart the TCP server: {ex.Message}");
                throw;
            }
            _isRunning = true;

            Log($"TCP Server started on port {port}");

            // 开始接受客户端连接
            Task.Run(async () => await AcceptClientsAsync(_cancellationTokenSource.Token));
        }
        /// <summary>
        /// stop TCP server
        /// </summary>
        public void Stop()
        {
            if (!_isRunning) return;

            _cancellationTokenSource?.Cancel();

            lock (_clientsLock)
            {
                foreach (var client in _connectedClients)
                {
                    try
                    {
                        client.Close();
                    }
                    catch (Exception ex)
                    {
                        Log($"Error closing client: {ex.Message}");
                    }
                }
                _connectedClients.Clear();
            }

            try
            {
                _listener?.Stop();
            }
            catch (Exception ex)
            {
                Log($"Error stopping listener: {ex.Message}");
            }

            _isRunning = false;
            Log("TCP Server stopped");
        }
        /// <summary>
        /// client accept loop
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task AcceptClientsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client, cancellationToken));
                }
                catch (ObjectDisposedException)
                {
                    // 监听器已被释放，正常退出
                    break;
                }
                catch (Exception ex)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        Log($"Error accepting client: {ex.Message}");
                    }
                }
            }
        }
        /// <summary>
        /// handle connected client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            var clientEndPoint = client.Client.RemoteEndPoint.ToString();

            lock (_clientsLock)
            {
                _connectedClients.Add(client);
            }

            Log($"Client connected: {clientEndPoint}");
            ClientConnected?.Invoke(this, new TcpClientConnectedEventArgs(client, clientEndPoint));

            try
            {
                using (var stream = client.GetStream())
                {
                    var buffer = new byte[4096];

                    while (!cancellationToken.IsCancellationRequested && client.Connected)
                    {
                        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

                        if (bytesRead == 0)
                        {
                            // 客户端断开连接
                            break;
                        }

                        var data = Encoding.GetString(buffer, 0, bytesRead);
                        Log($"Received from {clientEndPoint}: {data}");

                        DataReceived?.Invoke(this, new TcpDataReceivedEventArgs(client, clientEndPoint, data));
                    }
                }
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException)
            {
                // 客户端断开连接
            }
            catch (Exception ex)
            {
                Log($"Error handling client {clientEndPoint}: {ex.Message}");
            }
            finally
            {
                lock (_clientsLock)
                {
                    _connectedClients.Remove(client);
                }

                try
                {
                    client.Close();
                }
                catch { /* 忽略关闭异常 */ }

                Log($"Client disconnected: {clientEndPoint}");
                ClientDisconnected?.Invoke(this, new TcpClientDisconnectedEventArgs(client, clientEndPoint));
            }
        }
        /// <summary>
        /// send message to specific client
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task SendToClientAsync(TcpClient client, string message)
        {
            if (client == null || !client.Connected)
                throw new InvalidOperationException("Client is not connected");

            try
            {
                var data = Encoding.GetBytes(message);
                var stream = client.GetStream();
                await stream.WriteAsync(data, 0, data.Length);
                Log($"Sent to {client.Client.RemoteEndPoint}: {message}");
            }
            catch (Exception ex)
            {
                Log($"Error sending to client: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// broadcast message to all connected clients
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task BroadcastAsync(string message)
        {
            List<TcpClient> clients;
            lock (_clientsLock)
            {
                clients = new List<TcpClient>(_connectedClients);
            }

            var tasks = new List<Task>();
            foreach (var client in clients)
            {
                if (client.Connected)
                {
                    tasks.Add(SendToClientAsync(client, message));
                }
            }

            await Task.WhenAll(tasks);
        }
        /// <summary>
        /// get list of connected clients
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<TcpClient> GetConnectedClients()
        {
            lock (_clientsLock)
            {
                return new List<TcpClient>(_connectedClients);
            }
        }

        private void Log(string message)
        {
            LogMessage?.Invoke(this, $"{message}");
        }

        public void Dispose()
        {
            Stop();
            _cancellationTokenSource?.Dispose();
        }
    }
    /// <summary>
    /// client connected event args
    /// </summary>
    public class TcpClientConnectedEventArgs : EventArgs
    {
        public TcpClient Client { get; }
        public string ClientEndPoint { get; }

        public TcpClientConnectedEventArgs(TcpClient client, string clientEndPoint)
        {
            Client = client;
            ClientEndPoint = clientEndPoint;
        }
    }
    /// <summary>
    /// client disconnected event args
    /// </summary>
    public class TcpClientDisconnectedEventArgs : EventArgs
    {
        public TcpClient Client { get; }
        public string ClientEndPoint { get; }

        public TcpClientDisconnectedEventArgs(TcpClient client, string clientEndPoint)
        {
            Client = client;
            ClientEndPoint = clientEndPoint;
        }
    }
    /// <summary>
    /// client data received event args
    /// </summary>
    public class TcpDataReceivedEventArgs : EventArgs
    {
        public TcpClient Client { get; }
        public string ClientEndPoint { get; }
        public string Data { get; }

        public TcpDataReceivedEventArgs(TcpClient client, string clientEndPoint, string data)
        {
            Client = client;
            ClientEndPoint = clientEndPoint;
            Data = data;
        }
    }
}
