namespace VDEApp.Views.Main
{
    partial class UCTitleBar
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
            this.menu1 = new AntdUI.Menu();
            this.pageHeader1 = new AntdUI.PageHeader();
            this.SuspendLayout();
            // 
            // menu1
            // 
            this.menu1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.menu1.BackColor = System.Drawing.Color.Transparent;
            this.menu1.Location = new System.Drawing.Point(135, 4);
            this.menu1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.menu1.Mode = AntdUI.TMenuMode.Horizontal;
            this.menu1.Name = "menu1";
            this.menu1.Size = new System.Drawing.Size(526, 160);
            this.menu1.TabIndex = 3;
            // 
            // pageHeader1
            // 
            this.pageHeader1.BackColor = System.Drawing.Color.White;
            this.pageHeader1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageHeader1.Location = new System.Drawing.Point(0, 0);
            this.pageHeader1.Name = "pageHeader1";
            this.pageHeader1.ShowButton = true;
            this.pageHeader1.Size = new System.Drawing.Size(1316, 169);
            this.pageHeader1.TabIndex = 5;
            this.pageHeader1.Text = "Insnex App";
            this.pageHeader1.UseTitleFont = true;
            // 
            // UCTitleBar
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.menu1);
            this.Controls.Add(this.pageHeader1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UCTitleBar";
            this.Size = new System.Drawing.Size(1316, 169);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Menu menu1;
        private AntdUI.PageHeader pageHeader1;
    }
}
