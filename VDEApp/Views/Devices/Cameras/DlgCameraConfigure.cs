using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Insnex.Vision2D.ToolBlock;
using Insnex.Vision2D.Controls;

using VDEApp.Commons;
using VDEApp.Infrastructure;
using VDEApp.Controllers;
using VDEApp.Devices;
using VDEApp.Devices.Cameras;
using VDEApp.LogModule;
using static VDEApp.Program;
using VDEApp.Configs;

namespace VDEApp.Views.Devices.Cameras
{

    internal delegate void MenuItemTriggered(AntdUI.MenuItem item, AntdUI.MenuSelectEventArgs args);
    internal struct MenuItemInfo
    {
        public string LocalizationKey;
        public MenuItemTriggered Handler;
    }

    public partial class DlgCameraConfigure : AntdUI.Window, ILocalizableForm
    {
        private ICamera _camera;
        private InsRecordDisplayControl _display;
        private InsToolBlock _toolBlock;
        private Dictionary<string, MenuItemInfo> _menuHandlers;
        private UserControl _cameraConfigureControl;

        public DlgCameraConfigure(ICamera camera)
        {
            _camera = camera;
            _toolBlock = new InsToolBlock();
            _cameraConfigureControl = ServiceLocator.DeviceController.CameraModels[_camera.GetType()]
                .CreateConfigureControl(camera);

            InitializeComponent();
            InitializeUI();
            RefreshLanguage();
        }

        public void InitializeUI()
        {
            this.header.ShowButton = true;

            _cameraConfigureControl.Dock = DockStyle.Fill;
            tlLeft.Controls.Add(_cameraConfigureControl, 1, 0);

            _menuHandlers = new Dictionary<string, MenuItemInfo>() {
                { "loadconfig",   new MenuItemInfo(){ LocalizationKey="dlg_cam_conf_menu_loadconfig",   Handler=this.loadConfig   } },
                { "exportconfig", new MenuItemInfo(){ LocalizationKey="dlg_cam_conf_menu_exportconfig", Handler=this.exportConfig } }
            };

            this.RefreshTitleBar();
            menu.SelectChanged += Menu_SelectChanged;

            _display = new InsRecordDisplayControl();
            _display.Dock = DockStyle.Fill;
            gbDisplay.Controls.Add(_display);

            this.AcceptButton = this.btnOK;
            this.btnOK.DialogResult = DialogResult.OK;

            if (_camera is InsVirtualCamera)
            {
                this.btnConnect.Enabled = false;
                this.btnDisconnect.Enabled = false;
                this.btnCapture.Enabled = true;
            }
            else
            {
                this.btnConnect.Enabled = !_camera.IsConnected;
                this.btnDisconnect.Enabled = !btnConnect.Enabled;
                this.btnCapture.Enabled = btnDisconnect.Enabled;
            }
        }
        private void RefreshTitleBar()
        {
            menu.Items.Clear();
            var menuItemConfig = new AntdUI.MenuItem() {
                Text = GlobalConfig.Instance.GlobalLocalizer.GetString("dlg_cam_conf_menu_config", "Config")
            };
            foreach (var item in _menuHandlers)
            {
                menuItemConfig.Sub.Add(new AntdUI.MenuItem() {
                    Text = GlobalConfig.Instance.GlobalLocalizer.GetString(item.Value.LocalizationKey, item.Value.LocalizationKey),
                    Tag = item.Key
                });
            }
            menu.Items.Add(menuItemConfig);
        }
        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }

        private void loadConfig(AntdUI.MenuItem item, AntdUI.MenuSelectEventArgs args)
        {
            var dlg = new OpenFileDialog();
            dlg.DefaultExt = _camera.ConfigFileExtension;
            dlg.Filter = "Config File" + $"|*{_camera.ConfigFileExtension}";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            try {
                _camera.LoadConfig(dlg.FileName);
            } catch (Exception ex) {

            }
        }
        private void exportConfig(AntdUI.MenuItem item, AntdUI.MenuSelectEventArgs args)
        {
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = _camera.ConfigFileExtension;
            dlg.Filter = "Config File" + $"|*{_camera.ConfigFileExtension}";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _camera.ExportConfig(dlg.FileName);
            }
        }

        private void Menu_SelectChanged(object sender, AntdUI.MenuSelectEventArgs e)
        {
            var subitem = e.Value as AntdUI.MenuItem;
            if (subitem == null) return;

            var tag = subitem.Tag as string;
            if (tag == null) return;

            if (!_menuHandlers.ContainsKey(tag)) return;

            _menuHandlers[tag].Handler.Invoke(subitem, e);
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            var acquired_images = new List<VDEApp.Devices.Datas.AcquiredImageInfo>();
            try {
                acquired_images = _camera.AcquireImageOnce();
                if (acquired_images.Count == 0)
                {
                    MessageBox.Show(
                        // No image was captured, check camera config!
                        GlobalConfig.Instance.GlobalLocalizer.GetString("msg_warning_no_image_captured"),
                        GlobalConfig.Instance.GlobalLocalizer.GetString("Warning", "Warning"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning
                    );
                    LogModule.Log.Warn(
                        // Camera "{_camera.Name}": No image was capture, there may be problems with camera config
                        string.Format(GlobalConfig.Instance.GlobalLocalizer.GetString("log_warning_no_image_captured"), _camera.Name)
                    );
                    return;
                }
            } catch (Exception ex) {
                MessageBox.Show(
                    // Image capure failed: {ex.Message}
                    string.Format(
                         GlobalConfig.Localizer.GetString("msg_error_image_capture_failed"),
                         ex.Message
                    ),
                    GlobalConfig.Localizer.GetString("Warning", "Warning"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                LogModule.Log.Warn(
                    // Camera "{_camera.Name}": Image capture failed: {ex.Message}
                    string.Format(
                        GlobalConfig.Localizer.GetString("log_error_image_capture_failed"),
                        _camera.Name,
                        ex.Message
                    )
                );
                return;
            }
            _toolBlock.Run();
            var record = _toolBlock.CreateLastRunRecord();

            int idx = 0;
            foreach (var img in acquired_images)
            {
                record.AddSubRecord(
                    $"captured_image_{idx++}",
                    img.Image.GetType(), false,	img.Image,
                    ""
                );
            }
            _display.Record = record;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try {
                _camera.ConnectCamera();
                btnDisconnect.Enabled = true;
            } catch (Exception ex) {
                Log.Error(GlobalConfig.Instance.GlobalLocalizer.GetString(ex.Message));
                btnConnect.Enabled = false;
            }
            btnCapture.Enabled = btnDisconnect.Enabled;
            btnConnect.Enabled = !btnDisconnect.Enabled;

            if (!btnDisconnect.Enabled)
                MessageBox.Show(
                    // Camera "{_camera.Name}": Failed to connect to camera
                    string.Format(GlobalConfig.Instance.GlobalLocalizer.GetString("msg_failed_connect_camera"), _camera.Name),
                    GlobalConfig.Instance.GlobalLocalizer.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _camera.CloseCamera();
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = !btnConnect.Enabled;
            btnCapture.Enabled = btnDisconnect.Enabled;
        }
    }
}
