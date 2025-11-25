using System.Windows.Forms;
using VDEApp.Infrastructure;
using VDEApp.Devices;
using VDEApp.Devices.Enums;
using VDEApp.Devices.Cameras;
using VDEApp.Configs;

namespace VDEApp.Views.Devices.Cameras
{
    public partial class UCDaHuaConfigure : UserControl
    {
        private InsCamera2DDaHua _camera;
        public UCDaHuaConfigure(InsCamera2DDaHua camera)
        {
            _camera = camera;

            InitializeComponent();

            InitializeUI();
        }

        public void InitializeUI()
        {
            tabs.SelectedIndex = 0;
            cbbDevices.Items = new AntdUI.BaseCollection();
            foreach (var info in InsCamera2DDaHua.SearchCameras())
            {
                cbbDevices.Items.Add(new DeviceInfoWrapper { Info = info });
            }
            cbbMode.Items = new AntdUI.BaseCollection();
            foreach (var mode in _camera.SupportedTriggerModes)
            {
                cbbMode.Items.Add(mode);
            }
            cbbFormat.Items = new AntdUI.BaseCollection();
            foreach (var format in _camera.SupportedImageFormats)
            {
                cbbFormat.Items.Add(format);
            }

            inBatchSize.Maximum = 99;
            inBatchSize.Minimum = 1;

            inTimeout.Maximum = int.MaxValue;
            inTimeout.Minimum = 0;

            inExposureTime.Value = 0;
            inGain.Value = 0;

            if (_camera.Info != null)
                setInformation(_camera.Info);
            if (_camera.IsConnected)
                onCameraConnected();
            else
                this.cbbDevices.SelectedValueChanged += this.cbbDevices_SelectedValueChanged;

            _camera.OnCamConnectionChange += (object sender, CamConnectChangeEventArgs e) =>
            {
                if (e.IsConnected)
                    onCameraConnected();
                else
                    onCameraDisconnected();
            };
        }

        private void cbbDevices_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            var info = e.Value as DeviceInfoWrapper;
            if (info == null)
                return;

            if (_camera.SN != info.Info.SerialNumber)
            {
                // if selected device is already in camera list
                // the selection should be ignored, and user is prompted with a error message
                foreach (var camera in ServiceLocator.DeviceController.FindCamerasByType<InsCamera2DDaHua>())
                {
                    if (camera.SN == info.Info.SerialNumber)
                    {
                        cbbDevices.SelectedValue = null;
                        MessageBox.Show(
                            GlobalConfig.Localizer.GetString(
                                "msg_duplicate_camera_selected",
                                "This camera is already added"
                            ),
                            GlobalConfig.Localizer.GetString(
                                "msg_duplicate_camera_selected_title",
                                "Camera Duplicated"
                            ),
                            MessageBoxButtons.OK, MessageBoxIcon.Asterisk
                        );
                        return;
                    }
                }
            }

            setInformation(info.Info);
            _camera.SetDeviceInfo(info.Info);
        }
        private void setInformation(InsCamera2DDaHua.DeviceInfo info)
        {
            cbbDevices.Text = $"{info.ModelName} [{info.SerialNumber}]";
            txtIPSN.Text = info.Connection.Value;
            if ((info.Connection.CameraType & InsCameraType.CameraGige) != 0)
                txtType.Text = "GigE";
            else if ((info.Connection.CameraType & InsCameraType.CameraUSB) != 0)
                txtType.Text = "USB";
            else
                txtType.Text = "Unkown";
            txtCameraKey.Text = info.Key;
            txtVendorName.Text = info.VendorName;
            txtVersion.Text = info.DeviceVersion;
        }

        private void tabs_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {

        }

        private void onCameraConnected()
        {
            cbbDevices.Enabled = false;
            this.cbbDevices.SelectedValueChanged -= this.cbbDevices_SelectedValueChanged;

            tabSettings.Enabled = true;
            tabTrigger.Enabled = true;
            tabImageFormat.Enabled = true;

            inBatchSize.Value = _camera.BatchSize;
            inTimeout.Value = _camera.TimeOut;

            inExposureTime.Maximum = (decimal)_camera.ExposureTimeMax;
            inExposureTime.Minimum = (decimal)_camera.ExposureTimeMin;
            inExposureTime.Value = (decimal)_camera.ExposureTime;

            inGain.Maximum = (decimal)_camera.GainMax;
            inGain.Minimum = (decimal)_camera.GainMin;
            inGain.Value = (decimal)_camera.Gain;

            cbbMode.SelectedValue = _camera.TriggerMode;
            cbbMode.Text = _camera.TriggerMode.ToString();

            cbbFormat.SelectedValue = _camera.ImageFormat;
            cbbFormat.Text = _camera.ImageFormat.ToString();


            this.inExposureTime.ValueChanged += this.inExposureTime_ValueChanged;
            this.inGain.ValueChanged += this.inGain_ValueChanged;
            this.inTimeout.ValueChanged += this.inTimeout_ValueChanged;
            this.swEnable.CheckedChanged += this.swEnable_CheckedChanged;
            this.cbbMode.SelectedValueChanged += this.cbbMode_SelectedValueChanged;
            this.cbbFormat.SelectedValueChanged += this.cbbFormat_SelectedValueChanged;
            this.inBatchSize.ValueChanged += this.inBatchSize_ValueChanged;
        }
        private void onCameraDisconnected()
        {
            this.inExposureTime.ValueChanged -= this.inExposureTime_ValueChanged;
            this.inGain.ValueChanged -= this.inGain_ValueChanged;
            this.inTimeout.ValueChanged -= this.inTimeout_ValueChanged;
            this.swEnable.CheckedChanged -= this.swEnable_CheckedChanged;
            this.cbbMode.SelectedValueChanged -= this.cbbMode_SelectedValueChanged;
            this.cbbFormat.SelectedValueChanged -= this.cbbFormat_SelectedValueChanged;
            this.inBatchSize.ValueChanged -= this.inBatchSize_ValueChanged;

            cbbDevices.Enabled = true;

            tabSettings.Enabled = false;
            tabTrigger.Enabled = false;
            tabImageFormat.Enabled = false;
            tabs.SelectedIndex = 0;

            this.cbbDevices.SelectedValueChanged += this.cbbDevices_SelectedValueChanged;
        }

        private void swEnable_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            if (e.Value)
                _camera.TriggerModeOn();
            else
                _camera.TriggerModeOff();
        }

        private void cbbMode_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            var mode = (InsCameraTriggerMode)e.Value;
            cbbMode.Text = mode.ToString();
            _camera.TriggerMode = mode;
        }
        private void cbbFormat_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            var type = (InsImageType)e.Value;
            cbbFormat.Text = type.ToString();
            _camera.ImageFormat = type;
        }

        private void inBatchSize_ValueChanged(object sender, AntdUI.DecimalEventArgs e)
        {
            _camera.BatchSize = (int)e.Value;
        }

        private void inTimeout_ValueChanged(object sender, AntdUI.DecimalEventArgs e)
        {
            _camera.TimeOut = (int)e.Value;
        }

        private void inExposureTime_ValueChanged(object sender, AntdUI.DecimalEventArgs e)
        {
            _camera.ExposureTime = (double)e.Value;
        }

        private void inGain_ValueChanged(object sender, AntdUI.DecimalEventArgs e)
        {
            _camera.Gain = (double)e.Value;
        }
    }

    internal class DeviceInfoWrapper
    {
        public InsCamera2DDaHua.DeviceInfo Info;

        public override string ToString()
        {
            return $"{Info.ModelName} [{Info.SerialNumber}]";
        }
    }
}
