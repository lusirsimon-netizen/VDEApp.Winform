using System;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Configs.Display;
using VDEApp.Views.Main;


namespace VDEApp
{
    public partial class MainForm : AntdUI.Window, ILocalizableForm
    {
        public MainForm()
        {
            InitializeComponent();

            RefreshMainProjectData();

            InitializeUI();

            RefreshLanguage();
        }

        private void InitializeUI()
        {
            // TitleBar
            AppModuleSingleton.TitleBarInstance.Dock = DockStyle.Fill;
            mainLayout.SetRow(AppModuleSingleton.TitleBarInstance, 0);
            mainLayout.SetColumnSpan(AppModuleSingleton.TitleBarInstance, 2);
            mainLayout.Controls.Add(AppModuleSingleton.TitleBarInstance);
            AppModuleSingleton.TitleBarInstance.MouseDown += ToolBarInstance_MouseDown;
            AppModuleSingleton.TitleBarInstance.OnLayoutSelected += TitleBar_OnLayoutSelected;

            // ToolBar
            AppModuleSingleton.ToolBarInstance.Dock = DockStyle.Fill;
            mainLayout.SetRow(AppModuleSingleton.ToolBarInstance, 1);
            mainLayout.SetColumnSpan(AppModuleSingleton.ToolBarInstance, 2);
            mainLayout.Controls.Add(AppModuleSingleton.ToolBarInstance);

            // Display
            AppModuleSingleton.ProductDisplay.Dock = DockStyle.Fill;
            this.tabProductDisplay.Controls.Add(AppModuleSingleton.ProductDisplay);

            // Record
            AppModuleSingleton.ProductionRecord.Dock = DockStyle.Fill;
            splitContainer2.Panel1.Controls.Add(AppModuleSingleton.ProductionRecord);

            // Log
            AppModuleSingleton.ProductionLog.Dock = DockStyle.Fill;
            splitContainer2.Panel2.Controls.Add(AppModuleSingleton.ProductionLog);

            // StatusBar
            AppModuleSingleton.StatusBarInstance.Dock = DockStyle.Fill;
            mainLayout.SetColumnSpan(AppModuleSingleton.StatusBarInstance, 2);
            mainLayout.SetRow(AppModuleSingleton.StatusBarInstance, 4);
            mainLayout.Controls.Add(AppModuleSingleton.StatusBarInstance);
        }
        private void TitleBar_OnLayoutSelected(object sender, LayoutSelectedEventArgs e)
        {
            AppModuleSingleton.ProductDisplay.GenerateLayoutUI(e.Rows, e.Columns);
        }
        private void ToolBarInstance_MouseDown(object sender, MouseEventArgs e)
        {
            DraggableMouseDown();
            base.OnMouseDown(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            DraggableMouseDown();
            base.OnMouseDown(e);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Refresh Project Data
        /// </summary>
        private void RefreshMainProjectData()
        {
            this.Text = AppConstant.AppName;
            var currentProject = ServiceLocator.GlobalConfig.CurrentProject;
            if (currentProject != null)
                this.Text += $" - {currentProject.Name} {currentProject.Description}";
        }
        /// <summary>
        /// Exit
        /// </summary> 
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ServiceLocator.TcpCommunicationController.Server.IsRunning)
            {
                ServiceLocator.TcpCommunicationController.StopServer();
            }
            var currentProject = ServiceLocator.GlobalConfig.CurrentProject;
            if (currentProject != null && currentProject.IsModified)
            {
                var result = MessageBox.Show("当前项目有未保存的更改，是否要保存？", "保存更改", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    ServiceLocator.ProjectController.SaveCurrentProject();
                }
            }
        }
        /// <summary>
        /// 实现 Program.ILocalizableForm 接口的 RefreshLanguage 方法
        /// </summary>
        public void RefreshLanguage()
        {
            // 调用工具类自动更新当前窗口及所有子控件
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }
    }
}
