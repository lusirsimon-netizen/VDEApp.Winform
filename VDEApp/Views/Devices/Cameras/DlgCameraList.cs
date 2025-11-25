using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Devices;
using VDEApp.Devices.Cameras;
using VDEApp.Infrastructure;
using VDEApp.LogModule;

namespace VDEApp.Views.Devices.Cameras
{
    public partial class DlgCameraList : AntdUI.Window, ILocalizableForm
    {
        private BindingList<CameraRow> _items;
        private Dictionary<string, Func<ICamera, UserControl>> _controlCreators;

        public DlgCameraList()
        {
            _items = new BindingList<CameraRow>();
            _controlCreators = new Dictionary<string, Func<ICamera, UserControl>>();

            InitializeComponent();
            InitializeUI();
            RefreshLanguage();
        }

        private void InitializeUI()
        {
            this.header.ShowButton = true;
            this.btnOK.DialogResult = DialogResult.OK;
            this.FormClosed += DlgCameraList_FormClosed;

            this.tableCameras.Columns = new AntdUI.ColumnCollection() {
                new AntdUI.Column("Index", "No.")
                  .SetLocalizationTitleID("Table.Column")
                  .SetFixed(true)
                  .SetEditable(false)
                  .SetAlign(AntdUI.ColumnAlign.Center)
                  .SetWidth("50"),

                new AntdUI.Column("Name", GlobalConfig.Localizer.GetString("DlgCameraList_ColName", "Name"))
                  .SetLocalizationTitleID("Table.Column")
                  .SetAlign(AntdUI.ColumnAlign.Center)
                  .SetMinWidth("100"),

                new AntdUI.Column("Model", GlobalConfig.Localizer.GetString("DlgCameraList_ColModel", "Model"))
                  .SetLocalizationTitleID("Table.Column")
                  .SetEditable(false)
                  .SetAlign(AntdUI.ColumnAlign.Center)
                  .SetMinWidth("100"),

                new AntdUI.Column("Info", GlobalConfig.Localizer.GetString("DlgCameraList_ColInfo", "Info"))
                  .SetLocalizationTitleID("Table.Column")
                  .SetAlign(AntdUI.ColumnAlign.Center)
                  .SetEditable(false)
                  .SetMinWidth("100"),

                new AntdUI.Column("Configure", GlobalConfig.Localizer.GetString("DlgCameraList_ColConfigure", "Configure"))
                  .SetRender((val, data, index) => {
                    return new AntdUI.CellButton("ColBtnConfigure", GlobalConfig.Localizer.GetString("DlgCameraList_ColConfigure", "Configure"));;
                }).SetLocalizationTitleID("Table.Column")
                  .SetFixed(true)
                  .SetAlign(AntdUI.ColumnAlign.Center)
                  .SetWidth("125"),
            };
            tableCameras.VisibleHeader = true;
            tableCameras.FixedHeader = true;
            tableCameras.VisibleHeader = true;
            tableCameras.EmptyHeader = true;

            tableCameras.Bordered = true;
            tableCameras.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            tableCameras.EditMode = AntdUI.TEditMode.DoubleClick;
            tableCameras.EditInputStyle = AntdUI.TEditInputStyle.Default;

            tableCameras.CellButtonClick += this.tableCameras_ConfigureButton_Click;
            tableCameras.CellEndEdit += this.tableCameras_EndEdit;

            RebindTable();
        }

        private void DlgCameraList_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalConfig.Instance.CurrentProject.IsModified = true;
        }

        private void RebindTable()
        {
            RefreshTable();
            tableCameras.Binding(_items);
        }

        private void RefreshTable()
        {
            _items.Clear();
            int index = 0;
            foreach (var c in ServiceLocator.DeviceController.Cameras)
                _items.Add(new CameraRow(++index, c, $"{( c.IsConnected ? "[C]" : "")} {c.SN}"));
        }
        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }

        private void btnAddCamera_Click(object sender, EventArgs evt)
        {
            try
            {
                var dlg = new DlgAddCamera() { Owner = this, StartPosition = FormStartPosition.CenterParent };
                dlg.CameraName = "Camera " + (_items.Count + 1);
                var ret = dlg.ShowDialog();
                if (ret == DialogResult.Cancel) return;

                dlg.Camera.Name = dlg.CameraName;
                ServiceLocator.DeviceController.AddCamera(dlg.Camera);

                RebindTable();
            }
            catch (ArgumentException e)
            {
                MessageBox.Show($"{e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogModule.Log.Error($"{e.Message}");
            }
        }
        private void btnRemoveCamera_Click(object sender, EventArgs e)
        {
            var row_idx = tableCameras.SelectedIndex - 1;
            if (row_idx < 0 || row_idx >= _items.Count) return;

            ServiceLocator.DeviceController.RemoveCamera(row_idx);
            RebindTable();
        }

        private void tableCameras_ConfigureButton_Click(object sender, AntdUI.TableButtonEventArgs e)
        {
            var camera = ServiceLocator.DeviceController.Cameras[e.RowIndex - 1];
            var dlg = new DlgCameraConfigure(camera) { Owner = this, StartPosition = FormStartPosition.CenterParent };

            var ret = dlg.ShowDialog();
            RebindTable();
        }
        private bool tableCameras_EndEdit(object sender, AntdUI.TableEndEditEventArgs e)
        {
            try
            {
                ServiceLocator.DeviceController.RenameCamera(e.RowIndex - 1, e.Value);
            }
            catch (ArgumentException)
            {
                string msg = string.Format(GlobalConfig.Instance.GlobalLocalizer.GetString("msg_new_camera_duplicate_name"), e.Value);
                MessageBox.Show(
                    msg,
                    GlobalConfig.Instance.GlobalLocalizer.GetString("Error", "Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                Log.Info(msg);
                return false;
            }
            return true;
        }
    }

    internal class CameraRow
    {
        public CameraRow(int index, ICamera camera, string info = "")
        {
            Camera = camera;
            Index = index;
            Info = info;
        }

        public ICamera Camera { get; set; }

        public int Index { get; set; }
        public string Name
        {
            get => Camera.Name;
        }
        public string Model
        {
            get => Camera.ModelName;
        }
        public string Info { get; set; }
    }
}
