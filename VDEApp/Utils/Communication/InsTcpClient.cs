using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VDEApp.Utils.Communication
{
    public class InsTcpClient : IDisposable
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isConnected;

        public event EventHandler<string> LogMessage;
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<string> DataReceived;
        public event EventHandler<Exception> ErrorOccurred;

        public string ServerIp { get; private set; }
        public int ServerPort { get; private set; }
        public Encoding Encoding { get; set; } = Encoding.UTF8;
        public bool IsConnected => _isConnected && _tcpClient?.Connected == true;

        public async Task ConnectAsync(string ip, int port, int timeoutMilliseconds = 5000)
        {
            if (_isConnected)
                throw new InvalidOperationException("Client is already connected");

            ServerIp = ip;
            ServerPort = port;
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                _tcpClient = new System.Net.Sockets.TcpClient();
                var connectTask = _tcpClient.ConnectAsync(ip, port);
                var timeoutTask = Task.Delay(timeoutMilliseconds);

                var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    throw new TimeoutException($"Connection to {ip}:{port} timed out");
                }

                await connectTask; // 确保连接完成或抛出异常

                _stream = _tcpClient.GetStream();
                _isConnected = true;

                Log($"Connected to server {ip}:{port}");
                Connected?.Invoke(this, EventArgs.Empty);

                // 开始接收数据
                _ = Task.Run(() => ReceiveDataAsync(_cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                Log($"Connection failed: {ex.Message}");
                ErrorOccurred?.Invoke(this, ex);
                throw;
            }
        }

        public void Disconnect()
        {
            if (!_isConnected) return;

            _cancellationTokenSource?.Cancel();
            _isConnected = false;

            try
            {
                _stream?.Close();
                _tcpClient?.Close();
            }
            catch (Exception ex)
            {
                Log($"Error during disconnect: {ex.Message}");
            }

            Log("Disconnected from server");
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        public async Task SendAsync(string message)
        {
            if (!_isConnected)
                throw new InvalidOperationException("Client is not connected");

            try
            {
                var data = Encoding.GetBytes(message);
                await _stream.WriteAsync(data, 0, data.Length);
                Log($"Sent: {message}");
            }
            catch (Exception ex)
            {
                Log($"Error sending data: {ex.Message}");
                ErrorOccurred?.Invoke(this, ex);
                throw;
            }
        }

        private async Task ReceiveDataAsync(CancellationToken cancellationToken)
        {
            var buffer = new byte[4096];

            while (!cancellationToken.IsCancellationRequested && _isConnected)
            {
                try
                {
                    var bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

                    if (bytesRead == 0)
                    {
                        // 服务器断开连接
                        break;
                    }

                    var data = Encoding.GetString(buffer, 0, bytesRead);
                    Log($"Received: {data}");
                    DataReceived?.Invoke(this, data);
                }
                catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException)
                {
                    // 连接断开
                    break;
                }
                catch (Exception ex)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        Log($"Error receiving data: {ex.Message}");
                        ErrorOccurred?.Invoke(this, ex);
                    }
                    break;
                }
            }

            // 如果是因为异常退出循环，断开连接
            if (_isConnected)
            {
                Disconnect();
            }
        }

        private void Log(string message)
        {
            LogMessage?.Invoke(this, $"[{DateTime.Now:HH:mm:ss}] {message}");
        }

        public void Dispose()
        {
            Disconnect();
            _cancellationTokenSource?.Dispose();
            _tcpClient?.Dispose();
            _stream?.Dispose();
        }
    }
}