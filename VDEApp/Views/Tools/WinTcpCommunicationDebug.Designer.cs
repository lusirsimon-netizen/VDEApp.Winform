namespace VDEApp.Views.Tools
{
    partial class WinTcpCommunicationDebug
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
            this.TCPHeader = new AntdUI.PageHeader();
            this.labelTcpIP = new AntdUI.Label();
            this.TcpTextIP = new AntdUI.Input();
            this.btnTcpReStart = new AntdUI.Button();
            this.TcpBtnStop = new AntdUI.Button();
            this.labelTcpPort = new AntdUI.Label();
            this.TcpTextPort = new AntdUI.Input();
            this.TCPGroupresave = new System.Windows.Forms.GroupBox();
            this.TcpTextReceive = new AntdUI.Input();
            this.TcpBtnReceiveClear = new AntdUI.Button();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SelectTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.AllSwitch = new AntdUI.Switch();
            this.labelAll = new AntdUI.Label();
            this.TCPGroupresave.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SelectTableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // TCPHeader
            // 
            this.tableLayoutPanel10.SetColumnSpan(this.TCPHeader, 6);
            this.TCPHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TCPHeader.Location = new System.Drawing.Point(3, 3);
            this.TCPHeader.MaximumSize = new System.Drawing.Size(0, 25);
            this.TCPHeader.Name = "TCPHeader";
            this.TCPHeader.ShowButton = true;
            this.TCPHeader.Size = new System.Drawing.Size(637, 25);
            this.TCPHeader.TabIndex = 0;
            this.TCPHeader.Text = "WinTcpCommunicationDebug";
            // 
            // labelTcpIP
            // 
            this.labelTcpIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTcpIP.Location = new System.Drawing.Point(3, 43);
            this.labelTcpIP.Name = "labelTcpIP";
            this.labelTcpIP.Size = new System.Drawing.Size(44, 54);
            this.labelTcpIP.TabIndex = 2;
            this.labelTcpIP.Text = "IP：";
            this.labelTcpIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TcpTextIP
            // 
            this.TcpTextIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TcpTextIP.Location = new System.Drawing.Point(53, 43);
            this.TcpTextIP.Name = "TcpTextIP";
            this.TcpTextIP.Size = new System.Drawing.Size(161, 54);
            this.TcpTextIP.TabIndex = 3;
            this.TcpTextIP.Text = "127.0.0.1";
            this.TcpTextIP.TextChanged += new System.EventHandler(this.TcpTextIP_TextChanged);
            // 
            // btnTcpReStart
            // 
            this.btnTcpReStart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnTcpReStart.Location = new System.Drawing.Point(405, 48);
            this.btnTcpReStart.Name = "btnTcpReStart";
            this.btnTcpReStart.Size = new System.Drawing.Size(114, 44);
            this.btnTcpReStart.TabIndex = 4;
            this.btnTcpReStart.Text = "重新启动";
            this.btnTcpReStart.Click += new System.EventHandler(this.btnTcpReStart_Click);
            // 
            // TcpBtnStop
            // 
            this.TcpBtnStop.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TcpBtnStop.Location = new System.Drawing.Point(526, 51);
            this.TcpBtnStop.Name = "TcpBtnStop";
            this.TcpBtnStop.Size = new System.Drawing.Size(114, 38);
            this.TcpBtnStop.TabIndex = 5;
            this.TcpBtnStop.Text = "停止";
            this.TcpBtnStop.Click += new System.EventHandler(this.TcpBtnStop_Click);
            // 
            // labelTcpPort
            // 
            this.labelTcpPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTcpPort.Location = new System.Drawing.Point(220, 43);
            this.labelTcpPort.Name = "labelTcpPort";
            this.labelTcpPort.Size = new System.Drawing.Size(54, 54);
            this.labelTcpPort.TabIndex = 2;
            this.labelTcpPort.Text = "Port：";
            this.labelTcpPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TcpTextPort
            // 
            this.TcpTextPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TcpTextPort.Location = new System.Drawing.Point(280, 43);
            this.TcpTextPort.Name = "TcpTextPort";
            this.TcpTextPort.Size = new System.Drawing.Size(119, 54);
            this.TcpTextPort.TabIndex = 3;
            this.TcpTextPort.Text = "8080";
            this.TcpTextPort.TextChanged += new System.EventHandler(this.TcpTextPort_TextChanged);
            // 
            // TCPGroupresave
            // 
            this.tableLayoutPanel10.SetColumnSpan(this.TCPGroupresave, 4);
            this.TCPGroupresave.Controls.Add(this.TcpTextReceive);
            this.TCPGroupresave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TCPGroupresave.Location = new System.Drawing.Point(220, 103);
            this.TCPGroupresave.Name = "TCPGroupresave";
            this.TCPGroupresave.Size = new System.Drawing.Size(420, 248);
            this.TCPGroupresave.TabIndex = 0;
            this.TCPGroupresave.TabStop = false;
            this.TCPGroupresave.Text = "接收框";
            // 
            // TcpTextReceive
            // 
            this.TcpTextReceive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TcpTextReceive.Location = new System.Drawing.Point(3, 24);
            this.TcpTextReceive.Multiline = true;
            this.TcpTextReceive.Name = "TcpTextReceive";
            this.TcpTextReceive.ReadOnly = true;
            this.TcpTextReceive.Size = new System.Drawing.Size(414, 221);
            this.TcpTextReceive.TabIndex = 0;
            // 
            // TcpBtnReceiveClear
            // 
            this.TcpBtnReceiveClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TcpBtnReceiveClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(211)))), ((int)(((byte)(80)))));
            this.TcpBtnReceiveClear.Location = new System.Drawing.Point(526, 365);
            this.TcpBtnReceiveClear.Name = "TcpBtnReceiveClear";
            this.TcpBtnReceiveClear.Size = new System.Drawing.Size(114, 38);
            this.TcpBtnReceiveClear.TabIndex = 1;
            this.TcpBtnReceiveClear.Text = "清除";
            this.TcpBtnReceiveClear.Click += new System.EventHandler(this.TcpBtnReceiveClear_Click);
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.ColumnCount = 6;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57.14286F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.85714F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel10.Controls.Add(this.TcpTextPort, 3, 1);
            this.tableLayoutPanel10.Controls.Add(this.labelTcpPort, 2, 1);
            this.tableLayoutPanel10.Controls.Add(this.TcpBtnStop, 5, 1);
            this.tableLayoutPanel10.Controls.Add(this.btnTcpReStart, 4, 1);
            this.tableLayoutPanel10.Controls.Add(this.TcpTextIP, 1, 1);
            this.tableLayoutPanel10.Controls.Add(this.labelTcpIP, 0, 1);
            this.tableLayoutPanel10.Controls.Add(this.TCPHeader, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.TcpBtnReceiveClear, 5, 3);
            this.tableLayoutPanel10.Controls.Add(this.TCPGroupresave, 2, 2);
            this.tableLayoutPanel10.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 4;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(643, 414);
            this.tableLayoutPanel10.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel10.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.SelectTableLayout);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 103);
            this.groupBox1.Name = "groupBox1";
            this.tableLayoutPanel10.SetRowSpan(this.groupBox1, 2);
            this.groupBox1.Size = new System.Drawing.Size(211, 308);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择回复时机";
            // 
            // SelectTableLayout
            // 
            this.SelectTableLayout.ColumnCount = 2;
            this.SelectTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.36795F));
            this.SelectTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.63205F));
            this.SelectTableLayout.Controls.Add(this.AllSwitch, 1, 0);
            this.SelectTableLayout.Controls.Add(this.labelAll, 0, 0);
            this.SelectTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectTableLayout.Location = new System.Drawing.Point(3, 24);
            this.SelectTableLayout.Name = "SelectTableLayout";
            this.SelectTableLayout.RowCount = 5;
            this.SelectTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.SelectTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.SelectTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.SelectTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.SelectTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SelectTableLayout.Size = new System.Drawing.Size(205, 281);
            this.SelectTableLayout.TabIndex = 1;
            // 
            // AllSwitch
            // 
            this.AllSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AllSwitch.Location = new System.Drawing.Point(142, 3);
            this.AllSwitch.Name = "AllSwitch";
            this.AllSwitch.Size = new System.Drawing.Size(60, 34);
            this.AllSwitch.TabIndex = 0;
            this.AllSwitch.Text = "switch1";
            this.AllSwitch.CheckedChanged += new AntdUI.BoolEventHandler(this.AllSwitch_CheckedChanged);
            // 
            // labelAll
            // 
            this.labelAll.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelAll.Location = new System.Drawing.Point(3, 8);
            this.labelAll.Name = "labelAll";
            this.labelAll.Size = new System.Drawing.Size(72, 23);
            this.labelAll.TabIndex = 1;
            this.labelAll.Text = "所有";
            // 
            // WinTcpCommunicationDebug
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(643, 414);
            this.Controls.Add(this.tableLayoutPanel10);
            this.Name = "WinTcpCommunicationDebug";
            this.Text = "WinTcpCommunicationDebug";
            this.TCPGroupresave.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.SelectTableLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader TCPHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private AntdUI.Button TcpBtnReceiveClear;
        private System.Windows.Forms.GroupBox TCPGroupresave;
        private AntdUI.Input TcpTextReceive;
        private AntdUI.Input TcpTextPort;
        private AntdUI.Label labelTcpPort;
        private AntdUI.Button TcpBtnStop;
        private AntdUI.Button btnTcpReStart;
        private AntdUI.Input TcpTextIP;
        private AntdUI.Label labelTcpIP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel SelectTableLayout;
        private AntdUI.Switch AllSwitch;
        private AntdUI.Label labelAll;
    }
}