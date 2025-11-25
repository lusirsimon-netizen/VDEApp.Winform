using AntdUI;
using Insnex.Vision2D;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.Controls;
using Insnex.Vision2D.ToolBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Controllers.Display;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Models;
using VDEApp.Models.Product;
using VDEApp.Models.TaskNodes;
using VDEApp.Views.Main;
using VDEApp.Configs.Display;

namespace VDEApp.Views.Display
{
    /// <summary>
    /// 产品显示绑定窗口
    /// 专门负责绑定界面的展示
    /// </summary>
    public partial class WProductDisplaybind : AntdUI.Window, ILocalizableForm
    {
        #region 事件定义

        public event EventHandler<BindingConfirmedEventArgs> BindingConfirmed;

        #endregion

        #region 成员变量

        private UCProductDisplay _parentDisplay;
        private string _currentTaskName;
        private string _currentNodeName;
        private string _currentRecordName;
        private string _currentRecordDisplayName;
        private bool _isBound = false;
        private bool _isDisplayNameEdited = false;
        private string _selectedNodeName = "";
        private string _selectedRecordName = "";
        private bool _isAutoUpdatingDisplayName = false; // 新增：标识是否是系统自动更新显示名称

        #endregion

        #region 构造函数
        public WProductDisplaybind(UCProductDisplay parentDisplay,
                                  string currentTaskName = "",
                                  string currentNodeName = "",
                                  string currentRecordName = "",
                                  string currentRecordDisplayName = "")
        {
            _parentDisplay = parentDisplay;
            _currentTaskName = currentTaskName;
            _currentNodeName = currentNodeName;
            _currentRecordName = currentRecordName;
            _currentRecordDisplayName = currentRecordDisplayName;

            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            _isBound = !string.IsNullOrEmpty(currentRecordName);
            InitializeComponent();
            InitDropdowns();
            SetDefaultValues();
            RefreshLanguage();
            if (btnRefreshRecord != null)
            {
                btnRefreshRecord.Click += BtnRefreshRecord_Click;
            }
            if (txtRecordDisplayName != null)
            {
                txtRecordDisplayName.TextChanged += TxtRecordDisplayName_TextChanged;
            }
        }

        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;

        /// <summary>
        /// 跟踪显示名称是否被用户手动修改
        /// </summary>
        private void TxtRecordDisplayName_TextChanged(object sender, EventArgs e)
        {
            // 只有当不是系统自动更新时，才标记为用户编辑
            if (!_isAutoUpdatingDisplayName)
            {
                _isDisplayNameEdited = true;
            }
        }

        /// <summary>
        /// 安全地更新显示名称（避免触发用户编辑标记）
        /// </summary>
        private void UpdateDisplayNameSafely(string newName)
        {
            try
            {
                _isAutoUpdatingDisplayName = true;
                txtRecordDisplayName.Text = newName;
            }
            finally
            {
                _isAutoUpdatingDisplayName = false;
            }
        }

        /// <summary>
        /// 运行完整的任务流程（Acquire → Calibration → Inspection）
        /// </summary>
        private async System.Threading.Tasks.Task<bool> RunCompleteTaskAsync(TaskModel task)
        {
            try
            {
                // 记录当前任务
                TaskModel originalCurrentTask = GlobalConfig.Instance.CurrentProject.CurrentTask;
                GlobalConfig.Instance.CurrentProject.CurrentTask = task;

                try
                {
                    // 刷新绑定
                    AppModuleSingleton.ProductionRecord.RefreshBinding();
                    GlobalConfig.Instance.CurrentRunType = RunType.SimulationRun;

                    // 使用任务控制器运行完整任务
                    await ServiceLocator.TaskController.RunAsyncTask(task);

                    // 检查任务是否成功完成
                    return true;
                }
                finally
                {
                    // 恢复原来的当前任务
                    GlobalConfig.Instance.CurrentProject.CurrentTask = originalCurrentTask;
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToExecuteCompleteTask", "Failed to execute complete task: {0}"), ex.Message), ex);
                throw;
            }
        }

        #endregion

        #region 初始化方法

        private void InitDropdowns()
        {
            List<string> taskNames = ServiceLocator.DisplayController.GetTaskNames();
            foreach (string taskName in taskNames)
            {
                select1.Items.Add(taskName);
            }
        }

        #endregion

        #region 默认值设置
        /// <summary>
        /// 设置默认值
        /// </summary>
        private void SetDefaultValues()
        {
            if (!string.IsNullOrEmpty(_currentTaskName) && select1.Items.Contains(_currentTaskName))
            {
                select1.Text = _currentTaskName;
            }
            else if (select1.Items.Count > 0)
            {
                select1.Text = GlobalConfig.Instance.CurrentProject.CurrentTask.Name;
            }

            // 触发任务选择改变事件，填充Tree数据
            Select1_SelectedValueChanged(select1, new ObjectNEventArgs(null));

            // 如果有当前选择的记录，尝试选中它
            if (!string.IsNullOrEmpty(_currentNodeName) && !string.IsNullOrEmpty(_currentRecordName))
            {
                SelectTreeNodeByPath(_currentNodeName, _currentRecordName);
            }

            // 设置显示名称
            if (_isBound)
            {
                // 已绑定状态：显示已保存的显示名称
                UpdateDisplayNameSafely(!string.IsNullOrEmpty(_currentRecordDisplayName)
                    ? _currentRecordDisplayName
                    : _currentRecordName);
            }
            else if (!string.IsNullOrEmpty(_selectedRecordName))
            {
                // 未绑定状态：显示选中记录的名称
                UpdateDisplayNameSafely(_selectedRecordName);
            }
        }
        #endregion


        #region 界面事件处理
        /// <summary>
        /// 根据节点名称和记录名称选择Tree节点
        /// </summary>
        private void SelectTreeNodeByPath(string nodeName, string recordName)
        {
            foreach (TreeItem nodeItem in treeNodeRecord.Items)
            {
                if (nodeItem.Text == nodeName)
                {
                    foreach (TreeItem recordItem in nodeItem.Sub)
                    {
                        if (recordItem.Text == recordName)
                        {
                            treeNodeRecord.Select(recordItem);
                            return;
                        }
                    }

                    // 如果记录不存在，选中节点
                    treeNodeRecord.Select(nodeItem);
                    return;
                }
            }
        }

        private void BtnRecordBind_Click(object sender, EventArgs e)
        {
            string taskName = select1.Text;
            string nodeName = _selectedNodeName;
            string recordName = _selectedRecordName;
            string recordDisplayName = txtRecordDisplayName.Text.Trim();

            // 验证选择
            if (string.IsNullOrEmpty(taskName))
            {
                AntdUI.Message.info(this, Localizer.GetString("Message_PleaseSelectATaskName", "Please select a task name"), null, 2);
                return;
            }

            if (string.IsNullOrEmpty(nodeName))
            {
                AntdUI.Message.info(this, Localizer.GetString("Message_PleaseSelectANode", "Please select a node"), null, 2);
                return;
            }

            if (string.IsNullOrEmpty(recordName))
            {
                AntdUI.Message.info(this, Localizer.GetString("Message_PleaseSelectARecord", "Please select a record"), null, 2);
                return;
            }

            var recordExists = _parentDisplay.GetRecordNamesByTaskAndNode(taskName, nodeName).Contains(recordName);
            if (!recordExists)
            {
                AntdUI.Message.warn(this, Localizer.GetString("Message_RecordNoLongerExists", "This record no longer exists in the node"), null, 3);
                return;
            }

            if (string.IsNullOrEmpty(recordDisplayName))
            {
                recordDisplayName = recordName;
            }

            BindingConfirmed?.Invoke(this, new BindingConfirmedEventArgs(
                taskName, nodeName, recordName, recordDisplayName));

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Select1_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            treeNodeRecord.Items.Clear(); // 清空Tree
            _isDisplayNameEdited = false; // 重置编辑状态
            _selectedNodeName = "";
            _selectedRecordName = "";

            string selectedTask = select1.Text;
            if (string.IsNullOrEmpty(selectedTask))
            {
                UpdateDisplayNameSafely(string.Empty);
                return;
            }

            // 获取节点列表
            List<string> nodeNames = ServiceLocator.DisplayController.GetNodeNames(selectedTask);

            foreach (string nodeName in nodeNames)
            {
                // 创建节点
                TreeItem nodeItem = new TreeItem(nodeName)
                {
                    CanExpand = true,
                    Expand = true, // 默认展开节点
                    Tag = new { Type = "Node", Name = nodeName }
                };

                // 获取该节点下的记录列表
                List<string> recordNames = _parentDisplay.GetRecordNamesByTaskAndNode(selectedTask, nodeName);

                foreach (string recordName in recordNames)
                {
                    // 创建记录子节点
                    TreeItem recordItem = new TreeItem(recordName)
                    {
                        CanExpand = false,
                        Tag = new { Type = "Record", NodeName = nodeName, RecordName = recordName }
                    };
                    nodeItem.Sub.Add(recordItem);
                }

                treeNodeRecord.Items.Add(nodeItem);
            }

            // 默认选中第一个节点的第一个记录（如果有）
            if (treeNodeRecord.Items.Count > 0)
            {
                TreeItem firstNode = treeNodeRecord.Items[0];
                if (firstNode.Sub.Count > 0)
                {
                    treeNodeRecord.Select(firstNode.Sub[0]);
                }
                else
                {
                    treeNodeRecord.Select(firstNode);
                    UpdateDisplayNameSafely(string.Empty);
                }
            }
            else
            {
                UpdateDisplayNameSafely(string.Empty);
            }
        }

        /// <summary>
        /// Tree控件选择改变事件
        /// </summary>
        private void TreeNodeRecord_SelectChanged(object sender, AntdUI.TreeSelectEventArgs e)
        {
            if (e.Item == null || e.Item.Tag == null)
                return;

            dynamic tag = e.Item.Tag;
            string type = tag.Type;

            if (type == "Node")
            {
                // 选中的是节点
                _selectedNodeName = tag.Name;
                _selectedRecordName = "";

                // 如果节点有子记录，自动选中第一个记录
                if (e.Item.Sub.Count > 0)
                {
                    treeNodeRecord.Select(e.Item.Sub[0]);
                    return;
                }
                else
                {
                    // 节点没有记录时清空显示名称
                    if (!_isBound && !_isDisplayNameEdited)
                    {
                        UpdateDisplayNameSafely(string.Empty);
                    }
                }
            }
            else if (type == "Record")
            {
                // 选中的是记录
                _selectedNodeName = tag.NodeName;
                _selectedRecordName = tag.RecordName;

                // 更新显示名称
                if (!_isBound && !_isDisplayNameEdited)
                {
                    // 未绑定状态：跟随选择变换
                    UpdateDisplayNameSafely(_selectedRecordName);
                }
                else if (_isBound && string.IsNullOrEmpty(_currentRecordDisplayName) && !_isDisplayNameEdited)
                {
                    // 已绑定但没有保存的显示名称：使用记录名称
                    UpdateDisplayNameSafely(_selectedRecordName);
                }
                // 已绑定且有保存的显示名称：不自动更新
            }

            // 更新当前选择状态
            UpdateCurrentSelection();
        }

        /// <summary>
        /// 刷新记录列表按钮点击事件
        /// </summary>
        private async void BtnRefreshRecord_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedTask = select1.Text;

                if (string.IsNullOrEmpty(selectedTask))
                {
                    AntdUI.Message.warn(this, Localizer.GetString("Message_PleaseSelectATaskFirst", "Please select a task first"), null, 3);
                    return;
                }
                if (GlobalConfig.Instance?.CurrentProject == null)
                {
                    AntdUI.Message.error(this, Localizer.GetString("Message_NoProjectLoaded", "No project loaded"), null, 3);
                    return;
                }

                var taskGroup = GlobalConfig.Instance.CurrentProject.TaskGroup;
                if (taskGroup == null)
                {
                    AntdUI.Message.error(this, Localizer.GetString("Message_NoTaskGroupFound", "No task group found"), null, 3);
                    return;
                }

                var task = taskGroup.Cast<TaskModel>().FirstOrDefault(t => t.Name == selectedTask);
                if (task == null)
                {
                    AntdUI.Message.error(this, string.Format(Localizer.GetString("Message_TaskNotFound", "Task not found: {0}"), selectedTask), null, 3);
                    return;
                }

                btnRefreshRecord.Enabled = false;

                try
                {
                    bool success = await RunCompleteTaskAsync(task);

                    if (!success)
                    {
                        AntdUI.Message.error(this, Localizer.GetString("Message_TaskExecutionFailed", "Task execution failed"), null, 3);
                        return;
                    }

                    // 等待一下，让记录更新
                    await System.Threading.Tasks.Task.Delay(200);

                    // 保存当前的显示名称编辑状态
                    bool wasDisplayNameEdited = _isDisplayNameEdited;
                    string currentDisplayName = txtRecordDisplayName.Text;

                    // 重新加载Tree数据
                    Select1_SelectedValueChanged(select1, new ObjectNEventArgs(null));

                    // 恢复显示名称编辑状态
                    _isDisplayNameEdited = wasDisplayNameEdited;
                    if (wasDisplayNameEdited)
                    {
                        UpdateDisplayNameSafely(currentDisplayName);
                    }

                    AntdUI.Message.success(this, Localizer.GetString("Message_TaskCompletedSuccessfullyRecordsRefreshed", "Task completed successfully, records refreshed"), null, 2);
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format(Localizer.GetString("Message_ExceptionOccurredWhileExecutingTask", "Exception occurred while executing task: {0}"), ex.Message), ex);
                    AntdUI.Message.error(this, string.Format(Localizer.GetString("Message_TaskExecutionFailed", "Task execution failed: {0}"), ex.Message), null, 3);
                }
                finally
                {
                    // 恢复刷新按钮
                    btnRefreshRecord.Enabled = true;
                    btnRefreshRecord.Text = "↻";
                }
            }
            catch (Exception ex)
            {
                btnRefreshRecord.Enabled = true;
                btnRefreshRecord.Text = "↻";
                Log.Error(string.Format(Localizer.GetString("Message_FailedToRefreshRecordList", "Failed to refresh record list: {0}"), ex.Message), ex);
                AntdUI.Message.error(this, string.Format(Localizer.GetString("Message_FailedToRefreshRecordList", "Failed to refresh record list: {0}"), ex.Message), null, 3);
            }
        }

        #endregion

        /// <summary>
        /// 更新当前选择状态
        /// </summary>
        private void UpdateCurrentSelection()
        {
            // 可以在这里添加选择状态的UI反馈
            btnRecordBind.Enabled = !string.IsNullOrEmpty(_selectedNodeName) && !string.IsNullOrEmpty(_selectedRecordName);
        }

        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }
    }
}
