using Insnex.VisionCore;
using MetaLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Infrastructure;
using VDEApp.LogModule;

namespace VDEApp
{
    internal static class Program
    {
        // 定义互斥体名称（建议使用唯一标识，如公司+产品名）
        private const string MutexName = "Insnex_VDEApp_Singleton_Mutex_{8F6F0AC4-B9A1-45FD-A8CF-72F04E6BDE8F}";
        private static Mutex _mutex;

        private static List<string> _assemblySearchPaths = BuildAssemblySearchPaths();

        /// <summary>
        /// 依赖注入服务提供者
        /// </summary>
        public static IServiceProvider Services { get; private set; }

        /// <summary>
        /// 启动
        /// </summary>
        static Program()
        {
            // 若没有在根目录找到依赖的程序集，则在自定义路径中搜索
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            // 全局异常处理器
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //默认样式
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 配置依赖注入容器
            Services = ServiceConfigurator.ConfigureServices();
            ServiceLocator.Initialize(Services);
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 检查是否已有实例在运行
            bool isNewInstance;
            _mutex = new Mutex(true, MutexName, out isNewInstance);
            if (!isNewInstance)
            {
                ActivateExistingInstance();
                return;
            }
            try
            {
                // 检查VDE，初始化VDE运行时
                InsLicenseManager.CheckInsWorksVde2DModule();
                InsLicenseManager.CheckInsWorksVde3DModule();
                InsLicenseManager.CheckInsWorksVdeAIModule();



                // 配置 AntdUI 全局样式
                AntdUI.Config.Theme().Dark("#000", "#fff").Light("#fff", "#000");
                AntdUI.Config.TextRenderingHighQuality = true;
                AntdUI.Config.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                // 初始化加载默认语言（通过 ServiceLocator 获取）
                var globalConfig = ServiceLocator.GlobalConfig;
                globalConfig.GlobalLocalizer.LoadLanguage(globalConfig.CurrentLang);
                globalConfig.Initialization();

                // 初始化日志模块
                AppModuleSingleton.MainFormInstance.Shown += (sender, e) => Log.ProcessLogQueue();
                AppModuleSingleton.MainFormInstance.Show();

                // 打开项目（通过 ServiceLocator 获取）
                var projectController = ServiceLocator.ProjectController;
                if (projectController.OpenProject(globalConfig.UserOperation.CurrentProject))
                {
                    //启动主窗口
                    AppModuleSingleton.ReloadUI();
                    //启动服务器（通过 ServiceLocator 获取）
                    ServiceLocator.TcpCommunicationController.StartServer();
                    Application.Run(AppModuleSingleton.MainFormInstance);
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                WriteDebugLog(ex);

                string errorMessage = $"The application failed to start！\n\n" +
                            $"ExceptionType: {ex.GetType().Name}\n" +
                            $"ExceptionMessage: {ex.Message}\n\n" +
                            $"Please check the log file 'debug.log'.";

                MessageBox.Show(errorMessage, "VDEApp 启动错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                LogModule.Log.StopLogProcessing();
                if (_mutex != null && isNewInstance)
                {
                    try
                    {
                        _mutex.ReleaseMutex();
                    }
                    catch (ApplicationException)
                    {
                        // 忽略已经释放的 Mutex 异常
                    }
                }
            }
        }

        /// <summary>
        /// 激活已存在的应用程序实例
        /// </summary>
        private static void ActivateExistingInstance()
        {
            try
            {
                // 查找已运行的实例
                var currentProcess = Process.GetCurrentProcess();
                var existingProcess = Process.GetProcessesByName(currentProcess.ProcessName)
                    .FirstOrDefault(p => p.Id != currentProcess.Id);

                if (existingProcess != null && existingProcess.MainWindowHandle != IntPtr.Zero)
                {
                    // 激活窗口并置于前台
                    NativeMethods.ShowWindowAsync(existingProcess.MainWindowHandle, NativeMethods.SW_RESTORE);
                    NativeMethods.SetForegroundWindow(existingProcess.MainWindowHandle);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Activation of existing instance failed：{ex.Message}");
            }
        }

        /// <summary>
        /// 重启
        /// </summary>
        public static void RestartApplication()
        {
            try
            {
                // 1.检查当前项目是否在运行
                var currentProject = ServiceLocator.GlobalConfig.CurrentProject;
                if (currentProject != null && currentProject.IsRunning)
                {
                    var ret = MessageBox.Show("当前项目正在运行，无法关闭应用程序。是否停止运行。", "无法关闭", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (ret == DialogResult.Yes)
                        ServiceLocator.TaskController.StopLoopRun();
                    else
                        return;
                }

                // 2.关闭主窗口
                AppModuleSingleton.MainFormInstance.Close();
                if (!AppModuleSingleton.MainFormInstance.IsDisposed)
                {
                    return;
                }
                // 3. 获取当前程序路径和进程ID
                string exePath = Application.ExecutablePath;
                int currentProcessId = Process.GetCurrentProcess().Id;

                // 4. 获取当前启动参数
                string args = string.Join(" ", Environment.GetCommandLineArgs().Skip(1));

                // 5. 配置并启动新实例
                ProcessStartInfo startInfo = new ProcessStartInfo(exePath, args)
                {
                    WorkingDirectory = Application.StartupPath
                };
                Process.Start(startInfo);

                if (_mutex != null)
                {
                    try
                    {
                        _mutex.ReleaseMutex();
                    }
                    catch (ApplicationException)
                    {
                        // 忽略 Mutex 释放异常
                    }
                }
                Application.Exit();
            }
            catch (Exception ex)
            {
                Log.Error($"Restart failed：{ex.Message}");
            }
        }

        /// <summary>
        /// 自定义程序集加载路径
        /// </summary>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);
            foreach (var path in _assemblySearchPaths)
            {
                var dll = Path.Combine(path, $"{name.Name}.dll");
                if (File.Exists(dll))
                    return Assembly.LoadFrom(dll);
            }
            return null;
        }

        /// <summary>
        /// 构造自定义程序集搜索路径
        /// </summary>
        private static List<string> BuildAssemblySearchPaths()
        {
            var ret = new List<string>();
            if (Environment.GetEnvironmentVariable("INSWORKS_VDEROOT") is string vderoot && !string.IsNullOrEmpty(vderoot))
                ret.Add(Path.Combine(vderoot, "ReferencedAssemblies"));
            ret.Add(AppPathRouter.AssembliesPath);
            ret.Add(AppPathRouter.CameraAssembliesPath);

            foreach (var dir in Directory.GetDirectories(AppPathRouter.CameraAssembliesPath))
            {
                ret.Add(dir);
            }
            return ret;
        }

        /// <summary>
        /// 全局异常处理器 - UI 线程异常
        /// </summary>
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            WriteDebugLog(ex);

            string errorMessage = $"The application encountered an unprocessed UI thread exception!\n\n" +
                        $"ExceptionType: {ex.GetType().Name}\n" +
                        $"ExceptionMessage: {ex.Message}\n\n" +
                        $"Recorded to {exceptionLogPath}" +
                        $"Click OK to continue running, or click Cancel to exit the program！";

            DialogResult result = MessageBox.Show(errorMessage, "VDEApp UI Thread exception",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Error);

            if (result == DialogResult.Cancel)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// 全局异常处理器 - 非 UI 线程异常
        /// </summary>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                WriteDebugLog(ex);

                string errorMessage = $"The application encountered an unprocessed non UI thread exception！\n\n" +
                            $"ExceptionType: {ex.GetType().Name}\n" +
                            $"ExceptionMessage: {ex.Message}\n" +
                            $"Recorded to {exceptionLogPath}" +
                            $"Do you want to terminate the program: {(e.IsTerminating ? "Yes" : "No")}\n\n";

                MessageBox.Show(errorMessage, "VDEApp Unprocessed exception",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ExceptionLogPath
        /// </summary>
        private static string exceptionLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "debug.log");

        /// <summary>
        /// WriteDebugLog
        /// </summary>
        private static void WriteDebugLog(Exception exception)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"ExceptionType:{exception.GetType().FullName}");
                stringBuilder.Append($"ExceptionMessage:{exception.Message}");
                stringBuilder.Append($"ExceptionStackTrace:{exception.StackTrace}");

                if (exception.InnerException != null)
                {
                    stringBuilder.Append($"InnerExceptionType: {exception.InnerException.GetType().FullName}");
                    stringBuilder.Append($"InnerExceptionMessage: {exception.InnerException.Message}");
                    stringBuilder.Append($"InnerExceptionStackTrace: {exception.InnerException.StackTrace}");
                }

                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {stringBuilder.ToString()}\n";
                File.AppendAllText(exceptionLogPath, logMessage);
            }
            catch
            {
                // 忽略日志写入失败，避免二次异常
            }
        }

        /// <summary>
        /// 原生方法调用（用于窗口激活）
        /// </summary>
        private static class NativeMethods
        {
            public const int SW_RESTORE = 9;

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);
        }
    }
}
