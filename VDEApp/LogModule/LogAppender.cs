using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic; 
using VDEApp.Configs;
using VDEApp.Views.Main;

namespace VDEApp.LogModule
{
    public class LogAppender :  AppenderSkeleton
    {
        public UCProductionLog ProductionLogControl { get; set; }
        // 日志缓存队列（线程安全）
        private List<string> _logQueue = new List<string>();
        private object _lockObj = new object();
        private bool _isThreadRunning = false;

        protected override void Append(LoggingEvent loggingEvent)
        {
            // 解析日志为“类型 | 时间 | 信息”格式
            string level = loggingEvent.Level.DisplayName.ToUpper(); // 类型（日志级别）
            string levelName = GlobalConfig.Instance.GlobalLocalizer.GetString(level);//本地化
            string time = loggingEvent.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"); // 时间
            string message = loggingEvent.RenderedMessage; // 信息
            if (loggingEvent.ExceptionObject != null)
            {
                message += Environment.NewLine + "异常信息: " + loggingEvent.ExceptionObject.Message;
            }
            string logLine = $"[{levelName}] | {time} | {message}";

         
            // 传递日志给 UCProductionLog 处理（筛选和显示由它负责）
            LogReceived?.Invoke(this, logLine);
        }
        protected override bool RequiresLayout => false;

        public event EventHandler<string> LogReceived;
    }
}
