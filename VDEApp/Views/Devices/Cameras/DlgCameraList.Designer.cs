using System.Windows.Forms;

namespace VDEApp.Views.Devices.Cameras
{
    partial class DlgCameraList
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
            this.tlMain = new System.Windows.Forms.TableLayoutPanel();
            this.tlDialogButtonBox = new System.Windows.Forms.TableLayoutPanel();
            this.btnOK = new AntdUI.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddCamera = new AntdUI.Button();
            this.btnRemoveCamera = new AntdUI.Button();
            this.header = new AntdUI.PageHeader();
            this.tableCameras = new AntdUI.Table();
            this.tlMain.SuspendLayout();
            this.tlDialogButtonBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlMain
            // 
            this.tlMain.BackColor = System.Drawing.SystemColors.Menu;
            this.tlMain.ColumnCount = 1;
            this.tlMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlMain.Controls.Add(this.tlDialogButtonBox, 0, 3);
            this.tlMain.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tlMain.Controls.Add(this.header, 0, 0);
            this.tlMain.Controls.Add(this.tableCameras, 0, 2);
            this.tlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlMain.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlMain.Location = new System.Drawing.Point(0, 0);
            this.tlMain.Margin = new System.Windows.Forms.Padding(2);
            this.tlMain.Name = "tlMain";
            this.tlMain.RowCount = 4;
            this.tlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tlMain.Size = new System.Drawing.Size(899, 494);
            this.tlMain.TabIndex = 0;
            // 
            // tlDialogButtonBox
            // 
            this.tlDialogButtonBox.ColumnCount = 2;
            this.tlDialogButtonBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 86.24577F));
            this.tlDialogButtonBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.75423F));
            this.tlDialogButtonBox.Controls.Add(this.btnOK, 1, 0);
            this.tlDialogButtonBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlDialogButtonBox.Location = new System.Drawing.Point(2, 450);
            this.tlDialogButtonBox.Margin = new System.Windows.Forms.Padding(2);
            this.tlDialogButtonBox.Name = "tlDialogButtonBox";
            this.tlDialogButtonBox.Padding = new System.Windows.Forms.Padding(0, 4, 8, 4);
            this.tlDialogButtonBox.RowCount = 1;
            this.tlDialogButtonBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlDialogButtonBox.Size = new System.Drawing.Size(895, 42);
            this.tlDialogButtonBox.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(765, 4);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 34);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Type = AntdUI.TTypeMini.Primary;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnAddCamera, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRemoveCamera, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 37);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(4, 4, 8, 4);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(895, 42);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // btnAddCamera
            // 
            this.btnAddCamera.Location = new System.Drawing.Point(4, 4);
            this.btnAddCamera.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddCamera.Name = "btnAddCamera";
            this.btnAddCamera.Size = new System.Drawing.Size(61, 34);
            this.btnAddCamera.TabIndex = 2;
            this.btnAddCamera.Text = "Add";
            this.btnAddCamera.Click += new System.EventHandler(this.btnAddCamera_Click);
            // 
            // btnRemoveCamera
            // 
            this.btnRemoveCamera.Location = new System.Drawing.Point(65, 4);
            this.btnRemoveCamera.Margin = new System.Windows.Forms.Padding(0);
            this.btnRemoveCamera.Name = "btnRemoveCamera";
            this.btnRemoveCamera.Size = new System.Drawing.Size(62, 34);
            this.btnRemoveCamera.TabIndex = 3;
            this.btnRemoveCamera.Text = "Del";
            this.btnRemoveCamera.Click += new System.EventHandler(this.btnRemoveCamera_Click);
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.header.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.header.Location = new System.Drawing.Point(2, 2);
            this.header.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(895, 33);
            this.header.TabIndex = 4;
            this.header.Text = "Camera List";
            this.header.UseTextBold = false;
            // 
            // tableCameras
            // 
            this.tableCameras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCameras.Gap = 6;
            this.tableCameras.Gaps = new System.Drawing.Size(6, 6);
            this.tableCameras.Location = new System.Drawing.Point(2, 83);
            this.tableCameras.Margin = new System.Windows.Forms.Padding(2);
            this.tableCameras.Name = "tableCameras";
            this.tableCameras.Size = new System.Drawing.Size(895, 363);
            this.tableCameras.TabIndex = 5;
            this.tableCameras.Text = "table1";
            // 
            // DlgCameraList
            // 
            this.ClientSize = new System.Drawing.Size(899, 494);
            this.Controls.Add(this.tlMain);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DlgCameraList";
            this.Text = "Camera List";
            this.tlMain.ResumeLayout(false);
            this.tlDialogButtonBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tlMain;
        private TableLayoutPanel tlDialogButtonBox;
        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Button btnOK;
        private AntdUI.PageHeader header;
        private AntdUI.Table tableCameras;
        private AntdUI.Button btnAddCamera;
        private AntdUI.Button btnRemoveCamera;
    }
}
