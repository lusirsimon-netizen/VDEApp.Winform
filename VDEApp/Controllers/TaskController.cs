using MetaLog;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers.Display;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Models;
using VDEApp.Models.Product;
using VDEApp.Models.Project;
using VDEApp.Models.TaskNodes;
using VDEApp.Utils.Communication;

namespace VDEApp.Controllers
{
    public class TaskController : IDisposable
    {
        private Localizer _ => ServiceLocator.GlobalConfig.GlobalLocalizer;

        #region 事件定义
        /// <summary>
        /// 任务运行状态变更事件，布尔参数表示当前是否有任务在执行。
        /// </summary>
        public event Action<bool> TaskRunningStateChanged;

        /// <summary>
        /// 任务列表或当前任务发生变更时触发的通知事件。
        /// </summary>
        public event Action TasksChanged;

        /// <summary>
        /// 通知界面任务列表或当前任务发生了变更。
        /// </summary>
        private void RaiseTasksChanged()
        {
            try
            { TasksChanged?.Invoke(); }
            catch { }
        }
        #endregion

        #region 运行状态
        private int _runningTaskCount = 0;
        private bool _lastIsTaskRunning = false;

        /// <summary>
        /// 指示是否有任务正在运行（包含循环运行与单次运行）。
        /// </summary>
        public bool IsTaskRunning => Volatile.Read(ref _runningTaskCount) > 0;

        /// <summary>
        /// 将运行中的任务计数加一并刷新运行状态标志。
        /// </summary>
        private void IncrementRunningTaskCount()
        {
            Interlocked.Increment(ref _runningTaskCount);
            UpdateTaskRunningState();
        }

        /// <summary>
        /// 将运行中的任务计数减一并在计数异常时回退到零，然后刷新运行状态。
        /// </summary>
        private void DecrementRunningTaskCount()
        {
            int newCount = Interlocked.Decrement(ref _runningTaskCount);
            if (newCount < 0)
                Interlocked.Exchange(ref _runningTaskCount, 0);
            UpdateTaskRunningState();
        }

        /// <summary>
        /// 循环模式为true时保持运行状态；循环停止后等待任务计数为0才切换为false
        /// 外部触发任务不影响此状态
        /// </summary>
        private void UpdateTaskRunningState()
        {
            bool currentIsRunning = _isLoopMode || Volatile.Read(ref _runningTaskCount) > 0;

            if (currentIsRunning != _lastIsTaskRunning)
            {
                _lastIsTaskRunning = currentIsRunning;
                TaskRunningStateChanged?.Invoke(currentIsRunning);
                Log.Info($"任务运行状态变更：{(currentIsRunning ? "运行中" : "已停止")}");
            }
        }
        #endregion

        #region 任务锁、状态变量
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _taskLocks = new ConcurrentDictionary<string, SemaphoreSlim>();
        private CancellationTokenSource _taskCancellationTokenSource;
        private bool _isLoopMode = false;
        private readonly object _taskRunLock = new object();

        /// <summary>
        /// 获取循环运行状态
        /// </summary>
        public bool IsLoopRunning => _isLoopMode;

        /// <summary>
        /// 获取任务锁（支持排队，同一任务串行执行；不同任务锁独立，支持并行）。
        /// </summary>
        /// <param name="task">需要获取锁的任务实例。</param>
        /// <returns>对应任务的信号量实例。</returns>
        private SemaphoreSlim GetOrCreateTaskLock(TaskModel task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), _.GetString("TaskController_TaskCannotBeNull"));
            // 每个任务独立锁，初始化为1：同一任务串行，不同任务并行
            return _taskLocks.GetOrAdd(task.Guid, _ => new SemaphoreSlim(1, 1));
        }

        /// <summary>
        /// 移除指定任务对应的信号量并释放资源。
        /// </summary>
        /// <param name="task">需要释放锁的任务实例。</param>
        private void RemoveTaskLock(TaskModel task)
        {
            if (task == null)
                return;
            if (_taskLocks.TryRemove(task.Guid, out var semaphore))
                semaphore.Dispose();
        }

        /// <summary>
        /// 检查是否有外部任务正在运行（用于内部任务触发时的互斥）
        /// </summary>
        /// <returns>存在外部任务占用锁时返回 true。</returns>
        private bool IsAnyExternalTaskRunning()
        {
            foreach (var semaphore in _taskLocks.Values)
            {
                if (semaphore.CurrentCount == 0) // 任何任务锁被占用=有外部任务在运行
                    return true;
            }
            return false;
        }
        #endregion

        #region 任务管理（增删改查、复制、重命名等）
        /// <summary>
        /// 内部任务触发检查（UI/循环/任务组）：阻止内部任务与外部任务并行
        /// </summary>
        /// <param name="triggerSource">触发来源</param>
        /// <returns>当内部任务可执行时返回 true。</returns>
        private bool CheckInternalTaskCanRun(string triggerSource)
        {
            // 检查1：内部任务/循环是否正在运行
            if (IsTaskRunning)
            {
                ShowTaskRunningWarning(triggerSource);
                return false;
            }

            // 检查2：是否有外部任务正在运行（避免内部+外部任务并行冲突）
            if (IsAnyExternalTaskRunning())
            {
                ShowTaskRunningWarning($"{triggerSource}（外部任务正在运行）");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 外部任务触发检查（TCP）：同一任务排队，不同任务并行
        /// </summary>
        /// <param name="task">要执行的任务</param>
        /// <param name="clientKey">客户端标识</param>
        /// <param name="errorMsg">错误信息（输出）</param>
        /// <returns>外部任务允许运行时返回 true。</returns>
        private bool CheckExternalTaskCanRun(TaskModel task, string clientKey, out string errorMsg)
        {
            errorMsg = string.Empty;

            // 检查1：内部任务/循环是否正在运行（内部任务优先级高，阻止外部任务）
            if (IsTaskRunning)
            {
                errorMsg = string.Format(_.GetString("TaskController_InternalTaskRunning_BlockExternal"), task.Name);
                Log.Error($"客户端[{clientKey}]触发任务[{task.Name}]失败：{errorMsg}");
                return false;
            }

            // 检查2：当前任务是否正在执行（同一任务排队）
            var taskLock = GetOrCreateTaskLock(task);
            if (taskLock.CurrentCount == 0)
            {
                errorMsg = string.Format(_.GetString("TaskController_TaskRunning_CannotTrigger_External"), task.Name);
                Log.Error($"客户端[{clientKey}]触发任务[{task.Name}]失败：{errorMsg}（排队中）");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 显示任务运行中警告弹窗
        /// </summary>
        /// <param name="triggerSource">导致冲突的触发来源描述。</param>
        private void ShowTaskRunningWarning(string triggerSource)
        {
            try
            {
                if (Application.OpenForms.Count > 0)
                {
                    Application.OpenForms[0].Invoke(new Action(() =>
                    {
                        MessageBox.Show(
                            string.Format(_.GetString("TaskController_TaskRunning_BlockNewTask"), triggerSource),
                            _.GetString("Title_OperationFailed"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }));
                }
                else
                {
                    MessageBox.Show(
                        string.Format(_.GetString("TaskController_TaskRunning_BlockNewTask"), triggerSource),
                        _.GetString("Title_OperationFailed"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"显示任务运行警告弹窗失败：{ex.Message}", ex);
            }
        }
        #endregion

        #region 任务管理（增删改查、复制、重命名等）
        // 【原有代码不变】保持删除、保存、加载、复制、重命名、添加任务等逻辑

        /// <summary>
        /// 删除指定任务并维护当前项目的任务列表、当前任务指针以及事件订阅。
        /// </summary>
        /// <param name="task">需要删除的任务模型。</param>
        public void DeleteTask(TaskModel task)
        {
            var project = ServiceLocator.ProjectController.CurrentProject;
            if (project == null)
            {//当前没有打开的项目，无法删除任务。
                throw new InvalidOperationException(_.GetString("TaskController_NoOpenProject_CannotDeleteTask"));
            }
            if (project.TaskGroup.Contains(task))
            {
                // 先获取任务锁，确保任务不在运行中
                var taskLock = GetOrCreateTaskLock(task);
                if (!taskLock.Wait(0))
                {
                    throw new InvalidOperationException(string.Format(_.GetString("TaskController_CannotDeleteRunningTask"), task.Name));
                }

                try
                {
                    // 记录删除前索引，用于选中下一个
                    int idx = project.TaskGroup.IndexOf(task);
                    bool isCurrent = ReferenceEquals(project.CurrentTask, task);

                    project.TaskGroup.Remove(task);
                    //调用文件IO删除任务文件
                    task.Delete();

                    // 如果删的是当前：切到“同位置”或最后一个
                    if (isCurrent)
                    {
                        if (project.TaskGroup.Count == 0)
                        {
                            project.CurrentTask = null;
                        }
                        else
                        {
                            if (idx >= project.TaskGroup.Count)
                                idx = project.TaskGroup.Count - 1;
                            project.CurrentTask = project.TaskGroup[idx];
                        }
                    }

                    RaiseTasksChanged();
                    ResubscribeDisplayControllerEvents();
                }
                finally
                {
                    taskLock.Release();
                    RemoveTaskLock(task);
                }
            }
            else
            {//指定的任务不存在于当前项目中，无法删除。
                throw new ArgumentException(_.GetString("TaskController_TaskNotInProject_CannotDelete"));
            }
        }

        /// <summary>
        /// 保存指定任务到磁盘。
        /// </summary>
        /// <param name="task">需要持久化的任务模型。</param>
        public void SaveTask(TaskModel task)
        {
            try
            {
                task.Save();
            }
            catch (Exception ex)
            {//保存任务时发生错误
                throw new Exception(string.Format(_.GetString("TaskController_SaveTaskFailed_WithMsg"), ex.Message));
            }
        }

        /// <summary>
        /// 加载任务节点配置。
        /// </summary>
        /// <param name="task">需要加载配置的任务模型。</param>
        /// <exception cref="Exception"></exception>
        public void LoadTask(TaskModel task)
        {
            try
            {
                task.Load();
            }
            catch (Exception ex)
            {//加载任务时发生错误
                throw new Exception(string.Format(_.GetString("TaskController_LoadTaskFailed_WithMsg"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 复制现有任务并返回新任务实例。
        /// </summary>
        /// <param name="sourceTask">作为复制模板的源任务。</param>
        /// <returns>复制得到的新任务模型。</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public TaskModel CopyTask(TaskModel sourceTask)
        {
            var project = ServiceLocator.ProjectController.CurrentProject;
            if (project == null)
                throw new InvalidOperationException(_.GetString("TaskController_NoOpenProject_CannotCopyTask"));
            if (sourceTask == null)
                throw new ArgumentNullException(nameof(sourceTask), _.GetString("TaskController_SourceTaskCannotBeNull"));

            var projectDir = Path.GetDirectoryName(project.Path) ?? "";
            var tasksRoot = Path.Combine(projectDir, "Tasks");
            var srcDir = Path.Combine(tasksRoot, sourceTask.Name ?? "");

            string baseName = string.Format("{0}_{1}",
                sourceTask.Name ?? _.GetString("TaskController_DefaultTaskName"),
                _.GetString("TaskController_CopySuffix"));
            string newName = baseName;
            int i = 2;
            while (project.TaskGroup.Any(t => string.Equals(t.Name, newName, StringComparison.OrdinalIgnoreCase)) ||
                   Directory.Exists(Path.Combine(tasksRoot, newName)))
            {
                newName = string.Format("{0}{1}", baseName, i);
                i++;
            }

            var dstDir = Path.Combine(tasksRoot, newName);

            // 拷贝目录
            if (Directory.Exists(srcDir))
                CopyDirectory(srcDir, dstDir);
            else
                Directory.CreateDirectory(dstDir);

            // 新建 TaskModel 加入项目
            var newTask = TaskModel.Create(newName);
            newTask.CreatedAt = DateTime.Now;
            newTask.Parameter = sourceTask.Parameter;
            newTask.Spec = sourceTask.Spec;
            newTask.AcquireNode.Load(newTask.GetAcquireNodeConfigPath());
            newTask.AcquireNode.BatchSize = sourceTask.AcquireNode.BatchSize;
            newTask.AcquireNode.ExposureTime = sourceTask.AcquireNode.ExposureTime;
            newTask.AcquireNode.Timeout = sourceTask.AcquireNode.Timeout;
            newTask.CalibrationNode.Load(newTask.GetCalibrationToolBlockPath());
            newTask.InspectionNode.Load(newTask.GetInspectionToolBlockPath());

            project.TaskGroup.Add(newTask);

            // 不设置为当前任务
            RaiseTasksChanged();
            ResubscribeDisplayControllerEvents();
            return newTask;
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDir">源目录路径。</param>
        /// <param name="destDir">目标目录路径。</param>
        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (var file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                var rel = file.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var target = Path.Combine(destDir, rel);
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                File.Copy(file, target, overwrite: false);
            }
        }

        /// <summary>
        /// 重命名任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="newName"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void ReNameTask(TaskModel task, string newName)
        {
            var project = ServiceLocator.ProjectController.CurrentProject;
            if (project == null)
                throw new InvalidOperationException(_.GetString("TaskController_NoOpenProject_CannotRenameTask"));
            if (task == null)
                throw new ArgumentNullException(nameof(task), _.GetString("TaskController_TaskCannotBeNull"));
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException(_.GetString("TaskController_TaskNameEmpty"), nameof(newName));
            if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException(string.Format(_.GetString("TaskController_TaskNameInvalidChars"), newName), nameof(newName));

            if (string.Equals(task.Name, newName, StringComparison.OrdinalIgnoreCase))
                return;

            newName = newName.Trim();
            string oldTaskName = task.Name;

            if (string.Equals(oldTaskName, newName, StringComparison.OrdinalIgnoreCase))
                return;

            // 任务列表重名校验（忽略大小写）
            foreach (var t in project.TaskGroup)
            {
                if (!ReferenceEquals(t, task) &&
                    string.Equals(t?.Name?.Trim(), newName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(string.Format(_.GetString("TaskController_TaskNameDuplicate"), newName));
                }
            }

            var projectDir = Path.GetDirectoryName(project.Path) ?? "";
            var tasksRoot = Path.Combine(projectDir, "Tasks");
            var oldDir = Path.Combine(tasksRoot, oldTaskName ?? "");
            var newDir = Path.Combine(tasksRoot, newName);
            bool configMoved = false;
            try
            {
                MoveDirectoryStrict(oldDir, newDir, GlobalConfig.Localizer.GetString("Folder_TaskConfig", "任务配置"));
                configMoved = true;
            }
            catch
            {
                throw;
            }

            try
            {
                var imageCfg = project.ImageSave;
                string defaultImageRoot = Path.Combine(projectDir, "Images");

                string rawRoot = string.IsNullOrWhiteSpace(imageCfg.RawDir) ? defaultImageRoot : imageCfg.RawDir;
                string shotRoot = string.IsNullOrWhiteSpace(imageCfg.ShotDir) ? defaultImageRoot : imageCfg.ShotDir;

                string absRawRoot = Path.GetFullPath(rawRoot);
                string absShotRoot = Path.GetFullPath(shotRoot);

                void MoveImageFolder(string root, string typeDesc)
                {
                    if (string.IsNullOrWhiteSpace(root))
                        return;
                    string oldP = Path.Combine(root, oldTaskName);
                    string newP = Path.Combine(root, newName);

                    if (Directory.Exists(oldP))
                    {
                        MoveDirectoryStrict(oldP, newP, typeDesc);
                    }
                }

                MoveImageFolder(absRawRoot, GlobalConfig.Localizer.GetString("Folder_RawImages", "原图数据"));
                if (!string.Equals(absRawRoot, absShotRoot, StringComparison.OrdinalIgnoreCase))
                {
                    MoveImageFolder(absShotRoot, GlobalConfig.Localizer.GetString("Folder_Snapshots", "截图数据"));
                }
            }
            catch (Exception ex)
            {
                if (configMoved)
                {
                    try
                    {
                        if (Directory.Exists(newDir))
                        {
                            Directory.Move(newDir, oldDir);
                        }
                    }
                    catch (Exception rollbackEx)
                    {
                        string fatalMsg = string.Format(GlobalConfig.Localizer.GetString("Rename_FatalRollbackFail"), rollbackEx.Message);
                        Log.Fatal(fatalMsg);
                        MessageBox.Show(fatalMsg, GlobalConfig.Localizer.GetString("Title_FatalError", "致命错误"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw;
                    }
                }
                throw;
            }

            task.Name = newName;

            RaiseTasksChanged();
            ResubscribeDisplayControllerEvents();
        }

        /// <summary>
        /// 严格移动文件夹：如果被占用直接报错抛出异常
        /// </summary>
        /// <param name="oldPath">原始目录路径。</param>
        /// <param name="newPath">新的目标目录路径。</param>
        /// <param name="folderDescription">目录用途描述，用于提示信息。</param>
        private void MoveDirectoryStrict(string oldPath, string newPath, string folderDescription)
        {
            if (!Directory.Exists(oldPath))
                return;

            try
            {
                if (Directory.Exists(newPath))
                {
                    throw new IOException(string.Format(GlobalConfig.Localizer.GetString("Rename_TargetExists"), newPath));
                }

                Directory.Move(oldPath, newPath);
            }
            catch (IOException ex)
            {
                string msgBusy = string.Format(GlobalConfig.Localizer.GetString("Rename_Busy_Message"), folderDescription, oldPath);
                MessageBox.Show(msgBusy, GlobalConfig.Localizer.GetString("Title_OperationFailed", "操作失败"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Warn(string.Format(GlobalConfig.Localizer.GetString("Rename_Busy_Blocked"), oldPath));
                throw new InvalidOperationException(string.Format(GlobalConfig.Localizer.GetString("Folder_Busy"), folderDescription), ex);
            }
            catch (Exception ex)
            {
                string msgUnknown = string.Format(GlobalConfig.Localizer.GetString("Rename_UnknownError"), folderDescription, ex.Message);
                MessageBox.Show(msgUnknown, GlobalConfig.Localizer.GetString("Title_SystemError", "系统错误"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(msgUnknown, ex);
                throw;
            }
        }

        /// <summary>
        /// 添加新任务并初始化对应文件结构。
        /// </summary>
        /// <param name="name">新任务的名称。</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void AddTask(string name)
        {
            var project = ServiceLocator.ProjectController.CurrentProject;
            if (project == null)
                throw new InvalidOperationException(_.GetString("TaskController_NoOpenProject_CannotAddTask"));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(_.GetString("TaskController_TaskNameEmpty"), nameof(name));
            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException(string.Format(_.GetString("TaskController_TaskNameInvalidChars"), name), nameof(name));

            var newTask = TaskModel.Create(name);
            try
            {
                var projectDir = Path.GetDirectoryName(project.Path) ?? "";
                var tasksRoot = Path.Combine(projectDir, "Tasks");
                var taskFolderPath = Path.Combine(tasksRoot, newTask.Name);
                Directory.CreateDirectory(taskFolderPath);
                var toolBlocksPath = Path.Combine(taskFolderPath, "ToolBlocks");
                Directory.CreateDirectory(toolBlocksPath);
            }
            catch (Exception ex)
            {
                var msg = string.Format(GlobalConfig.Localizer.GetString("TaskController_AddTask_CreateTaskDirFailed"), newTask.Name, ex.Message);
                throw new IOException(msg, ex);
            }
            project.TaskGroup.Add(newTask);
            if(project.CurrentTask == null)
            {
                project.CurrentTask = newTask;
            }

            RaiseTasksChanged();
            ResubscribeDisplayControllerEvents();
        }


        /// <summary>
        /// 重新订阅显示控制器事件以刷新显示状态。
        /// </summary>
        public void ResubscribeDisplayControllerEvents()
        {
            try
            {
                var displayController = DisplayController.Instance;
                if (displayController != null)
                {
                    var subscribeMethod = typeof(DisplayController).GetMethod("SubscribeToEvents",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (subscribeMethod != null)
                    {
                        subscribeMethod.Invoke(displayController, null);
                    }
                    else
                    {
                        displayController.TaskNumChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(GlobalConfig.Localizer.GetString("DisplayController_ResubscribeFailed", "重新订阅显示控制器事件失败"), ex);
            }
        }


        /// <summary>
        /// 获取任务配置的单次采集批量数量。
        /// </summary>
        /// <param name="task">需要查询采集批量的任务。</param>
        /// <returns>采集节点相机配置的批量大小。</returns>
        public int GetAcqCameraBatchSize(TaskModel task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), _.GetString("TaskController_TaskCannotBeNull"));
            if (task.AcquireNode == null || task.AcquireNode.Camera == null)
                throw new InvalidOperationException(string.Format(_.GetString("TaskController_CameraConfigMissing"), task.Name));

            return task.AcquireNode.Camera.BatchSize;
        }
        #endregion

        #region 任务运行（内部任务：UI/循环/任务组）
        /// <summary>
        /// 执行任务流程，并在异常时记录日志后向上抛出。
        /// </summary>
        /// <param name="task">待运行的任务模型。</param>
        private void RunTask(TaskModel task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), _.GetString("TaskController_TaskCannotBeNull"));

            try
            {
                if (string.IsNullOrEmpty(task.SN))
                    task.SN = null;
                task.Run();
            }
            catch (Exception ex)
            {
                Log.Error($"任务{task.Name}执行失败：{ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// UI触发选中任务（内部任务，影响运行状态）
        /// </summary>
        public async Task RunAsyncTask(TaskModel task, CancellationToken cancellationToken = default)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task), _.GetString("TaskController_TaskCannotBeNull"));

            // 内部任务检查：阻止与外部任务并行
            if (!CheckInternalTaskCanRun("UI"))
                throw new InvalidOperationException(_.GetString("TaskController_TaskRunning_CannotTrigger"));

            var taskLock = GetOrCreateTaskLock(task);
            bool isCountIncremented = false;
            try
            {
                await taskLock.WaitAsync(cancellationToken);
                IncrementRunningTaskCount();
                isCountIncremented = true;

                await Task.Run(() => RunTask(task), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Log.Info(string.Format(_.GetString("TaskController_TaskCancelled"), task.Name));
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0}: {1}", _.GetString("TaskController_AsyncTaskExecutionFailed"), ex.Message), ex);
                throw;
            }
            finally
            {
                if (isCountIncremented)
                    DecrementRunningTaskCount();
                if (taskLock.CurrentCount == 0)
                    taskLock.Release();
            }
        }

        /// <summary>
        /// 异步运行任务组（内部任务，支持组内多任务并行）
        /// </summary>
        public async Task RunTaskGroupAsync(CancellationToken cancellationToken)
        {
            var project = ServiceLocator.ProjectController.CurrentProject;
            if (project == null)
                throw new InvalidOperationException(_.GetString("TaskController_NoOpenProject_CannotRunTaskGroup"));
            if (project.TaskGroup == null || !project.TaskGroup.Any())
                throw new InvalidOperationException(_.GetString("TaskController_TaskGroupEmpty"));

            // 内部任务检查：阻止与外部任务并行
            if (!CheckInternalTaskCanRun("任务组"))
                throw new InvalidOperationException(_.GetString("TaskController_TaskRunning_CannotRunGroup"));

            // 任务组内并行度：CPU核心数（可根据需求调整）
            int maxGroupConcurrency = Environment.ProcessorCount;
            using (var groupSemaphore = new SemaphoreSlim(maxGroupConcurrency, maxGroupConcurrency))
            {
                var taskTasks = project.TaskGroup.Select(async task =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await groupSemaphore.WaitAsync(cancellationToken);
                    try
                    {
                        var taskLock = GetOrCreateTaskLock(task);
                        await taskLock.WaitAsync(cancellationToken);
                        try
                        {
                            await ExecuteSingleTaskAsync(task, cancellationToken);
                        }
                        finally
                        {
                            taskLock.Release();
                        }
                    }
                    finally
                    {
                        groupSemaphore.Release();
                    }
                });

                var allTasks = Task.WhenAll(taskTasks);
                var cancellationTask = Task.Delay(Timeout.Infinite, cancellationToken);
                var completedTask = await Task.WhenAny(allTasks, cancellationTask);
                if (completedTask == cancellationTask)
                    cancellationToken.ThrowIfCancellationRequested();

                await allTasks;
            }
        }

        /// <summary>
        /// 异步执行单个内部任务
        /// </summary>
        private async Task ExecuteSingleTaskAsync(TaskModel task, CancellationToken cancellationToken)
        {
            if (task == null)
                throw new InvalidOperationException(_.GetString("TaskController_AttemptToExecuteNullTask"));

            bool isCountIncremented = false;
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                IncrementRunningTaskCount();
                isCountIncremented = true;

                await Task.Run(() =>
                {
                    task.SN = null;
                    task.Run();
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Log.Info(string.Format("{0}: {1}", _.GetString("TaskController_TaskCancelled"), task.Name));
                throw;
            }
            catch (Exception ex)
            {
                string taskName = task.Name ?? _.GetString("TaskController_UnknownTaskName");
                Log.Error(string.Format("{0}: {1}",
                    string.Format(_.GetString("TaskController_TaskExecutionFailed"), taskName),
                    ex.Message));
                throw;
            }
            finally
            {
                if (isCountIncremented)
                    DecrementRunningTaskCount();
            }
        }

        /// <summary>
        /// 开始循环（内部任务）
        /// </summary>
        public void StartLoopRun()
        {
            lock (_taskRunLock)
            {
                if (!CheckInternalTaskCanRun("循环"))
                {
                    Log.Warn(_.GetString("TaskController_TaskRunning_CannotStartLoop"));
                    return;
                }

                if (_isLoopMode)
                {
                    Log.Warn(_.GetString("TaskController_LoopAlreadyRunning"));
                    return;
                }

                _isLoopMode = true;
                _taskCancellationTokenSource?.Dispose();
                _taskCancellationTokenSource = new CancellationTokenSource();

                if (!_lastIsTaskRunning)
                {
                    _lastIsTaskRunning = true;
                    TaskRunningStateChanged?.Invoke(true);
                }

                Task.Run(() => RunTaskGroupLoopAsync(_taskCancellationTokenSource.Token))
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Log.Error(string.Format("{0}: {1}",
                                _.GetString("TaskController_LoopOperationAbnormal"),
                                t.Exception?.GetBaseException().Message));
                        }
                    });
            }
        }

        /// <summary>
        /// 停止循环
        /// </summary>
        public void StopLoopRun()
        {
            if (Monitor.TryEnter(_taskRunLock, 1000))
            {
                try
                {
                    if (!_isLoopMode)
                    {
                        Log.Warn(_.GetString("TaskController_LoopNotRunning"));
                        return;
                    }

                    _taskCancellationTokenSource?.Cancel();
                    Log.Info(_.GetString("TaskController_LoopStopping_WaitTasksComplete"));
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("{0}: {1}", _.GetString("TaskController_StopLoopException"), ex.Message));
                }
                finally
                {
                    Monitor.Exit(_taskRunLock);
                }
            }
            else
            {
                Log.Warn(_.GetString("TaskController_AcquireTaskRunLockTimeout"));
                _taskCancellationTokenSource?.Cancel();
                Log.Info(_.GetString("TaskController_LoopStopping_WaitTasksComplete"));
            }
        }

        /// <summary>
        /// 循环运行任务组
        /// </summary>
        private async Task RunTaskGroupLoopAsync(CancellationToken cancellationToken)
        {
            Log.Info(_.GetString("TaskController_StartLoopRunTaskGroup"));

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        await RunTaskGroupAsync(cancellationToken);
                    }
                    catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                    {
                        Log.Info(_.GetString("TaskController_LoopRunCancelledNormally"));
                        break;
                    }
                    catch (Exception ex)
                    {
                        lock (_taskRunLock)
                        {
                            _isLoopMode = false;
                        }
                        Log.Warn(string.Format("{0}:{1}", _.GetString("TaskController_LoopAbnormalTermination"), ex.Message));
                        MessageBox.Show(string.Format("{0}:{1}", _.GetString("TaskController_LoopAbnormalTermination"), ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            }
            finally
            {
                while (Volatile.Read(ref _runningTaskCount) > 0)
                {
                    await Task.Delay(100);
                }

                lock (_taskRunLock)
                {
                    _isLoopMode = false;
                }

                UpdateTaskRunningState();
                Log.Info(_.GetString("TaskController_LoopRunStopped_AllTasksComplete"));
            }
        }
        #endregion

        #region 外部触发逻辑
        private readonly ConcurrentDictionary<string, TaskClientEventHandlers> _taskClientEventMap = new ConcurrentDictionary<string, TaskClientEventHandlers>();

        private class TaskClientEventHandlers
        {
            public string TaskGuid { get; set; }
            public string ClientKey { get; set; }
            public TcpClient Client { get; set; }
            public EventHandler<TaskArgEvent> AcquireHandler { get; set; }
            public EventHandler<TaskArgEvent> CalibrationHandler { get; set; }
            public EventHandler<TaskArgEvent> InspectionHandler { get; set; }
        }

        public void ReciveDataHandler(object sender, TcpDataReceivedEventArgs e)
        {
            if (!ServiceLocator.TcpCommunicationController.IsListening)
            {
                SendResponseToClient(e.Client, _.GetString("TaskController_TcpNotListening"));
                return;
            }

            string clientKey = e.ClientEndPoint.ToString();
            string rawData = e.Data ?? string.Empty;

            CommandParseResult parseResult = CommandParser.ParseFullCommand(rawData, clientKey);

            // 根据解析结果处理响应
            if (parseResult.IsOnlySNBinding)
            {
                SendResponseToClient(e.Client, parseResult.ErrorMessage);
                return;
            }
            if (!parseResult.IsValid)
            {
                SendResponseToClient(e.Client, parseResult.ErrorMessage);
                return;
            }

            int taskNumber = parseResult.TaskNumber;
            string validSN = parseResult.ValidSN;
            string command = parseResult.Command;

            var project = ServiceLocator.ProjectController.CurrentProject;
            if (project == null || project.TaskGroup == null || taskNumber < 1 || taskNumber > project.TaskGroup.Count)
            {
                SendResponseToClient(e.Client, _.GetString("TaskController_TaskNumberNotFound"));
                return;
            }

            var task = project.TaskGroup[taskNumber - 1];
            if (task == null)
            {
                SendResponseToClient(e.Client, _.GetString("TaskController_TaskNumberNotFound"));
                return;
            }
            task.SN = validSN;

            HandleTaskCommand(task, parseResult, e.Client, clientKey);
        }

        /// <summary>
        /// 指令处理
        /// </summary>
        private async void HandleTaskCommand(TaskModel task, CommandParseResult result, TcpClient client, string clientKey)
        {
            switch (result.Command)
            {
                case "R":
                    await RunAsyncTaskAndNotify(task, client, clientKey);
                    break;
                case "JC":
                    ChangeCurrentTask(task, client, clientKey);
                    break;
                default:
                    SendResponseToClient(client, _.GetString("TaskController_InvalidCommandReceived"));
                    break;
            }
        }

        /// <summary>
        /// 将指定任务设置为当前任务并回复客户端。
        /// </summary>
        /// <param name="task">需要切换到的任务实例。</param>
        /// <param name="client">TCP 客户端连接。</param>
        /// <param name="clientKey">客户端唯一标识。</param>
        private void ChangeCurrentTask(TaskModel task, TcpClient client, string clientKey)
        {
            GlobalConfig.Instance.CurrentProject.CurrentTask = task;
            RaiseTasksChanged();
            SendResponseToClient(client, "ChangeSuccess!");

        }

        /// <summary>
        /// 构建用于 TCP 返回的任务执行响应消息。
        /// </summary>
        /// <param name="_task">关联的任务实例。</param>
        /// <param name="_status">任务状态描述。</param>
        /// <param name="_step">当前执行步骤。</param>
        /// <param name="_message">附加提示信息。</param>
        /// <returns>包含任务状态信息的 JSON 对象。</returns>
        private JObject GreateResponseMessage(TaskModel _task, string _status, string _step, string _message)
        {
            var response = new JObject
            {
                ["Task"] = _task.Name,
                ["Status"] = _status,
                ["Step"] = _step,
                ["Message"] = _message,
                ["Time"] = DateTime.Now
            };
            return response;
        }

        /// <summary>
        /// 外部TCP触发任务（支持不同任务并行，同一任务排队）
        /// </summary>
        private async Task RunAsyncTaskAndNotify(TaskModel task, TcpClient client, string clientKey)
        {
            if (task == null || client == null)
                return;

            // 外部任务检查：同一任务排队，不同任务并行，阻止内部任务运行时触发
            if (!CheckExternalTaskCanRun(task, clientKey, out string errorMsg))
            {
                SendResponseToClient(client, errorMsg);
                return;
            }

            var taskLock = GetOrCreateTaskLock(task);
            TaskClientEventHandlers handlers = null;
            string mapKey = $"{task.Guid}_{clientKey}";

            try
            {
                // 同一任务排队（不同任务不会进入此等待）
                await taskLock.WaitAsync(CancellationToken.None);

                handlers = BindTaskNodeEvents(task, client, clientKey, mapKey);

                // 执行外部任务（不影响内部运行状态）
                await Task.Run(() => task.Run());

                var successMsg = GreateResponseMessage(
                    task,
                    task.Status.ToString(),
                    "end",
                    string.Format(_.GetString("TaskController_TaskRunSuccess"), task.Name)).ToString();
                SendResponseToClient(client, successMsg);
                Log.Info($"客户端[{clientKey}]触发任务[{task.Name}]执行成功");
            }
            catch (OperationCanceledException)
            {
                var cancelMsg = GreateResponseMessage(
                    task,
                    task.Status.ToString(),
                    "end",
                    _.GetString("TaskController_TaskCancelled")).ToString();
                SendResponseToClient(client, cancelMsg);
                Log.Info($"客户端[{clientKey}]触发任务[{task.Name}]被取消");
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0}（客户端[{1}]）: {2}", _.GetString("TaskController_TaskRunFailed_External"), clientKey, ex.Message), ex);
                var failMsg = GreateResponseMessage(
                    task,
                    task.Status.ToString(),
                    "end",
                    string.Format(_.GetString("TaskController_TaskRunFailed"), task.Name)).ToString();
                SendResponseToClient(client, failMsg);
            }
            finally
            {
                UnbindTaskNodeEvents(task, handlers, mapKey);
                if (taskLock.CurrentCount == 0)
                    taskLock.Release(); // 释放当前任务锁，允许下一个排队任务执行
            }
        }

        /// <summary>
        /// 绑定任务节点完成事件（仅当前客户端）
        /// </summary>
        /// <param name="task">需要绑定事件的任务实例。</param>
        /// <param name="client">负责接收通知的客户端。</param>
        /// <param name="clientKey">客户端唯一标识。</param>
        /// <param name="mapKey">用于事件映射字典的键。</param>
        /// <returns>封装了事件处理器的结构体。</returns>
        private TaskClientEventHandlers BindTaskNodeEvents(TaskModel task, TcpClient client, string clientKey, string mapKey)
        {
            var handlers = new TaskClientEventHandlers
            {
                TaskGuid = task.Guid,
                ClientKey = clientKey,
                Client = client,
                AcquireHandler = (o, t) => {
                    if ((ServiceLocator.ProjectController.CurrentProject.CurrentResponseType & ResponseType.AfterAcquireNode) != 0)
                        SendResponseToClient(client, GreateResponseMessage(t.Task, "OK", ((INode)o).NodeName, string.Format(_.GetString("TaskController_AcquireNodeCompleted"), t.Task.Name)).ToString());
                },
                CalibrationHandler = (o, t) =>
                {
                    if ((ServiceLocator.ProjectController.CurrentProject.CurrentResponseType & ResponseType.AfterCalibrationNode) != 0)
                        SendResponseToClient(client, GreateResponseMessage(t.Task, "OK", ((INode)o).NodeName, string.Format(_.GetString("TaskController_CalibrationNodeCompleted"), t.Task.Name)).ToString());
                },
                InspectionHandler = (o, t) =>
                {
                    if ((ServiceLocator.ProjectController.CurrentProject.CurrentResponseType & ResponseType.AfterInspectionNode) != 0)
                        SendResponseToClient(client, GreateResponseMessage(t.Task, "OK", ((INode)o).NodeName, string.Format(_.GetString("TaskController_InspectionNodeCompleted"), t.Task.Name)).ToString());
                },
            };

            task.AcquireNode.NodeAfterEvent += handlers.AcquireHandler;
            task.CalibrationNode.NodeAfterEvent += handlers.CalibrationHandler;
            task.InspectionNode.NodeAfterEvent += handlers.InspectionHandler;

            _taskClientEventMap.TryAdd(mapKey, handlers);

            return handlers;
        }
        /// <summary>
        /// 解绑任务节点事件并清理客户端映射。
        /// </summary>
        /// <param name="task">需要解绑事件的任务实例。</param>
        /// <param name="handlers">绑定时创建的事件处理器集合。</param>
        /// <param name="mapKey">事件映射表使用的键。</param>
        private void UnbindTaskNodeEvents(TaskModel task, TaskClientEventHandlers handlers, string mapKey)
        {
            if (handlers == null || task == null)
                return;

            task.AcquireNode.NodeAfterEvent -= handlers.AcquireHandler;
            task.CalibrationNode.NodeAfterEvent -= handlers.CalibrationHandler;
            task.InspectionNode.NodeAfterEvent -= handlers.InspectionHandler;

            _taskClientEventMap.TryRemove(mapKey, out var _);
        }
        /// <summary>
        /// 解绑客户端关联 事件
        /// </summary>
        /// <param name="clientKey"></param>
        public void UnbindClientRelatedEvents(string clientKey)
        {
            if (string.IsNullOrWhiteSpace(clientKey))
                return;

            var keysToRemove = _taskClientEventMap.Keys
                .Where(key => key.EndsWith($"_{clientKey}"))
                .ToList();

            foreach (var key in keysToRemove)
            {
                if (_taskClientEventMap.TryRemove(key, out var handlers))
                {
                    if (handlers.TaskGuid != null)
                    {
                        var project = ServiceLocator.ProjectController.CurrentProject;
                        var task = project?.TaskGroup?.FirstOrDefault(t => t.Guid == handlers.TaskGuid);
                        if (task != null)
                        {
                            task.AcquireNode.NodeAfterEvent -= handlers.AcquireHandler;
                            task.CalibrationNode.NodeAfterEvent -= handlers.CalibrationHandler;
                            task.InspectionNode.NodeAfterEvent -= handlers.InspectionHandler;
                        }
                    }

                    handlers.Client?.Dispose();
                }
            }
        }
        /// <summary>
        /// 发送回应给客户端
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private async void SendResponseToClient(TcpClient client, string message)
        {
            try
            {
                if (!client.Connected)
                {
                    return;
                }
                string clientKey = client.Client.RemoteEndPoint.ToString();
                await ServiceLocator.TcpCommunicationController.SendToClientAsync(clientKey, message);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0}: {1}, {2}",
                    _.GetString("TaskController_SendResponseFailed"),
                    message,
                    ex.Message), ex);
            }
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _taskCancellationTokenSource?.Cancel();

            foreach (var semaphore in _taskLocks.Values)
            {
                try
                {
                    semaphore.Release();
                }
                catch { }
                semaphore.Dispose();
            }
            _taskLocks.Clear();

            foreach (var handler in _taskClientEventMap.Values)
            {
                if (handler.TaskGuid != null)
                {
                    var project = ServiceLocator.ProjectController.CurrentProject;
                    var task = project?.TaskGroup?.FirstOrDefault(t => t.Guid == handler.TaskGuid);
                    if (task != null)
                    {
                        task.AcquireNode.NodeAfterEvent -= handler.AcquireHandler;
                        task.CalibrationNode.NodeAfterEvent -= handler.CalibrationHandler;
                        task.InspectionNode.NodeAfterEvent -= handler.InspectionHandler;
                    }
                }
                handler.Client?.Dispose();
            }
            _taskClientEventMap.Clear();

            lock (_taskRunLock)
            {
                _isLoopMode = false;
                Interlocked.Exchange(ref _runningTaskCount, 0);
            }

            _taskCancellationTokenSource?.Dispose();
        }
    }
}
