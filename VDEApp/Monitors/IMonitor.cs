using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Monitors
{ 
    public interface IMonitor
    {
        /// <summary>
        /// 运行状态
        /// </summary>
        bool IsRunning { get; }
        /// <summary>
        /// 间隔(毫秒)
        /// </summary>
        int Interval { get; }
        /// <summary>
        /// 开始运行
        /// </summary>
        void Start();
        /// <summary>
        /// 终止监控
        /// </summary>
        void Stop();
    }
}
