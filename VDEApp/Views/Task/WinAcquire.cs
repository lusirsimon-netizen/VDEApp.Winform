using System;
using System.Windows.Forms;

using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Devices;
using VDEApp.Infrastructure;
using VDEApp.Devices.Cameras;
using VDEApp.Models.TaskNodes;
using VDEApp.Views.Devices.Cameras;

namespace VDEApp.Views.Task
{
    public partial class WinAcquire : AntdUI.Window, ILocalizableForm
    {
        private AntdUI.BaseCollection _cameraItems;
        private ICamera _selectedCamera;

        public AcquireNode AcquireNode { get; set; }

        public WinAcquire(AcquireNode node = null)
        {
            AcquireNode = node;
            _selectedCamera = node?.Camera;
            _cameraItems = new AntdUI.BaseCollection();

            InitializeComponent();
            InitializeUI();
            RefreshLanguage();
        }

        public void InitializeUI()
        {
            header.Text = $"{AcquireNode.Task.Name}  {GlobalConfig.Localizer.GetString("Acquire Node")}";
            alert.Visible = false;

            foreach (var cam in ServiceLocator.DeviceController.Cameras)
                _cameraItems.Add(new CameraData{ Camera=cam });
            cbbCamera.Items = _cameraItems;
            cbbCamera.SelectedValue = _selectedCamera;

            if (_selectedCamera != null)
            {
                btnOK.Enabled = true;
                btnConfigure.Enabled = true;
                cbbCamera.Text = _selectedCamera.Name;
            }

            this.inBatchSize.Value = AcquireNode.BatchSize;
            this.inBatchSize.Minimum = 1;
            this.inBatchSize.Maximum = 99;

            this.inTimeout.Enabled = !(_selectedCamera is InsVirtualCamera);
            this.inExposureTime.Enabled = this.inTimeout.Enabled;

            this.inTimeout.Value = AcquireNode.Timeout;
            this.inExposureTime.Value = (decimal)AcquireNode.ExposureTime;
        }
        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AcquireNode.Camera = _selectedCamera;
            AcquireNode.Save(AcquireNode.Task.GetAcquireNodeConfigPath());
            DialogResult = DialogResult.OK;

            AcquireNode.BatchSize = (int)inBatchSize.Value;
            if (!(_selectedCamera is InsVirtualCamera))
            {
                AcquireNode.Timeout = (int)inTimeout.Value;
                AcquireNode.ExposureTime = (double)inExposureTime.Value;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void cbbCamera_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            var camera = e.Value as CameraData;
            if (camera == null) return;

            cbbCamera.Text = camera.Camera.Name;
            btnConfigure.Enabled = true;

            this.inTimeout.Enabled = !(camera.Camera is InsVirtualCamera);
            this.inExposureTime.Enabled = this.inTimeout.Enabled;

            if (_selectedCamera == null)
            {
                this.inBatchSize.Value = camera.Camera.BatchSize;
                this.inTimeout.Value = camera.Camera.TimeOut;
                this.inExposureTime.Value = (decimal)camera.Camera.ExposureTime;
            }

            _selectedCamera = camera.Camera;
            btnOK.Enabled = _selectedCamera.IsConnected;
            alert.Visible = !btnOK.Enabled;
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            new DlgCameraConfigure(_selectedCamera).ShowDialog();
            btnOK.Enabled = _selectedCamera.IsConnected;
            alert.Visible = !btnOK.Enabled;

            this.inBatchSize.Value = _selectedCamera.BatchSize;
            if (!(_selectedCamera is InsVirtualCamera))
            {
                this.inTimeout.Value = _selectedCamera.TimeOut;
                this.inExposureTime.Value = (decimal)_selectedCamera.ExposureTime;
            }
        }
    }

    internal class CameraData
    {
        public ICamera Camera{ get; set; }

        public override string ToString()
        {
            return Camera?.Name;
        }
    }
}
