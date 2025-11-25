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
using Button = AntdUI.Button;
using Label = AntdUI.Label;
using Message = AntdUI.Message;

namespace VDEApp.Views.Task
{
    public partial class WinSPECInput : AntdUI.Window
    {
        public Tuple<double, double> Result { get; private set; }

        public WinSPECInput(Tuple<double,double> existingRange = null)
        {
            InitializeComponent();
            this.inputLower.Value = (decimal)existingRange.Item1;
            this.inputHigher.Value = (decimal)existingRange.Item2;
            this.inputLower.ValueChanged += new DecimalEventHandler(ValidateInputRange);
            this.inputHigher.ValueChanged += new DecimalEventHandler(ValidateInputRange);
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            double lowerValue = (double)this.inputLower.Value;
            double higherValue = (double)this.inputHigher.Value;
            this.Result = new Tuple<double,double>(lowerValue,higherValue);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void ValidateInputRange(object sender, EventArgs e)
        {
            double lowerValue = (double)this.inputLower.Value;
            double higherValue = (double)this.inputHigher.Value;

            if (higherValue < lowerValue)
            {
                Message.error(this, "最大值小于最小值，请重新输入！");
                this.btnConfirm.Enabled = false;
            }
            else
            {
                this.btnConfirm.Enabled = true;
            }
        }
    }
}
