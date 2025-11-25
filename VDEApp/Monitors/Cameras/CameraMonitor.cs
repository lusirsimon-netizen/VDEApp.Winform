using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VDEApp.Devices;
using VDEApp.Infrastructure;

namespace VDEApp.Monitors.Cameras
{
    public class CameraMonitor : IMonitor
    {
        public bool IsRunning { get; private set; }

        public int Interval => 1000;

        public event EventHandler<CameraMonitorEventArgs> CameraStatusChanged;
        private CancellationTokenSource cancellationTokenSource;
        public void Start()
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
                cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            IsRunning = true;
            Task.Run(async () =>
            {
                while (IsRunning)
                {
                    CameraStatusChanged?.Invoke(this, new CameraMonitorEventArgs() { Cameras = ServiceLocator.DeviceController.Cameras });
                    await Task.Delay(Interval);
                }
            }, cancellationTokenSource.Token);
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
            IsRunning = false;
        }
    }

    public class CameraMonitorEventArgs : EventArgs
    {
        public List<ICamera> Cameras { get; set; }
    }
}
