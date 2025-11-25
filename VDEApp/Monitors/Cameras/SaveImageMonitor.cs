using System;
using System.Threading;
using System.Threading.Tasks;
using VDEApp.Infrastructure;

namespace VDEApp.Monitors.Cameras
{
    public class SaveImageMonitor : IMonitor
    {
        public bool IsRunning { get; private set; }
        public int Interval => 300;

        public event EventHandler<SaveImageMonitorEventArgs> SaveImageEvent;
        private CancellationTokenSource _cts;



        public void Start()
        {
            Stop();
            _cts = new CancellationTokenSource();
            IsRunning = true;

            Task.Run(async () =>
            {
                var token = _cts.Token;
                while (!token.IsCancellationRequested && IsRunning)
                {
                    int count = 0;
                    try
                    {
                        // 从SaveImageController 实例里拿队列长度
                        count = ServiceLocator.SaveImageController?.CurrentQueueLength ?? 0;
                    }
                    catch { /* 忽略取数失败 */ }

                    try
                    {
                        SaveImageEvent?.Invoke(this, new SaveImageMonitorEventArgs { ImageCount = count });
                    }
                    catch { /* 避免监听侧异常卡死循环 */ }

                    try
                    {
                        await Task.Delay(Interval, token);
                    }
                    catch (TaskCanceledException) { }
                }
            }, _cts.Token);
        }

        public void Stop()
        {
            IsRunning = false;
            try
            { _cts?.Cancel(); }
            catch { }
            try
            { _cts?.Dispose(); }
            catch { }
            _cts = null;
        }
    }

    public class SaveImageMonitorEventArgs : EventArgs
    {
        public int ImageCount { get; set; }
    }
}
