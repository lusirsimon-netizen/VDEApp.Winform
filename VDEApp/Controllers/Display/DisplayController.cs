using Insnex.Vision2D;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Configs.Display;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Models;
using VDEApp.Models.Product;
using VDEApp.Models.TaskNodes;

namespace VDEApp.Controllers.Display
{
    /// <summary>
    /// 显示控制器（单例模式）
    /// 专门负责显示系统的业务逻辑
    /// </summary>
    public class DisplayController
    {
        #region 单例模式实现

        /// <summary>
        /// 单例实例
        /// </summary>
        private static volatile DisplayController _instance;

        /// <summary>
        /// 线程同步锁
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static DisplayController Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DisplayController();
                        }
                    }
                }
                return _instance;
            }
        }
        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;
        /// <summary>
        /// 私有构造函数，防止外部实例化
        /// </summary>
        public DisplayController()
        {
            _records = new Dictionary<Tuple<string, string, string>, InsRecord>();
            _bindingConfigs = new Dictionary<string, BindingConfig>();
        }

        #endregion

        #region 成员变量

        /// <summary>
        /// 记录缓存字典
        /// Key: Tuple(TaskName, NodeName, RecordName)
        /// Value: InsRecord对象
        /// </summary>
        private readonly Dictionary<Tuple<string, string, string>, InsRecord> _records;

        /// <summary>
        /// 绑定配置字典（内存缓存）
        /// Key: ControlId
        /// Value: BindingConfig对象
        /// </summary>
        private readonly Dictionary<string, BindingConfig> _bindingConfigs;

        /// <summary>
        /// 初始化状态标识
        /// </summary>
        private bool _isInitialized = false;

        /// <summary>
        /// 记录缓存清理定时器
        /// </summary>
        private System.Timers.Timer _cacheCleanupTimer;

        /// <summary>
        /// 缓存清理间隔（分钟）
        /// </summary>
        private const int CACHE_CLEANUP_INTERVAL_MINUTES = 30;

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化控制器
        /// </summary>
        public void Initialize()
        {
            if (!_isInitialized)
            {
                lock (_lock)
                {
                    if (!_isInitialized)
                    {
                        try
                        {
                            // 加载绑定配置到内存
                            LoadBindingConfigs();

                            // 订阅事件
                            SubscribeToEvents();
                            SubscribeToTaskRunEvents();

                            // 初始化缓存清理定时器
                            InitializeCacheCleanupTimer();

                            _isInitialized = true;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(Localizer.GetString("Message_DisplayControllerInitializationFailed", "DisplayController initialization failed"), ex);
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 加载绑定配置到内存
        /// </summary>
        private void LoadBindingConfigs()
        {
            try
            {
                var bindings = DisplayConfigManager.Instance.GetAllBindings();
                foreach (var binding in bindings)
                {
                    if (!_bindingConfigs.ContainsKey(binding.ControlId))
                    {
                        _bindingConfigs.Add(binding.ControlId, binding);
                    }
                    else
                    {
                        _bindingConfigs[binding.ControlId] = binding;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToLoadBindingConfigurations", "Failed to load binding configurations"), ex);
            }
        }

        /// <summary>
        /// 初始化缓存清理定时器
        /// </summary>
        private void InitializeCacheCleanupTimer()
        {
            try
            {
                _cacheCleanupTimer = new System.Timers.Timer();
                _cacheCleanupTimer.Interval = CACHE_CLEANUP_INTERVAL_MINUTES * 60 * 1000;
                _cacheCleanupTimer.Elapsed += CacheCleanupTimer_Elapsed;
                _cacheCleanupTimer.AutoReset = true;
                _cacheCleanupTimer.Start();
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToInitializeCacheCleanupTimer", "Failed to initialize cache cleanup timer"), ex);
            }
        }

        /// <summary>
        /// 缓存清理定时器事件
        /// </summary>
        private void CacheCleanupTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (_records)
                {
                    _records.Clear();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_CacheCleanupFailed", "Cache cleanup failed"), ex);
            }
        }


        #endregion

        #region 布局管理业务逻辑

        /// <summary>
        /// 获取当前布局配置
        /// </summary>
        public (int rows, int columns) GetCurrentLayout()
        {
            try
            {
                var config = DisplayConfigManager.Instance.CurrentConfig;
                return (config.LayoutRows, config.LayoutColumns);
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToGetCurrentLayoutConfiguration", "Failed to get current layout configuration"), ex);
                return (2, 2); // 默认布局
            }
        }

        /// <summary>
        /// 创建显示布局
        /// </summary>
        public void CreateLayout(int rows, int columns)
        {
            if (rows < 1 || rows > 10 || columns < 1 || columns > 10)
                throw new ArgumentOutOfRangeException(Localizer.GetString("Message_NumberOfRowsAndColumnsMustBeBetween1And10", "Number of rows and columns must be between 1 and 10"));

            try
            {
                DisplayConfigManager.Instance.UpdateLayout(rows, columns);
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToCreateDisplayLayout", "Failed to create display layout"), ex);
                throw;
            }
        }

        /// <summary>
        /// 生成布局控件ID列表
        /// </summary>
        public List<string> GenerateLayoutControlIds(int rows, int columns)
        {
            if (rows < 1 || rows > 10 || columns < 1 || columns > 10)
                throw new ArgumentOutOfRangeException(Localizer.GetString("Message_NumberOfRowsAndColumnsMustBeBetween1And10", "Number of rows and columns must be between 1 and 10"));

            var controlIds = new List<string>();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    controlIds.Add($"Child_{row}_{col}");
                }
            }
            return controlIds;
        }

        #endregion

        #region 绑定管理业务逻辑

        /// <summary>
        /// 绑定控件到记录
        /// </summary>
        public void BindControl(string controlId, string taskName, string nodeName, string recordName, string recordDisplayName = "")
        {
            if (string.IsNullOrEmpty(controlId))
                throw new ArgumentNullException(nameof(controlId), Localizer.GetString("Message_ControlIdCannotBeNull", "ControlId cannot be null"));
            if (string.IsNullOrEmpty(taskName))
                throw new ArgumentNullException(nameof(taskName), Localizer.GetString("Message_TaskNameCannotBeNull", "TaskName cannot be null"));
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException(nameof(nodeName), Localizer.GetString("Message_NodeNameCannotBeNull", "NodeName cannot be null"));
            if (string.IsNullOrEmpty(recordName))
                throw new ArgumentNullException(nameof(recordName), Localizer.GetString("Message_RecordNameCannotBeNull", "RecordName cannot be null"));

            try
            {
                var bindingConfig = new BindingConfig(taskName, nodeName, recordName, controlId, recordDisplayName);

                // 更新配置管理器
                DisplayConfigManager.Instance.AddBinding(bindingConfig);

                // 更新内存缓存
                lock (_bindingConfigs)
                {
                    if (_bindingConfigs.ContainsKey(controlId))
                        _bindingConfigs[controlId] = bindingConfig;
                    else
                        _bindingConfigs.Add(controlId, bindingConfig);
                }

                // 触发绑定更新事件
                OnBindingUpdated(controlId, taskName, nodeName, recordName, recordDisplayName);

            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToBindControl", "Failed to bind control"), ex);
                throw;
            }
        }

        /// <summary>
        /// 解绑控件
        /// </summary>
        public void UnbindControl(string controlId)
        {
            if (string.IsNullOrEmpty(controlId))
                throw new ArgumentNullException(nameof(controlId), Localizer.GetString("Message_ControlIdCannotBeNull", "ControlId cannot be null"));

            try
            {
                // 更新配置管理器
                DisplayConfigManager.Instance.RemoveBinding(controlId);

                // 更新内存缓存
                lock (_bindingConfigs)
                {
                    _bindingConfigs.Remove(controlId);
                }

                // 触发绑定更新事件
                OnBindingUpdated(controlId, string.Empty, string.Empty, string.Empty, string.Empty);

            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToUnbindControl", "Failed to unbind control"), ex);
                throw;
            }
        }

        /// <summary>
        /// 获取控件的绑定信息
        /// </summary>
        public BindingConfig GetControlBinding(string controlId)
        {
            if (string.IsNullOrEmpty(controlId))
                throw new ArgumentNullException(nameof(controlId), Localizer.GetString("Message_ControlIdCannotBeNull", "ControlId cannot be null"));

            try
            {
                // 先从内存缓存获取
                lock (_bindingConfigs)
                {
                    if (_bindingConfigs.TryGetValue(controlId, out var binding))
                    {
                        return binding;
                    }
                }

                // 内存缓存中没有，从配置管理器获取
                var config = DisplayConfigManager.Instance.GetBindingByControlId(controlId);

                // 更新内存缓存
                if (config != null)
                {
                    lock (_bindingConfigs)
                    {
                        if (!_bindingConfigs.ContainsKey(controlId))
                        {
                            _bindingConfigs.Add(controlId, config);
                        }
                    }
                }

                return config;
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToGetControlBindingInformation", "Failed to get control binding information - ControlId: {0}"), controlId), ex);
                return null;
            }
        }

        /// <summary>
        /// 获取所有绑定配置
        /// </summary>
        public List<BindingConfig> GetAllBindings()
        {
            try
            {
                lock (_bindingConfigs)
                {
                    return _bindingConfigs.Values.ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToGetAllBindingConfigurations", "Failed to get all binding configurations"), ex);
                return new List<BindingConfig>();
            }
        }

        #endregion

        #region 数据管理业务逻辑

        /// <summary>
        /// 获取记录数据
        /// </summary>
        public InsRecord GetRecord(string taskName, string nodeName, string recordName)
        {
            if (string.IsNullOrEmpty(taskName))
                throw new ArgumentNullException(nameof(taskName), Localizer.GetString("Message_TaskNameCannotBeNull", "TaskName cannot be null"));
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException(nameof(nodeName), Localizer.GetString("Message_NodeNameCannotBeNull", "NodeName cannot be null"));
            if (string.IsNullOrEmpty(recordName))
                throw new ArgumentNullException(nameof(recordName), Localizer.GetString("Message_RecordNameCannotBeNull", "RecordName cannot be null"));

            try
            {
                var key = Tuple.Create(taskName, nodeName, recordName);

                // 先从缓存获取
                lock (_records)
                {
                    if (_records.TryGetValue(key, out var cachedRecord))
                    {
                        return cachedRecord;
                    }
                }

                // 缓存中没有，从任务节点加载
                LoadRecordFromTaskNode(taskName, nodeName, recordName);

                // 再次尝试从缓存获取
                lock (_records)
                {
                    _records.TryGetValue(key, out var record);
                    return record;
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToGetRecordData", "Failed to get record data - Task: {0}, Node: {1}, Record: {2}"), taskName, nodeName, recordName), ex);
                return null;
            }
        }

        /// <summary>
        /// 从任务节点中加载记录
        /// </summary>
        private void LoadRecordFromTaskNode(string taskName, string nodeName, string recordName)
        {
            try
            {
                if (GlobalConfig.Instance?.CurrentProject?.TaskGroup == null)
                {
                    Log.Warn(Localizer.GetString("Message_NoProjectOrTaskGroupIsCurrentlyLoaded", "No project or task group is currently loaded"));
                    return;
                }

                var task = GlobalConfig.Instance.CurrentProject.TaskGroup
                    .FirstOrDefault(t => t.Name == taskName);

                if (task == null)
                {
                    return;
                }

                INode node = GetNodeFromTask(task, nodeName);
                if (node == null)
                {
                    return;
                }

                if (node.NodeRecords == null || node.NodeRecords.Count == 0)
                {
                    return;
                }

                var record = node.NodeRecords
                    .FirstOrDefault(r => r.Annotation == recordName);

                if (record != null)
                {
                    var key = Tuple.Create(taskName, nodeName, recordName);
                    lock (_records)
                    {
                        if (!_records.ContainsKey(key))
                        {
                            _records.Add(key, record);
                        }
                        else
                        {
                            _records[key] = record;
                        }
                    }
                }
                else
                {
                    Log.Warn(string.Format(Localizer.GetString("Message_RecordNotFound", "Record not found: {0}"), recordName));
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToLoadRecordFromTaskNode", "Failed to load record from task node: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 从任务中获取指定节点
        /// </summary>
        private INode GetNodeFromTask(TaskModel task, string nodeName)
        {
            switch (nodeName.ToLower())
            {
                case "acquire":
                    return task.AcquireNode;
                case "calibration":
                    return task.CalibrationNode;
                case "inspection":
                    return task.InspectionNode;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        public List<string> GetTaskNames()
        {
            try
            {
                if (GlobalConfig.Instance.CurrentProject == null)
                {
                    Log.Warn(Localizer.GetString("Message_NoProjectIsCurrentlyLoaded", "No project is currently loaded"));
                    return new List<string>();
                }

                return GlobalConfig.Instance.CurrentProject.TaskGroup
                    .Select(task => task.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToGetTaskList", "Failed to get task list"), ex);
                return new List<string>();
            }
        }

        /// <summary>
        /// 获取节点列表
        /// </summary>
        public List<string> GetNodeNames(string taskName)
        {
            if (string.IsNullOrEmpty(taskName))
                throw new ArgumentNullException(nameof(taskName), Localizer.GetString("Message_TaskNameCannotBeNull", "TaskName cannot be null"));

            return new List<string> { "Acquire", "Calibration", "Inspection" };
        }

        /// <summary>
        /// 获取记录名称列表
        /// </summary>
        public List<string> GetRecordNames(string taskName, string nodeName)
        {
            if (string.IsNullOrEmpty(taskName))
                throw new ArgumentNullException(nameof(taskName), Localizer.GetString("Message_TaskNameCannotBeNull", "TaskName cannot be null"));
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException(nameof(nodeName), Localizer.GetString("Message_NodeNameCannotBeNull", "NodeName cannot be null"));

            try
            {
                // 先从缓存获取
                var cachedRecords = _records.Keys
                    .Where(key => key.Item1 == taskName && key.Item2 == nodeName)
                    .Select(key => key.Item3)
                    .ToList();

                if (cachedRecords.Count > 0)
                {
                    return cachedRecords;
                }

                // 缓存中没有，尝试从任务节点获取
                var task = GlobalConfig.Instance?.CurrentProject?.TaskGroup
                    ?.FirstOrDefault(t => t.Name == taskName);

                if (task != null)
                {
                    var node = GetNodeFromTask(task, nodeName);
                    if (node != null && node.NodeRecords != null)
                    {
                        return node.NodeRecords
                            .Select(r => r.Annotation)
                            .ToList();
                    }
                }

                return new List<string>();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToGetRecordNameList", "Failed to get record name list - Task: {0}, Node: {1}"), taskName, nodeName), ex);
                return new List<string>();
            }
        }

        #endregion

        #region 事件订阅和处理

        /// <summary>
        /// 订阅节点事件
        /// </summary>
        private void SubscribeToEvents()
        {
            try
            {
                if (GlobalConfig.Instance == null ||
                    GlobalConfig.Instance.CurrentProject == null ||
                    GlobalConfig.Instance.CurrentProject.TaskGroup == null)
                {
                    Log.Warn(Localizer.GetString("Message_CannotSubscribeToNodeEventsProjectOrTaskGroupIsNull", "Cannot subscribe to node events, project or task group is null"));
                    return;
                }

                foreach (var task in GlobalConfig.Instance.CurrentProject.TaskGroup)
                {
                    if (task == null)
                    {
                        continue;
                    }

                    // 移除旧的事件订阅，避免重复订阅
                    if (task.AcquireNode != null)
                    {
                        task.AcquireNode.NodeAfterEvent -= OnNodeAfterEvent;
                        task.AcquireNode.NodeAfterEvent += OnNodeAfterEvent;
                    }

                    if (task.CalibrationNode != null)
                    {
                        task.CalibrationNode.NodeAfterEvent -= OnNodeAfterEvent;
                        task.CalibrationNode.NodeAfterEvent += OnNodeAfterEvent;
                    }

                    if (task.InspectionNode != null)
                    {
                        task.InspectionNode.NodeAfterEvent -= OnNodeAfterEvent;
                        task.InspectionNode.NodeAfterEvent += OnNodeAfterEvent;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToSubscribeToNodeEvents", "Failed to subscribe to node events"), ex);
                throw;
            }
        }

        /// <summary>
        /// 订阅任务运行完成事件
        /// </summary>
        private void SubscribeToTaskRunEvents()
        {
            try
            {
                foreach (var item in ServiceLocator.ProjectController.CurrentProject.TaskGroup)
                {
                    item.TaskRunAfterEvent -= OnTaskRunAfterEvent;
                    item.TaskRunAfterEvent += OnTaskRunAfterEvent;
                }

                //Log.Info(Localizer.GetString("Message_TaskRunEventSubscriptionCompleted", "Task run event subscription completed"));
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToSubscribeToTaskRunEvents", "Failed to subscribe to task run events"), ex);
            }
        }



        /// <summary>
        /// 任务数量改变
        /// </summary>
        public void TaskNumChanged()
        {
            try
            {
                // 重新订阅事件
                SubscribeToEvents();
                TaskNumberChanged?.Invoke(this, EventArgs.Empty);

            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToHandleProjectChangeEvent", "Failed to handle project change event"), ex);
            }
        }

        /// <summary>
        /// 节点事件处理 - 在更新前先清空对应节点的记录
        /// </summary>
        private void OnNodeAfterEvent(object sender, TaskArgEvent e)
        {
            try
            {
                if (e == null || sender == null)
                    return;

                string taskName = e?.Task?.Name ?? string.Empty;
                string nodeName = ((INode)sender).NodeName;

                if (string.IsNullOrEmpty(taskName) || string.IsNullOrEmpty(nodeName))
                    return;
                if (e.NodeRecords != null && e.NodeRecords.Count > 0)
                {
                    lock (_records)
                    {
                        // 先清空对应节点的所有记录
                        var keysToRemove = _records.Keys
                            .Where(key => key.Item1 == taskName && key.Item2 == nodeName)
                            .ToList();

                        foreach (var key in keysToRemove)
                        {
                            _records.Remove(key);
                        }

                        // 然后添加新的记录
                        foreach (var record in e.NodeRecords)
                        {
                            string recordName = record.Annotation;
                            var key = Tuple.Create(taskName, nodeName, recordName);

                            if (_records.ContainsKey(key))
                                _records[key] = record;
                            else
                                _records.Add(key, record);
                            //TODO:新建未绑定
                            // 触发记录更新事件
                            RecordUpdated?.Invoke(this, new RecordUpdatedEventArgs(
                                taskName, nodeName, recordName, record));
                        }
                    }


                }
                else
                {
                    // 如果没有记录，也清空对应节点的缓存
                    lock (_records)
                    {
                        var keysToRemove = _records.Keys
                            .Where(key => key.Item1 == taskName && key.Item2 == nodeName)
                            .ToList();

                        foreach (var key in keysToRemove)
                        {
                            _records.Remove(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToHandleNodeEvent", "Failed to handle node event: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 任务运行完成事件处理
        /// </summary>
        private void OnTaskRunAfterEvent(object sender, ProductionDataEvent e)
        {
            try
            {
                if (e == null)
                    return;

                // 触发任务完成事件（用于高亮未绑定控件）
                TaskCompleted?.Invoke(this, EventArgs.Empty);

                // 处理缺陷图像
                if (e.DefectImages != null && e.DefectImages.Count > 0)
                {
                    List<Image> defectImages = new List<Image>();
                    foreach (var img in e.DefectImages)
                    {
                        if (img is Image image)
                        {
                            defectImages.Add(CreateImageCopy(image));
                        }
                        else if (img is IInsImage insImage)
                        {
                            defectImages.Add(insImage.ToBitmap());
                        }
                    }

                    // 触发缺陷图像更新事件
                    DefectImagesUpdated?.Invoke(this, new DefectImagesUpdatedEventArgs(
                        defectImages, e));
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToHandleTaskRunEvent", "Failed to handle task run event"), ex);
            }
        }

        /// <summary>
        /// 触发绑定更新事件
        /// </summary>
        private void OnBindingUpdated(string controlId, string taskName, string nodeName, string recordName, string displayName)
        {
            try
            {
                BindingUpdated?.Invoke(this, new BindingUpdatedEventArgs(
                    controlId, taskName, nodeName, recordName, displayName));
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToTriggerBindingUpdateEvent", "Failed to trigger binding update event"), ex);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 创建图像的副本
        /// </summary>
        private Image CreateImageCopy(Image originalImage)
        {
            if (originalImage == null)
                return null;

            try
            {
                // 创建新的Bitmap作为副本
                Bitmap copy = new Bitmap(originalImage.Width, originalImage.Height, originalImage.PixelFormat);

                // 使用Graphics复制图像内容
                using (Graphics g = Graphics.FromImage(copy))
                {
                    g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
                }

                return copy;
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToCreateImageCopy", "Failed to create image copy"), ex);
                // 如果复制失败，返回原图像的克隆
                return originalImage.Clone() as Image;
            }
        }

        /// <summary>
        /// 检查控制器是否已初始化
        /// </summary>
        public bool IsInitialized()
        {
            return _isInitialized;
        }

        /// <summary>
        /// 重置控制器状态
        /// </summary>
        public void Reset()
        {
            lock (_lock)
            {
                try
                {
                    // 停止定时器
                    if (_cacheCleanupTimer != null)
                    {
                        _cacheCleanupTimer.Stop();
                        _cacheCleanupTimer.Dispose();
                        _cacheCleanupTimer = null;
                    }

                    // 清理缓存
                    lock (_records)
                    {
                        _records.Clear();
                    }

                    lock (_bindingConfigs)
                    {
                        _bindingConfigs.Clear();
                    }

                    // 取消事件订阅
                    UnsubscribeFromEvents();

                    _isInitialized = false;
                }
                catch (Exception ex)
                {
                    Log.Error(Localizer.GetString("Message_FailedToResetDisplayController", "Failed to reset DisplayController"), ex);
                }
            }
        }

        /// <summary>
        /// 取消事件订阅
        /// </summary>
        private void UnsubscribeFromEvents()
        {
            try
            {
                if (GlobalConfig.Instance != null &&
                    GlobalConfig.Instance.CurrentProject != null &&
                    GlobalConfig.Instance.CurrentProject.TaskGroup != null)
                {
                    foreach (var task in GlobalConfig.Instance.CurrentProject.TaskGroup)
                    {
                        if (task == null)
                            continue;

                        if (task.AcquireNode != null)
                        {
                            task.AcquireNode.NodeAfterEvent -= OnNodeAfterEvent;
                        }

                        if (task.CalibrationNode != null)
                        {
                            task.CalibrationNode.NodeAfterEvent -= OnNodeAfterEvent;
                        }

                        if (task.InspectionNode != null)
                        {
                            task.InspectionNode.NodeAfterEvent -= OnNodeAfterEvent;
                        }
                    }
                }
                foreach (var item in ServiceLocator.ProjectController.CurrentProject.TaskGroup)
                {
                    item.TaskRunAfterEvent -= OnTaskRunAfterEvent;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToUnsubscribeFromEvents", "Failed to unsubscribe from events"), ex);
            }
        }

        #endregion

        #region 事件定义

        /// <summary>
        /// 记录更新事件
        /// </summary>
        public event EventHandler<RecordUpdatedEventArgs> RecordUpdated;

        /// <summary>
        /// 缺陷图像更新事件
        /// </summary>
        public event EventHandler<DefectImagesUpdatedEventArgs> DefectImagesUpdated;

        /// <summary>
        /// 绑定更新事件
        /// </summary>
        public event EventHandler<BindingUpdatedEventArgs> BindingUpdated;

        /// <summary>
        /// 任务完成事件
        /// </summary>
        public event EventHandler TaskCompleted;

        /// <summary>
        /// 任务数量改变事件
        /// </summary>
        public event EventHandler TaskNumberChanged;

        #endregion
    }
}
