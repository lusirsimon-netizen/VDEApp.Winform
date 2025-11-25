using AntdUI;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Models;
using VDEApp.Models.Product;
using VDEApp.Models.TaskNodes;
using VDEApp.Views.ReplayImages;
using VDEApp.Views.Task;
using VDEApp.Views.Tools;

namespace VDEApp.Views.Main
{
    public partial class UCToolBar : UserControl, ILocalizableForm, IMainLoader
    {
        public UCToolBar()
        {
            InitializeComponent();

            InitializeUI();

            RefreshLanguage();
        }
        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;

        private void OnTasksChangedFromController()
        {
            if (IsDisposed)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnTasksChangedFromController));
                return;
            }
            RefreshTask();
        }

        /// <summary>
        /// Initializes the user interface components and prepares the UI for interaction.
        /// </summary>
        public void InitializeUI()
        {
            try
            {
                btnTask.IconSvg = Properties.Resources.icon_projectmanager;
                btnCamera.IconSvg = Properties.Resources.icon_camera;
                btnCalibration.IconSvg = Properties.Resources.icon_calibration;
                btnInspection.IconSvg = Properties.Resources.icon_inspection;
                btnCommunication.IconSvg = Properties.Resources.icon_communication;
                btnReplayImage.IconSvg = Properties.Resources.icon_replayimage;
                btnPlay.IconSvg = Properties.Resources.icon_play;
                btnPlay.ToggleIconSvg = Properties.Resources.icon_pause;
                btnOneceRun.IconSvg = Properties.Resources.icon_onece;
                btnLoopRun.IconSvg = Properties.Resources.icon_looprun;

                var buttons = new List<AntdUI.Button>() { btnTask, btnCalibration, btnCamera, btnInspection, btnCommunication, btnReplayImage, btnPlay, btnOneceRun, btnLoopRun };
                foreach (var item in buttons)
                {
                    item.MouseMove -= BtnTask_MouseMove;
                    item.MouseMove += BtnTask_MouseMove;
                    item.MouseLeave -= Item_MouseLeave;
                    item.MouseLeave += Item_MouseLeave;
                }
                AppModuleSingleton.ProductionRecord.RefreshBinding();

                ServiceLocator.TaskController.TasksChanged -= OnTasksChangedFromController;
                ServiceLocator.TaskController.TasksChanged += OnTasksChangedFromController;

                ServiceLocator.TaskController.TaskRunningStateChanged -= TaskController_TaskRunningStateChanged;
                ServiceLocator.TaskController.TaskRunningStateChanged += TaskController_TaskRunningStateChanged;
                //绑定任务
                RefreshTask();
            }
            catch (Exception)
            {

            }
        }
        private void Item_MouseLeave(object sender, EventArgs e)
        {
            if (sender is AntdUI.Button button)
                toolBarTooltip.Hide(button);
        }

        private void BtnTask_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is AntdUI.Button button && button.Tag is string tooltip)
            {
                toolBarTooltip.AutoPopDelay = 5000;
                toolBarTooltip.InitialDelay = 100;
                toolBarTooltip.ReshowDelay = 500;
                toolBarTooltip.ShowAlways = true;
                toolBarTooltip.Show(tooltip, button, e.Location.X + 10, e.Location.Y + 10, 5000);
            }
        }
        public void RefreshLanguage()
        {
            btnTask.Tag = Localizer.GetString("NewProject", "Task Manage");
            btnCamera.Tag = Localizer.GetString("CameraSetting", "Camera Setting");
            btnCalibration.Tag = Localizer.GetString("CalibrationManage", "Calibration Setting");
            btnInspection.Tag = Localizer.GetString("InspectionSetting", "Inspection Setting");
            btnCommunication.Tag = Localizer.GetString("CommunicationSetting", "Communication Setting");
            btnReplayImage.Tag = Localizer.GetString("ReplayImage", "ReplayImage");
            btnPlay.Tag = Localizer.GetString("Run", "Run");
            btnOneceRun.Tag = Localizer.GetString("OneceRun", "Onece Run");
            btnLoopRun.Tag = Localizer.GetString("LoopRun", "Loop Run");
        }

        /// <summary>
        /// 刷新任务列表（下拉框）
        /// </summary>
        public void RefreshTask()
        {
            var proj = GlobalConfig.Instance?.CurrentProject;

            // 事件解绑/重绑
            cmbTask.SelectedIndexChanged -= CmbTask_SelectedIndexChanged;
            try
            {
                cmbTask.Items.Clear();
                cmbTask.SelectedIndex = -1;

                if (proj?.TaskGroup == null || proj.TaskGroup.Count == 0)
                {
                    return;
                }

                foreach (var task in proj.TaskGroup)
                {
                    cmbTask.Items.Add(new SelectItem(task.Name, task));
                }

                var currentTask = proj.CurrentTask;
                if (currentTask != null)
                {
                    for (int i = 0; i < cmbTask.Items.Count; i++)
                    {
                        if ((cmbTask.Items[i] as SelectItem)?.Tag is TaskModel itemTask && itemTask.Guid == currentTask.Guid)
                        {
                            cmbTask.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            finally
            {
                cmbTask.SelectedIndexChanged += CmbTask_SelectedIndexChanged;
            }
        }

        /// <summary>
        /// 打开任务管理窗体（模态），关闭后刷新下拉
        /// </summary>
        private WinTaskManage _winTaskManageInstance;
        private void BtnTask_Click(object sender, EventArgs e)
        {
            if (_winTaskManageInstance != null && !_winTaskManageInstance.IsDisposed)
            {
                _winTaskManageInstance.Activate();
                return;
            }
            _winTaskManageInstance = new WinTaskManage
            {
                StartPosition = FormStartPosition.CenterParent
            };

            _winTaskManageInstance.FormClosed += (s, args) => _winTaskManageInstance = null;

            _winTaskManageInstance.Show(this.FindForm());
        }

        /// <summary>
        /// 任务下拉改变：同步到 CurrentTask
        /// </summary>
        private void CmbTask_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            var proj = GlobalConfig.Instance?.CurrentProject;
            if (proj == null)
                return;

            int newIndex = e.Value;

            if (newIndex >= 0 && newIndex < cmbTask.Items.Count)
            {
                if (cmbTask.Items[newIndex] is SelectItem selectedItem)
                {
                    if (!ReferenceEquals(proj.CurrentTask, selectedItem.Tag))
                    {
                        proj.CurrentTask = selectedItem.Tag as TaskModel;
                    }
                }
            }
        }
        /// <summary>
        /// 循环运行按钮
        /// </summary> 
        private async void btnLoopRun_Click(object sender, EventArgs e)
        {
            GlobalConfig.Instance.CurrentRunType = RunType.SimulationRun;
            if (!btnPlay.Toggle)
            {
                try
                {
                    ServiceLocator.TaskController.StartLoopRun();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{Localizer.GetString("ToolBar_FailedToStartLoopOperation", "Failed to start loop operation!")}: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        /// <summary>
        /// 单次运行按钮
        /// </summary> 
        private async void btnOneceRun_Click(object sender, EventArgs e)
        {
            GlobalConfig.Instance.CurrentRunType = RunType.SimulationRun;
            if (!btnPlay.Toggle)
            {
                try
                {
                    if (GlobalConfig.Instance.CurrentProject.CurrentTask == null)
                    {
                        MessageBox.Show(Localizer.GetString("ToolBar_PleaseSelectTask", "Please select the task first before running it!"), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    await ServiceLocator.TaskController.RunAsyncTask(GlobalConfig.Instance.CurrentProject.CurrentTask);
                }
                catch (Exception ex)
                {
                    btnOneceRun.Enabled = true;
                    MessageBox.Show($"{Localizer.GetString("ToolBar_OneceRunFailed", "Single run failure!")}: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        /// <summary>
        /// Play按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (btnPlay.Toggle)
            {
                if (ServiceLocator.TaskController.IsTaskRunning)
                {

                    // 异步执行停止循环
                    System.Threading.Tasks.Task.Run(() => ServiceLocator.TaskController.StopLoopRun())
                        .ContinueWith(t =>
                        {
                            if (t.Exception != null)
                            {
                                Log.Error(Localizer.GetString("ToolBar_StopLoopRunException", "Stop loop exception!"), t.Exception);
                                // 在UI线程显示错误提示
                                Invoke(new Action(() =>
                                {
                                    MessageBox.Show($"{Localizer.GetString("ToolBar_StopLoopRunFailed", "Stop loop failed!")}：" + t.Exception.Message, "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }));
                            }
                        });
                }
                else
                {
                    ServiceLocator.TcpCommunicationController.IsListening = false;
                    Log.Info(Localizer.GetString("ToolBar_StopMonitoring", "Stop monitoring!"));
                    ChangeControllEnabled(ServiceLocator.TcpCommunicationController.IsListening);
                }
            }
            else
            {
                Log.Info("Start SaveImage！");
                ServiceLocator.TcpCommunicationController.IsListening = true;
                Log.Info(Localizer.GetString("ToolBar_StartMonitoring", "Start monitoring!"));
                GlobalConfig.Instance.CurrentRunType = RunType.Production;
                ChangeControllEnabled(ServiceLocator.TcpCommunicationController.IsListening);
            }
        }
        /// <summary>
        /// 改变按钮状态
        /// </summary>
        /// <param name="enabled"></param>
        private void ChangeControllEnabled(bool enabled)
        {
            btnLoopRun.Enabled = !enabled;
            btnOneceRun.Enabled = !enabled;
            //btnInspection.Enabled = !enabled;
            //btnCalibration.Enabled = !enabled;
            btnCamera.Enabled = !enabled;
            btnCommunication.Enabled = !enabled;
            btnTask.Enabled = !enabled;
            btnReplayImage.Enabled = !enabled;
            //cmbTask.Enabled = !enabled;
            btnPlay.Toggle = enabled;
        }
        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (!checkCurrentProject(Localizer.GetString("ToolBar_PleaseSelectedTaskOpenCamera", "Please select a task before setting up imaging!")))
                return;
            if (ServiceLocator.DeviceController.Cameras.Count == 0)
            {
                var res = MessageBox.Show(Localizer.GetString("ToolBar_PleaseConfigCamera", "There are currently no cameras configured. Do you want to jump to the camera configuration page!"), Localizer.GetString("ToolBar_ConfigCamera", "Camera not configured"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (res == DialogResult.OK)
                {
                    new Devices.Cameras.DlgCameraList()
                    {
                        Owner = AppModuleSingleton.MainFormInstance,
                        StartPosition = FormStartPosition.CenterParent
                    }.ShowDialog();
                    return;
                }
            }
            var proj = GlobalConfig.Instance?.CurrentProject;

            var winAcquire = new Task.WinAcquire(proj.CurrentTask.AcquireNode)
            {
                Owner = AppModuleSingleton.MainFormInstance,
                StartPosition = FormStartPosition.CenterParent,
            };
            if (ServiceLocator.GlobalConfig.CurrentRunType == RunType.Production)
                winAcquire.ShowDialog(this.FindForm());
            else
                winAcquire.Show(this.FindForm());
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {
            if (!checkCurrentProject(Localizer.GetString("ToolBar_PleaseSelectedTaskOpenCalibration", "Please select a task before opening the calibration window!")))
                return;
            var proj = GlobalConfig.Instance?.CurrentProject;

            var winCalibration = new Task.WinCalibration()
            {
                CalibrationNode = proj.CurrentTask.CalibrationNode,
                Owner = AppModuleSingleton.MainFormInstance,
                StartPosition = FormStartPosition.CenterParent
            };
            int original_image_count = winCalibration.CalibrationNode.InsToolBlock.Inputs.Count((t) =>
            {
                return t.ValueType == typeof(IInsImage) && t.Name.Contains("InputImage_");
            });
            for (int i = 0; i < proj.CurrentTask.AcquireNode.BatchSize; i++)
            {
                if (winCalibration.CalibrationNode.InsToolBlock.Inputs.Contains($"InputImage_{i + 1}"))
                    continue;

                winCalibration.CalibrationNode.InsToolBlock.Inputs.Add(new InsToolBlockTerminal(
                        $"InputImage_{i + 1}",
                        null,
                        typeof(IInsImage)));
            }
            for (int i = proj.CurrentTask.AcquireNode.BatchSize; i < original_image_count; i++)
            {
                winCalibration.CalibrationNode.InsToolBlock.Inputs.Remove($"InputImage_{i + 1}");
            }
            if (ServiceLocator.GlobalConfig.CurrentRunType == RunType.Production)
                winCalibration.ShowDialog(this.FindForm());
            else
                winCalibration.Show(this.FindForm());
        }

        private void btnInspection_Click(object sender, EventArgs e)
        {
            if (!checkCurrentProject(Localizer.GetString("ToolBar_PleaseSelectedTaskOpenInspection", "Please select a task before opening the inspection window!")))
                return;
            var proj = GlobalConfig.Instance?.CurrentProject;

            var winInspection = new WinInspection
            {
                InspectionNode = proj.CurrentTask.InspectionNode,
                Owner = AppModuleSingleton.MainFormInstance,
                StartPosition = FormStartPosition.CenterParent
            };
            foreach (var output in proj.CurrentTask.CalibrationNode.InsToolBlock.Outputs)
            {
                if (winInspection.InspectionNode.InsToolBlock.Inputs.Contains(output.Name))
                    winInspection.InspectionNode.InsToolBlock.Inputs[output.Name].Value = output.Value;
                else
                    winInspection.InspectionNode.InsToolBlock.Inputs.Add(output);
            }
            if (!winInspection.InspectionNode.InsToolBlock.Outputs.Contains("DefectImages"))
                winInspection.InspectionNode.InsToolBlock.Outputs.Add(new InsToolBlockTerminal("DefectImages", null, typeof(ArrayList)));
            if (!winInspection.InspectionNode.InsToolBlock.Outputs.Contains("Status"))
                winInspection.InspectionNode.InsToolBlock.Outputs.Add(new InsToolBlockTerminal("Status", null, typeof(bool)));
            //winInspection.InspectionNode.InsToolBlock.Inputs.Add(proj.CurrentTask.CalibrationNode.InsToolBlock.Inputs["parameter"]);
            if (ServiceLocator.GlobalConfig.CurrentRunType == RunType.Production)
                winInspection.ShowDialog(this.FindForm());
            else
                winInspection.Show(this.FindForm());
        }

        private void btnReplayImage_Click(object sender, EventArgs e)
        {
            var win = new WinReplay
            {
                StartPosition = FormStartPosition.CenterParent
            };
            win.Show(this.FindForm());
        }
        private void btnCommunication_Click(object sender, EventArgs e)
        {
            WinTcpCommunicationDebug wintcp = new WinTcpCommunicationDebug()
            {
                StartPosition = FormStartPosition.CenterParent
            };
            wintcp.ShowDialog(this.FindForm());
        }

        /// <summary>
        /// Check if current project is not null and create a warning notification if null
        /// </summary>
        private bool checkCurrentProject(string message)
        {
            if (GlobalConfig.Instance?.CurrentProject?.CurrentTask == null)
            {
                Notification.warn(FindForm(), Localizer.GetString("ToolBar_SelectTask", "No task selected!"), message);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 循环运行状态改变事件处理
        /// </summary>
        private void TaskController_TaskRunningStateChanged(bool isRunning)
        {
            //运行状态改变时，重新刷新UI产品信息绑定
            AppModuleSingleton.ProductionRecord.RefreshBinding();
            //ServiceLocator.DisplayController.TaskNumChanged();
            ServiceLocator.ProjectController.CurrentProject.IsRunning = isRunning;
            var action = new Action(() =>
            {
                ChangeControllEnabled(isRunning);
                if (isRunning)
                {
                    var runType = Localizer.GetString($"ToolBar_{GlobalConfig.Instance.CurrentRunType.ToString()}", GlobalConfig.Instance.CurrentRunType.ToString());
                    lblProduction.Text = runType;
                    AppModuleSingleton.StatusPrompt($"{runType}.");
                }
                else
                {
                    lblProduction.Text = Localizer.GetString($"ToolBar_{RunType.Production.ToString()}", RunType.Production.ToString());
                    AppModuleSingleton.StatusPrompt(Localizer.GetString($"ToolBar_{RunType.Ready.ToString()}", RunType.Ready.ToString()));
                }
            });
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
