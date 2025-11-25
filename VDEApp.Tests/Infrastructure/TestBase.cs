using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using VDEApp.Infrastructure;

namespace VDEApp.Tests.Infrastructure
{
    /// <summary>
    /// 测试基类
    /// 提供 DI 容器和公共测试设施
    /// </summary>
    public class TestBase : IDisposable
    {
        protected IServiceProvider Services { get; private set; }

        public TestBase()
        {
            // 为每个测试创建独立的 DI 容器
            Services = ServiceConfigurator.ConfigureServices();
        }
        static TestBase()
        {
            // 禁用强名称验证
            try
            {
                var method = typeof(AppDomain).GetMethod("EnableResolveAssembliesForIntrospection",
                    BindingFlags.Static | BindingFlags.NonPublic);
                if (method != null)
                {
                    method.Invoke(null, new object[] { "VDEApp.Tests" });
                }
            }
            catch
            {
                // 忽略错误,继续执行
            }
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        protected T GetService<T>() where T : class
        {
            return Services.GetRequiredService<T>();
        }

        public virtual void Dispose()
        {
            // 清理资源
            if (Services is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
