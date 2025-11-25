using System;
using System.IO;
using VDEApp.Attributes;
using VDEApp.Configs;
using VDEApp.Models.TaskNodes;

namespace VDEApp.Commons
{
    public static class AppPathRouter
    {
        #region Path
        /// <summary>
        /// 程序目录
        /// </summary>
        public static string AppPath => AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 程序依赖的程序集根目录
        /// </summary>
        public static string AssembliesPath => Path.Combine(AppPath, "Assemblies");
        /// <summary>
        /// 相机SDK程序集根目录
        /// </summary>
        public static string CameraAssembliesPath => Path.Combine(AssembliesPath, "Cameras");
        /// <summary>
        /// 默认工作目录
        /// </summary>
        [InitializeAppDirectory]
        public static string WorkRoot => Path.Combine(AppPath, "WorkRoot");
        /// <summary>
        /// 软件设置目录
        /// </summary>
        [InitializeAppDirectory]
        public static string AppData => Path.Combine(AppPath, "AppData");
        /// <summary>
        /// 用户操作记录
        /// </summary>
        public static string AppOperation => Path.Combine(AppData, $"operation{AppConstant.ProjectHistoryExtension}");
        #endregion

        #region 项目相关路径
        /// <summary>
        /// 当前项目路径
        /// </summary>
        public static string CurrentProjectPath => GlobalConfig.Instance.CurrentProject.Path;
        /// <summary>
        /// 获取任务目录
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static string GetTaskPath(this TaskModel task)
            => Path.Combine(Path.GetDirectoryName(CurrentProjectPath), "Tasks", task.Name);
        /// <summary>
        /// 获取当前项目任务取像节点配置文件路径
        /// </summary>
        public static string GetAcquireNodeConfigPath(this TaskModel task)
            => Path.Combine(Path.GetDirectoryName(CurrentProjectPath), "Tasks", task.Name, "ToolBlocks", $"Acquire.txt");
        /// <summary>
        /// 获取当前项目任务标定ToolBlock路径
        /// </summary>
        public static string GetCalibrationToolBlockPath(this TaskModel task)
            => Path.Combine(Path.GetDirectoryName(CurrentProjectPath), "Tasks", task.Name, "ToolBlocks", $"Calibration.tolb");
        /// <summary>
        /// 获取当前项目任务检测ToolBlock路径
        /// </summary>
        public static string GetInspectionToolBlockPath(this TaskModel task)
           => Path.Combine(Path.GetDirectoryName(CurrentProjectPath), "Tasks", task.Name, "ToolBlocks", $"Inspection.tolb");
        /// <summary>
        /// 项目设置目录
        /// </summary>
        [InitializeProjectDirectory]
        public static string ConfigPath => Path.Combine(Path.GetDirectoryName(GlobalConfig.Instance.CurrentProject.Path), "Configs");
        /// <summary>
        /// 显示设置目录
        /// </summary>
        [InitializeProjectDirectory]
        public static string DisplayConfigPath => Path.Combine(ConfigPath, "Displays");

        /// <summary>
        /// 设备设置目录
        /// </summary>
        [InitializeProjectDirectory]
        public static string DeviceConfigPath => Path.Combine(ConfigPath, "Devices");

        /// <summary>
        /// 相机设置目录
        /// </summary>
        [InitializeProjectDirectory]
        public static string CameraConfigPath => Path.Combine(DeviceConfigPath, "Cameras");
        [InitializeProjectDirectory]
        public static string GetSingleCameraConfigPath(string fileName) => Path.Combine(CameraConfigPath, fileName);
        /// <summary>
        /// 相机状态设置文件
        /// </summary>
        public static string CameraListConfigPath => Path.Combine(DeviceConfigPath, "Cameras.json");
        /// <summary>
        /// 显示设置文件路径
        /// </summary>
        public static string GetDisplayConfigPath(string moduleName) => Path.Combine(DisplayConfigPath, $"{moduleName}{AppConstant.ConfigExtension}");
        #endregion
    }
}
