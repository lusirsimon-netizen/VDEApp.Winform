using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VDEApp.LogModule
{
    public enum LogType
    {
        Info,
        Debug,
        Warn,
        Error,
        Fatal
    }
    public class Log
    {
        /// <summary>
        /// Get log instance
        /// </summary>
        public static ILog log = LogManager.GetLogger("mylog");
        /// <summary>
        /// Log processing interval time, unit: milliseconds
        /// </summary>
        public static int Interval = 1;
        /// <summary>
        /// Indicates whether the process or service is currently running.
        /// </summary>
        public static bool IsRunning = false;
        /// <summary>
        /// Log queue
        /// </summary>
        private static ConcurrentQueue<Tuple<LogType, string, Exception>> logQueue = new ConcurrentQueue<Tuple<LogType, string, Exception>>();
        /// <summary>
        /// Processes all pending log entries in the log queue by writing each entry in order.
        /// </summary> 
        public static void ProcessLogQueue()
        {
            IsRunning = true;
            Task.Run(async () =>
            {
                while (IsRunning)
                {
                    if (logQueue.TryDequeue(out var logItem))
                    {
                        Write(logItem.Item1, logItem.Item2, logItem.Item3);
                    }
                    await Task.Delay(Interval);
                } 
            });
        }
        /// <summary>
        /// output log
        /// </summary> 
        public static void Write(LogType type, string message, Exception ex = null)
        {
            switch (type)
            {
                case LogType.Info:
                    log.Info(message);
                    break;
                case LogType.Debug:
                    if (ex != null)
                        log.Debug(message, ex);
                    else
                        log.Debug(message);
                    break;
                case LogType.Warn:
                    if (ex != null)
                        log.Warn(message, ex);
                    else
                        log.Warn(message);
                    break;
                case LogType.Error:
                    if (ex != null)
                        log.Error(message, ex);
                    else
                        log.Error(message);
                    break;
                case LogType.Fatal:
                    if (ex != null)
                        log.Fatal(message, ex);
                    else
                        log.Fatal(message);
                    break;
            }
        }

        #region Add logs to the queue
        public static void Info(string message)
        {
            logQueue.Enqueue(new Tuple<LogType, string, Exception>(LogType.Info, message, null));
        }
        public static void Debug(string message)
        {
            logQueue.Enqueue(new Tuple<LogType, string, Exception>(LogType.Debug, message, null));
        }
        public static void Warn(string message)
        {
            logQueue.Enqueue(new Tuple<LogType, string, Exception>(LogType.Warn, message, null));
        }
        public static void Error(string message)
        {
            logQueue.Enqueue(new Tuple<LogType, string, Exception>(LogType.Error, message, null));
        }
        public static void Fatal(string message)
        {
            logQueue.Enqueue(new Tuple<LogType, string, Exception>(LogType.Fatal, message, null));
        }
        public static void Warn(string message, Exception ex)
        {
            logQueue.Enqueue(new Tuple<LogType, string, Exception>(LogType.Warn, message, ex));
        }
        public static void Debug(string message, Exception ex)
        {
            logQueue.Enqueue(new Tuple<LogType, string, Exception>(LogType.Debug, message, ex));
        }
        public static void Error(string message, Exception ex)
        {
            logQueue.Enqueue(new Tuple<LogType, string, Exception>(LogType.Error, message, ex));
        }
        public static void Fatal(string message, Exception ex)
        {
            logQueue.Enqueue(new Tuple<LogType, string, Exception>(LogType.Fatal, message, ex));
        }
        #endregion
        /// <summary>
        /// 停止日志处理线程
        /// </summary>
        public static void StopLogProcessing()
        {
            IsRunning = false;

            // 等待队列中的剩余日志处理完成
            Task.Run(async () =>
            {
                while (logQueue.Count > 0)
                {
                    await Task.Delay(10);
                }
            }).Wait(Interval);
        }
    }

}
