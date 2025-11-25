namespace VDEApp.Views.Display
{
    partial class WProductDisplaybind
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
            this.pageHeader1 = new AntdUI.PageHeader();
            this.select1 = new AntdUI.Select();
            this.lbTaskName = new AntdUI.Label();
            this.treeNodeRecord = new AntdUI.Tree();
            this.lbNodeRecord = new AntdUI.Label();
            this.btnRefreshRecord = new AntdUI.Button();
            this.btnRecordBind = new AntdUI.Button();
            this.lbRecordDisplayName = new AntdUI.Label();
            this.txtRecordDisplayName = new AntdUI.Input();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageHeader1
            // 
            this.pageHeader1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageHeader1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader1.Location = new System.Drawing.Point(3, 3);
            this.pageHeader1.MaximizeBox = false;
            this.pageHeader1.Name = "pageHeader1";
            this.pageHeader1.ShowButton = true;
            this.pageHeader1.Size = new System.Drawing.Size(474, 29);
            this.pageHeader1.TabIndex = 0;
            this.pageHeader1.Text = "显示绑定界面";
            // 
            // select1
            // 
            this.select1.Location = new System.Drawing.Point(113, 3);
            this.select1.Name = "select1";
            this.select1.List = true;
            this.select1.Size = new System.Drawing.Size(266, 40);
            this.select1.TabIndex = 1;
            this.select1.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.Select1_SelectedValueChanged);
            // 
            // lbTaskName
            // 
            this.lbTaskName.Location = new System.Drawing.Point(35, 3);
            this.lbTaskName.Name = "lbTaskName";
            this.lbTaskName.Size = new System.Drawing.Size(72, 40);
            this.lbTaskName.TabIndex = 2;
            this.lbTaskName.Text = "任务";
            // 
            // treeNodeRecord
            // 
            this.treeNodeRecord.BlockNode = true;
            this.treeNodeRecord.Gap = 3;
            this.treeNodeRecord.Location = new System.Drawing.Point(113, 45);
            this.treeNodeRecord.Name = "treeNodeRecord";
            this.treeNodeRecord.Size = new System.Drawing.Size(319, 228);
            this.treeNodeRecord.TabIndex = 5;
            this.treeNodeRecord.SelectChanged += new AntdUI.TreeSelectEventHandler(this.TreeNodeRecord_SelectChanged);
            // 
            // lbNodeRecord
            // 
            this.lbNodeRecord.Location = new System.Drawing.Point(35, 45);
            this.lbNodeRecord.Name = "lbNodeRecord";
            this.lbNodeRecord.Size = new System.Drawing.Size(72, 40);
            this.lbNodeRecord.TabIndex = 4;
            this.lbNodeRecord.Text = "节点/记录";
            // 
            // btnRefreshRecord
            // 
            this.btnRefreshRecord.Location = new System.Drawing.Point(385, 3);
            this.btnRefreshRecord.Name = "btnRefreshRecord";
            this.btnRefreshRecord.Size = new System.Drawing.Size(47, 40);
            this.btnRefreshRecord.TabIndex = 9;
            this.btnRefreshRecord.Text = "↻";
            // 
            // btnRecordBind
            // 
            this.btnRecordBind.DefaultBack = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.btnRecordBind.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRecordBind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnRecordBind.Location = new System.Drawing.Point(312, 325);
            this.btnRecordBind.Name = "btnRecordBind";
            this.btnRecordBind.Size = new System.Drawing.Size(120, 40);
            this.btnRecordBind.TabIndex = 8;
            this.btnRecordBind.Text = "确认绑定";
            this.btnRecordBind.Click += new System.EventHandler(this.BtnRecordBind_Click);
            // 
            // lbRecordDisplayName
            // 
            this.lbRecordDisplayName.Location = new System.Drawing.Point(35, 279);
            this.lbRecordDisplayName.Name = "lbRecordDisplayName";
            this.lbRecordDisplayName.Size = new System.Drawing.Size(72, 40);
            this.lbRecordDisplayName.TabIndex = 10;
            this.lbRecordDisplayName.Text = "显示名称";
            // 
            // txtRecordDisplayName
            // 
            this.txtRecordDisplayName.Location = new System.Drawing.Point(113, 279);
            this.txtRecordDisplayName.Name = "txtRecordDisplayName";
            this.txtRecordDisplayName.Size = new System.Drawing.Size(319, 40);
            this.txtRecordDisplayName.TabIndex = 11;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pageHeader1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(480, 417);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbTaskName);
            this.panel1.Controls.Add(this.btnRecordBind);
            this.panel1.Controls.Add(this.select1);
            this.panel1.Controls.Add(this.lbNodeRecord);
            this.panel1.Controls.Add(this.treeNodeRecord);
            this.panel1.Controls.Add(this.btnRefreshRecord);
            this.panel1.Controls.Add(this.lbRecordDisplayName);
            this.panel1.Controls.Add(this.txtRecordDisplayName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 376);
            this.panel1.TabIndex = 1;
            // 
            // WProductDisplaybind
            // 
            this.ClientSize = new System.Drawing.Size(480, 417);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.Name = "WProductDisplaybind";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WProductDisplaybind";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader1;
        private AntdUI.Select select1;
        private AntdUI.Label lbTaskName;
        private AntdUI.Label lbNodeRecord; // 修改标签名称
        private AntdUI.Tree treeNodeRecord; // 新增Tree控件
        private AntdUI.Button btnRefreshRecord;
        private AntdUI.Button btnRecordBind;
        private AntdUI.Label lbRecordDisplayName;
        private AntdUI.Input txtRecordDisplayName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
    }
}
