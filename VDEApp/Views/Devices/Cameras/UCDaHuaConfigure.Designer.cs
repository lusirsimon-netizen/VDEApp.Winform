namespace VDEApp.Views.Devices.Cameras
{
    partial class UCDaHuaConfigure
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbDevices = new AntdUI.Label();
            this.cbbDevices = new AntdUI.Dropdown();
            this.tabs = new AntdUI.Tabs();
            this.tabTrigger = new AntdUI.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.swEnable = new AntdUI.Switch();
            this.cbbMode = new AntdUI.Dropdown();
            this.lbEnable = new AntdUI.Label();
            this.lbMode = new AntdUI.Label();
            this.tabImageFormat = new AntdUI.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.lbFormat = new AntdUI.Label();
            this.cbbFormat = new AntdUI.Dropdown();
            this.tabSettings = new AntdUI.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lbBatchSize = new AntdUI.Label();
            this.lbTimeout = new AntdUI.Label();
            this.lbExposureTime = new AntdUI.Label();
            this.lbGain = new AntdUI.Label();
            this.inBatchSize = new AntdUI.InputNumber();
            this.inExposureTime = new AntdUI.InputNumber();
            this.inGain = new AntdUI.InputNumber();
            this.inTimeout = new AntdUI.InputNumber();
            this.tabInformation = new AntdUI.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lbType = new AntdUI.Label();
            this.txtType = new AntdUI.Input();
            this.lbIPSN = new AntdUI.Label();
            this.txtIPSN = new AntdUI.Input();
            this.lbCameraKey = new AntdUI.Label();
            this.txtCameraKey = new AntdUI.Input();
            this.lbVendorName = new AntdUI.Label();
            this.txtVendorName = new AntdUI.Input();
            this.lbVersion = new AntdUI.Label();
            this.txtVersion = new AntdUI.Input();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabTrigger.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tabImageFormat.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabInformation.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbDevices, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbbDevices, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabs, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(658, 811);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // lbDevices
            //
            this.lbDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDevices.Location = new System.Drawing.Point(3, 3);
            this.lbDevices.Name = "lbDevices";
            this.lbDevices.Size = new System.Drawing.Size(54, 44);
            this.lbDevices.TabIndex = 0;
            this.lbDevices.Text = "Devices";
            //
            // cbbDevices
            //
            this.cbbDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbbDevices.Location = new System.Drawing.Point(63, 3);
            this.cbbDevices.Name = "cbbDevices";
            this.cbbDevices.Size = new System.Drawing.Size(592, 44);
            this.cbbDevices.TabIndex = 1;
            //
            // tabs
            //
            this.tableLayoutPanel1.SetColumnSpan(this.tabs, 2);
            this.tabs.Controls.Add(this.tabSettings);
            this.tabs.Controls.Add(this.tabInformation);
            this.tabs.Controls.Add(this.tabTrigger);
            this.tabs.Controls.Add(this.tabImageFormat);
            this.tabs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(3, 53);
            this.tabs.Name = "tabs";
            this.tabs.Pages.Add(this.tabInformation);
            this.tabs.Pages.Add(this.tabSettings);
            this.tabs.Pages.Add(this.tabTrigger);
            this.tabs.Pages.Add(this.tabImageFormat);
            this.tabs.SelectedIndex = 1;
            this.tabs.Size = new System.Drawing.Size(652, 755);
            this.tabs.Style = styleLine1;
            this.tabs.TabIndex = 8;
            this.tabs.Text = "tabs1";
            //
            // tabTrigger
            //
            this.tabTrigger.Controls.Add(this.tableLayoutPanel4);
            this.tabTrigger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTrigger.Enabled = false;
            this.tabTrigger.Location = new System.Drawing.Point(0, 31);
            this.tabTrigger.Name = "tabTrigger";
            this.tabTrigger.Size = new System.Drawing.Size(652, 724);
            this.tabTrigger.TabIndex = 2;
            this.tabTrigger.Text = "Trigger";
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.swEnable, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.cbbMode, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.lbEnable, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lbMode, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(652, 724);
            this.tableLayoutPanel4.TabIndex = 2;
            //
            // swEnable
            //
            this.swEnable.Dock = System.Windows.Forms.DockStyle.Right;
            this.swEnable.Location = new System.Drawing.Point(567, 3);
            this.swEnable.Name = "swEnable";
            this.swEnable.Size = new System.Drawing.Size(82, 44);
            this.swEnable.TabIndex = 0;
            this.swEnable.Text = "switch1";
            //
            // cbbMode
            //
            this.cbbMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbbMode.Location = new System.Drawing.Point(76, 53);
            this.cbbMode.Name = "cbbMode";
            this.cbbMode.Size = new System.Drawing.Size(573, 44);
            this.cbbMode.TabIndex = 1;
            //
            // lbEnable
            //
            this.lbEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbEnable.Location = new System.Drawing.Point(3, 3);
            this.lbEnable.Name = "lbEnable";
            this.lbEnable.Size = new System.Drawing.Size(67, 44);
            this.lbEnable.TabIndex = 2;
            this.lbEnable.Text = "Enable";
            //
            // lbMode
            //
            this.lbMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMode.Location = new System.Drawing.Point(3, 53);
            this.lbMode.Name = "lbMode";
            this.lbMode.Size = new System.Drawing.Size(67, 44);
            this.lbMode.TabIndex = 3;
            this.lbMode.Text = "Mode";
            //
            // tabImageFormat
            //
            this.tabImageFormat.Controls.Add(this.tableLayoutPanel5);
            this.tabImageFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabImageFormat.Enabled = false;
            this.tabImageFormat.Location = new System.Drawing.Point(0, 31);
            this.tabImageFormat.Name = "tabImageFormat";
            this.tabImageFormat.Size = new System.Drawing.Size(652, 724);
            this.tabImageFormat.TabIndex = 3;
            this.tabImageFormat.Text = "Image Format";
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.lbFormat, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cbbFormat, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(652, 724);
            this.tableLayoutPanel5.TabIndex = 0;
            //
            // lbFormat
            //
            this.lbFormat.Location = new System.Drawing.Point(3, 3);
            this.lbFormat.Name = "lbFormat";
            this.lbFormat.Size = new System.Drawing.Size(67, 44);
            this.lbFormat.TabIndex = 0;
            this.lbFormat.Text = "Format";
            //
            // cbbFormat
            //
            this.cbbFormat.Location = new System.Drawing.Point(76, 3);
            this.cbbFormat.Name = "cbbFormat";
            this.cbbFormat.Size = new System.Drawing.Size(533, 44);
            this.cbbFormat.TabIndex = 1;
            //
            // tabSettings
            //
            this.tabSettings.Controls.Add(this.tableLayoutPanel3);
            this.tabSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSettings.Enabled = false;
            this.tabSettings.Location = new System.Drawing.Point(0, 31);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Showed = true;
            this.tabSettings.Size = new System.Drawing.Size(652, 724);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.lbBatchSize, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lbTimeout, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lbExposureTime, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lbGain, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.inBatchSize, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.inExposureTime, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.inGain, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.inTimeout, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(652, 724);
            this.tableLayoutPanel3.TabIndex = 0;
            //
            // lbBatchSize
            //
            this.lbBatchSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBatchSize.Location = new System.Drawing.Point(3, 3);
            this.lbBatchSize.Name = "lbBatchSize";
            this.lbBatchSize.Size = new System.Drawing.Size(67, 44);
            this.lbBatchSize.TabIndex = 0;
            this.lbBatchSize.Text = "Batch Size";
            //
            // lbTimeout
            //
            this.lbTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTimeout.Location = new System.Drawing.Point(3, 53);
            this.lbTimeout.Name = "lbTimeout";
            this.lbTimeout.Size = new System.Drawing.Size(67, 44);
            this.lbTimeout.TabIndex = 1;
            this.lbTimeout.Text = "Timeout";
            //
            // lbExposureTime
            //
            this.lbExposureTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbExposureTime.Location = new System.Drawing.Point(3, 103);
            this.lbExposureTime.Name = "lbExposureTime";
            this.lbExposureTime.Size = new System.Drawing.Size(67, 44);
            this.lbExposureTime.TabIndex = 2;
            this.lbExposureTime.Text = "Exposure Time";
            //
            // lbGain
            //
            this.lbGain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbGain.Location = new System.Drawing.Point(3, 153);
            this.lbGain.Name = "lbGain";
            this.lbGain.Size = new System.Drawing.Size(67, 44);
            this.lbGain.TabIndex = 3;
            this.lbGain.Text = "Gain";
            //
            // inBatchSize
            //
            this.inBatchSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inBatchSize.Location = new System.Drawing.Point(76, 3);
            this.inBatchSize.Name = "inBatchSize";
            this.inBatchSize.Size = new System.Drawing.Size(573, 44);
            this.inBatchSize.TabIndex = 4;
            this.inBatchSize.Text = "0";
            //
            // inExposureTime
            //
            this.inExposureTime.DecimalPlaces = 2;
            this.inExposureTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inExposureTime.Location = new System.Drawing.Point(76, 103);
            this.inExposureTime.Name = "inExposureTime";
            this.inExposureTime.Size = new System.Drawing.Size(573, 44);
            this.inExposureTime.TabIndex = 6;
            this.inExposureTime.Text = "0.00";
            //
            // inGain
            //
            this.inGain.DecimalPlaces = 2;
            this.inGain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inGain.Location = new System.Drawing.Point(76, 153);
            this.inGain.Name = "inGain";
            this.inGain.Size = new System.Drawing.Size(573, 44);
            this.inGain.TabIndex = 7;
            this.inGain.Text = "0.00";
            //
            // inTimeout
            //
            this.inTimeout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inTimeout.Location = new System.Drawing.Point(76, 53);
            this.inTimeout.Name = "inTimeout";
            this.inTimeout.Size = new System.Drawing.Size(573, 44);
            this.inTimeout.TabIndex = 8;
            this.inTimeout.Text = "0";
            //
            // tabInformation
            //
            this.tabInformation.Controls.Add(this.tableLayoutPanel2);
            this.tabInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabInformation.Location = new System.Drawing.Point(0, 31);
            this.tabInformation.Name = "tabInformation";
            this.tabInformation.Size = new System.Drawing.Size(652, 724);
            this.tabInformation.TabIndex = 0;
            this.tabInformation.Text = "Information";
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.lbType, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtType, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbIPSN, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtIPSN, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lbCameraKey, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtCameraKey, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lbVendorName, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtVendorName, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lbVersion, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtVersion, 1, 4);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(652, 724);
            this.tableLayoutPanel2.TabIndex = 0;
            //
            // lbType
            //
            this.lbType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbType.Location = new System.Drawing.Point(3, 3);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(54, 44);
            this.lbType.TabIndex = 2;
            this.lbType.Text = "Type";
            //
            // txtType
            //
            this.txtType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtType.Location = new System.Drawing.Point(63, 3);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(586, 44);
            this.txtType.TabIndex = 4;
            //
            // lbIPSN
            //
            this.lbIPSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbIPSN.Location = new System.Drawing.Point(3, 53);
            this.lbIPSN.Name = "lbIPSN";
            this.lbIPSN.Size = new System.Drawing.Size(54, 44);
            this.lbIPSN.TabIndex = 3;
            this.lbIPSN.Text = "IPSN";
            //
            // txtIPSN
            //
            this.txtIPSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIPSN.Location = new System.Drawing.Point(63, 53);
            this.txtIPSN.Name = "txtIPSN";
            this.txtIPSN.ReadOnly = true;
            this.txtIPSN.Size = new System.Drawing.Size(586, 44);
            this.txtIPSN.TabIndex = 5;
            //
            // lbCameraKey
            //
            this.lbCameraKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCameraKey.Location = new System.Drawing.Point(3, 103);
            this.lbCameraKey.Name = "lbCameraKey";
            this.lbCameraKey.Size = new System.Drawing.Size(54, 44);
            this.lbCameraKey.TabIndex = 0;
            this.lbCameraKey.Text = "Key";
            //
            // txtCameraKey
            //
            this.txtCameraKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCameraKey.Location = new System.Drawing.Point(63, 103);
            this.txtCameraKey.Name = "txtCameraKey";
            this.txtCameraKey.ReadOnly = true;
            this.txtCameraKey.Size = new System.Drawing.Size(586, 44);
            this.txtCameraKey.TabIndex = 5;
            //
            // lbVendorName
            //
            this.lbVendorName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbVendorName.Location = new System.Drawing.Point(3, 153);
            this.lbVendorName.Name = "lbVendorName";
            this.lbVendorName.Size = new System.Drawing.Size(54, 44);
            this.lbVendorName.TabIndex = 1;
            this.lbVendorName.Text = "Vendor";
            //
            // txtVendorName
            //
            this.txtVendorName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVendorName.Location = new System.Drawing.Point(63, 153);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.ReadOnly = true;
            this.txtVendorName.Size = new System.Drawing.Size(586, 44);
            this.txtVendorName.TabIndex = 6;
            //
            // lbVersion
            //
            this.lbVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbVersion.Location = new System.Drawing.Point(3, 203);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(54, 44);
            this.lbVersion.TabIndex = 2;
            this.lbVersion.Text = "Version";
            //
            // txtVersion
            //
            this.txtVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtVersion.Location = new System.Drawing.Point(63, 203);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ReadOnly = true;
            this.txtVersion.Size = new System.Drawing.Size(586, 44);
            this.txtVersion.TabIndex = 7;
            //
            // UCDaHuaConfigure
            //
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UCDaHuaConfigure";
            this.Size = new System.Drawing.Size(658, 811);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabTrigger.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tabImageFormat.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabInformation.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Label lbDevices;
        private AntdUI.Dropdown cbbDevices;
        private AntdUI.Label lbType;
        private AntdUI.Label lbIPSN;
        private AntdUI.Input txtType;
        private AntdUI.Input txtIPSN;
        private AntdUI.Tabs tabs;
        private AntdUI.TabPage tabInformation;
        private AntdUI.TabPage tabSettings;
        private AntdUI.TabPage tabTrigger;
        private AntdUI.TabPage tabImageFormat;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Label lbCameraKey;
        private AntdUI.Label lbVendorName;
        private AntdUI.Label lbVersion;
        private AntdUI.Input txtCameraKey;
        private AntdUI.Input txtVendorName;
        private AntdUI.Input txtVersion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private AntdUI.Label lbBatchSize;
        private AntdUI.Label lbTimeout;
        private AntdUI.Label lbExposureTime;
        private AntdUI.Label lbGain;
        private AntdUI.InputNumber inBatchSize;
        private AntdUI.InputNumber inExposureTime;
        private AntdUI.InputNumber inGain;
        private AntdUI.InputNumber inTimeout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private AntdUI.Switch swEnable;
        private AntdUI.Dropdown cbbMode;
        private AntdUI.Label lbEnable;
        private AntdUI.Label lbMode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private AntdUI.Label lbFormat;
        private AntdUI.Dropdown cbbFormat;
    }
}
