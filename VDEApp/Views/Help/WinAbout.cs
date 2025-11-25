using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.LogModule;
using static VDEApp.Program;

namespace VDEApp.Views.Help
{
    public partial class WinAbout : AntdUI.Window, ILocalizableForm
    {
        public WinAbout()
        {
            InitializeComponent();
            RefreshLanguage();
        }

        public void RefreshLanguage()
        {
            // 调用工具类自动更新当前窗口及所有子控件
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }

        private void Shield_Project_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("功能待开发");
        }

        private void Shield_License_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("功能待开发");
        }

        private void Shield_Ins_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.insworks.com/");
            }
            catch
            {
                Log.Error("打开浏览器失败");
            }
        }
    }
}