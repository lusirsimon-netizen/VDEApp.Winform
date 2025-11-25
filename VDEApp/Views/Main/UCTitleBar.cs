using System;
using System.IO;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Configs.Display;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.Models;
using VDEApp.Models.Project;
using VDEApp.Properties;
using VDEApp.Views.Devices.Cameras;
using VDEApp.Views.Help;
using VDEApp.Views.Project;
using VDEApp.Views.Tools;

namespace VDEApp.Views.Main
{
    public partial class UCTitleBar : UserControl, IMainLoader
    {
        public UCTitleBar()
        {
            InitializeComponent();

            ServiceLocator.ProjectController.CurrentProject.ProjectChangedEvent += CurrentProject_ProjectChangedEvent;
        }
        /// <summary>
        /// 本地语言
        /// </summary>
        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;
        /// <summary>
        /// 可修改的菜单项
        /// </summary>
        private AntdUI.MenuItem _historyMenuItem, _titleCurrentProject, _showrecord, _menuChinese, _menuEnglish;
        /// <summary>
        /// Initializes the user interface components and prepares the UI for interaction.
        /// </summary>
        public void InitializeUI()
        {
            menu1.Items.Clear();
            var titleProject = new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Project", "Project") };
            titleProject.Sub.Add(new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_NewProject", "NewProject"), Tag = "newproject" });
            titleProject.Sub.Add(new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_OpenProject", "OpenProject"), Tag = "openproject" });
            _historyMenuItem = new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_History", "History") };
            titleProject.Sub.Add(_historyMenuItem);
            titleProject.Sub.Add(new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Save", "Save"), Tag = "saveproject" });
            titleProject.Sub.Add(new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Exit", "Exit"), Tag = "exitapplication" });
            menu1.Items.Add(titleProject);

            var titleTool = new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Tool", "Tool") };
            titleTool.Sub.Add(new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_DeviceManagement", "Device Management"), Tag = "devicemanagement" });
            titleTool.Sub.Add(new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Communication", "Communication"), Tag = "communication" });
            titleTool.Sub.Add(new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Options", "Options"), Tag = "options" });
            menu1.Items.Add(titleTool);

            var titleDisplay = new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Display", "Display") };
            _showrecord = new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Show_record_configuration", "Show record configuration") };
            _showrecord.Sub.Add(new AntdUI.MenuItem() { Text = "1 x 1", Tag = "layout-1x1" });
            _showrecord.Sub.Add(new AntdUI.MenuItem() { Text = "2 x 2", Tag = "layout-2x2" });
            _showrecord.Sub.Add(new AntdUI.MenuItem() { Text = "N x M", Tag = "InterfaceStructure" });
            titleDisplay.Sub.Add(_showrecord);
            menu1.Items.Add(titleDisplay);

            var titleHelp = new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Help", "Help") };
            titleHelp.Sub.Add(new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_UserManual", "User Manual"), Tag = "usermanual" });
            titleHelp.Sub.Add(new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_About", "About"), Tag = "about" });
            menu1.Items.Add(titleHelp);

            var titleLanguage = new AntdUI.MenuItem() { Text = Localizer.GetString("TitleBar_Language", "Language") };
            _menuChinese = new AntdUI.MenuItem()
            {
                Text = Localizer.GetString("Chinese", "Chinese"),
                Tag = "Chinese",
                IconSvg = GlobalConfig.Instance.CurrentLang == "zh-CN" ? Resources.icon_true : null
            };
            _menuEnglish = new AntdUI.MenuItem()
            {
                Text = Localizer.GetString("English", "English"),
                Tag = "English",
                IconSvg = GlobalConfig.Instance.CurrentLang == "en-US" ? Resources.icon_true : null
            };
            titleLanguage.Sub.Add(_menuChinese);
            titleLanguage.Sub.Add(_menuEnglish);
            menu1.Items.Add(titleLanguage);

            _titleCurrentProject = new AntdUI.MenuItem()
            {
                Text = $"{Localizer.GetString("TitleBar_Current", "Current")}：{GlobalConfig.Instance.CurrentProject.Name}{(GlobalConfig.Instance.CurrentProject.IsModified ? "*" : "")}"
            };
            _titleCurrentProject.Sub.Add(new AntdUI.MenuItem() { Text = $"{Localizer.GetString("TitleBar_ProjectPath", "Project Path")}  {GlobalConfig.Instance.CurrentProject.Path}", Tag = "projectpath" });
            menu1.Items.Add(_titleCurrentProject);

            //加载最近打开历史
            foreach (var item in GlobalConfig.Instance.UserOperation.ProjectHistorys)
            {
                if (File.Exists(item.Path))
                    _historyMenuItem.Sub.Add(new AntdUI.MenuItem() { Text = item.Path, Tag = "OpenFile" });
            }
            //刷新可用项目历史
            GlobalConfig.Instance.UserOperation.ProjectHistorys = GlobalConfig.Instance.UserOperation.ProjectHistorys.FindAll(x => System.IO.File.Exists(x.Path));

            menu1.SelectChanged -= Menu1_SelectChanged;
            menu1.SelectChanged += Menu1_SelectChanged;
        }
        /// <summary>
        /// 刷新当前项目状态
        /// </summary>
        private void CurrentProject_ProjectChangedEvent(object sender, EventArgs e)
        {
            if (sender is ProjectModel project)
                _titleCurrentProject.Text = $"{Localizer.GetString("TitleBar_Current", "Current")}：{project.Name}{(project.IsModified ? "*" : "")}";
        }
        /// <summary>
        /// 视图切换改变
        /// </summary>
        public event EventHandler<LayoutSelectedEventArgs> OnLayoutSelected;
        /// <summary>
        /// 菜单触发事件
        /// </summary> 
        private void Menu1_SelectChanged(object sender, AntdUI.MenuSelectEventArgs e)
        {
            if (e.Value is AntdUI.MenuItem subitem && subitem.Tag is string tag)
            {
                try
                {
                    switch (tag)
                    {
                        case "newproject":
                        {
                            new WinProjectNew() { Owner = AppModuleSingleton.MainFormInstance }.ShowDialog();
                        }
                        break;
                        case "openproject":
                        {
                            OpenFileDialog openDialog = new OpenFileDialog
                            {
                                Filter = $"{Localizer.GetString("TitleBar_ProjectFile", "Project File")} (*{AppConstant.ProjectExtension})|*{AppConstant.ProjectExtension}|所有文件 (*.*)|*.*",
                                FilterIndex = 1,
                                Title = Localizer.GetString("TitleBar_LoadProject", "Load Project"),
                                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                Multiselect = false
                            };

                            // 显示打开对话框
                            if (openDialog.ShowDialog() == DialogResult.OK)
                            {
                                GlobalConfig.Instance.UserOperation.CurrentProject = openDialog.FileName;
                                GlobalConfig.Instance.SaveOperation();
                                Program.RestartApplication();
                            }
                        }
                        break;
                        case "OpenFile":
                            GlobalConfig.Instance.UserOperation.CurrentProject = subitem.Text;
                            GlobalConfig.Instance.SaveOperation();
                            Program.RestartApplication();
                            break;
                        case "saveproject":
                        {
                            try
                            {
                                if (!GlobalConfig.Instance.CurrentProject.IsValid)
                                    new WinProjectNew(true) { Owner = AppModuleSingleton.MainFormInstance }.ShowDialog();
                                else
                                {
                                    ServiceLocator.ProjectController.SaveCurrentProject();
                                    MessageBox.Show(Localizer.GetString("TitleBar_ProjectSavedSuccessfully", "Project saved successfully!"));
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"{Localizer.GetString("TitleBar_ProjectSavedFailed", "Project saved failed")} Error={ex.Message}");
                            }
                        }
                        break;
                        case "projectpath":
                            if (!string.IsNullOrEmpty(GlobalConfig.Instance.CurrentProject.Path))
                            {
                                var directory = new FileInfo(GlobalConfig.Instance.CurrentProject.Path).DirectoryName;
                                if (Directory.Exists(directory))
                                {
                                    System.Diagnostics.Process.Start("explorer", $"/n, {directory}");
                                    return;
                                }
                            }
                            throw new FileNotFoundException(Localizer.GetString("TitleBar_NeedSaveProject", "The project path does not exist, please save the project！"));
                        case "rename":
                            break;
                        case "exitapplication":
                        {
                            if (MessageBox.Show(Localizer.GetString("TitleBar_Exitapplication", "Are you sure you want to exit the application?"), AppConstant.AppName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                AppModuleSingleton.MainFormInstance.Close();
                        }
                        break;
                        case "devicemanagement":
                        {
                            new DlgCameraList()
                            {
                                Owner = AppModuleSingleton.MainFormInstance,
                                StartPosition = FormStartPosition.CenterParent,
                            }.ShowDialog();
                        }
                        break;
                        case "communication":
                        {
                            WinTcpCommunicationDebug winComm = new WinTcpCommunicationDebug();
                            winComm.Show();
                        }
                        break;
                        case "options":
                            new WinOption().ShowDialog();
                            break;
                        case "usermanual":
                            break;
                        case "about":
                        {
                            new WinAbout().ShowDialog();
                        }
                        break;
                        case "Chinese":
                        {
                            _menuChinese.IconSvg = Resources.icon_true;
                            _menuEnglish.IconSvg = "";
                            LanguageController.SwitchLanguage("zh-CN");
                            InitializeUI();
                        }
                        break;
                        case "English":
                        {
                            _menuChinese.IconSvg = "";
                            _menuEnglish.IconSvg = Resources.icon_true;
                            LanguageController.SwitchLanguage("en-US");
                            InitializeUI();
                        }
                        break;
                        case "InterfaceStructure":
                            DisplayInterfaceMenuItem_Click(subitem, EventArgs.Empty);
                            break;
                        case "layout-1x1":
                            OnLayoutSelected?.Invoke(this, new LayoutSelectedEventArgs(1, 1));
                            break;
                        case "layout-2x2":
                            OnLayoutSelected?.Invoke(this, new LayoutSelectedEventArgs(2, 2));
                            break;
                        case "example":
                            new FromExample().ShowDialog();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void DisplayInterfaceMenuItem_Click(object sender, EventArgs e)
        {
            UCProductDisplaystructure _uCProductDisplaystructure = new UCProductDisplaystructure();
            _uCProductDisplaystructure.LayoutSelected += LayoutForm_LayoutSelected;
            _uCProductDisplaystructure.ShowDialog(this);

        }

        private void LayoutForm_LayoutSelected(object sender, LayoutSelectedEventArgs selected)
        {
            OnLayoutSelected?.Invoke(
                this,
                new LayoutSelectedEventArgs(selected.Item1, selected.Item2)
            );
        }
    }
}
