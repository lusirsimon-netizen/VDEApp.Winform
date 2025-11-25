using System.Windows.Forms;
namespace VDEApp.Views.Main
{
    partial class UCProductDisplayRecordItem
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.refreshStatusBadge = new AntdUI.Badge();
            this.bindingStatusbutton = new AntdUI.Button();
            this.label1 = new AntdUI.Label();
            this.txtRename = new AntdUI.Input();
            this.insRecordDisplayControl1 = new Insnex.Vision2D.Controls.InsRecordDisplayControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelTitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitContainer1.Panel1.Controls.Add(this.panelTitleBar);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.insRecordDisplayControl1);
            this.splitContainer1.Size = new System.Drawing.Size(497, 597);
            this.splitContainer1.SplitterDistance = 41;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelTitleBar
            // 
            this.panelTitleBar.Controls.Add(this.refreshStatusBadge);
            this.panelTitleBar.Controls.Add(this.bindingStatusbutton);
            this.panelTitleBar.Controls.Add(this.label1);
            this.panelTitleBar.Controls.Add(this.txtRename);
            this.panelTitleBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTitleBar.Location = new System.Drawing.Point(0, 0);
            this.panelTitleBar.Margin = new System.Windows.Forms.Padding(1);
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.Size = new System.Drawing.Size(497, 41);
            this.panelTitleBar.TabIndex = 1;
            // 
            // refreshStatusBadge
            // 
            this.refreshStatusBadge.BackColor = System.Drawing.Color.White;
            this.refreshStatusBadge.BadgeSize = 0.8F;
            this.refreshStatusBadge.Dock = System.Windows.Forms.DockStyle.Right;
            this.refreshStatusBadge.Location = new System.Drawing.Point(447, 0);
            this.refreshStatusBadge.Name = "refreshStatusBadge";
            this.refreshStatusBadge.Size = new System.Drawing.Size(25, 41);
            this.refreshStatusBadge.TabIndex = 4;
            this.refreshStatusBadge.Text = "";
            // 
            // bindingStatusbutton
            // 
            this.bindingStatusbutton.Dock = System.Windows.Forms.DockStyle.Right;
            this.bindingStatusbutton.IconSize = new System.Drawing.Size(13, 13);
            this.bindingStatusbutton.Location = new System.Drawing.Point(472, 0);
            this.bindingStatusbutton.Name = "bindingStatusbutton";
            this.bindingStatusbutton.Size = new System.Drawing.Size(25, 41);
            this.bindingStatusbutton.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(7, 3, 7, 0);
            this.label1.Size = new System.Drawing.Size(497, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.DoubleClick += new System.EventHandler(this.Label1_DoubleClick);
            // 
            // txtRename
            // 
            this.txtRename.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRename.Location = new System.Drawing.Point(0, 0);
            this.txtRename.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtRename.Name = "txtRename";
            this.txtRename.Size = new System.Drawing.Size(497, 41);
            this.txtRename.TabIndex = 1;
            this.txtRename.Visible = false;
            // 
            // insRecordDisplayControl1
            // 
            this.insRecordDisplayControl1.AutoFitImage = true;
            this.insRecordDisplayControl1.ConfigDisplay3D_Half = 2;
            this.insRecordDisplayControl1.DisplayBackGroundColor = System.Drawing.Color.Empty;
            this.insRecordDisplayControl1.DisplayDrawEnable = true;
            this.insRecordDisplayControl1.DisplayImage = null;
            this.insRecordDisplayControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.insRecordDisplayControl1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.insRecordDisplayControl1.KeepViewport = false;
            this.insRecordDisplayControl1.Location = new System.Drawing.Point(0, 0);
            this.insRecordDisplayControl1.Margin = new System.Windows.Forms.Padding(1);
            this.insRecordDisplayControl1.MouseMode = Insnex.Vision2D.Core.InsDisplayMouseModeConstants.Pointer;
            this.insRecordDisplayControl1.Name = "insRecordDisplayControl1";
            this.insRecordDisplayControl1.ShowRecordID = 0;
            this.insRecordDisplayControl1.ShowRecordList = false;
            this.insRecordDisplayControl1.ShowStatusBar = false;
            this.insRecordDisplayControl1.Size = new System.Drawing.Size(497, 553);
            this.insRecordDisplayControl1.TabIndex = 0;
            // 
            // UCProductDisplayRecordItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "UCProductDisplayRecordItem";
            this.Size = new System.Drawing.Size(497, 597);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelTitleBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelTitleBar;
        private AntdUI.Badge refreshStatusBadge;
        private AntdUI.Label label1;
        private AntdUI.Input txtRename;
        private Insnex.Vision2D.Controls.InsRecordDisplayControl insRecordDisplayControl1;
        private AntdUI.Button bindingStatusbutton;
    }
}
