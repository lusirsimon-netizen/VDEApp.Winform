using VDEApp.Configs;
using VDEApp.Models.Project;

namespace VDEApp.Views.ReplayImages
{
    partial class WinReplay
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
            this.flowLayoutPanel2 = new AntdUI.In.FlowLayoutPanel();
            this.ReplayText = new AntdUI.Label();
            this.lblNowTask = new AntdUI.Label();
            this.labImageFolder = new AntdUI.Label();
            this.InputImageFloder = new AntdUI.Input();
            this.btnSelectFloder = new AntdUI.Button();
            this.tabHeader1 = new AntdUI.TabHeader();
            this.panel1 = new AntdUI.Panel();
            this.InputRunCount = new AntdUI.Input();
            this.labRunCount = new AntdUI.Label();
            this.txtBoxTime = new AntdUI.Input();
            this.labTime = new AntdUI.Label();
            this.btnOneRun = new AntdUI.Button();
            this.btnContinueRun = new AntdUI.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.table1 = new AntdUI.Table();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.ReplayText);
            this.flowLayoutPanel2.Controls.Add(this.lblNowTask);
            this.flowLayoutPanel2.Controls.Add(this.labImageFolder);
            this.flowLayoutPanel2.Controls.Add(this.InputImageFloder);
            this.flowLayoutPanel2.Controls.Add(this.btnSelectFloder);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(4, 39);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(722, 44);
            this.flowLayoutPanel2.TabIndex = 8;
            // 
            // ReplayText
            // 
            this.ReplayText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ReplayText.Location = new System.Drawing.Point(4, 4);
            this.ReplayText.Margin = new System.Windows.Forms.Padding(4);
            this.ReplayText.Name = "ReplayText";
            this.ReplayText.Size = new System.Drawing.Size(59, 40);
            this.ReplayText.TabIndex = 1;
            this.ReplayText.Text = "当前任务";
            // 
            // lblNowTask
            // 
            this.lblNowTask.BackColor = System.Drawing.Color.Transparent;
            this.lblNowTask.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblNowTask.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
            this.lblNowTask.Location = new System.Drawing.Point(71, 4);
            this.lblNowTask.Margin = new System.Windows.Forms.Padding(4);
            this.lblNowTask.Name = "lblNowTask";
            this.lblNowTask.Size = new System.Drawing.Size(88, 41);
            this.lblNowTask.TabIndex = 9;
            this.lblNowTask.Text = "Task";
            // 
            // labImageFolder
            // 
            this.labImageFolder.Font = new System.Drawing.Font("宋体", 10.5F);
            this.labImageFolder.Location = new System.Drawing.Point(166, 3);
            this.labImageFolder.Name = "labImageFolder";
            this.labImageFolder.Size = new System.Drawing.Size(77, 41);
            this.labImageFolder.TabIndex = 3;
            this.labImageFolder.Text = "图片文件夹";
            // 
            // InputImageFloder
            // 
            this.InputImageFloder.Location = new System.Drawing.Point(249, 3);
            this.InputImageFloder.Name = "InputImageFloder";
            this.InputImageFloder.Size = new System.Drawing.Size(398, 42);
            this.InputImageFloder.TabIndex = 4;
            this.InputImageFloder.Text = "ImageFloder";
            // 
            // btnSelectFloder
            // 
            this.btnSelectFloder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectFloder.Location = new System.Drawing.Point(653, 3);
            this.btnSelectFloder.Name = "btnSelectFloder";
            this.btnSelectFloder.Size = new System.Drawing.Size(61, 43);
            this.btnSelectFloder.TabIndex = 5;
            this.btnSelectFloder.Text = "...";
            // 
            // tabHeader1
            // 
            this.tabHeader1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabHeader1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabHeader1.Location = new System.Drawing.Point(4, 4);
            this.tabHeader1.Margin = new System.Windows.Forms.Padding(4);
            this.tabHeader1.Name = "tabHeader1";
            this.tabHeader1.ShowButton = true;
            this.tabHeader1.Size = new System.Drawing.Size(722, 27);
            this.tabHeader1.TabIndex = 0;
            this.tabHeader1.Text = "仿图设置";
            this.tabHeader1.UseTextBold = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.InputRunCount);
            this.panel1.Controls.Add(this.labRunCount);
            this.panel1.Controls.Add(this.txtBoxTime);
            this.panel1.Controls.Add(this.labTime);
            this.panel1.Controls.Add(this.btnOneRun);
            this.panel1.Controls.Add(this.btnContinueRun);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 498);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(722, 52);
            this.panel1.TabIndex = 10;
            this.panel1.Text = "panel1";
            // 
            // InputRunCount
            // 
            this.InputRunCount.Location = new System.Drawing.Point(301, 10);
            this.InputRunCount.Name = "InputRunCount";
            this.InputRunCount.Size = new System.Drawing.Size(82, 34);
            this.InputRunCount.TabIndex = 10;
            this.InputRunCount.Text = "1";
            // 
            // labRunCount
            // 
            this.labRunCount.BackColor = System.Drawing.Color.Transparent;
            this.labRunCount.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labRunCount.Location = new System.Drawing.Point(232, 10);
            this.labRunCount.Margin = new System.Windows.Forms.Padding(4);
            this.labRunCount.Name = "labRunCount";
            this.labRunCount.Size = new System.Drawing.Size(84, 34);
            this.labRunCount.TabIndex = 9;
            this.labRunCount.Text = "运行次数";
            // 
            // txtBoxTime
            // 
            this.txtBoxTime.CaretSpeed = 1;
            this.txtBoxTime.Location = new System.Drawing.Point(123, 10);
            this.txtBoxTime.Name = "txtBoxTime";
            this.txtBoxTime.Size = new System.Drawing.Size(82, 34);
            this.txtBoxTime.TabIndex = 8;
            this.txtBoxTime.Text = "1";
            // 
            // labTime
            // 
            this.labTime.BackColor = System.Drawing.Color.Transparent;
            this.labTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labTime.Location = new System.Drawing.Point(14, 9);
            this.labTime.Margin = new System.Windows.Forms.Padding(4);
            this.labTime.Name = "labTime";
            this.labTime.Size = new System.Drawing.Size(130, 34);
            this.labTime.TabIndex = 6;
            this.labTime.Text = "时间间隔(s)";
            // 
            // btnOneRun
            // 
            this.btnOneRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOneRun.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOneRun.Location = new System.Drawing.Point(474, 7);
            this.btnOneRun.Margin = new System.Windows.Forms.Padding(4);
            this.btnOneRun.Name = "btnOneRun";
            this.btnOneRun.Size = new System.Drawing.Size(120, 45);
            this.btnOneRun.TabIndex = 5;
            this.btnOneRun.Text = "单步运行";
            this.btnOneRun.Type = AntdUI.TTypeMini.Primary;
            this.btnOneRun.Click += new System.EventHandler(this.btnOneRun_Click);
            // 
            // btnContinueRun
            // 
            this.btnContinueRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinueRun.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnContinueRun.Location = new System.Drawing.Point(593, 7);
            this.btnContinueRun.Margin = new System.Windows.Forms.Padding(4);
            this.btnContinueRun.Name = "btnContinueRun";
            this.btnContinueRun.Size = new System.Drawing.Size(120, 45);
            this.btnContinueRun.TabIndex = 3;
            this.btnContinueRun.Text = "持续运行";
            this.btnContinueRun.Type = AntdUI.TTypeMini.Primary;
            this.btnContinueRun.Click += new System.EventHandler(this.btnContinueRun_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.table1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabHeader1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(730, 554);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // table1
            // 
            this.table1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.table1.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.table1.BackColor = System.Drawing.Color.White;
            this.table1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.table1.Gap = 6;
            this.table1.Gaps = new System.Drawing.Size(6, 6);
            this.table1.Location = new System.Drawing.Point(10, 97);
            this.table1.Margin = new System.Windows.Forms.Padding(10);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(710, 387);
            this.table1.TabIndex = 4;
            this.table1.Text = "table1";
            // 
            // WinReplay
            // 
            this.ClientSize = new System.Drawing.Size(730, 554);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(730, 554);
            this.Name = "WinReplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WinReplay";
            this.flowLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.In.FlowLayoutPanel flowLayoutPanel2;
        private AntdUI.Label labImageFolder;
        private AntdUI.Input InputImageFloder;
        private AntdUI.Button btnSelectFloder;
        public AntdUI.TabHeader tabHeader1;
        private AntdUI.Panel panel1;
        private AntdUI.Input txtBoxTime;
        private AntdUI.Label labTime;
        private AntdUI.Button btnOneRun;
        private AntdUI.Button btnContinueRun;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Table table1;
        private AntdUI.Label ReplayText;
        private AntdUI.Label lblNowTask;
        private AntdUI.Input InputRunCount;
        private AntdUI.Label labRunCount;
    }
}
