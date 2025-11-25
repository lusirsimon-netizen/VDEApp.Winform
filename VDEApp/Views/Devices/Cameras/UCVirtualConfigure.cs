using System;
using System.IO;
using System.Windows.Forms;

using VDEApp.Devices.Cameras;

namespace VDEApp.Views.Devices.Cameras
{
    public partial class UCVirtualConfigure : UserControl
    {
        private InsVirtualCamera _camera;

        public UCVirtualConfigure(InsVirtualCamera camera)
        {
            InitializeComponent();
            this._camera = camera;

            if (_camera.Path != null)
                this.inPath.Text = _camera.Path;

            inNumBatchSize.Value = _camera.BatchSize;
            inNumBatchSize.Minimum = 1;
            inNumBatchSize.Increment = 1;
            inNumBatchSize.Maximum = 99;
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select Directory";
                folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;

                if (folderDialog.ShowDialog() != DialogResult.OK)
                    return;

                string selectedFolder = folderDialog.SelectedPath;

                inPath.Text = selectedFolder;
                _camera.Path = selectedFolder;
            }
        }

        private void inNumBatchSize_ValueChanged(object sender, AntdUI.DecimalEventArgs e)
        {
            _camera.BatchSize = (int)e.Value;
        }

        private void inPath_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !checkPath();
        }

        private void inPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13) return;
            e.Handled = true;
            checkPath();
        }

        private bool checkPath()
        {
            if (!Directory.Exists(inPath.Text))
            {
                AntdUI.Notification.warn(ParentForm, "Invalid Path", "Path is not a directory.");
                return false;
            }

            _camera.Path = inPath.Text;
            return true;
        }
    }
}
