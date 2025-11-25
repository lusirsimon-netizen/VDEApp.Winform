using AntdUI;
using System.Drawing;
using System.Windows.Forms;

namespace VDEApp.Views.Main
{
    partial class UCProductDisplay
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
            AntdUI.SegmentedItem segmentedItem1 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem2 = new AntdUI.SegmentedItem();
            AntdUI.SegmentedItem segmentedItem3 = new AntdUI.SegmentedItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.segmented1 = new AntdUI.Segmented();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLayout22 = new AntdUI.Button();
            this.btnLayout11 = new AntdUI.Button();
            this.btnLayoutSwitch = new AntdUI.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 31);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(887, 472);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panelHeader, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(891, 604);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.segmented1);
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.btnLayout22);
            this.panelHeader.Controls.Add(this.btnLayout11);
            this.panelHeader.Controls.Add(this.btnLayoutSwitch);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(3, 3);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(885, 24);
            this.panelHeader.TabIndex = 2;
            // 
            // segmented1
            // 
            this.segmented1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            segmentedItem1.Text = "1X1";
            segmentedItem2.Text = "2X2";
            segmentedItem3.Text = "Custom Layout";
            this.segmented1.Items.Add(segmentedItem1);
            this.segmented1.Items.Add(segmentedItem2);
            this.segmented1.Items.Add(segmentedItem3);
            this.segmented1.Location = new System.Drawing.Point(725, 0);
            this.segmented1.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.segmented1.Name = "segmented1";
            this.segmented1.Size = new System.Drawing.Size(157, 24);
            this.segmented1.TabIndex = 0;
            this.segmented1.SelectIndexChanged += new AntdUI.IntEventHandler(this.Segmented1_SelectIndexChanged);
            this.segmented1.ItemClick += new AntdUI.SegmentedItemEventHandler(this.Segmented1_ItemClick);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Production display";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLayout22
            // 
            this.btnLayout22.Location = new System.Drawing.Point(0, 0);
            this.btnLayout22.Name = "btnLayout22";
            this.btnLayout22.Size = new System.Drawing.Size(0, 0);
            this.btnLayout22.TabIndex = 4;
            // 
            // btnLayout11
            // 
            this.btnLayout11.Location = new System.Drawing.Point(0, 0);
            this.btnLayout11.Name = "btnLayout11";
            this.btnLayout11.Size = new System.Drawing.Size(0, 0);
            this.btnLayout11.TabIndex = 5;
            // 
            // btnLayoutSwitch
            // 
            this.btnLayoutSwitch.Location = new System.Drawing.Point(0, 0);
            this.btnLayoutSwitch.Name = "btnLayoutSwitch";
            this.btnLayoutSwitch.Size = new System.Drawing.Size(0, 0);
            this.btnLayoutSwitch.TabIndex = 6;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoScroll = true;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 507);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(885, 94);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // UCProductDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "UCProductDisplay";
            this.Size = new System.Drawing.Size(891, 604);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panelHeader;
        private AntdUI.Button btnLayoutSwitch;
        private AntdUI.Button btnLayout22;
        private AntdUI.Button btnLayout11;
        private System.Windows.Forms.Label label1;
        private AntdUI.Segmented segmented1;
    }
}
