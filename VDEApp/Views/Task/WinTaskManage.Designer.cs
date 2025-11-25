using System.Drawing;
using System.Windows.Forms;

namespace VDEApp.Views.Task
{
    partial class WinTaskManage
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.WinTaskSetting = new AntdUI.PageHeader();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvTasks = new AntdUI.Table();
            this.btnAddTask = new AntdUI.Button();
            this.panelMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // WinTaskSetting
            // 
            this.WinTaskSetting.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.WinTaskSetting.CloseSize = 30;
            this.WinTaskSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WinTaskSetting.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WinTaskSetting.Location = new System.Drawing.Point(3, 3);
            this.WinTaskSetting.Name = "WinTaskSetting";
            this.WinTaskSetting.ShowButton = true;
            this.WinTaskSetting.Size = new System.Drawing.Size(871, 29);
            this.WinTaskSetting.TabIndex = 0;
            this.WinTaskSetting.Text = "任务管理";
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelMain.Controls.Add(this.tableLayoutPanel1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(877, 508);
            this.panelMain.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgvTasks, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.WinTaskSetting, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddTask, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(877, 508);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // dgvTasks
            // 
            this.dgvTasks.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.dgvTasks.BackColor = System.Drawing.Color.White;
            this.dgvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTasks.Gap = 6;
            this.dgvTasks.Gaps = new System.Drawing.Size(6, 6);
            this.dgvTasks.Location = new System.Drawing.Point(10, 85);
            this.dgvTasks.Margin = new System.Windows.Forms.Padding(10);
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.Radius = 6;
            this.dgvTasks.Size = new System.Drawing.Size(857, 413);
            this.dgvTasks.TabIndex = 1;
            // 
            // btnAddTask
            // 
            this.btnAddTask.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddTask.Location = new System.Drawing.Point(3, 38);
            this.btnAddTask.Name = "btnAddTask";
            this.btnAddTask.Size = new System.Drawing.Size(107, 34);
            this.btnAddTask.TabIndex = 0;
            this.btnAddTask.Text = "新建任务";
            this.btnAddTask.Type = AntdUI.TTypeMini.Primary;
            this.btnAddTask.Click += new System.EventHandler(this.button1_Click);
            // 
            // WinTaskManage
            // 
            this.ClientSize = new System.Drawing.Size(877, 508);
            this.Controls.Add(this.panelMain);
            this.Name = "WinTaskManage";
            this.Text = "任务管理";
            this.Load += new System.EventHandler(this.Task_Load);
            this.panelMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader WinTaskSetting;
        private System.Windows.Forms.Panel panelMain;
        private AntdUI.Button btnAddTask;
        private AntdUI.Table dgvTasks;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
