using Microsoft.Extensions.DependencyInjection;
using System;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Controllers.Display;

namespace VDEApp.Infrastructure
{
    /// <summary>
    /// 依赖注入服务配置器
    /// </summary>
    public static class ServiceConfigurator
    {
        /// <summary>
        /// 配置所有服务
        /// </summary>
        /// <returns>服务提供者</returns>
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // 注册配置服务（单例）
            services.AddSingleton(provider => GlobalConfig.Instance);

            // 注册控制器服务（单例）
            services.AddSingleton(typeof(ProjectController));
            services.AddSingleton(typeof(TaskController));
            services.AddSingleton(typeof(DeviceController));
            services.AddSingleton(typeof(DisplayController));
            services.AddSingleton(typeof(SaveImageController));
            services.AddSingleton(typeof(TcpCommunicationController));

            // 注意：LanguageController 是静态类，不需要注册到 DI 容器

            // 构建服务提供者
            return services.BuildServiceProvider();
        }

        /// <summary>
        /// 获取服务（用于测试或手动解析）
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns>服务实例</returns>
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetRequiredService<T>();
        }
    }
}
