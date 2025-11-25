namespace VDEApp.Views.Devices.Cameras
{
    partial class UCVirtualConfigure
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.inPath = new AntdUI.Input();
            this.btnSelectPath = new AntdUI.Button();
            this.lbPath = new AntdUI.Label();
            this.inNumBatchSize = new AntdUI.InputNumber();
            this.lbBatchSize = new AntdUI.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Controls.Add(this.inPath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectPath, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbPath, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.inNumBatchSize, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbBatchSize, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(2220, 853);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // inPath
            // 
            this.inPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inPath.Location = new System.Drawing.Point(68, 8);
            this.inPath.Name = "inPath";
            this.inPath.Size = new System.Drawing.Size(2084, 39);
            this.inPath.TabIndex = 1;
            this.inPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inPath_KeyPress);
            this.inPath.Validating += new System.ComponentModel.CancelEventHandler(this.inPath_Validating);
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectPath.Location = new System.Drawing.Point(2158, 8);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(54, 39);
            this.btnSelectPath.TabIndex = 2;
            this.btnSelectPath.Text = "Select";
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // lbPath
            // 
            this.lbPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPath.Location = new System.Drawing.Point(8, 8);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(54, 39);
            this.lbPath.TabIndex = 3;
            this.lbPath.Text = "Path";
            this.lbPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inNumBatchSize
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.inNumBatchSize, 2);
            this.inNumBatchSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inNumBatchSize.Location = new System.Drawing.Point(68, 53);
            this.inNumBatchSize.Name = "inNumBatchSize";
            this.inNumBatchSize.Size = new System.Drawing.Size(2144, 39);
            this.inNumBatchSize.TabIndex = 5;
            this.inNumBatchSize.Text = "0";
            this.inNumBatchSize.ValueChanged += new AntdUI.DecimalEventHandler(this.inNumBatchSize_ValueChanged);
            // 
            // lbBatchSize
            // 
            this.lbBatchSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbBatchSize.Location = new System.Drawing.Point(8, 53);
            this.lbBatchSize.Name = "lbBatchSize";
            this.lbBatchSize.Size = new System.Drawing.Size(54, 39);
            this.lbBatchSize.TabIndex = 4;
            this.lbBatchSize.Text = "Batch Size";
            this.lbBatchSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UCVirtualConfigure
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UCVirtualConfigure";
            this.Size = new System.Drawing.Size(1776, 682);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Input inPath;
        private AntdUI.Button btnSelectPath;
        private AntdUI.Label lbPath;
        private AntdUI.InputNumber inNumBatchSize;
        private AntdUI.Label lbBatchSize;
    }
}
