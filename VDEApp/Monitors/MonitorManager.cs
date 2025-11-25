using System;
using System.Collections.Generic;
using System.Linq;

namespace VDEApp.Monitors
{
    public class MonitorManager
    {
        private static readonly Lazy<MonitorManager> monitorManager = new Lazy<MonitorManager>(() => new MonitorManager());
        public static MonitorManager Instance => monitorManager.Value;
        private MonitorManager() { }
        private List<IMonitor> _monitors { get; set; } = new List<IMonitor>();
        public void RegisterMonitor(IMonitor monitor)
        {
            if (!_monitors.Any(s => s.GetType() == monitor.GetType()))
                _monitors.Add(monitor);
            else
                throw new InvalidOperationException("This type of monitoring has already been registered！");
        }
        public void UnRegisterMonitor(IMonitor monitor)
        {
            var findMonitor = _monitors.FirstOrDefault(s => s.GetType() == monitor.GetType());
            if (findMonitor != null)
                _monitors.Remove(findMonitor);
        }

        public void StartAllMonitors()
        {
            foreach (var monitor in _monitors)
            {
                if (!monitor.IsRunning)
                {
                    monitor.Start();
                }
            }
        } 
        public void StopAllMonitors()
        {
            foreach (var monitor in _monitors)
            {
                if (monitor.IsRunning)
                {
                    monitor.Stop();
                }
            }
        }
    }
}
