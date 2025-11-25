using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Devices;
using VDEApp.Infrastructure;

namespace VDEApp.Views.Devices.Cameras
{
    public partial class DlgAddCamera : AntdUI.Window, ILocalizableForm
    {
        internal class CameraModelInfo
        {
            public Type CameraType;
            public CameraModelData CameraModel;

            public CameraModelInfo(KeyValuePair<Type, CameraModelData> kv)
            {
                CameraType = kv.Key;
                CameraModel = kv.Value;
            }

            public override string ToString() => CameraModel.Name;
        }
        private CameraModelInfo _selectedModel;
        private ICamera _camera;

        public string CameraName
        {
            get => inName.Text;
            set => inName.Text = value;
        }
        public ICamera Camera
        {
            get
            {
                if (_camera == null)
                    _camera = createCameraInstance();
                return _camera;
            }
        }

        public DlgAddCamera()
        {
            _camera = null;

            InitializeComponent();
            InitializeUI();
            RefreshLanguage();
        }
        public void InitializeUI()
        {
            this.header.ShowButton = true;

            var cbb_items = new AntdUI.BaseCollection();
            var models = ServiceLocator.DeviceController.CameraModels;
            foreach (var model in models)
                cbb_items.Add(new CameraModelInfo(model));
            this.cbbModel.Items = cbb_items;

            this._selectedModel = new CameraModelInfo(models.FirstOrDefault());
            this.cbbModel.Text = _selectedModel.CameraModel.Name;
        }
        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }

        private void cbbModel_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            var cam = e.Value as CameraModelInfo;
            if (cam == null) return;
            this.cbbModel.Text = cam.CameraModel.Name;

            _selectedModel = cam;
        }

        /// <summary>
        /// Create camera instance from type
        /// </summary>
        /// <exception cref="ArgumentException">
        /// throws when camera type is not ICamera
        /// </exception>
        private ICamera createCameraInstance()
        {
            var camera = Activator.CreateInstance(_selectedModel.CameraType) as ICamera;
            if (camera == null)
                throw new Exception($"TODO: INVALID CAMERA TYPE");
            camera.Name = CameraName;
            return camera;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ServiceLocator.DeviceController.HasCamera(CameraName))
            {
                this.DialogResult = DialogResult.OK;
                return;
            }

            MessageBox.Show(
                string.Format(GlobalConfig.Instance.GlobalLocalizer.GetString("msg_new_camera_duplicate_name"), inName.Text),
                GlobalConfig.Instance.GlobalLocalizer.GetString("Error", "Error"),
                MessageBoxButtons.OK, MessageBoxIcon.Error
            );

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void inName_TextChanged(object sender, EventArgs e)
        {
            if (_camera != null)
                _camera.Name = CameraName;
        }
    }
}
