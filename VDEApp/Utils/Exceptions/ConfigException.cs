using System;

namespace VDEApp.Utils.Exceptions
{
    /// <summary>
    /// 配置相关异常
    /// </summary>
    public class ConfigException : Exception
    {
        public ConfigException() : base() { }
        public ConfigException(string message) : base(message) { }
        public ConfigException(string message, Exception innerException) : base(message, innerException) { }
    }
}
