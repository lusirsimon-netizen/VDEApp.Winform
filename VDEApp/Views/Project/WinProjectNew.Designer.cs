namespace VDEApp.Views.Project
{
    partial class WinProjectNew
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
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblProjectPath = new System.Windows.Forms.Label();
            this.headerCreateProject = new AntdUI.PageHeader();
            this.txtName = new AntdUI.Input();
            this.txtPath = new AntdUI.Input();
            this.btnCreate = new AntdUI.Button();
            this.btnCancel = new AntdUI.Button();
            this.btnChoosePath = new AntdUI.Button();
            this.SuspendLayout();
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblProjectName.Location = new System.Drawing.Point(22, 52);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(127, 24);
            this.lblProjectName.TabIndex = 0;
            this.lblProjectName.Text = "Project Name";
            // 
            // lblProjectPath
            // 
            this.lblProjectPath.AutoSize = true;
            this.lblProjectPath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblProjectPath.Location = new System.Drawing.Point(22, 94);
            this.lblProjectPath.Name = "lblProjectPath";
            this.lblProjectPath.Size = new System.Drawing.Size(114, 24);
            this.lblProjectPath.TabIndex = 2;
            this.lblProjectPath.Text = "Project Path";
            // 
            // headerCreateProject
            // 
            this.headerCreateProject.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerCreateProject.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.headerCreateProject.Location = new System.Drawing.Point(0, 0);
            this.headerCreateProject.MaximizeBox = false;
            this.headerCreateProject.MinimizeBox = false;
            this.headerCreateProject.Name = "headerCreateProject";
            this.headerCreateProject.ShowButton = true;
            this.headerCreateProject.Size = new System.Drawing.Size(379, 35);
            this.headerCreateProject.TabIndex = 7;
            this.headerCreateProject.Text = "Create Project";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.Location = new System.Drawing.Point(115, 45);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(242, 33);
            this.txtName.TabIndex = 8;
            this.txtName.Text = "input1";
            // 
            // txtPath
            // 
            this.txtPath.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPath.Location = new System.Drawing.Point(115, 85);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(197, 33);
            this.txtPath.TabIndex = 9;
            this.txtPath.Text = "input1";
            // 
            // btnCreate
            // 
            this.btnCreate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCreate.Location = new System.Drawing.Point(195, 124);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(78, 34);
            this.btnCreate.TabIndex = 10;
            this.btnCreate.Text = "Create";
            this.btnCreate.Type = AntdUI.TTypeMini.Primary;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(279, 124);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 34);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Location = new System.Drawing.Point(318, 85);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(39, 33);
            this.btnChoosePath.TabIndex = 12;
            this.btnChoosePath.Text = "...";
            this.btnChoosePath.Click += new System.EventHandler(this.btnChoosePath_Click);
            // 
            // WinProjectNew
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(379, 175);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.headerCreateProject);
            this.Controls.Add(this.lblProjectPath);
            this.Controls.Add(this.lblProjectName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinProjectNew";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblProjectPath;
        private AntdUI.PageHeader headerCreateProject;
        private AntdUI.Input txtName;
        private AntdUI.Input txtPath;
        private AntdUI.Button btnCreate;
        private AntdUI.Button btnCancel;
        private AntdUI.Button btnChoosePath;
    }
}