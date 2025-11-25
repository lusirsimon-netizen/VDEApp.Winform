using System.Windows.Forms;
using VDEApp.Configs;

namespace VDEApp.Views.Task
{
    partial class WinInspection
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new AntdUI.Button();
            this.btnRestore = new AntdUI.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pageHeader1 = new AntdUI.PageHeader();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabs = new AntdUI.Tabs();
            this.tabSPEC = new AntdUI.TabPage();
            this.panel3 = new AntdUI.Panel();
            this.tableSPEC = new AntdUI.Table();
            this.panel4 = new AntdUI.Panel();
            this.btnNewSPEC = new AntdUI.Button();
            this.inputName = new AntdUI.Input();
            this.selectType = new AntdUI.Select();
            this.btnSaveSPEC = new AntdUI.Button();
            this.tabEdit = new AntdUI.TabPage();
            this.insToolEditorControl1 = new Insnex.ToolEditor.Controls.InsToolEditorControl();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabSPEC.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Menu;
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnRestore);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 547);
            this.panel1.MinimumSize = new System.Drawing.Size(900, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 50);
            this.panel1.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(700, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 35);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.Type = AntdUI.TTypeMini.Primary;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRestore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRestore.Location = new System.Drawing.Point(800, 7);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(80, 35);
            this.btnRestore.TabIndex = 1;
            this.btnRestore.Text = "Restore";
            this.btnRestore.Click += new System.EventHandler(this.BtnRestore_Click);
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 0;
            // 
            // pageHeader1
            // 
            this.pageHeader1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageHeader1.Font = new System.Drawing.Font("微软雅黑 Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader1.Location = new System.Drawing.Point(3, 3);
            this.pageHeader1.Name = "pageHeader1";
            this.pageHeader1.ShowButton = true;
            this.pageHeader1.Size = new System.Drawing.Size(900, 35);
            this.pageHeader1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.pageHeader1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabs, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 600);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabSPEC);
            this.tabs.Controls.Add(this.tabEdit);
            this.tabs.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabs.Location = new System.Drawing.Point(3, 44);
            this.tabs.Name = "tabs";
            this.tabs.Pages.Add(this.tabEdit);
            this.tabs.Pages.Add(this.tabSPEC);
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(900, 497);
            this.tabs.Style = styleLine1;
            this.tabs.TabIndex = 0;
            this.tabs.Text = "tabs1";
            // 
            // tabSPEC
            // 
            this.tabSPEC.Controls.Add(this.panel3);
            this.tabSPEC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSPEC.Location = new System.Drawing.Point(0, 36);
            this.tabSPEC.Name = "tabSPEC";
            this.tabSPEC.Showed = true;
            this.tabSPEC.Size = new System.Drawing.Size(900, 461);
            this.tabSPEC.TabIndex = 2;
            this.tabSPEC.Text = "SPEC";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tableSPEC);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(900, 461);
            this.panel3.TabIndex = 0;
            this.panel3.Text = "panel3";
            // 
            // tableSPEC
            // 
            this.tableSPEC.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.tableSPEC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSPEC.Gap = 8;
            this.tableSPEC.Gaps = new System.Drawing.Size(8, 8);
            this.tableSPEC.Location = new System.Drawing.Point(0, 45);
            this.tableSPEC.Name = "tableSPEC";
            this.tableSPEC.Size = new System.Drawing.Size(900, 416);
            this.tableSPEC.TabIndex = 1;
            this.tableSPEC.Text = "table1";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Menu;
            this.panel4.Controls.Add(this.btnNewSPEC);
            this.panel4.Controls.Add(this.inputName);
            this.panel4.Controls.Add(this.selectType);
            this.panel4.Controls.Add(this.btnSaveSPEC);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(900, 45);
            this.panel4.TabIndex = 0;
            this.panel4.Text = "panel4";
            // 
            // btnNewSPEC
            // 
            this.btnNewSPEC.Location = new System.Drawing.Point(429, 6);
            this.btnNewSPEC.Name = "btnNewSPEC";
            this.btnNewSPEC.Size = new System.Drawing.Size(90, 35);
            this.btnNewSPEC.TabIndex = 3;
            this.btnNewSPEC.Text = "New";
            this.btnNewSPEC.Type = AntdUI.TTypeMini.Info;
            this.btnNewSPEC.Click += new System.EventHandler(this.BtnNewSPEC_Click);
            // 
            // inputName
            // 
            this.inputName.Location = new System.Drawing.Point(161, 6);
            this.inputName.Name = "inputName";
            this.inputName.Size = new System.Drawing.Size(240, 35);
            this.inputName.TabIndex = 2;
            this.inputName.Text = "Name";
            // 
            // selectType
            // 
            this.selectType.Items.AddRange(new object[] {
            "Boolean",
            "Int",
            "Double",
            "String",
            "Range"});
            this.selectType.List = true;
            this.selectType.Location = new System.Drawing.Point(23, 6);
            this.selectType.Name = "selectType";
            this.selectType.SelectedIndex = 3;
            this.selectType.SelectedValue = "String";
            this.selectType.Size = new System.Drawing.Size(100, 35);
            this.selectType.TabIndex = 1;
            this.selectType.Text = "String";
            // 
            // btnSaveSPEC
            // 
            this.btnSaveSPEC.Location = new System.Drawing.Point(780, 6);
            this.btnSaveSPEC.Name = "btnSaveSPEC";
            this.btnSaveSPEC.Size = new System.Drawing.Size(90, 35);
            this.btnSaveSPEC.TabIndex = 0;
            this.btnSaveSPEC.Text = "Apply";
            this.btnSaveSPEC.Type = AntdUI.TTypeMini.Warn;
            this.btnSaveSPEC.Click += new System.EventHandler(this.BtnSaveSPEC_Click);
            // 
            // tabEdit
            // 
            this.tabEdit.Controls.Add(this.insToolEditorControl1);
            this.tabEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEdit.Location = new System.Drawing.Point(0, 36);
            this.tabEdit.Name = "tabEdit";
            this.tabEdit.Size = new System.Drawing.Size(900, 461);
            this.tabEdit.TabIndex = 1;
            this.tabEdit.Text = "Edit";
            // 
            // insToolEditorControl1
            // 
            this.insToolEditorControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(18)))), ((int)(((byte)(19)))));
            this.insToolEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.insToolEditorControl1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.insToolEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.insToolEditorControl1.MinimumSize = new System.Drawing.Size(900, 450);
            this.insToolEditorControl1.Name = "insToolEditorControl1";
            this.insToolEditorControl1.Size = new System.Drawing.Size(900, 461);
            this.insToolEditorControl1.TabIndex = 0;
            // 
            // WinInspection
            // 
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "WinInspection";
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabSPEC.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabEdit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Insnex.ToolEditor.Controls.InsToolEditorControl insToolEditorControl1;
        private AntdUI.Button btnSave;
        private AntdUI.Button btnRestore;
        private AntdUI.PageHeader pageHeader1;
        private AntdUI.Tabs tabs;
        private AntdUI.TabPage tabEdit;
        private AntdUI.TabPage tabSPEC;
        private AntdUI.Panel panel3;
        private AntdUI.Table tableSPEC;
        private AntdUI.Panel panel4;
        private AntdUI.Input inputName;
        private AntdUI.Select selectType;
        private AntdUI.Button btnNewSPEC;
        private AntdUI.Button btnSaveSPEC;
    }
}
