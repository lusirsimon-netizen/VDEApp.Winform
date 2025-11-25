namespace VDEApp.Views.Main
{
    partial class UCToolBar
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
            this.components = new System.ComponentModel.Container();
            this.btnPlay = new AntdUI.Button();
            this.cmbTask = new AntdUI.Select();
            this.btnReplayImage = new AntdUI.Button();
            this.btnCommunication = new AntdUI.Button();
            this.btnInspection = new AntdUI.Button();
            this.btnCalibration = new AntdUI.Button();
            this.btnCamera = new AntdUI.Button();
            this.btnTask = new AntdUI.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolBarTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.btnLoopRun = new AntdUI.Button();
            this.btnOneceRun = new AntdUI.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTask = new System.Windows.Forms.Label();
            this.lblCommunication = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblProduction = new System.Windows.Forms.Label();
            this.lblCamera = new System.Windows.Forms.Label();
            this.lblCalibration = new System.Windows.Forms.Label();
            this.lblInspection = new System.Windows.Forms.Label();
            this.lblOneceRun = new System.Windows.Forms.Label();
            this.lblLoopRun = new System.Windows.Forms.Label();
            this.lblReplayImage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.IconPosition = AntdUI.TAlignMini.Top;
            this.btnPlay.IconSize = new System.Drawing.Size(40, 40);
            this.btnPlay.Location = new System.Drawing.Point(1026, 7);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(50, 50);
            this.btnPlay.TabIndex = 47;
            this.btnPlay.ToggleType = AntdUI.TTypeMini.Default;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // cmbTask
            // 
            this.cmbTask.List = true;
            this.cmbTask.Location = new System.Drawing.Point(91, 7);
            this.cmbTask.Name = "cmbTask";
            this.cmbTask.Size = new System.Drawing.Size(137, 40);
            this.cmbTask.TabIndex = 46;
            this.cmbTask.WheelModifyEnabled = false;
            this.cmbTask.SelectedIndexChanged += new AntdUI.IntEventHandler(this.CmbTask_SelectedIndexChanged);
            // 
            // btnReplayImage
            // 
            this.btnReplayImage.IconPosition = AntdUI.TAlignMini.Top;
            this.btnReplayImage.IconSize = new System.Drawing.Size(26, 26);
            this.btnReplayImage.Location = new System.Drawing.Point(771, 3);
            this.btnReplayImage.Name = "btnReplayImage";
            this.btnReplayImage.Size = new System.Drawing.Size(50, 50);
            this.btnReplayImage.TabIndex = 45;
            this.btnReplayImage.TextMultiLine = true;
            this.btnReplayImage.Click += new System.EventHandler(this.btnReplayImage_Click);
            // 
            // btnCommunication
            // 
            this.btnCommunication.IconPosition = AntdUI.TAlignMini.Top;
            this.btnCommunication.IconSize = new System.Drawing.Size(26, 26);
            this.btnCommunication.Location = new System.Drawing.Point(693, 3);
            this.btnCommunication.Name = "btnCommunication";
            this.btnCommunication.Size = new System.Drawing.Size(50, 50);
            this.btnCommunication.TabIndex = 44;
            this.btnCommunication.TextMultiLine = true;
            this.btnCommunication.Click += new System.EventHandler(this.btnCommunication_Click);
            // 
            // btnInspection
            // 
            this.btnInspection.IconPosition = AntdUI.TAlignMini.Top;
            this.btnInspection.IconSize = new System.Drawing.Size(26, 26);
            this.btnInspection.Location = new System.Drawing.Point(398, 3);
            this.btnInspection.Name = "btnInspection";
            this.btnInspection.Size = new System.Drawing.Size(50, 50);
            this.btnInspection.TabIndex = 43;
            this.btnInspection.TextMultiLine = true;
            this.btnInspection.Click += new System.EventHandler(this.btnInspection_Click);
            // 
            // btnCalibration
            // 
            this.btnCalibration.IconPosition = AntdUI.TAlignMini.Top;
            this.btnCalibration.IconSize = new System.Drawing.Size(22, 22);
            this.btnCalibration.Location = new System.Drawing.Point(321, 3);
            this.btnCalibration.Name = "btnCalibration";
            this.btnCalibration.Size = new System.Drawing.Size(50, 50);
            this.btnCalibration.TabIndex = 42;
            this.btnCalibration.TextMultiLine = true;
            this.btnCalibration.Click += new System.EventHandler(this.btnCalibration_Click);
            // 
            // btnCamera
            // 
            this.btnCamera.IconPosition = AntdUI.TAlignMini.Top;
            this.btnCamera.IconSize = new System.Drawing.Size(26, 26);
            this.btnCamera.Location = new System.Drawing.Point(244, 3);
            this.btnCamera.Name = "btnCamera";
            this.btnCamera.Size = new System.Drawing.Size(50, 50);
            this.btnCamera.TabIndex = 41;
            this.btnCamera.TextMultiLine = true;
            this.btnCamera.Click += new System.EventHandler(this.btnCamera_Click);
            // 
            // btnTask
            // 
            this.btnTask.IconPosition = AntdUI.TAlignMini.Top;
            this.btnTask.IconSize = new System.Drawing.Size(24, 24);
            this.btnTask.Location = new System.Drawing.Point(26, 4);
            this.btnTask.Name = "btnTask";
            this.btnTask.Size = new System.Drawing.Size(50, 50);
            this.btnTask.TabIndex = 40;
            this.btnTask.Click += new System.EventHandler(this.BtnTask_Click);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.Location = new System.Drawing.Point(662, 17);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1, 20);
            this.panel3.TabIndex = 39;
            // 
            // btnLoopRun
            // 
            this.btnLoopRun.IconPosition = AntdUI.TAlignMini.Top;
            this.btnLoopRun.IconRatio = 0.8F;
            this.btnLoopRun.IconSize = new System.Drawing.Size(25, 25);
            this.btnLoopRun.Location = new System.Drawing.Point(582, 3);
            this.btnLoopRun.Name = "btnLoopRun";
            this.btnLoopRun.Size = new System.Drawing.Size(50, 50);
            this.btnLoopRun.TabIndex = 50;
            this.btnLoopRun.TextMultiLine = true;
            this.btnLoopRun.Click += new System.EventHandler(this.btnLoopRun_Click);
            // 
            // btnOneceRun
            // 
            this.btnOneceRun.IconPosition = AntdUI.TAlignMini.Top;
            this.btnOneceRun.IconRatio = 0.8F;
            this.btnOneceRun.IconSize = new System.Drawing.Size(25, 25);
            this.btnOneceRun.Location = new System.Drawing.Point(504, 3);
            this.btnOneceRun.Name = "btnOneceRun";
            this.btnOneceRun.Size = new System.Drawing.Size(50, 50);
            this.btnOneceRun.TabIndex = 49;
            this.btnOneceRun.TextMultiLine = true;
            this.btnOneceRun.Click += new System.EventHandler(this.btnOneceRun_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Location = new System.Drawing.Point(475, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 20);
            this.panel1.TabIndex = 48;
            // 
            // lblTask
            // 
            this.lblTask.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTask.Location = new System.Drawing.Point(2, 56);
            this.lblTask.Name = "lblTask";
            this.lblTask.Size = new System.Drawing.Size(98, 23);
            this.lblTask.TabIndex = 51;
            this.lblTask.Text = "任务管理";
            this.lblTask.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCommunication
            // 
            this.lblCommunication.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCommunication.Location = new System.Drawing.Point(662, 56);
            this.lblCommunication.Name = "lblCommunication";
            this.lblCommunication.Size = new System.Drawing.Size(110, 23);
            this.lblCommunication.TabIndex = 53;
            this.lblCommunication.Text = "通信";
            this.lblCommunication.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Location = new System.Drawing.Point(0, 86);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1099, 1);
            this.panel2.TabIndex = 54;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.Gainsboro;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1099, 1);
            this.panel4.TabIndex = 55;
            // 
            // lblProduction
            // 
            this.lblProduction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProduction.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblProduction.Location = new System.Drawing.Point(1003, 60);
            this.lblProduction.Name = "lblProduction";
            this.lblProduction.Size = new System.Drawing.Size(96, 23);
            this.lblProduction.TabIndex = 56;
            this.lblProduction.Text = "生产运行";
            this.lblProduction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCamera
            // 
            this.lblCamera.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCamera.Location = new System.Drawing.Point(224, 56);
            this.lblCamera.Name = "lblCamera";
            this.lblCamera.Size = new System.Drawing.Size(90, 23);
            this.lblCamera.TabIndex = 57;
            this.lblCamera.Text = "取像";
            this.lblCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCalibration
            // 
            this.lblCalibration.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCalibration.Location = new System.Drawing.Point(306, 56);
            this.lblCalibration.Name = "lblCalibration";
            this.lblCalibration.Size = new System.Drawing.Size(83, 23);
            this.lblCalibration.TabIndex = 58;
            this.lblCalibration.Text = "标定";
            this.lblCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInspection
            // 
            this.lblInspection.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblInspection.Location = new System.Drawing.Point(383, 56);
            this.lblInspection.Name = "lblInspection";
            this.lblInspection.Size = new System.Drawing.Size(83, 23);
            this.lblInspection.TabIndex = 59;
            this.lblInspection.Text = "检测";
            this.lblInspection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOneceRun
            // 
            this.lblOneceRun.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOneceRun.Location = new System.Drawing.Point(472, 57);
            this.lblOneceRun.Name = "lblOneceRun";
            this.lblOneceRun.Size = new System.Drawing.Size(111, 23);
            this.lblOneceRun.TabIndex = 60;
            this.lblOneceRun.Text = "单次运行";
            this.lblOneceRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLoopRun
            // 
            this.lblLoopRun.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoopRun.Location = new System.Drawing.Point(561, 57);
            this.lblLoopRun.Name = "lblLoopRun";
            this.lblLoopRun.Size = new System.Drawing.Size(96, 23);
            this.lblLoopRun.TabIndex = 61;
            this.lblLoopRun.Text = "持续运行";
            this.lblLoopRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReplayImage
            // 
            this.lblReplayImage.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblReplayImage.Location = new System.Drawing.Point(745, 56);
            this.lblReplayImage.Name = "lblReplayImage";
            this.lblReplayImage.Size = new System.Drawing.Size(102, 23);
            this.lblReplayImage.TabIndex = 62;
            this.lblReplayImage.Text = "仿图";
            this.lblReplayImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UCToolBar
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lblReplayImage);
            this.Controls.Add(this.lblLoopRun);
            this.Controls.Add(this.lblOneceRun);
            this.Controls.Add(this.lblInspection);
            this.Controls.Add(this.lblCalibration);
            this.Controls.Add(this.lblCamera);
            this.Controls.Add(this.lblProduction);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblCommunication);
            this.Controls.Add(this.lblTask);
            this.Controls.Add(this.btnLoopRun);
            this.Controls.Add(this.btnOneceRun);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.cmbTask);
            this.Controls.Add(this.btnReplayImage);
            this.Controls.Add(this.btnCommunication);
            this.Controls.Add(this.btnInspection);
            this.Controls.Add(this.btnCalibration);
            this.Controls.Add(this.btnCamera);
            this.Controls.Add(this.btnTask);
            this.Controls.Add(this.panel3);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCToolBar";
            this.Size = new System.Drawing.Size(1099, 87);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Button btnPlay;
        private AntdUI.Select cmbTask;
        private AntdUI.Button btnReplayImage;
        private AntdUI.Button btnCommunication;
        private AntdUI.Button btnInspection;
        private AntdUI.Button btnCalibration;
        private AntdUI.Button btnCamera;
        private AntdUI.Button btnTask;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolTip toolBarTooltip;
        private AntdUI.Button btnLoopRun;
        private AntdUI.Button btnOneceRun;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTask;
        private System.Windows.Forms.Label lblCommunication;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblProduction;
        private System.Windows.Forms.Label lblCamera;
        private System.Windows.Forms.Label lblCalibration;
        private System.Windows.Forms.Label lblInspection;
        private System.Windows.Forms.Label lblOneceRun;
        private System.Windows.Forms.Label lblLoopRun;
        private System.Windows.Forms.Label lblReplayImage;
    }
}
