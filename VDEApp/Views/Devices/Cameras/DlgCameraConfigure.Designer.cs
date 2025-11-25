namespace VDEApp.Views.Devices.Cameras
{
    partial class DlgCameraConfigure
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
            this.tlImagePreview = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCapture = new AntdUI.Button();
            this.btnConnect = new AntdUI.Button();
            this.btnDisconnect = new AntdUI.Button();
            this.gbDisplay = new System.Windows.Forms.GroupBox();
            this.tlLeft = new System.Windows.Forms.TableLayoutPanel();
            this.btnOK = new AntdUI.Button();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.header = new AntdUI.PageHeader();
            this.menu = new AntdUI.Menu();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tlImagePreview.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.header.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlImagePreview
            // 
            this.tlImagePreview.ColumnCount = 1;
            this.tlImagePreview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlImagePreview.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tlImagePreview.Controls.Add(this.gbDisplay, 0, 0);
            this.tlImagePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlImagePreview.Location = new System.Drawing.Point(0, 0);
            this.tlImagePreview.Name = "tlImagePreview";
            this.tlImagePreview.RowCount = 2;
            this.tlImagePreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlImagePreview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tlImagePreview.Size = new System.Drawing.Size(645, 500);
            this.tlImagePreview.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.Controls.Add(this.btnCapture, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnConnect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDisconnect, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 452);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(639, 45);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnCapture
            // 
            this.btnCapture.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCapture.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCapture.Location = new System.Drawing.Point(502, 3);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(134, 39);
            this.btnCapture.TabIndex = 0;
            this.btnCapture.Text = "Capture";
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConnect.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConnect.Location = new System.Drawing.Point(3, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(114, 39);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDisconnect.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDisconnect.Location = new System.Drawing.Point(123, 3);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(114, 39);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // gbDisplay
            // 
            this.gbDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDisplay.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbDisplay.Location = new System.Drawing.Point(3, 3);
            this.gbDisplay.Name = "gbDisplay";
            this.gbDisplay.Size = new System.Drawing.Size(639, 443);
            this.gbDisplay.TabIndex = 1;
            this.gbDisplay.TabStop = false;
            this.gbDisplay.Text = "Preview";
            // 
            // tlLeft
            // 
            this.tlLeft.ColumnCount = 1;
            this.tlLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlLeft.Controls.Add(this.btnOK, 0, 1);
            this.tlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlLeft.Location = new System.Drawing.Point(0, 0);
            this.tlLeft.Name = "tlLeft";
            this.tlLeft.RowCount = 2;
            this.tlLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tlLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlLeft.Size = new System.Drawing.Size(352, 500);
            this.tlLeft.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOK.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(3, 461);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(155, 36);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.Location = new System.Drawing.Point(3, 38);
            this.scMain.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.tlLeft);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.tlImagePreview);
            this.scMain.Size = new System.Drawing.Size(1001, 500);
            this.scMain.SplitterDistance = 352;
            this.scMain.TabIndex = 3;
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Controls.Add(this.menu);
            this.header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.header.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.header.Location = new System.Drawing.Point(3, 3);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(1001, 29);
            this.header.TabIndex = 4;
            this.header.Text = "Configure Camera";
            this.header.UseTextBold = false;
            // 
            // menu
            // 
            this.menu.Dock = System.Windows.Forms.DockStyle.Left;
            this.menu.Location = new System.Drawing.Point(180, 0);
            this.menu.Mode = AntdUI.TMenuMode.Horizontal;
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(99, 29);
            this.menu.TabIndex = 0;
            this.menu.Text = "menu1";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.header, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.scMain, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1007, 546);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // DlgCameraConfigure
            // 
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(1007, 546);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "DlgCameraConfigure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Camera Configure";
            this.tlImagePreview.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tlLeft.ResumeLayout(false);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.header.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlImagePreview;
        private System.Windows.Forms.TableLayoutPanel tlLeft;
        private System.Windows.Forms.SplitContainer scMain;
        private AntdUI.Button btnOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Button btnCapture;
        private AntdUI.Button btnConnect;
        private AntdUI.Button btnDisconnect;
        private AntdUI.PageHeader header;
        private AntdUI.Menu menu;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox gbDisplay;
    }
}
