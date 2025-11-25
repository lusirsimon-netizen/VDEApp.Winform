using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using VDEApp.Attributes;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Configs.Display;
using VDEApp.Infrastructure;
using VDEApp.Models;
using VDEApp.Models.Operations;
using VDEApp.Models.Project;
using VDEApp.Models.TaskNodes;
using VDEApp.Views.Project;

namespace VDEApp.Controllers
{
    public class ProjectController
    {
        /// <summary>
        /// 当前项目
        /// </summary>
        public ProjectModel CurrentProject => GlobalConfig.Instance.CurrentProject;
        /// <summary>
        /// 国际化
        /// </summary>
        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;
        /// <summary>
        /// 打开项目
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public bool OpenProject(string projectPath)
        {
            try
            {
                //创建项目
                if (!File.Exists(projectPath))
                {
                    return ValidCurrentProjectNotExistsCreate(false, false);
                }
                else
                {
                    // 读入并挂到全局
                    var projectJson = File.ReadAllText(projectPath);
                    GlobalConfig.Instance.CurrentProject = JsonConvert.DeserializeObject<ProjectModel>(projectJson);
                    GlobalConfig.Instance.CurrentProject.Path = projectPath;

                    // 加载配置
                    LoadConfigs();

                    // 加载仍然存在的任务节点
                    if (GlobalConfig.Instance.CurrentProject.TaskGroup != null)
                    {
                        foreach (TaskModel task in GlobalConfig.Instance.CurrentProject.TaskGroup)
                            ServiceLocator.TaskController.LoadTask(task);
                    }

                    // 加载 CurrentTask
                    var proj = GlobalConfig.Instance.CurrentProject;
                    if (proj != null)
                    {
                        // 检查 CurrentTask 是否有效
                        if (proj.CurrentTask != null && proj.TaskGroup != null)
                        {
                            var taskInGroup = proj.TaskGroup.FirstOrDefault(t => t.Guid == proj.CurrentTask.Guid);

                            if (taskInGroup != null)
                            {
                                proj.CurrentTask = taskInGroup;
                            }
                            else
                            {
                                proj.CurrentTask = proj.TaskGroup.FirstOrDefault();
                            }
                        }
                        else if (proj.TaskGroup != null && proj.TaskGroup.Any())
                        {
                            proj.CurrentTask = proj.TaskGroup.FirstOrDefault();
                        }
                        if (proj.CurrentTask != null)
                            ServiceLocator.TaskController.LoadTask(proj.CurrentTask);
                    }

                    // 维护最近打开历史
                    var history = GlobalConfig.Instance.UserOperation.ProjectHistorys.FirstOrDefault(s => s.Path == projectPath);
                    if (history == null)
                        GlobalConfig.Instance.UserOperation.ProjectHistorys.Insert(0, new OpenHistory() { Path = projectPath, OpenTime = DateTime.Now, });
                    else
                    {
                        history.OpenTime = DateTime.Now;
                        GlobalConfig.Instance.UserOperation.ProjectHistorys.Remove(history);
                        GlobalConfig.Instance.UserOperation.ProjectHistorys.Insert(0, history);
                    }
                    GlobalConfig.Instance.SaveOperation();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogModule.Log.Error(ex.Message);
                MessageBox.Show(ex.Message, "Load project", MessageBoxButtons.OK);
            }

            return false;
        }

        /// <summary>
        /// 加载必要配置文件
        /// </summary>
        public void LoadConfigs()
        {
            #region 初始化App/项目必要文件夹
            var fields = typeof(AppPathRouter).GetProperties();
            foreach (var item in fields)
            {
                var projectDirectory = item.GetCustomAttribute<InitializeProjectDirectoryAttribute>();
                var appDirectory = item.GetCustomAttribute<InitializeAppDirectoryAttribute>();
                if (projectDirectory != null || appDirectory != null)
                {
                    var path = item.GetValue(null);
                    if (path is string directory && !Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                }
            }
            #endregion

            #region Camera Configs
            new CameraConfiguration().Initialization();
            #endregion
        }

        /// <summary>
        /// 新建项目
        /// 注意：新增了可选参数 restart，默认 true（保持你原有菜单行为）。
        /// 在任务管理窗口的“按需创建”里传入 false，避免点击“新建任务”时重启应用。
        /// </summary>
        public void NewProject(string projectName, string projectFolder, bool isAddHistory = false)
        {
            if (string.IsNullOrEmpty(projectName))
                throw new OperationCanceledException(Localizer.GetString("ProjectController_EnterName", "Please enter the project name!"));

            var projectPath = Path.Combine(projectFolder, projectName);
            if (Directory.Exists(projectPath))
            {
                if (Directory.GetFiles(projectPath).Length > 0)
                    throw new Exception(Localizer.GetString("ProjectController_ChooseDirectory", "The current selected directory is not empty. Please choose an empty directory to create!"));
            }
            else
            {
                Directory.CreateDirectory(projectPath);
            }

            // 确保 tasks 目录存在
            var tasksDir = Path.Combine(projectPath, "Tasks");
            if (!Directory.Exists(tasksDir))
                Directory.CreateDirectory(tasksDir);

            string projectFilePath = Path.Combine(projectPath, $"{projectName}{AppConstant.ProjectExtension}");
            if (File.Exists(projectFilePath))
                throw new Exception(Localizer.GetString("ProjectController_ProjectAlreadyExists", "The current project file has been created. Please select an empty directory to create it!"));

            var newProject = new ProjectModel()
            {
                Name = projectName,
                CreateTime = DateTime.Now,
                Guid = Guid.NewGuid().ToString(),
                Path = projectFilePath,
                //TaskGroup = new List<TaskModel>() { TaskModel.Create("Task1") }
            };
            GlobalConfig.Instance.CurrentProject = newProject;

            try
            {
                ServiceLocator.TaskController.AddTask("Task1");
            }
            catch (Exception ex)
            {
                // 如果添加默认任务失败，整个项目创建流程也应该失败
                throw new Exception($"创建项目的默认任务 'Task1' 失败: {ex.Message}", ex);
            }

            newProject.CurrentTask = newProject.TaskGroup[0];
            File.WriteAllText(projectFilePath, JsonConvert.SerializeObject(newProject));

            // 更新全局状态与最近打开
            GlobalConfig.Instance.UserOperation.CurrentProject = projectFilePath;
            //加入最近打开历史
            if (isAddHistory)
            {
                GlobalConfig.Instance.UserOperation.ProjectHistorys.Insert(0, new OpenHistory()
                {
                    Path = projectFilePath,
                    OpenTime = DateTime.Now,
                });
            }
            GlobalConfig.Instance.SaveOperation();

            GlobalConfig.Instance.CurrentProject = newProject;
            GlobalConfig.Instance.CurrentProject.Path = projectFilePath;

            try
            {
                // 加载所有必要的配置文件，这通常是加载项目的一部分
                LoadConfigs();

                // 遍历新项目中的任务并加载它们，这将触发事件订阅
                if (newProject.TaskGroup != null)
                {
                    foreach (var task in newProject.TaskGroup)
                    {
                        ServiceLocator.TaskController.LoadTask(task);
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录日志，但不应中断主流程
                LogModule.Log.Error($"Failed to load newly created project '{projectName}': {ex.Message}");
                // (可选) 弹窗提示用户
                // MessageBox.Show($"项目已创建，但加载时出现问题: {ex.Message}");
            }
        }

        /// <summary>
        /// 对账/自愈：让内存中的 TaskGroup 和磁盘上的 Tasks 目录保持一致
        /// removeMissing:  移除“内存里有但磁盘上已删除”的任务
        /// bringOrphans:   是否收编“磁盘有但内存没有”的孤儿目录为任务（默认否）
        /// 返回：变更的数量（用于判断是否需要保存）
        /// </summary>
        public int RepairTaskListAgainstDisk(bool removeMissing = true, bool bringOrphans = false)
        {
            var proj = CurrentProject;
            if (proj == null || string.IsNullOrEmpty(proj.Path))
                return 0;

            var projectDir = Path.GetDirectoryName(proj.Path) ?? "";
            var tasksRoot = Path.Combine(projectDir, "Tasks");
            Directory.CreateDirectory(tasksRoot);

            var diskNames = new HashSet<string>(
                Directory.EnumerateDirectories(tasksRoot).Select(Path.GetFileName),
                StringComparer.OrdinalIgnoreCase
            );

            proj.TaskGroup ??= new List<TaskModel>();
            int changes = 0;

            // 1) 移除磁盘已不存在的任务
            for (int i = proj.TaskGroup.Count - 1; i >= 0; i--)
            {
                var t = proj.TaskGroup[i];
                var name = t?.Name?.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    proj.TaskGroup.RemoveAt(i);
                    changes++;
                    continue;
                }

                if (!diskNames.Contains(name))
                {
                    if (removeMissing)
                    {
                        proj.TaskGroup.RemoveAt(i);
                        changes++;
                    }
                }
                else
                {
                    // 任务在磁盘上存在：补齐 ToolBlocks 目录
                    var tb = Path.Combine(tasksRoot, name, "ToolBlocks");
                    if (!Directory.Exists(tb))
                    {
                        Directory.CreateDirectory(tb);
                        changes++;
                    }
                    diskNames.Remove(name);
                }
            }

            // 2) （可选）把磁盘孤儿任务加回内存
            if (bringOrphans)
            {
                foreach (var orphan in diskNames)
                {
                    var t = TaskModel.Create(orphan);
                    proj.TaskGroup.Add(t);
                    try
                    { ServiceLocator.TaskController.LoadTask(t); }
                    catch { /* 忽略加载异常 */ }
                    changes++;
                }
            }

            // 3) 修正 CurrentTask
            if (proj.CurrentTask != null)
            {
                // 检查 TaskGroup 列表中是否存在一个任务，其 Guid 与 CurrentTask 的 Guid 相同。
                bool currentTaskStillExists = proj.TaskGroup.Any(t => t.Guid == proj.CurrentTask.Guid);

                if (!currentTaskStillExists)
                {
                    proj.CurrentTask = proj.TaskGroup.FirstOrDefault();
                    changes++;
                }
            }
            else if (proj.TaskGroup.Any())
            {
                // 如果 CurrentTask 本身是 null，但任务列表不为空，
                // 自动选择第一个作为当前任务。
                proj.CurrentTask = proj.TaskGroup.FirstOrDefault();
                changes++;
            }

            return changes;
        }

        /// <summary>
        /// 保存当前项目
        /// </summary>
        public void SaveCurrentProject()
        {
            if (GlobalConfig.Instance.CurrentProject != null)
            {
                if (string.IsNullOrEmpty(GlobalConfig.Instance.CurrentProject.Path))
                    GlobalConfig.Instance.CurrentProject.Path = Path.Combine(AppPathRouter.WorkRoot, GlobalConfig.Instance.CurrentProject.Name, $"{GlobalConfig.Instance.CurrentProject.Name}{AppConstant.ProjectExtension}");
                var dir = new FileInfo(GlobalConfig.Instance.CurrentProject.Path).DirectoryName;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(GlobalConfig.Instance.CurrentProject.Path, JsonConvert.SerializeObject(GlobalConfig.Instance.CurrentProject));
                GlobalConfig.Instance.CurrentProject.IsModified = false;
                GlobalConfig.Instance.CurrentProject.Description = "";

                #region Camera Configs
                new CameraConfiguration().Save();
                #endregion

                #region Display Configs
                DisplayConfigManager.Instance.SaveConfig(false);
                #endregion
            }
        }

        /// <summary>
        /// 验证项目有效性
        /// </summary>
        public bool ValidCurrentProjectNotExistsCreate(bool save, bool restart, bool setOwer = false)
        {
            return new WinProjectNew(isSave: save, isRestart: restart) { Owner = setOwer ? AppModuleSingleton.MainFormInstance : null }.ShowDialog() == DialogResult.OK;
        }
    }
}

