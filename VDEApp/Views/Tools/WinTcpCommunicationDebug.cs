using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.Models;
using System.Drawing;
using VDEApp.Models.Product;

namespace VDEApp.Views.Tools
{
    public partial class WinTcpCommunicationDebug : AntdUI.Window, ILocalizableForm
    {
        private Localizer _ => ServiceLocator.GlobalConfig.GlobalLocalizer;
        private readonly (string DisplayText, ResponseType Flag, string SwitchName)[] _responseMappings = new[]
       {
            ("取相节点", ResponseType.AfterAcquireNode, "switch_AfterAcquireNode"),
            ("标定节点", ResponseType.AfterCalibrationNode, "switch_AfterCalibrationNode"),
            ("检测节点", ResponseType.AfterInspectionNode, "switch_AfterInspectionNode")
        };
        public WinTcpCommunicationDebug()
        {
            InitializeComponent();
            RefreshLanguage();
            init();
        }
        public TcpCommunicationController TcpCommunication => ServiceLocator.TcpCommunicationController;
        /// <summary>
        /// initialize TCP communication event handlers
        /// </summary>
        private void init()
        {
            var preferredIP = TcpCommunication.GetPreferredLocalIPAddress();
            TcpTextIP.Enabled = false;
            TcpTextPort.TextChanged -= TcpTextPort_TextChanged;
            this.TcpTextIP.Text = string.Join(".", preferredIP.Split('.').Select(s => s.Trim()));
            this.TcpTextPort.Text = TcpCommunication.Port.ToString();
            TcpTextPort.TextChanged += TcpTextPort_TextChanged;
            TcpTextReceive.AppendText($"{_.GetString("form_tcp_debug_tip_command_format")}\r\n{_.GetString("form_tcp_debug_tip_command_example")}\r\n");
            TcpCommunication.Server.ClientConnected += (s, e) => TcpTextReceive.AppendText($"[SERVER] {_.GetString("form_tcp_debug_log_client_connected")}: {e.Client.Client.RemoteEndPoint}\r\n");
            TcpCommunication.Server.ClientDisconnected += (s, e) => TcpTextReceive.AppendText($"[SERVER] {_.GetString("form_tcp_debug_log_client_disconnected")}: {e.Client.Client.RemoteEndPoint} \r\n");
            TcpCommunication.Server.DataReceived += (s, e) =>
            {
                TcpTextReceive.AppendText($"{_.GetString("form_tcp_debug_log_received_from")} {e.ClientEndPoint}: {e.Data}\r\n");
            };

            var ischecked = ServiceLocator.ProjectController.CurrentProject.CurrentResponseType == ResponseType.ALL ? true : false;
            AllSwitch.Checked = ischecked;
            ALLSwitchChangd(AllSwitch.Checked);
        }

        /// <summary>
        /// 总开关状态变更处理（显示/隐藏子开关）
        /// </summary>
        /// <param name="isAllChecked">总开关是否勾选</param>
        private void ALLSwitchChangd(bool isAllChecked)
        {
            try
            {
                // 先移除所有子开关和标签（避免重复创建）
                RemoveAllSubControls();

                if (isAllChecked)
                {
                    // 总开关勾选：设置为全部响应
                    ServiceLocator.ProjectController.CurrentProject.CurrentResponseType = ResponseType.ALL;
                }
                else
                {
                    // 总开关取消：创建子开关，初始状态同步枚举配置
                    int startRowIndex = 1; // 第2行（索引1）开始
                    var currentResponseType = ServiceLocator.ProjectController.CurrentProject.CurrentResponseType;

                    foreach (var (displayText, flag, switchName) in _responseMappings)
                    {
                        int currentRow = startRowIndex++; // 依次占用第2、3、4行

                        // 创建标签
                        AntdUI.Label label = new AntdUI.Label
                        {
                            Text = displayText,
                            Dock = DockStyle.Fill,
                            TextAlign = ContentAlignment.MiddleLeft,
                            Margin = new Padding(5, 0, 0, 0),
                            Name = $"label_{switchName}" // 标签命名，方便后续移除
                        };

                        // 创建子开关
                        AntdUI.Switch switchCtrl = new AntdUI.Switch
                        {
                            Size = new Size(60, 35),
                            Anchor = AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                            Margin = new Padding(0, 5, 5, 5),
                            Name = switchName,
                            // 初始状态：判断当前枚举是否包含该标志（按位与运算）
                            Checked = (currentResponseType & flag) != 0
                        };

                        // 绑定开关事件（仅绑定一次）
                        switchCtrl.CheckedChanged += ONSwitch_CheckedChanged;

                        // 添加到TableLayout指定行列（列0=标签，列1=开关）
                        SelectTableLayout.Controls.Add(label, 0, currentRow);
                        SelectTableLayout.Controls.Add(switchCtrl, 1, currentRow);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    _.GetString("form_tcp_debug_msg_switch_init_fail") + $": {ex.Message}",
                    _.GetString("form_tcp_debug_title_error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// 移除所有子开关和标签
        /// </summary>
        private void RemoveAllSubControls()
        {
            foreach (var (_, _, switchName) in _responseMappings)
            {
                // 移除开关（通过Name精准移除）
                SelectTableLayout.Controls.RemoveByKey(switchName);
                // 移除对应的标签
                SelectTableLayout.Controls.RemoveByKey($"label_{switchName}");
            }
        }
        /// <summary>
        /// ip changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TcpTextIP_TextChanged(object sender, EventArgs e)
        {
            TcpCommunication.Ip = TcpTextIP.Text.Split('.').Select(s => int.Parse(s)).ToArray();
        }
        /// <summary>
        /// Port changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TcpTextPort_TextChanged(object sender, EventArgs e)
        {
            if (TcpCommunication.Server.IsRunning)
            {
                MessageBox.Show(
                    _.GetString("form_tcp_debug_msg_stop_server_first"),
                    _.GetString("form_tcp_debug_title_hint"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                TcpTextPort.TextChanged -= TcpTextPort_TextChanged;
                TcpTextPort.Text = TcpCommunication.Port.ToString();
                TcpTextPort.TextChanged += TcpTextPort_TextChanged;
                return;
            }
            ServiceLocator.ProjectController.CurrentProject.IsModified = true;
            TcpCommunication.Port = int.Parse(TcpTextPort.Text);
        }
        /// <summary>
        /// restart TCP server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTcpReStart_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
               _.GetString("form_tcp_debug_msg_confirm_stop_server"),
               _.GetString("form_tcp_debug_title_warning"),
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Warning
           );
            if (result == DialogResult.OK)
            {

                TcpCommunication.Reinitialize();
                init();
                TcpCommunication.StartServer();
            }
            else
                return;
        }
        /// <summary>
        /// stop TCP server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TcpBtnStop_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
               _.GetString("form_tcp_debug_msg_confirm_stop_server"),
               _.GetString("form_tcp_debug_title_warning"),
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Warning
           );
            if (result == DialogResult.OK)
                TcpCommunication.StopServer();
        }

        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }

        private void TcpBtnReceiveClear_Click(object sender, EventArgs e)
        {
            TcpTextReceive.Clear();
        }

        private void AllSwitch_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            ServiceLocator.ProjectController.CurrentProject.IsModified = true;
            ALLSwitchChangd(e.Value);
        }
        private void ONSwitch_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            var sw = (AntdUI.Switch)sender;
            var current = ServiceLocator.ProjectController.CurrentProject.CurrentResponseType;

            // 根据开关名称更新枚举状态
            switch (sw.Name)
            {
                case "switch_AfterAcquireNode":
                    // 勾选：添加标志（按位或 |）；取消：移除标志（按位与 & 取反 ~）
                    current = sw.Checked
                        ? current | ResponseType.AfterAcquireNode
                        : current & ~ResponseType.AfterAcquireNode;
                    break;
                case "switch_AfterCalibrationNode":
                    current = sw.Checked
                        ? current | ResponseType.AfterCalibrationNode
                        : current & ~ResponseType.AfterCalibrationNode;
                    break;
                case "switch_AfterInspectionNode":
                    current = sw.Checked
                        ? current | ResponseType.AfterInspectionNode
                        : current & ~ResponseType.AfterInspectionNode;
                    break;
            }
            ServiceLocator.ProjectController.CurrentProject.IsModified = true;
            // 保存更新后的状态
            ServiceLocator.ProjectController.CurrentProject.CurrentResponseType = current;
        }

    }
}
