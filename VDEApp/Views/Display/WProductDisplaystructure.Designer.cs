namespace VDEApp
{
    partial class UCProductDisplaystructure
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
            this.pHLayoutsettings = new AntdUI.PageHeader();
            this.lbLines = new AntdUI.Label();
            this.colbox = new AntdUI.InputNumber();
            this.lbColumns = new AntdUI.Label();
            this.btnRefresh = new AntdUI.Button();
            this.rowbox = new AntdUI.InputNumber();
            this.SuspendLayout();
            // 
            // pHLayoutsettings
            // 
            this.pHLayoutsettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pHLayoutsettings.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pHLayoutsettings.Location = new System.Drawing.Point(0, 0);
            this.pHLayoutsettings.Margin = new System.Windows.Forms.Padding(2);
            this.pHLayoutsettings.MaximizeBox = false;
            this.pHLayoutsettings.Name = "pHLayoutsettings";
            this.pHLayoutsettings.ShowButton = true;
            this.pHLayoutsettings.Size = new System.Drawing.Size(226, 35);
            this.pHLayoutsettings.TabIndex = 0;
            this.pHLayoutsettings.Text = "布局设置";
            // 
            // lbLines
            // 
            this.lbLines.Location = new System.Drawing.Point(17, 53);
            this.lbLines.Margin = new System.Windows.Forms.Padding(2);
            this.lbLines.Name = "lbLines";
            this.lbLines.Size = new System.Drawing.Size(44, 18);
            this.lbLines.TabIndex = 1;
            this.lbLines.Text = "行数";
            // 
            // colbox
            // 
            this.colbox.Location = new System.Drawing.Point(69, 85);
            this.colbox.Margin = new System.Windows.Forms.Padding(2);
            this.colbox.Name = "colbox";
            this.colbox.Size = new System.Drawing.Size(141, 35);
            this.colbox.TabIndex = 4;
            this.colbox.Text = "0";
            // 
            // lbColumns
            // 
            this.lbColumns.Location = new System.Drawing.Point(17, 92);
            this.lbColumns.Margin = new System.Windows.Forms.Padding(2);
            this.lbColumns.Name = "lbColumns";
            this.lbColumns.Size = new System.Drawing.Size(44, 18);
            this.lbColumns.TabIndex = 3;
            this.lbColumns.Text = "列数";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefresh.Location = new System.Drawing.Point(95, 124);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(115, 35);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "确认刷新";
            this.btnRefresh.Type = AntdUI.TTypeMini.Primary;
            // 
            // rowbox
            // 
            this.rowbox.Location = new System.Drawing.Point(69, 46);
            this.rowbox.Margin = new System.Windows.Forms.Padding(2);
            this.rowbox.Name = "rowbox";
            this.rowbox.Size = new System.Drawing.Size(141, 35);
            this.rowbox.TabIndex = 6;
            this.rowbox.Text = "0";
            // 
            // UCProductDisplaystructure
            // 
            this.ClientSize = new System.Drawing.Size(226, 170);
            this.Controls.Add(this.rowbox);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.colbox);
            this.Controls.Add(this.lbColumns);
            this.Controls.Add(this.lbLines);
            this.Controls.Add(this.pHLayoutsettings);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UCProductDisplaystructure";
            this.Resizable = false;
            this.Text = "WProductDisplaystructure";
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pHLayoutsettings;
        private AntdUI.Label lbLines;
        private AntdUI.InputNumber colbox;
        private AntdUI.Label lbColumns;
        private AntdUI.Button btnRefresh;
        private AntdUI.InputNumber rowbox;
    }
}
