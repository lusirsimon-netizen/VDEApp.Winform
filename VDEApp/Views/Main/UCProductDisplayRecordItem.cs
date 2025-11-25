using AntdUI;
using Insnex.Vision2D;
using Insnex.Vision2D.Controls;
using Insnex.Vision2D.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers.Display;
using VDEApp.LogModule;
using VDEApp.Models;
using VDEApp.Models.TaskNodes;
using VDEApp.Views.Display;

namespace VDEApp.Views.Main
{
    /// <summary>
    /// 产品显示子控件
    /// 专门负责单个产品的界面展示
    /// </summary>
    public partial class UCProductDisplayRecordItem : UserControl
    {
        #region 成员变量
        // 控件标题
        private string _title;
        // 标签右键菜单
        private AntdUI.ContextMenuStripItem _labelRightMenu;
        private AntdUI.ContextMenuStripItem _unbindItem;
        // 绑定的记录对象
        private InsRecord _boundRecord;
        // 绑定的任务名称
        public string BoundTaskName { get; private set; }
        // 绑定的节点名称
        public string BoundNodeName { get; private set; }
        // 绑定的记录名称
        public string BoundRecordName { get; private set; }
        // Record 显示名称（重命名后的名称）
        public string RecordDisplayName { get; private set; }
        // 原始背景色（用于高亮后恢复）
        private Color _originalBackColor = SystemColors.Control;
        // 高亮定时器
        private System.Windows.Forms.Timer _highlightTimer;
        // 是否正在编辑重命名
        private bool _isRenaming = false;
        // 绑定状态（true = 已绑定 / 绿色，false = 未绑定 / 红色）
        private bool _isBound = false;
        // 刷新状态（true = 已刷新 / 绿色，false = 未刷新 / 红色）
        private bool _isRefreshed = false;
        // 用于跟踪控件的悬停状态
        private bool _isBindingButtonHovered = false;
        private bool _isRefreshBadgeHovered = false;
        // 悬停提示控件
        private ToolTip _statusTip = new ToolTip();
        // 是否正在绑定过程中
        private bool _isBindingInProgress = false;
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCProductDisplayRecordItem()
        {
            InitializeComponent();
            // 保存原始背景色
            _originalBackColor = this.BackColor;
            // 初始化界面控件
            InitializeControls();
            // 初始化事件处理
            InitializeEvents();
            // 初始化右键菜单
            InitLabelRightMenu();
            // 初始化悬停提示
            InitializeHoverTips();
            // 初始化状态显示
            UpdateBindingStatus();
            UpdateRefreshStatus(false);
            // 设置默认标题
            UpdateDefaultTitle();
            // 订阅语言切换事件
            AppModuleSingleton.LanguageSwitchEvent += (o, e) => UpdateLocalizedUI();
        }
        #endregion

        #region 本地化相关方法

        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;

        /// <summary>
        /// 更新本地化UI
        /// </summary>
        private void UpdateLocalizedUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateLocalizedUI));
                return;
            }

            try
            {
                // 更新默认标题（如果未绑定）
                if (!IsBound())
                {
                    UpdateDefaultTitle();
                }

                // 更新右键菜单文本
                UpdateContextMenuTexts();

                // 更新状态提示文本
                UpdateStatusTips();

                // 刷新绑定状态显示
                UpdateBindingStatusDisplay();

                // 刷新控件
                this.Refresh();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateLocalizedUI", "Failed to update localized UI: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 更新默认标题
        /// </summary>
        private void UpdateDefaultTitle()
        {
            try
            {
                if (string.IsNullOrEmpty(label1.Text) || label1.Text == "label1" ||
                    label1.Text == Localizer.GetString("Message_NoBoundDisplayImagePleaseRightClickToBind", "没有绑定的显示图像，请右键点击进行绑定"))
                {
                    string defaultText = Localizer.GetString("Message_NoBoundDisplayImagePleaseRightClickToBind", "没有绑定的显示图像，请右键点击进行绑定");
                    label1.Text = defaultText;
                    _title = defaultText;
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateDefaultTitle", "Failed to update default title: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 更新右键菜单文本
        /// </summary>
        private void UpdateContextMenuTexts()
        {
            try
            {
                if (_labelRightMenu != null)
                {
                    _labelRightMenu.Text = Localizer.GetString("Menu_OpenDisplayBindingInterface", "打开显示绑定窗口");
                }
                if (_unbindItem != null)
                {
                    _unbindItem.Text = Localizer.GetString("Menu_Unbind", "解绑");
                    // 根据绑定状态启用/禁用解绑菜单项
                    if (_unbindItem != null)
                    {
                        _unbindItem.Enabled = IsBound();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateContextMenuTexts", "Failed to update context menu texts: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 更新状态提示文本
        /// </summary>
        private void UpdateStatusTips()
        {
            try
            {
                // 如果正在显示悬停提示，更新它们
                if (_isBindingButtonHovered && bindingStatusbutton != null)
                {
                    string status = _isBound
                        ? Localizer.GetString("ProductDisplay_BoundStatus", "已绑定")
                        : Localizer.GetString("ProductDisplay_UnboundStatus", "未绑定");
                    string tipText = string.Format(Localizer.GetString("ProductDisplay_BindingStatusTip", "绑定状态: {0}"), status);
                    _statusTip.Show(tipText, this,
                    bindingStatusbutton.Location.X + bindingStatusbutton.Width / 2,
                    bindingStatusbutton.Location.Y + bindingStatusbutton.Height,
                    5000);
                }

                if (_isRefreshBadgeHovered && refreshStatusBadge != null)
                {
                    string status = _isRefreshed
                        ? Localizer.GetString("ProductDisplay_RefreshedStatus", "已刷新")
                        : Localizer.GetString("ProductDisplay_NotRefreshedStatus", "未刷新");
                    string tipText = string.Format(Localizer.GetString("ProductDisplay_RefreshStatusTip", "刷新状态: {0}"), status);
                    _statusTip.Show(tipText, this,
                    refreshStatusBadge.Location.X + refreshStatusBadge.Width / 2,
                    refreshStatusBadge.Location.Y + refreshStatusBadge.Height,
                    5000);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateStatusTips", "Failed to update status tips: {0}"), ex.Message), ex);
            }
        }

        #endregion

        #region 控件初始化

        /// <summary>
        /// 初始化悬停提示
        /// </summary>
        private void InitializeHoverTips()
        {
            try
            {
                _statusTip.AutoPopDelay = 5000; // 提示显示时长（毫秒）
                _statusTip.InitialDelay = 100;
                _statusTip.ReshowDelay = 500; // 再次显示的延迟（毫秒）
                _statusTip.ShowAlways = true; // 即使窗体非激活也显示
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToInitializeHoverTips", "Failed to initialize hover tips: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 初始化界面控件
        /// </summary>
        private void InitializeControls()
        {
            try
            {
                // 配置重命名输入框
                if (txtRename != null)
                {
                    txtRename.Visible = false;
                    txtRename.MaxLength = 50;
                    txtRename.Font = new Font("微软雅黑", 10F);
                    txtRename.Padding = new Padding(7, 3, 7, 0);
                    txtRename.TabIndex = 1;
                }
                // 配置标题标签
                if (label1 != null)
                {
                    label1.Font = new Font("微软雅黑", 10F);
                    label1.Padding = new Padding(7, 3, 7, 0);
                    label1.TabIndex = 0;
                    label1.Cursor = Cursors.Hand;
                }

                InitializeBindingStatusButton();
                this.BorderStyle = BorderStyle.FixedSingle;

                // 配置刷新状态 Badge
                if (refreshStatusBadge != null)
                {
                    refreshStatusBadge.ForeColor = Color.White;
                    refreshStatusBadge.BackColor = Color.White;
                    refreshStatusBadge.DotRatio = 0.9f;
                    refreshStatusBadge.Font = new Font("微软雅黑", 9f);

                    // 添加悬停事件
                    refreshStatusBadge.MouseEnter += RefreshStatusBadge_MouseEnter;
                    refreshStatusBadge.MouseLeave += RefreshStatusBadge_MouseLeave;

                    // 设置默认状态
                    UpdateRefreshStatus(false);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToInitializeInterfaceControls", "Failed to initialize interface controls: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 初始化绑定状态按钮
        /// </summary>
        private void InitializeBindingStatusButton()
        {
            try
            {
                if (bindingStatusbutton != null)
                {
                    bindingStatusbutton.Dock = DockStyle.Right;
                    bindingStatusbutton.Size = new Size(24, 41);
                    bindingStatusbutton.Cursor = Cursors.Default;
                    bindingStatusbutton.TextAlign = ContentAlignment.MiddleCenter;
                    bindingStatusbutton.Padding = new Padding(0);
                    bindingStatusbutton.Margin = new Padding(0);
                    bindingStatusbutton.BackColor = Color.White;
                    bindingStatusbutton.OriginalBackColor = Color.White;


                    // 添加事件
                    bindingStatusbutton.MouseEnter += BindingStatusButton_MouseEnter;
                    bindingStatusbutton.MouseLeave += BindingStatusButton_MouseLeave;
                    //bindingStatusbutton.Click += BindingStatusButton_Click;
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToInitializeBindingStatusButton", "Failed to initialize binding status button: {0}"), ex.Message), ex);
            }
        }

        #endregion

        #region 事件初始化
        /// <summary>
        /// 初始化事件处理
        /// </summary>
        private void InitializeEvents()
        {
            try
            {
                // 双击 label1 进入重命名模式
                if (label1 != null)
                {
                    label1.DoubleClick += Label1_DoubleClick;
                }
                else
                {
                    Log.Error(Localizer.GetString("Message_Label1ControlIsNullCannotBindDoubleClickEvent", "label1控件为空，无法绑定双击事件"));
                }
                if (txtRename != null)
                {
                    txtRename.LostFocus += TxtRename_LostFocus;
                    txtRename.KeyDown += TxtRename_KeyDown;
                    txtRename.Leave += TxtRename_Leave;
                }
                else
                {
                    Log.Error(Localizer.GetString("Message_TxtRenameControlIsNullCannotBindEvents", "txtRename控件为空，无法绑定事件"));
                }
                // 记录显示控件事件
                if (insRecordDisplayControl1 != null)
                {
                    splitContainer1.Panel2.SizeChanged += (s, e) =>
                    {
                        insRecordDisplayControl1.Size = splitContainer1.Panel2.ClientSize;
                    };
                }
            }
            catch (Exception ex)
            {
                string tipText = string.Format(Localizer.GetString("Message_FailedToInitializeEventHandling", "初始化事件处理失败: {0}"), ex.Message);
                Log.Error(tipText, ex);
            }
        }
        #endregion

        #region 悬停事件处理
        /// <summary>
        /// 绑定状态按钮鼠标进入事件
        /// </summary>
        private void BindingStatusButton_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (bindingStatusbutton != null)
                {
                    _isBindingButtonHovered = true;
                    // 显示悬停提示
                    string status = _isBound
                        ? Localizer.GetString("ProductDisplay_BoundStatus", "已绑定")
                        : Localizer.GetString("ProductDisplay_UnboundStatus", "未绑定");
                    string tipText = string.Format(Localizer.GetString("ProductDisplay_BindingStatusTip", "绑定状态: {0}"), status);
                    _statusTip.Show(tipText, this,
                    bindingStatusbutton.Location.X + bindingStatusbutton.Width / 2,
                    bindingStatusbutton.Location.Y + bindingStatusbutton.Height,
                    5000);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToHandleBindingStatusButtonMouseEnter", "Failed to handle binding status button mouse enter: {0}"), ex.Message), ex);
            }
        }
        /// <summary>
        /// 更新刷新状态的悬停提示
        /// </summary>
        private void UpdateRefreshStatusTooltip()
        {
            try
            {
                if (refreshStatusBadge != null)
                {
                    string statusText;
                    string description;

                    if (!IsBound())
                    {
                        statusText = Localizer.GetString("ProductDisplay_UnboundStatus", "未绑定");
                        description = Localizer.GetString("ProductDisplay_UnboundDescription", "该显示区域尚未绑定任何数据");
                    }
                    else if (_isRefreshed)
                    {
                        statusText = Localizer.GetString("ProductDisplay_RefreshedStatus", "已刷新");
                        description = Localizer.GetString("ProductDisplay_RefreshedDescription", "数据已最新");
                    }
                    else
                    {
                        statusText = Localizer.GetString("ProductDisplay_NotRefreshedStatus", "未刷新");
                        description = Localizer.GetString("ProductDisplay_NotRefreshedDescription", "数据可能不是最新的");
                    }

                    string tipText = string.Format("{0}\n{1}", statusText, description);

                    // 更新 ToolTip
                    if (_statusTip != null)
                    {
                        _statusTip.SetToolTip(refreshStatusBadge, tipText);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateRefreshStatusTooltip", "Failed to update refresh status tooltip: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 绑定状态按钮鼠标离开事件
        /// </summary>
        private void BindingStatusButton_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (bindingStatusbutton != null)
                {
                    _isBindingButtonHovered = false;
                    // 隐藏悬停提示
                    _statusTip.Hide(this);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToHandleBindingStatusButtonMouseLeave", "Failed to handle binding status button mouse leave: {0}"), ex.Message), ex);
            }
        }
        /// <summary>
        /// 刷新状态 Badge 鼠标进入事件
        /// </summary>
        private void RefreshStatusBadge_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (refreshStatusBadge != null)
                {
                    _isRefreshBadgeHovered = true;
                    // 显示悬停提示
                    UpdateRefreshStatusTooltip();
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToHandleRefreshStatusBadgeMouseEnter", "Failed to handle refresh status badge mouse enter: {0}"), ex.Message), ex);
            }
        }
        /// <summary>
        /// 刷新状态 Badge 鼠标离开事件
        /// </summary>
        private void RefreshStatusBadge_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (refreshStatusBadge != null)
                {
                    _isRefreshBadgeHovered = false;
                    // 隐藏悬停提示
                    _statusTip.Hide(this);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToHandleRefreshStatusBadgeMouseLeave", "Failed to handle refresh status badge mouse leave: {0}"), ex.Message), ex);
            }
        }
        #endregion

        #region 绑定状态按钮点击事件
        /// <summary>
        /// 绑定状态按钮点击事件
        /// </summary>
        private void BindingStatusButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isBindingInProgress)
                {
                    // 如果正在绑定过程中，忽略点击
                    return;
                }

                if (IsBound())
                {
                    // 已绑定状态，显示解绑确认
                    ShowUnbindConfirmation();
                }
                else
                {
                    // 未绑定状态，打开绑定窗口
                    OpenBindForm();
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToHandleBindingStatusButtonClick", "Failed to handle binding status button click: {0}"), ex.Message), ex);
            }
        }
        #endregion

        #region 重命名功能
        /// <summary>
        /// Label 双击事件 - 进入重命名模式
        /// </summary>
        private void Label1_DoubleClick(object sender, EventArgs e)
        {
            if (!_isRenaming && IsBound())
            {
                StartRenaming();
            }
        }
        /// <summary>
        /// 开始重命名
        /// </summary>
        private void StartRenaming()
        {
            try
            {
                if (txtRename == null || label1 == null)
                {
                    Log.Error(string.Format(Localizer.GetString("Message_RenameControlsNotInitialized", "Rename controls not initialized: txtRename={0}, label1={1}"), txtRename != null, label1 != null));
                    return;
                }
                _isRenaming = true;
                // 设置当前名称到输入框
                txtRename.Text = label1.Text;
                // 显示输入框，隐藏标签
                txtRename.Visible = true;
                label1.Visible = false;
                // 确保输入框在最前面
                txtRename.BringToFront();
                // 给输入框设置焦点并选中所有文本
                txtRename.Focus();
                txtRename.SelectAll();
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToStartRenaming", "Failed to start renaming"), ex);
                _isRenaming = false;
            }
        }
        /// <summary>
        /// 完成重命名
        /// </summary>
        private void FinishRenaming()
        {
            if (!_isRenaming || txtRename == null || label1 == null)
            {
                return;
            }
            try
            {
                _isRenaming = false;
                string newName = txtRename.Text.Trim();
                string oldName = label1.Text;
                if (!string.IsNullOrEmpty(newName) && newName != oldName)
                {
                    // 更新显示名称
                    label1.Text = newName;
                    _title = newName;
                    // 如果已绑定，更新显示名称到配置
                    if (IsBound())
                    {
                        DisplayController.Instance.BindControl(
                        this.Name,
                        BoundTaskName,
                        BoundNodeName,
                        BoundRecordName,
                        newName);
                        // 更新记录显示名称属性
                        RecordDisplayName = newName;

                        // 显示成功消息
                        ShowSuccessMessage(Localizer.GetString("Message_RenameSuccessful", "重命名成功！"));
                    }
                }
                else
                {
                    Log.Debug(Localizer.GetString("Message_RenameCancelledOrNameUnchanged", "Rename cancelled or name unchanged"));
                }
                // 恢复显示状态
                txtRename.Visible = false;
                label1.Visible = true;
                // 确保标签在最前面
                label1.BringToFront();
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToFinishRenaming", "Failed to finish renaming"), ex);
                ShowErrorMessage(string.Format(Localizer.GetString("Message_RenameFailed", "重命名失败: {0}"), ex.Message));
                // 异常时也恢复显示状态
                txtRename.Visible = false;
                label1.Visible = true;
                label1.BringToFront();
            }
        }
        /// <summary>
        /// 重命名输入框失去焦点事件
        /// </summary>
        private void TxtRename_LostFocus(object sender, EventArgs e)
        {
            if (_isRenaming)
            {
                FinishRenaming();
            }
        }
        /// <summary>
        /// 重命名输入框离开事件（额外的保险）
        /// </summary>
        private void TxtRename_Leave(object sender, EventArgs e)
        {
            if (_isRenaming)
            {
                FinishRenaming();
            }
        }
        /// <summary>
        /// 重命名输入框按键事件
        /// </summary>
        private void TxtRename_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_isRenaming || txtRename == null)
                return;
            if (e.KeyCode == Keys.Enter)
            {
                // 回车键确认重命名
                FinishRenaming();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                // ESC 键取消重命名
                _isRenaming = false;
                txtRename.Visible = false;
                label1.Visible = true;
                label1.BringToFront();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                // 禁用 Tab 键切换
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        #endregion

        #region 状态显示功能

        /// <summary>
        /// 更新绑定状态显示
        /// </summary>
        private void UpdateBindingStatus()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateBindingStatus));
                return;
            }

            try
            {
                _isBound = IsBound();
                UpdateBindingStatusDisplay();
                UpdateContextMenuTexts();

                // 当绑定状态改变时，更新刷新状态显示
                UpdateRefreshStatus(_isRefreshed);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateBindingStatus", "Failed to update binding status: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 更新绑定状态显示内容
        /// </summary>
        private void UpdateBindingStatusDisplay()
        {
            try
            {
                if (bindingStatusbutton != null)
                {
                    // 根据绑定状态设置不同的 SVG 图标和样式
                    if (_isBound)
                    {
                        // 绑定状态
                        bindingStatusbutton.IconSvg = @"<svg t=""1763458427948"" class=""icon"" viewBox=""0 0 1024 1024"" version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" p-id=""2170"" width=""200"" height=""200""><path d=""M704.32 105.408 519.872 289.856C473.152 336.512 466.176 408.192 497.856 465.152L465.152 497.856c-56.96-31.744-128.64-24.704-175.36 21.952l-184.384 184.384c-56.896 56.896-54.912 150.912 4.288 210.112 59.136 59.136 153.216 60.992 210.112 4.224l184.448-184.448c46.592-46.656 53.568-118.464 21.952-175.296l32.704-32.704c56.896 31.68 128.64 24.64 175.232-21.952l184.448-184.384c56.896-56.896 54.976-150.976-4.16-210.112C855.232 50.496 761.216 48.576 704.32 105.408zM407.872 705.408 286.08 827.2c-37.504 37.504-87.936 47.872-112.512 23.232-24.704-24.704-14.208-75.136 23.296-112.64l121.728-121.664c22.336-22.336 49.152-34.944 72.384-36.416C386.752 594.304 390.144 610.688 401.728 622.336c11.52 11.52 27.904 14.976 42.56 10.688C442.816 656.256 430.144 683.136 407.872 705.408zM824.704 288.64l-121.664 121.664c-21.248 21.312-46.72 33.856-69.12 36.224 6.464-15.616 3.264-34.304-9.408-46.976C611.776 386.816 593.216 383.808 577.472 390.272 579.968 367.744 592.384 342.336 613.76 321.024l121.664-121.728c37.568-37.504 87.936-47.936 112.576-23.296C872.768 200.64 862.272 251.072 824.704 288.64z"" fill=""#8F9EB2"" p-id=""2171""></path></svg>";
                        bindingStatusbutton.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        // 未绑定状态
                        bindingStatusbutton.IconSvg = @"<svg t=""1763458440936"" class=""icon"" viewBox=""0 0 1024 1024"" version=""1.1"" xmlns=""http://www.w3.org/2000/svg"" p-id=""2320"" width=""200"" height=""200""><path d=""M799.584 687.424l53.728-43.488c131.328-106.368 151.584-299.04 45.248-430.368S599.52 61.952 468.192 168.32l-51.52 41.728L306.72 72.96a32 32 0 0 0-49.28 40.832l640.416 798.4a32 32 0 0 0 49.28-40.832l-147.552-183.936zM518.528 230.496a225.984 225.984 0 1 1 284.448 351.264l-53.44 43.264-118.72-148 84.864-70.688-51.232-61.472-83.68 69.76-114.048-142.176 51.808-41.952zM504.384 823.552a225.984 225.984 0 1 1-284.448-351.264l110.976-89.856-50.336-62.176L169.6 410.112C38.272 516.448 18.016 709.152 124.352 840.48c106.368 131.328 299.04 151.584 430.368 45.248l105.12-85.12-50.336-62.176-105.12 85.12z"" p-id=""2321""></path><path d=""M446.72 526.304l51.2 61.44-125.856 104.864-51.2-61.44z"" p-id=""2322""></path></svg>";
                        bindingStatusbutton.Cursor = Cursors.Hand;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateBindingStatusDisplay", "Failed to update binding status display: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 更新刷新状态显示
        /// </summary>
        /// <param name="isRefreshed">true = 已刷新（绿色），false = 未刷新（红色）</param>
        public void UpdateRefreshStatus(bool isRefreshed)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool>(UpdateRefreshStatus), isRefreshed);
                return;
            }

            try
            {
                _isRefreshed = isRefreshed;

                if (refreshStatusBadge != null)
                {
                    // 根据绑定状态和刷新状态设置不同样式
                    if (!IsBound())
                    {
                        // 未绑定状态 - 灰色
                        refreshStatusBadge.Fill = Color.Gray;
                        refreshStatusBadge.Text = "未绑定";
                        refreshStatusBadge.State = TState.Default;
                    }
                    else if (_isRefreshed)
                    {
                        // 已绑定且已刷新 - 绿色
                        refreshStatusBadge.Fill = Color.Green;
                        refreshStatusBadge.Text = "已刷新";
                        refreshStatusBadge.State = TState.Success;
                    }
                    else
                    {
                        // 已绑定但未刷新 - 红色
                        refreshStatusBadge.Fill = Color.Red;
                        refreshStatusBadge.Text = "未刷新";
                        refreshStatusBadge.State = TState.Error;
                    }

                    // 更新悬停提示
                    UpdateRefreshStatusTooltip();

                    refreshStatusBadge.Update();
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateRefreshStatus", "Failed to update refresh status: {0}"), ex.Message), ex);
            }
        }

        #endregion

        #region 属性
        /// <summary>
        /// 控件标题属性
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    if (label1 != null && !_isRenaming)
                    {
                        label1.Text = _title;
                    }
                    if (_isRenaming && txtRename != null)
                    {
                        txtRename.Text = value;
                    }
                }
            }
        }
        /// <summary>
        /// 父显示控件
        /// </summary>
        public UCProductDisplay ParentDisplay { get; set; }
        #endregion

        #region 绑定配置方法
        /// <summary>
        /// 设置绑定配置
        /// </summary>
        public void SetBindingConfig(InsRecord record, string taskName, string nodeName, string recordName, string recordDisplayName = "")
        {
            try
            {
                _isBindingInProgress = true;

                // 验证参数
                if (string.IsNullOrEmpty(taskName) || string.IsNullOrEmpty(nodeName) || string.IsNullOrEmpty(recordName))
                {
                    throw new ArgumentException(Localizer.GetString("Message_BindingParametersCannotBeEmpty", "绑定参数不能为空"));
                }

                _boundRecord = record;
                BoundTaskName = taskName;
                BoundNodeName = nodeName;
                BoundRecordName = recordName;
                RecordDisplayName = !string.IsNullOrEmpty(recordDisplayName) ? recordDisplayName : recordName;

                // 如果正在重命名，先结束重命名
                if (_isRenaming)
                {
                    FinishRenaming();
                }

                UpdateTitleDisplay();
                // 如果绑定成功，取消高亮
                if (IsBound())
                {
                    RemoveHighlight();
                }
                // 更新绑定状态
                UpdateBindingStatus();

                if (record != null)
                {
                    RefreshDisplaySafe(record);
                    // 刷新后更新刷新状态
                    UpdateRefreshStatus(true);

                }
                else
                {
                    // 记录为空时，重置刷新状态
                    UpdateRefreshStatus(false);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToSetBindingConfig", "Failed to set binding config: {0}"), ex.Message), ex);
                ShowErrorMessage(string.Format(Localizer.GetString("Message_BindingFailed", "绑定失败: {0}"), ex.Message));
            }
            finally
            {
                _isBindingInProgress = false;
            }
        }

        /// <summary>
        /// 清除绑定
        /// </summary>
        public void ClearBinding()
        {
            try
            {
                _isBindingInProgress = true;

                // 如果正在重命名，先结束重命名
                if (_isRenaming)
                {
                    FinishRenaming();
                }

                _boundRecord = null;
                BoundTaskName = string.Empty;
                BoundNodeName = string.Empty;
                BoundRecordName = string.Empty;
                RecordDisplayName = string.Empty;

                if (insRecordDisplayControl1 != null)
                {
                    insRecordDisplayControl1.Record = null;
                    insRecordDisplayControl1.Update();
                }

                UpdateTitleDisplay();
                UpdateBindingStatus();
                UpdateRefreshStatus(false);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToClearBinding", "Failed to clear binding: {0}"), ex.Message), ex);
                ShowErrorMessage(string.Format(Localizer.GetString("Message_UnbindingFailed", "解绑失败: {0}"), ex.Message));
            }
            finally
            {
                _isBindingInProgress = false;
            }
        }

        /// <summary>
        /// 设置默认显示
        /// </summary> 
        public void SetDefaultDisplay()
        {
            try
            {
                Title = Localizer.GetString("Message_NoBoundDisplayImagePleaseRightClickToBind", "没有绑定的显示图像，请右键点击进行绑定");
                ClearBinding();
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToSetDefaultDisplay", "Failed to set default display: {0}"), ex.Message), ex);
            }
        }
        #endregion

        #region 右键菜单功能
        /// <summary>
        /// 初始化标签右键菜单
        /// </summary>
        private void InitLabelRightMenu()
        {
            try
            {
                _labelRightMenu = new AntdUI.ContextMenuStripItem(Localizer.GetString("Menu_OpenDisplayBindingInterface", "打开显示绑定窗口"));
                _unbindItem = new AntdUI.ContextMenuStripItem(Localizer.GetString("Menu_Unbind", "解绑"));

                if (label1 != null)
                {
                    label1.MouseDown += (sender, e) =>
                    {
                        // 如果正在重命名，不显示右键菜单
                        if (_isRenaming || _isBindingInProgress)
                            return;

                        if (e.Button == MouseButtons.Right)
                        {
                            var menuItems = new List<AntdUI.IContextMenuStripItem>();

                            // 添加绑定菜单项
                            menuItems.Add(_labelRightMenu);

                            // 如果已绑定，添加解绑菜单项
                            if (IsBound())
                            {
                                menuItems.Add(_unbindItem);
                            }

                            var menuConfig = new AntdUI.ContextMenuStrip.Config(
                            control: label1,
                            call: (clickedItem) =>
                            {
                                string openMenuText = Localizer.GetString("Menu_OpenDisplayBindingInterface", "打开显示绑定窗口");
                                string unbindMenuText = Localizer.GetString("Menu_Unbind", "解绑");

                                if (clickedItem.Text == openMenuText)
                                {
                                    OpenBindForm();
                                }
                                else if (clickedItem.Text == unbindMenuText)
                                {
                                    ShowUnbindConfirmation();
                                }
                            },
                            items: menuItems.ToArray()
                            )
                            .SetRadius(8)
                            .SetAlign(AntdUI.TAlign.RB);

                            menuConfig.open();
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToInitializeContextMenu", "Failed to initialize context menu: {0}"), ex.Message), ex);
            }
        }
        #endregion

        #region 绑定和解绑功能
        /// <summary>
        /// 打开绑定窗口
        /// </summary>
        private void OpenBindForm()
        {
            try
            {
                if (_isBindingInProgress)
                {
                    return;
                }

                if (ParentDisplay == null)
                {
                    ShowErrorMessage(Localizer.GetString("Message_CannotGetParentContainerControlInformation", "无法获取父容器控件信息"));
                    return;
                }

                WProductDisplaybind bindForm = new WProductDisplaybind(
                    ParentDisplay,
                    currentTaskName: this.BoundTaskName,
                    currentNodeName: this.BoundNodeName,
                    currentRecordName: this.BoundRecordName,
                    currentRecordDisplayName: this.RecordDisplayName
                );

                bindForm.BindingConfirmed += async (s, args) =>
                {
                    try
                    {
                        _isBindingInProgress = true;

                        // 验证绑定参数
                        if (string.IsNullOrEmpty(args.TaskName) || string.IsNullOrEmpty(args.NodeName) || string.IsNullOrEmpty(args.RecordName))
                        {
                            ShowErrorMessage(Localizer.GetString("Message_BindingParametersCannotBeEmpty", "绑定参数不能为空"));
                            return;
                        }

                        // 调用控制器绑定
                        DisplayController.Instance.BindControl(
                        this.Name,
                        args.TaskName,
                        args.NodeName,
                        args.RecordName,
                        args.RecordDisplayName);

                        ShowSuccessMessage(Localizer.GetString("Message_BindingSuccessful", "绑定成功！"));
                    }
                    catch (Exception ex)
                    {
                        ShowErrorMessage(string.Format(Localizer.GetString("Message_BindingFailed", "绑定失败: {0}"), ex.Message));
                    }
                    finally
                    {
                        _isBindingInProgress = false;
                    }
                };

                bindForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToOpenBindForm", "Failed to open bind form: {0}"), ex.Message), ex);
                ShowErrorMessage(string.Format(Localizer.GetString("Message_FailedToOpenBindingWindow", "打开绑定窗口失败: {0}"), ex.Message));
            }
        }

        /// <summary>
        /// 显示解绑确认对话框
        /// </summary>
        private void ShowUnbindConfirmation()
        {
            try
            {
                if (!IsBound() || _isBindingInProgress)
                {
                    return;
                }

                var confirmResult = MessageBox.Show(
                Localizer.GetString("Message_AreYouSureYouWantToUnbindTheCurrentDisplay", "确定要解绑当前显示吗？\n解绑后将返回未绑定状态。"),
                Localizer.GetString("Message_ConfirmUnbinding", "确认解绑"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
                );

                if (confirmResult == DialogResult.Yes)
                {
                    PerformUnbind();
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToShowUnbindConfirmation", "Failed to show unbind confirmation: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 执行解绑操作
        /// </summary>
        private void PerformUnbind()
        {
            try
            {
                _isBindingInProgress = true;

                // 调用控制器解绑
                DisplayController.Instance.UnbindControl(this.Name);

                ShowSuccessMessage(Localizer.GetString("Message_BindingHasBeenSuccessfullyCancelled", "绑定已成功取消！"));
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUnbind", "Failed to unbind: {0}"), ex.Message), ex);
                ShowErrorMessage(string.Format(Localizer.GetString("Message_UnbindingFailed", "解绑失败: {0}"), ex.Message));
            }
            finally
            {
                _isBindingInProgress = false;
            }
        }
        #endregion

        #region 消息显示方法
        /// <summary>
        /// 显示成功消息
        /// </summary>
        private void ShowSuccessMessage(string message)
        {
            try
            {
                if (label1 != null)
                {
                    AntdUI.Message.success(target: new AntdUI.Target(label1), text: message, null, 3);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToShowSuccessMessage", "Failed to show success message: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 显示错误消息
        /// </summary>
        private void ShowErrorMessage(string message)
        {
            try
            {
                if (label1 != null)
                {
                    AntdUI.Message.error(target: new AntdUI.Target(label1), text: message, null, 3);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToShowErrorMessage", "Failed to show error message: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 显示警告消息
        /// </summary>
        private void ShowWarningMessage(string message)
        {
            try
            {
                if (label1 != null)
                {
                    AntdUI.Message.warn(target: new AntdUI.Target(label1), text: message, null, 3);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToShowWarningMessage", "Failed to show warning message: {0}"), ex.Message), ex);
            }
        }
        #endregion

        #region 工具方法

        /// <summary>
        /// 递归查找指定类型的父控件
        /// </summary>
        /// <typeparam name="T">要查找的父控件类型</typeparam>
        /// <param name="control">当前控件</param>
        /// <returns>找到的父控件，未找到返回 null</returns>
        private T FindParentControl<T>(Control control) where T : Control
        {
            if (control == null)
                return null;

            // 若当前父控件就是目标类型，直接返回
            if (control.Parent is T targetParent)
                return targetParent;

            // 否则递归查找上一级父控件
            return FindParentControl<T>(control.Parent);
        }

        #endregion

        #region 显示刷新方法

        /// <summary>
        /// 更新标题显示
        /// </summary>
        private void UpdateTitleDisplay()
        {
            try
            {
                if (!_isRenaming && label1 != null)
                {
                    if (IsBound())
                    {
                        // 如果有显示名称，使用显示名称；否则使用记录名称
                        string displayName = !string.IsNullOrEmpty(RecordDisplayName) ? RecordDisplayName : BoundRecordName;
                        label1.Text = displayName;
                        _title = displayName;
                    }
                    else
                    {
                        label1.Text = Localizer.GetString("Message_NoBoundDisplayImagePleaseRightClickToBind", "未绑定显示图像，请右键点击进行绑定");
                        _title = label1.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateTitleDisplay", "Failed to update title display: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 安全刷新显示
        /// </summary>
        public void RefreshDisplaySafe(InsRecord record)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => RefreshDisplay(record)));
            }
            else
            {
                RefreshDisplay(record);
            }
        }

        /// <summary>
        /// 刷新显示内容
        /// </summary>
        public void RefreshDisplay(InsRecord record)
        {
            try
            {
                if (record != null && insRecordDisplayControl1 != null)
                {
                    // 更新记录对象
                    _boundRecord = record;

                    // 更新显示内容
                    insRecordDisplayControl1.Record = record;
                    insRecordDisplayControl1.Update();

                    // 更新标题显示（只有在非重命名模式下）
                    if (!_isRenaming)
                    {
                        UpdateTitleDisplay();
                    }

                    // 更新刷新状态
                    UpdateRefreshStatus(true);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToRefreshDisplay", "Failed to refresh display: {0}"), ex.Message), ex);
                UpdateRefreshStatus(false);
            }
        }

        /// <summary>
        /// 检查控件是否已绑定
        /// </summary>
        public bool IsBound()
        {
            return !string.IsNullOrEmpty(BoundTaskName) &&
                   !string.IsNullOrEmpty(BoundNodeName) &&
                   !string.IsNullOrEmpty(BoundRecordName);
        }

        /// <summary>
        /// 高亮显示控件（用于提示未绑定）
        /// </summary>
        /// <param name="durationMs">高亮持续时间（毫秒），0 表示永久高亮</param>
        public void Highlight(int durationMs = 0)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(Highlight), durationMs);
                return;
            }

            try
            {
                // 设置高亮颜色（醒目的红色背景）
                if (label1 != null)
                {
                    label1.BackColor = Color.FromArgb(255, 220, 220);
                }

                // 确保边框可见
                if (this.BorderStyle == BorderStyle.None)
                {
                    this.BorderStyle = BorderStyle.FixedSingle;
                }

                // 如果设置了持续时间，启动定时器恢复
                if (durationMs > 0)
                {
                    if (_highlightTimer == null)
                    {
                        _highlightTimer = new System.Windows.Forms.Timer();
                        _highlightTimer.Tick += HighlightTimer_Tick;
                    }
                    else
                    {
                        // 如果定时器已存在，先停止并重置
                        _highlightTimer.Stop();
                    }
                    _highlightTimer.Interval = durationMs;
                    _highlightTimer.Start();
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToHighlightDisplayControl", "Failed to highlight display control: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 取消高亮显示
        /// </summary>
        public void RemoveHighlight()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RemoveHighlight));
                return;
            }

            try
            {
                // 停止定时器
                if (_highlightTimer != null)
                {
                    _highlightTimer.Stop();
                    _highlightTimer.Dispose();
                    _highlightTimer = null;
                }

                // 恢复原始背景色
                if (label1 != null)
                {
                    label1.BackColor = _originalBackColor;
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToCancelHighlightDisplay", "Failed to cancel highlight display: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 高亮定时器事件处理
        /// </summary>
        private void HighlightTimer_Tick(object sender, EventArgs e)
        {
            RemoveHighlight();
        }
        #endregion

    }
}
