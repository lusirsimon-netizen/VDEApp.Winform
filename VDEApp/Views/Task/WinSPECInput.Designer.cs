namespace VDEApp.Views.Task
{
    partial class WinSPECInput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConfirm = new AntdUI.Button();
            this.btnCancel = new AntdUI.Button();
            this.inputLower = new AntdUI.InputNumber();
            this.labelLow = new AntdUI.Label();
            this.inputHigher = new AntdUI.InputNumber();
            this.labelHigh = new AntdUI.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pageHeader1 = new AntdUI.PageHeader();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(194, 137);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(102, 47);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "OK";
            this.btnConfirm.Type = AntdUI.TTypeMini.Primary;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(158)))), ((int)(((byte)(158)))));
            this.btnCancel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(303, 137);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 47);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // inputLower
            // 
            this.inputLower.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputLower.Font = new System.Drawing.Font("仿宋", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inputLower.Location = new System.Drawing.Point(92, 4);
            this.inputLower.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.inputLower.Name = "inputLower";
            this.inputLower.Size = new System.Drawing.Size(313, 59);
            this.inputLower.TabIndex = 1;
            this.inputLower.Text = "0";
            // 
            // labelLow
            // 
            this.labelLow.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.labelLow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelLow.Location = new System.Drawing.Point(24, 22);
            this.labelLow.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelLow.Name = "labelLow";
            this.labelLow.Size = new System.Drawing.Size(38, 24);
            this.labelLow.TabIndex = 0;
            this.labelLow.Text = "Low";
            this.labelLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // inputHigher
            // 
            this.inputHigher.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputHigher.Font = new System.Drawing.Font("仿宋", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inputHigher.Location = new System.Drawing.Point(92, 70);
            this.inputHigher.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.inputHigher.Name = "inputHigher";
            this.inputHigher.Size = new System.Drawing.Size(313, 59);
            this.inputHigher.TabIndex = 1;
            this.inputHigher.Text = "0";
            // 
            // labelHigh
            // 
            this.labelHigh.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.labelHigh.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelHigh.Location = new System.Drawing.Point(20, 88);
            this.labelHigh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelHigh.Name = "labelHigh";
            this.labelHigh.Size = new System.Drawing.Size(44, 24);
            this.labelHigh.TabIndex = 0;
            this.labelHigh.Text = "High";
            this.labelHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pageHeader1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(432, 248);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // pageHeader1
            // 
            this.pageHeader1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageHeader1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader1.Location = new System.Drawing.Point(3, 4);
            this.pageHeader1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pageHeader1.MaximizeBox = false;
            this.pageHeader1.Name = "pageHeader1";
            this.pageHeader1.ShowButton = true;
            this.pageHeader1.Size = new System.Drawing.Size(426, 34);
            this.pageHeader1.TabIndex = 0;
            this.pageHeader1.Text = "Spect Range Input";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnConfirm);
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.inputHigher);
            this.panel3.Controls.Add(this.labelHigh);
            this.panel3.Controls.Add(this.inputLower);
            this.panel3.Controls.Add(this.labelLow);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 46);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(426, 198);
            this.panel3.TabIndex = 1;
            // 
            // WinSPECInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 248);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "WinSPECInput";
            this.Text = "WinSPECInput";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private AntdUI.Button btnConfirm;
        private AntdUI.InputNumber inputLower;
        private AntdUI.Label labelLow;
        private AntdUI.InputNumber inputHigher;
        private AntdUI.Button btnCancel;
        private AntdUI.Label labelHigh;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.PageHeader pageHeader1;
        private System.Windows.Forms.Panel panel3;
    }
}