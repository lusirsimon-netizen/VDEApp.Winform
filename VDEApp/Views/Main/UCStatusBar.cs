using System.Drawing;
using System.Windows.Forms;
using VDEApp.Configs;
using VDEApp.Devices;
using VDEApp.Monitors;
using VDEApp.Monitors.Cameras;
using VDEApp.Views.Devices.Cameras;

namespace VDEApp.Views.Main
{
    public partial class UCStatusBar : UserControl, IMainLoader
    {
        public UCStatusBar()
        {
            InitializeComponent();
        }

        #region 状态栏初始化 
        /// <summary>
        /// 状态
        /// </summary>
        public ToolStripStatusLabel statusLabel, saveImageLabel;
        /// <summary>
        /// Initializes the user interface components and prepares the UI for interaction.
        /// </summary>
        public void InitializeUI()
        {
            // 左侧状态文本 
            statusLabel = new ToolStripStatusLabel();
            statusLabel.Text = GlobalConfig.Localizer.GetString("Status_Ready", "就绪.");
            statusLabel.Spring = true; // 弹簧效果，占据剩余空间
            statusLabel.TextAlign = ContentAlignment.MiddleLeft;
            statusLabel.Dock = DockStyle.Left;
            statusStrip1.Items.Add(statusLabel);

            //存图状态
            saveImageLabel = new ToolStripStatusLabel();
            saveImageLabel.Text = GlobalConfig.Localizer.GetString("SaveImage", "存图");            //存图：100
            saveImageLabel.Spring = true; // 弹簧效果，占据剩余空间
            saveImageLabel.TextAlign = ContentAlignment.MiddleRight;
            saveImageLabel.Dock = DockStyle.Left;
            statusStrip1.Items.Add(saveImageLabel);

            ToolStripSeparator separator = new ToolStripSeparator();
            separator.Alignment = ToolStripItemAlignment.Right;
            statusStrip1.Items.Add(separator);

            //相机监控
            CameraMonitor cameraMonitor = null;
            SaveImageMonitor saveimageMonitor = null;
            try
            {
                cameraMonitor = new CameraMonitor();
                cameraMonitor.CameraStatusChanged += UCStatusBar_CameraStatusChanged;
                cameraMonitor.Start();
                MonitorManager.Instance.RegisterMonitor(cameraMonitor);

                saveimageMonitor = new SaveImageMonitor();
                saveimageMonitor.SaveImageEvent += SaveimageMonitor_SaveImageEvent;
                saveimageMonitor.Start();
                MonitorManager.Instance.RegisterMonitor(saveimageMonitor);
            }
            catch (System.Exception)
            {
                cameraMonitor?.CameraStatusChanged -= UCStatusBar_CameraStatusChanged;
                saveimageMonitor?.SaveImageEvent -= SaveimageMonitor_SaveImageEvent;
            }
        }
        private void SaveimageMonitor_SaveImageEvent(object sender, SaveImageMonitorEventArgs e)
        {
            if (IsDisposed)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new System.Action<object, SaveImageMonitorEventArgs>(SaveimageMonitor_SaveImageEvent), sender, e);
                return;
            }
            SaveImageStatus(sender, e.ImageCount);
        }

        /// <summary>
        /// 相机状态
        /// </summary>  
        private void UCStatusBar_CameraStatusChanged(object sender, CameraMonitorEventArgs e)
        {
            foreach (ICamera camera in e.Cameras)
            {
                bool found = false;
                ToolStripButton deviceButton = null;
                foreach (var item_ in statusStrip1.Items)
                {
                    if (item_ is ToolStripButton item)
                    {
                        found = item.Tag is ICamera cam && cam.Guid == camera.Guid;
                        if (found)
                        {
                            deviceButton = item;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    deviceButton = new ToolStripButton();
                    deviceButton.Click += (sender, e) =>
                    {
                        ToolStripButton button = sender as ToolStripButton;
                        new DlgCameraConfigure(camera)
                        {
                            Owner = this.ParentForm,
                            StartPosition = FormStartPosition.CenterParent
                        }.ShowDialog();
                    };
                    deviceButton.Alignment = ToolStripItemAlignment.Left;
                    deviceButton.Tag = camera;
                    statusStrip1.Items.Add(deviceButton);
                }
                var statusText = GlobalConfig.Localizer.GetString(camera.IsConnected ? "Camera_Connected" : "Camera_Disconnected",camera.IsConnected ? "已连接" : "掉线");
                deviceButton.Text = $"{camera.Name} {statusText}";
            }
        }

        /// <summary>
        /// 刷新状态栏显示文本
        /// </summary>
        public void Prompt(string message)
        {
            statusLabel.Text = message;
        }

        /// <summary>
        /// 存图状态更新
        /// </summary> 
        public void SaveImageStatus(object sender, int waitSaveCount)
        {
            if (waitSaveCount > 0)
                saveImageLabel.Text = $"存图{waitSaveCount}";
            else
                saveImageLabel.Text = $"存图";
        }
        #endregion
    }
}
