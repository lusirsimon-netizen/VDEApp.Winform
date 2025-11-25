using System.Drawing;

namespace VDEApp.Views.Main
{
    partial class UCProductionLog
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LoggroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.filterPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.Infocheckbox = new AntdUI.Checkbox();
            this.Warncheckbox = new AntdUI.Checkbox();
            this.Debugcheckbox = new AntdUI.Checkbox();
            this.Errorcheckbox = new AntdUI.Checkbox();
            this.Fatalcheckbox = new AntdUI.Checkbox();
            this.LoglistBox = new System.Windows.Forms.ListBox();
            this.Clearbutton = new AntdUI.Button();
            this.LogTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.LoggroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoggroupBox
            // 
            this.LoggroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoggroupBox.Controls.Add(this.tableLayoutPanel1);
            this.LoggroupBox.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LoggroupBox.Location = new System.Drawing.Point(4, 4);
            this.LoggroupBox.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.LoggroupBox.Name = "LoggroupBox";
            this.LoggroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LoggroupBox.Size = new System.Drawing.Size(383, 385);
            this.LoggroupBox.TabIndex = 0;
            this.LoggroupBox.TabStop = false;
            this.LoggroupBox.Text = "日志";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.filterPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LoglistBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Clearbutton, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 18);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(379, 365);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // filterPanel
            // 
            this.filterPanel.AutoSize = true;
            this.filterPanel.BackColor = System.Drawing.Color.Transparent;
            this.filterPanel.Controls.Add(this.Infocheckbox);
            this.filterPanel.Controls.Add(this.Warncheckbox);
            this.filterPanel.Controls.Add(this.Debugcheckbox);
            this.filterPanel.Controls.Add(this.Errorcheckbox);
            this.filterPanel.Controls.Add(this.Fatalcheckbox);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterPanel.Location = new System.Drawing.Point(2, 2);
            this.filterPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(375, 22);
            this.filterPanel.TabIndex = 2;
            // 
            // Infocheckbox
            // 
            this.Infocheckbox.Location = new System.Drawing.Point(2, 2);
            this.Infocheckbox.Margin = new System.Windows.Forms.Padding(2, 2, 3, 2);
            this.Infocheckbox.Name = "Infocheckbox";
            this.Infocheckbox.Size = new System.Drawing.Size(51, 18);
            this.Infocheckbox.TabIndex = 3;
            this.Infocheckbox.Text = "INFO";
            // 
            // Warncheckbox
            // 
            this.Warncheckbox.Location = new System.Drawing.Point(56, 2);
            this.Warncheckbox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.Warncheckbox.Name = "Warncheckbox";
            this.Warncheckbox.Size = new System.Drawing.Size(60, 18);
            this.Warncheckbox.TabIndex = 4;
            this.Warncheckbox.Text = "WARN";
            // 
            // Debugcheckbox
            // 
            this.Debugcheckbox.Location = new System.Drawing.Point(119, 2);
            this.Debugcheckbox.Margin = new System.Windows.Forms.Padding(0, 2, 2, 2);
            this.Debugcheckbox.Name = "Debugcheckbox";
            this.Debugcheckbox.Size = new System.Drawing.Size(67, 18);
            this.Debugcheckbox.TabIndex = 7;
            this.Debugcheckbox.Text = "DEBUG";
            // 
            // Errorcheckbox
            // 
            this.Errorcheckbox.Location = new System.Drawing.Point(188, 2);
            this.Errorcheckbox.Margin = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.Errorcheckbox.Name = "Errorcheckbox";
            this.Errorcheckbox.Size = new System.Drawing.Size(59, 18);
            this.Errorcheckbox.TabIndex = 5;
            this.Errorcheckbox.Text = "ERROR";
            // 
            // Fatalcheckbox
            // 
            this.Fatalcheckbox.Location = new System.Drawing.Point(250, 2);
            this.Fatalcheckbox.Margin = new System.Windows.Forms.Padding(0, 2, 2, 2);
            this.Fatalcheckbox.Name = "Fatalcheckbox";
            this.Fatalcheckbox.Size = new System.Drawing.Size(53, 18);
            this.Fatalcheckbox.TabIndex = 6;
            this.Fatalcheckbox.Text = "FATAL";
            // 
            // LoglistBox
            // 
            this.LoglistBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LoglistBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoglistBox.FormattingEnabled = true;
            this.LoglistBox.ItemHeight = 17;
            this.LoglistBox.Location = new System.Drawing.Point(7, 32);
            this.LoglistBox.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.LoglistBox.Name = "LoglistBox";
            this.LoglistBox.Size = new System.Drawing.Size(365, 296);
            this.LoglistBox.TabIndex = 0;
            // 
            // Clearbutton
            // 
            this.Clearbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Clearbutton.DefaultBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(19)))), ((int)(((byte)(19)))));
            this.Clearbutton.Location = new System.Drawing.Point(320, 336);
            this.Clearbutton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Clearbutton.Name = "Clearbutton";
            this.Clearbutton.Size = new System.Drawing.Size(57, 27);
            this.Clearbutton.TabIndex = 3;
            this.Clearbutton.Text = "Clear";
            this.Clearbutton.Type = AntdUI.TTypeMini.Primary;
            this.Clearbutton.Click += new System.EventHandler(this.Clearbutton_Click);
            // 
            // UCProductionLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LoggroupBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "UCProductionLog";
            this.Size = new System.Drawing.Size(391, 393);
            this.LoggroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.filterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox LoggroupBox;
        public System.Windows.Forms.ListBox LoglistBox;
        private System.Windows.Forms.ToolTip LogTip;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private AntdUI.Button Clearbutton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel filterPanel;
        private AntdUI.Checkbox Infocheckbox;
        private AntdUI.Checkbox Warncheckbox;
        private AntdUI.Checkbox Debugcheckbox;
        private AntdUI.Checkbox Errorcheckbox;
        private AntdUI.Checkbox Fatalcheckbox;
    }
}
