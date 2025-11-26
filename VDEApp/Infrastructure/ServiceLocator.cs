using Microsoft.Extensions.DependencyInjection;
using System;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Controllers.Display;

namespace VDEApp.Infrastructure
{
    /// <summary>
    /// 服务定位器 - 提供便捷的服务访问方法
    /// 注意：这是一个过渡方案，推荐在新代码中使用构造函数注入
    /// </summary>
    public static class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化服务定位器
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        public static T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceLocator 未初始化，请先调用 Initialize 方法");
            }

            return _serviceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// 尝试获取服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="service">服务实例</param>
        /// <returns>是否成功获取</returns>
        public static bool TryGetService<T>(out T service) where T : class
        {
            if (_serviceProvider == null)
            {
                service = null;
                return false;
            }

            service = _serviceProvider.GetService<T>();
            return service != null;
        }

        // 便捷属性 - 提供快速访问常用服务

        /// <summary>
        /// 获取 ProjectController 实例
        /// </summary>
        public static ProjectController ProjectController => GetService<ProjectController>();

        /// <summary>
        /// 获取 TaskController 实例
        /// </summary>
        public static TaskController TaskController => GetService<TaskController>();

        /// <summary>
        /// 获取 DeviceController 实例
        /// </summary>
        public static DeviceController DeviceController => GetService<DeviceController>();

        /// <summary>
        /// 获取 DisplayController 实例
        /// </summary>
        public static DisplayController DisplayController => GetService<DisplayController>();

        /// <summary>
        /// 获取 SaveImageController 实例
        /// </summary>
        public static SaveImageController SaveImageController => GetService<SaveImageController>();

        /// <summary>
        /// 获取 TcpCommunicationController 实例
        /// </summary>
        public static TcpCommunicationController TcpCommunicationController => GetService<TcpCommunicationController>();

        /// <summary>
        /// 获取 GlobalConfig 实例
        /// </summary>
        public static GlobalConfig GlobalConfig => GetService<GlobalConfig>();
    }
}
