namespace VDEApp
{
    partial class FromExample
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
            this.table1 = new AntdUI.Table();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pageHeader1 = new AntdUI.PageHeader();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // table1
            // 
            this.table1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table1.Gap = 12;
            this.table1.Location = new System.Drawing.Point(3, 33);
            this.table1.Name = "table1";
            this.table1.Size = new System.Drawing.Size(1300, 816);
            this.table1.TabIndex = 0;
            this.table1.Text = "table1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.table1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pageHeader1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1306, 852);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // pageHeader1
            // 
            this.pageHeader1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageHeader1.Location = new System.Drawing.Point(3, 3);
            this.pageHeader1.Name = "pageHeader1";
            this.pageHeader1.ShowButton = true;
            this.pageHeader1.Size = new System.Drawing.Size(1300, 24);
            this.pageHeader1.TabIndex = 1;
            this.pageHeader1.Text = "pageHeader1";
            // 
            // FromExample
            // 
            this.ClientSize = new System.Drawing.Size(1306, 852);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FromExample";
            this.Text = "FromExample";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Table table1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.PageHeader pageHeader1;
    }
}