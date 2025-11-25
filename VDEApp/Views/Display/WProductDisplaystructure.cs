using AntdUI;
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
using VDEApp.Controllers.Display;
using VDEApp.Configs.Display;
using VDEApp.LogModule;
using VDEApp.Models;

namespace VDEApp
{
    public partial class UCProductDisplaystructure : AntdUI.Window, ILocalizableForm
    {
        public event EventHandler<LayoutSelectedEventArgs> LayoutSelected;

        public UCProductDisplaystructure()
        {
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            InitializeComponent();
            InitializeAntdControls();
            RefreshLanguage();
        }
        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;
        /// <summary>
        /// 初始化 AntdUI 控件条件
        /// </summary>
        private void InitializeAntdControls()
        {
            rowbox.Minimum = 1;
            rowbox.Maximum = 5;
            rowbox.Value = 2;
            rowbox.Increment = 1;
            rowbox.DecimalPlaces = 0;

            colbox.Minimum = 1;
            colbox.Maximum = 5;
            colbox.Value = 2;
            colbox.Increment = 1;
            colbox.DecimalPlaces = 0;

            btnRefresh.Click += Refreshbutton_Click;
        }
        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void Refreshbutton_Click(object sender, EventArgs e)
        {
            try
            {
                int rows = (int)rowbox.Value;
                int cols = (int)colbox.Value;

                if (rows < 1 || cols < 1)
                {
                    AntdUI.Message.warn(this, Localizer.GetString("Message_NumberOfRowsAndColumnsMustBeGreaterThan0", "Number of rows and columns must be greater than 0"), null, 3);
                    return;
                }

                if (rows > 5 || cols > 5)
                {
                    AntdUI.Message.warn(this, Localizer.GetString("Message_NumberOfRowsAndColumnsCannotExceed5", "Number of rows and columns cannot exceed 5"), null, 3);
                    return;
                }
                LayoutSelected?.Invoke(this, new LayoutSelectedEventArgs(rows, cols));
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, string.Format(Localizer.GetString("Message_OperationFailed", "Operation failed: {0}"), ex.Message), null, 5);
            }
        }
        /// <summary>
        ///  调用工具类自动更新当前窗口及所有子控件
        /// </summary>
        public void RefreshLanguage()
        {

            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }
    }
}
