namespace VDEApp.Views.Devices.Cameras
{
    partial class DlgAddCamera
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.header = new AntdUI.PageHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbName = new System.Windows.Forms.Label();
            this.lbModel = new System.Windows.Forms.Label();
            this.btnOK = new AntdUI.Button();
            this.inName = new AntdUI.Input();
            this.btnCancel = new AntdUI.Button();
            this.cbbModel = new AntdUI.Dropdown();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Menu;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.header, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(12, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(356, 180);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.header.Font = new System.Drawing.Font("宋体", 10.5F);
            this.header.Location = new System.Drawing.Point(2, 2);
            this.header.Margin = new System.Windows.Forms.Padding(2);
            this.header.MaximizeBox = false;
            this.header.Name = "header";
            this.header.ShowButton = true;
            this.header.Size = new System.Drawing.Size(352, 31);
            this.header.TabIndex = 0;
            this.header.Text = "Add Camera";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbName);
            this.panel1.Controls.Add(this.lbModel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.inName);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.cbbModel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(350, 139);
            this.panel1.TabIndex = 1;
            // 
            // lblName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbName.Location = new System.Drawing.Point(15, 27);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(35, 14);
            this.lbName.TabIndex = 9;
            this.lbName.Text = "Name";
            // 
            // lblMode
            // 
            this.lbModel.AutoSize = true;
            this.lbModel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbModel.Location = new System.Drawing.Point(15, 59);
            this.lbModel.Name = "lbModel";
            this.lbModel.Size = new System.Drawing.Size(35, 14);
            this.lbModel.TabIndex = 8;
            this.lbModel.Text = "Model";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(112, 87);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 33);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Type = AntdUI.TTypeMini.Primary;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // inName
            // 
            this.inName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inName.Location = new System.Drawing.Point(64, 16);
            this.inName.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.inName.MaximumSize = new System.Drawing.Size(0, 40);
            this.inName.MinimumSize = new System.Drawing.Size(0, 30);
            this.inName.Name = "inName";
            this.inName.Size = new System.Drawing.Size(268, 35);
            this.inName.TabIndex = 5;
            this.inName.Text = "Camera";
            this.inName.TextChanged += new System.EventHandler(this.inName_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(224, 87);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(108, 33);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbbModel
            // 
            this.cbbModel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbbModel.Location = new System.Drawing.Point(64, 50);
            this.cbbModel.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.cbbModel.MaximumSize = new System.Drawing.Size(0, 40);
            this.cbbModel.MinimumSize = new System.Drawing.Size(0, 30);
            this.cbbModel.Name = "cbbModel";
            this.cbbModel.Size = new System.Drawing.Size(268, 35);
            this.cbbModel.TabIndex = 7;
            this.cbbModel.Text = "dropdown1";
            this.cbbModel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbbModel.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.cbbModel_SelectedValueChanged);
            // 
            // DlgAddCamera
            this.Resizable = false;
            this.ClientSize = new System.Drawing.Size(356, 180);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Dark = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(4800, 400);
            this.MinimumSize = new System.Drawing.Size(300, 180);
            this.Mode = AntdUI.TAMode.Dark;
            this.Name = "DlgAddCamera";
            this.Text = "DlgAddCamera";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Input inName;
        private AntdUI.Button btnOK;
        private AntdUI.Button btnCancel;
        private AntdUI.Dropdown cbbModel;
        private AntdUI.PageHeader header;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbModel;
    }
}
