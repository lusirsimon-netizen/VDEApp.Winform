namespace VDEApp.Views.Task
{
    partial class WinAcquire
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
            this.header = new AntdUI.PageHeader();
            this.lbCamera = new System.Windows.Forms.Label();
            this.cbbCamera = new AntdUI.Dropdown();
            this.btnOK = new AntdUI.Button();
            this.btnCancel = new AntdUI.Button();
            this.btnConfigure = new AntdUI.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.alert = new AntdUI.Alert();
            this.inExposureTime = new AntdUI.InputNumber();
            this.lbExposureTime = new System.Windows.Forms.Label();
            this.inTimeout = new AntdUI.InputNumber();
            this.lbTimeout = new System.Windows.Forms.Label();
            this.inBatchSize = new AntdUI.InputNumber();
            this.lbBatchSize = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.header.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Margin = new System.Windows.Forms.Padding(0);
            this.header.MaximizeBox = false;
            this.header.Name = "header";
            this.header.ShowButton = true;
            this.header.Size = new System.Drawing.Size(448, 35);
            this.header.TabIndex = 0;
            this.header.Text = "Acquire Node";
            this.header.UseTextBold = false;
            // 
            // lbCamera
            // 
            this.lbCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCamera.Location = new System.Drawing.Point(18, 19);
            this.lbCamera.Name = "lbCamera";
            this.lbCamera.Size = new System.Drawing.Size(80, 35);
            this.lbCamera.TabIndex = 1;
            this.lbCamera.Text = "Camera";
            this.lbCamera.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbbCamera
            // 
            this.cbbCamera.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbbCamera.Location = new System.Drawing.Point(99, 19);
            this.cbbCamera.Name = "cbbCamera";
            this.cbbCamera.Size = new System.Drawing.Size(215, 35);
            this.cbbCamera.TabIndex = 3;
            this.cbbCamera.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.cbbCamera_SelectedValueChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(164, 198);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(122, 41);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Type = AntdUI.TTypeMini.Primary;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(292, 198);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(122, 41);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfigure
            // 
            this.btnConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigure.Enabled = false;
            this.btnConfigure.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfigure.Location = new System.Drawing.Point(320, 19);
            this.btnConfigure.Name = "btnConfigure";
            this.btnConfigure.Size = new System.Drawing.Size(94, 35);
            this.btnConfigure.TabIndex = 4;
            this.btnConfigure.Text = "Configure";
            this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.header, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(448, 289);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.alert);
            this.panel1.Controls.Add(this.inExposureTime);
            this.panel1.Controls.Add(this.lbExposureTime);
            this.panel1.Controls.Add(this.inTimeout);
            this.panel1.Controls.Add(this.lbTimeout);
            this.panel1.Controls.Add(this.inBatchSize);
            this.panel1.Controls.Add(this.lbBatchSize);
            this.panel1.Controls.Add(this.btnConfigure);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.cbbCamera);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.lbCamera);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(442, 248);
            this.panel1.TabIndex = 1;
            // 
            // alert
            // 
            this.alert.Icon = AntdUI.TType.Warn;
            this.alert.Location = new System.Drawing.Point(9, 198);
            this.alert.Loop = true;
            this.alert.Name = "alert";
            this.alert.Size = new System.Drawing.Size(149, 41);
            this.alert.TabIndex = 11;
            this.alert.Text = "Camera is not Connected    ";
            // 
            // inExposureTime
            // 
            this.inExposureTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inExposureTime.DecimalPlaces = 2;
            this.inExposureTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.inExposureTime.Location = new System.Drawing.Point(99, 152);
            this.inExposureTime.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            this.inExposureTime.Name = "inExposureTime";
            this.inExposureTime.Size = new System.Drawing.Size(315, 40);
            this.inExposureTime.TabIndex = 10;
            this.inExposureTime.Text = "2.00";
            this.inExposureTime.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            // 
            // lbExposureTime
            // 
            this.lbExposureTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbExposureTime.Location = new System.Drawing.Point(18, 152);
            this.lbExposureTime.Name = "lbExposureTime";
            this.lbExposureTime.Size = new System.Drawing.Size(80, 40);
            this.lbExposureTime.TabIndex = 9;
            this.lbExposureTime.Text = "Exposure Time";
            this.lbExposureTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inTimeout
            // 
            this.inTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inTimeout.Location = new System.Drawing.Point(99, 106);
            this.inTimeout.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.inTimeout.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.inTimeout.Name = "inTimeout";
            this.inTimeout.Size = new System.Drawing.Size(315, 40);
            this.inTimeout.TabIndex = 8;
            this.inTimeout.Text = "0";
            // 
            // lbTimeout
            // 
            this.lbTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTimeout.Location = new System.Drawing.Point(18, 106);
            this.lbTimeout.Name = "lbTimeout";
            this.lbTimeout.Size = new System.Drawing.Size(80, 40);
            this.lbTimeout.TabIndex = 7;
            this.lbTimeout.Text = "Timeout";
            this.lbTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // inBatchSize
            // 
            this.inBatchSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inBatchSize.Location = new System.Drawing.Point(99, 60);
            this.inBatchSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.inBatchSize.Name = "inBatchSize";
            this.inBatchSize.Size = new System.Drawing.Size(315, 40);
            this.inBatchSize.TabIndex = 6;
            this.inBatchSize.Text = "1";
            this.inBatchSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbBatchSize
            // 
            this.lbBatchSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBatchSize.Location = new System.Drawing.Point(18, 60);
            this.lbBatchSize.Name = "lbBatchSize";
            this.lbBatchSize.Size = new System.Drawing.Size(80, 40);
            this.lbBatchSize.TabIndex = 5;
            this.lbBatchSize.Text = "Batch Size";
            this.lbBatchSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WinAcquire
            // 
            this.ClientSize = new System.Drawing.Size(448, 289);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "WinAcquire";
            this.Resizable = false;
            this.Text = "WinAcquire";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private AntdUI.PageHeader header;
        private System.Windows.Forms.Label lbCamera;
        private AntdUI.Button btnOK;
        private AntdUI.Button btnCancel;
        private AntdUI.Dropdown cbbCamera;
        private AntdUI.Button btnConfigure;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.InputNumber inBatchSize;
        private System.Windows.Forms.Label lbBatchSize;
        private AntdUI.InputNumber inExposureTime;
        private System.Windows.Forms.Label lbExposureTime;
        private AntdUI.InputNumber inTimeout;
        private System.Windows.Forms.Label lbTimeout;
        private AntdUI.Alert alert;
    }
}
