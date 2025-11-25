using System;
using System.IO;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.Models;

namespace VDEApp.Views.Project
{
    public partial class WinProjectNew : AntdUI.Window, ILocalizableForm
    {
        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;
        public WinProjectNew(bool isSave = false, bool isRestart = true)
        {
            InitializeComponent();

            RefreshLanguage();

            txtName.Text = Localizer.GetString("NewProject", "NewProject");
            txtPath.Text = AppPathRouter.WorkRoot;
            _isSave = isSave;
            _isRestart = isRestart;
            if (isSave)
            {
                headerCreateProject.Text = Localizer.GetString("SaveProject", "Save Project");
                btnCreate.Text = Localizer.GetString("Save", "Save");
            }
            else
            {
                headerCreateProject.Text = Localizer.GetString("CreateProject", "Create Project");
                btnCreate.Text = Localizer.GetString("Create", "Create");
            }
        }
        private bool _isSave = false, _isRestart;

        private string SelectedFolder = AppPathRouter.WorkRoot;
        /// <summary>
        /// Choose project folder
        /// </summary> 
        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.SelectedPath = SelectedFolder;
                    fbd.Description = "请选择项目生成目录";
                    fbd.ShowNewFolderButton = true;
                    if (fbd.ShowDialog(this) == DialogResult.OK)
                    {
                        SelectedFolder = fbd.SelectedPath;
                        txtPath.Text = fbd.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "选择文件夹时发生错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Create project
        /// </summary> 
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isSave)
                {
                    GlobalConfig.Instance.CurrentProject.Name = txtName.Text;
                    ServiceLocator.ProjectController.CurrentProject.Path = Path.Combine(txtPath.Text, GlobalConfig.Instance.CurrentProject.Name, $"{GlobalConfig.Instance.CurrentProject.Name}{AppConstant.ProjectExtension}");
                    ServiceLocator.ProjectController.SaveCurrentProject();
                    MessageBox.Show(Localizer.GetString("SaveProjectSuccess", "Project saved successfully!"));
                    DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ServiceLocator.ProjectController.NewProject(txtName.Text, txtPath.Text, !_isRestart);
                    DialogResult = DialogResult.OK;
                    this.Close();
                    if (_isRestart)
                        Program.RestartApplication();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }
    }
}
