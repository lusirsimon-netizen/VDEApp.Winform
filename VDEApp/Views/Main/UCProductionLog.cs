using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VDEApp.LogModule;

namespace VDEApp.Views.Main
{
    public partial class UCProductionLog : UserControl, IMainLoader
    {
        private List<string> _allLogs = new List<string>();
        private readonly object _logLock = new object();
        private int _lastHoverIndex = -1;
        private LogAppender _logAppender; // 保存引用以便取消订阅

        public UCProductionLog()
        {
            InitializeComponent();
        }

        #region 实现接口 
        /// <summary>
        /// 初始化UI控件
        /// </summary>
        public void InitializeUI()
        {
            //确保默认选中所有级别（避免首次加载日志被过滤）
            Infocheckbox.Checked = false;
            Warncheckbox.Checked = false;
            Debugcheckbox.Checked = false;
            Errorcheckbox.Checked = false;
            Fatalcheckbox.Checked = false;
            Infocheckbox.CheckedChanged += Filter_CheckedChanged;
            Warncheckbox.CheckedChanged += Filter_CheckedChanged;
            Debugcheckbox.CheckedChanged += Filter_CheckedChanged;
            Errorcheckbox.CheckedChanged += Filter_CheckedChanged;
            Fatalcheckbox.CheckedChanged += Filter_CheckedChanged;

            //添加鼠标移动事件检测悬停
            LoglistBox.MouseMove += LoglistBox_MouseMove;
            //鼠标离开ListBox区域时隐藏提示
            LoglistBox.MouseLeave += LogListBox_MouseLeave;

            //日志绑定
            _logAppender = new LogAppender();
            _logAppender.LogReceived += LogAppender_Received;
            ((Logger)LogModule.Log.log.Logger).AddAppender(_logAppender);
        }
        #endregion

        /// <summary>
        /// 接收消息
        /// </summary> 
        public void LogAppender_Received(object sender, string logLine)
        {
            lock (_logLock)
            {
                _allLogs.Add(logLine);
                if (_allLogs.Count > 1000)
                {
                    _allLogs.RemoveRange(0, _allLogs.Count - 1000);
                }
                AddLog(logLine);
            }
        }
        /// <summary>
        /// 是否显示所有日志
        /// </summary>
        private bool CanShowAll => !Infocheckbox.Checked && !Warncheckbox.Checked && !Errorcheckbox.Checked && !Fatalcheckbox.Checked && !Debugcheckbox.Checked;
        /// <summary>
        /// 判断当前日志是否符合筛选条件
        /// </summary> 
        private bool CanAdd(string log)
        {
            if (Infocheckbox.Checked && (log.StartsWith("[INFO]") || log.StartsWith("[信息] ")))
                return true;
            if (Warncheckbox.Checked && (log.StartsWith("[WARN]") || log.StartsWith("[警告] ")))
                return true;
            if (Errorcheckbox.Checked && (log.StartsWith("[ERROR]") || log.StartsWith("[错误] ")))
                return true;
            if (Fatalcheckbox.Checked && (log.StartsWith("[FATAL]") || log.StartsWith("[致命]")))
                return true;
            if (Debugcheckbox.Checked && (log.StartsWith("[DEBUG]") || log.StartsWith("[调试] ")))
                return true;
            return false;
        }
        /// <summary>
        /// 加入日志
        /// </summary>
        private void AddLog(string log)
        {
            if (!CanShowAll && !CanAdd(log))
                return;

            // 检查控件状态，增加更严格的判断
            if (IsDisposed || !IsHandleCreated)
                return;

            var action = new Action(() =>
            {
                // 在UI线程中再次检查
                if (IsDisposed || !IsHandleCreated)
                    return;

                LoglistBox.Items.Add(log);
                if (LoglistBox.Items.Count > 0)
                {
                    LoglistBox.TopIndex = LoglistBox.Items.Count - 1;
                }
                if (LoglistBox.Items.Count > 1000)
                {
                    LoglistBox.Items.RemoveAt(0);
                }
            });

            if (LoglistBox.InvokeRequired)
            {
                try
                {
                    // 使用BeginInvoke而不是Invoke，避免阻塞
                    LoglistBox.BeginInvoke((MethodInvoker)delegate
                    {
                        action.Invoke();
                    });
                }
                catch (ObjectDisposedException)
                {
                    // 安全忽略
                }
                catch (InvalidOperationException)
                {
                    // 安全忽略
                }
            }
            else
            {
                action.Invoke();
            }
        }
        /// <summary>
        /// 筛选
        /// </summary>
        private void UpdateFilteredLogs()
        {
            // 检查控件是否已释放或句柄是否已创建
            if (IsDisposed || !IsHandleCreated)
                return;

            if (LoglistBox.InvokeRequired)
            {
                try
                {
                    LoglistBox.Invoke(new Action(UpdateFilteredLogs));
                }
                catch (ObjectDisposedException)
                {
                    // 控件已释放，忽略此操作
                }
                catch (InvalidOperationException)
                {
                    // 控件正在销毁，忽略此操作
                }
                return;
            }
            // 确保默认选中所有级别（避免首次加载日志被过滤）
            bool showInfo = Infocheckbox.Checked;
            bool showWarning = Warncheckbox.Checked;
            bool showError = Errorcheckbox.Checked;
            bool showFatal = Fatalcheckbox.Checked;
            bool showDebug = Debugcheckbox.Checked;

            bool allUnchecked = !showInfo && !showWarning && !showError && !showFatal && !showDebug;
            //选择符合条件的日志
            List<string> filteredLogs;
            lock (_logLock)
            {
                if (allUnchecked)
                {
                    // 全不选时，直接返回所有日志
                    filteredLogs = _allLogs;
                }
                else
                {
                    // 否则按选中的条件筛选
                    filteredLogs = _allLogs.Where(log =>
                    {
                        string upperLog = log.ToUpper();
                        if (showInfo && (upperLog.StartsWith("[INFO]") || upperLog.StartsWith("[信息] ")))
                            return true;
                        if (showWarning && (upperLog.StartsWith("[WARN]") || upperLog.StartsWith("[警告] ")))
                            return true;
                        if (showError && (upperLog.StartsWith("[ERROR]") || upperLog.StartsWith("[错误] ")))
                            return true;
                        if (showFatal && (upperLog.StartsWith("[FATAL]") || upperLog.StartsWith("[致命]")))
                            return true;
                        if (showDebug && (upperLog.StartsWith("[DEBUG]") || upperLog.StartsWith("[调试] ")))
                            return true;
                        return false;
                    }).ToList();
                }

            }
            // 刷新 ListBox 显示筛选结果
            LoglistBox.Items.Clear();
            LoglistBox.Items.AddRange(filteredLogs.ToArray());

            // 自动滚动到最后一行（保持最新日志可见）
            if (LoglistBox.Items.Count > 0)
            {
                LoglistBox.TopIndex = LoglistBox.Items.Count - 1;
            }
        }
        /// <summary>
        /// 复选框状态变化时触发筛选
        /// </summary>
        private void Filter_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilteredLogs();
        }
        /// <summary>
        /// 清空所有日志（原始列表和 ListBox）
        /// </summary>
        private void Clearbutton_Click(object sender, EventArgs e)
        {
            lock (_logLock)
            {
                _allLogs.Clear(); // 清空原始日志
            }
            LoglistBox.Items.Clear(); // 清空列表显示
        }
        private void LoglistBox_MouseMove(object sender, MouseEventArgs e)
        {
            int index = LoglistBox.IndexFromPoint(e.Location);
            if (index >= 0 && index != _lastHoverIndex)
            {
                _lastHoverIndex = index;
                string logText = LoglistBox.Items[index].ToString();

                LogTip.AutoPopDelay = 5000; // 提示显示时长（毫秒）
                LogTip.InitialDelay = 100;
                LogTip.ReshowDelay = 500; // 再次显示的延迟（毫秒）
                LogTip.ShowAlways = true; // 即使窗体非激活也显示
                //鼠标附近显示文本
                LogTip.Show(logText, LoglistBox, e.Location.X + 10, e.Location.Y + 10, 5000);
            }
            else if (index == -1 && _lastHoverIndex != -1)
            {
                LogTip.Hide(LoglistBox);
                _lastHoverIndex = -1;
            }
        }
        private void LogListBox_MouseLeave(Object Sender, EventArgs e)
        {
            LogTip.Hide(LoglistBox);
            _lastHoverIndex = -1;
        }
    }
}
