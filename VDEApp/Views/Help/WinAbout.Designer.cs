using System;
using System.Web.UI.HtmlControls;

namespace VDEApp.Views.Help
{
    partial class WinAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinAbout));
            this.pageHeader_About = new AntdUI.PageHeader();
            this.shield_C = new AntdUI.Shield();
            this.shield_Donwload = new AntdUI.Shield();
            this.shield_License = new AntdUI.Shield();
            this.shield_suppord = new AntdUI.Shield();
            this.shield_Ins = new AntdUI.Shield();
            this.label_CopyRight2 = new AntdUI.Label();
            this.label_copyright = new AntdUI.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new AntdUI.Label();
            this.label2 = new AntdUI.Label();
            this.label3 = new AntdUI.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageHeader_About
            // 
            this.pageHeader_About.DividerShow = true;
            this.pageHeader_About.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageHeader_About.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader_About.IconSvg = resources.GetString("pageHeader_About.IconSvg");
            this.pageHeader_About.Location = new System.Drawing.Point(3, 4);
            this.pageHeader_About.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pageHeader_About.MaximizeBox = false;
            this.pageHeader_About.MinimizeBox = false;
            this.pageHeader_About.Name = "pageHeader_About";
            this.pageHeader_About.ShowButton = true;
            this.pageHeader_About.ShowIcon = true;
            this.pageHeader_About.Size = new System.Drawing.Size(505, 32);
            this.pageHeader_About.TabIndex = 0;
            this.pageHeader_About.Text = "关于Insnex VDEApp";
            // 
            // shield_C
            // 
            this.shield_C.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.shield_C.Color = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(127)))), ((int)(((byte)(7)))));
            this.shield_C.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.shield_C.Label = "C#";
            this.shield_C.Location = new System.Drawing.Point(173, 145);
            this.shield_C.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.shield_C.Name = "shield_C";
            this.shield_C.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.shield_C.Size = new System.Drawing.Size(88, 33);
            this.shield_C.TabIndex = 6;
            this.shield_C.Text = "100%";
            // 
            // shield_Donwload
            // 
            this.shield_Donwload.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.shield_Donwload.Color = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(148)))), ((int)(((byte)(0)))));
            this.shield_Donwload.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.shield_Donwload.Label = "Donwload";
            this.shield_Donwload.Location = new System.Drawing.Point(250, 145);
            this.shield_Donwload.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.shield_Donwload.Name = "shield_Donwload";
            this.shield_Donwload.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.shield_Donwload.Size = new System.Drawing.Size(133, 33);
            this.shield_Donwload.TabIndex = 5;
            this.shield_Donwload.Text = "1.5K";
            // 
            // shield_License
            // 
            this.shield_License.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.shield_License.Color = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(164)))), ((int)(((byte)(180)))));
            this.shield_License.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.shield_License.Label = "License";
            this.shield_License.Location = new System.Drawing.Point(52, 145);
            this.shield_License.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.shield_License.Name = "shield_License";
            this.shield_License.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.shield_License.Size = new System.Drawing.Size(156, 33);
            this.shield_License.TabIndex = 3;
            this.shield_License.Text = "Apache1.0";
            this.shield_License.DoubleClick += new System.EventHandler(this.Shield_License_DoubleClick);
            // 
            // shield_suppord
            // 
            this.shield_suppord.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.shield_suppord.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(140)))), ((int)(((byte)(254)))));
            this.shield_suppord.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.shield_suppord.Label = "Email";
            this.shield_suppord.Location = new System.Drawing.Point(52, 176);
            this.shield_suppord.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.shield_suppord.Name = "shield_suppord";
            this.shield_suppord.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.shield_suppord.Size = new System.Drawing.Size(257, 33);
            this.shield_suppord.TabIndex = 2;
            this.shield_suppord.Text = "tech_support@insnex.com";
            // 
            // shield_Ins
            // 
            this.shield_Ins.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.shield_Ins.Color = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.shield_Ins.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.shield_Ins.Label = "Website";
            this.shield_Ins.Location = new System.Drawing.Point(240, 176);
            this.shield_Ins.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.shield_Ins.Name = "shield_Ins";
            this.shield_Ins.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.shield_Ins.Size = new System.Drawing.Size(260, 33);
            this.shield_Ins.TabIndex = 1;
            this.shield_Ins.Text = "https://www.insnex.com";
            this.shield_Ins.DoubleClick += new System.EventHandler(this.Shield_Ins_DoubleClick);
            // 
            // label_CopyRight2
            // 
            this.label_CopyRight2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label_CopyRight2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.label_CopyRight2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_CopyRight2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CopyRight2.Location = new System.Drawing.Point(54, 243);
            this.label_CopyRight2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_CopyRight2.Name = "label_CopyRight2";
            this.label_CopyRight2.Size = new System.Drawing.Size(582, 24);
            this.label_CopyRight2.TabIndex = 1;
            this.label_CopyRight2.Text = "Copyright © INSNEX Technologies Co., Ltd 2025.All rights reserved.";
            this.label_CopyRight2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_copyright
            // 
            this.label_copyright.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label_copyright.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_copyright.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_copyright.Location = new System.Drawing.Point(152, 219);
            this.label_copyright.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label_copyright.Name = "label_copyright";
            this.label_copyright.Size = new System.Drawing.Size(208, 16);
            this.label_copyright.TabIndex = 0;
            this.label_copyright.Text = "苏州苏映视图像软件有限公司 版权所有";
            this.label_copyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::VDEApp.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(122, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(260, 70);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(215, 69);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 48);
            this.label1.TabIndex = 8;
            this.label1.Text = "苏映视";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(53, 115);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 28);
            this.label2.TabIndex = 9;
            this.label2.Text = "Insnex VDEApp";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(1)))), ((int)(((byte)(47)))));
            this.label3.Location = new System.Drawing.Point(171, 117);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 24);
            this.label3.TabIndex = 10;
            this.label3.Text = "Version 1.0.0";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pageHeader_About, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(511, 333);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.shield_Ins);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.shield_suppord);
            this.panel1.Controls.Add(this.shield_License);
            this.panel1.Controls.Add(this.label_CopyRight2);
            this.panel1.Controls.Add(this.shield_Donwload);
            this.panel1.Controls.Add(this.shield_C);
            this.panel1.Controls.Add(this.label_copyright);
            this.panel1.Location = new System.Drawing.Point(3, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(505, 287);
            this.panel1.TabIndex = 1;
            // 
            // WinAbout
            // 
            this.ClientSize = new System.Drawing.Size(511, 333);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(682, 430);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "WinAbout";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

       

        #endregion

        private AntdUI.PageHeader pageHeader_About;
        private AntdUI.Label label_copyright;
        private AntdUI.Label label_CopyRight2;
        private AntdUI.Shield shield_Ins;
        private AntdUI.Shield shield_License;
        private AntdUI.Shield shield_suppord;
        private AntdUI.Shield shield_C;
        private AntdUI.Shield shield_Donwload;
        private System.Windows.Forms.PictureBox pictureBox1;
        private AntdUI.Label label1;
        private AntdUI.Label label2;
        private AntdUI.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
    }
}