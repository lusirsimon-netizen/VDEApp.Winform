using AntdUI;
using Insnex.Vision2D.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Configs.Display;
using VDEApp.Controllers;
using VDEApp.Controllers.Display;
using VDEApp.LogModule;
using VDEApp.Models;
using VDEApp.Models.Product;
using Label = AntdUI.Label;

namespace VDEApp.Views.Main
{
    /// <summary>
    /// 产品显示主控件
    /// 专门负责界面展示，不包含业务逻辑
    /// </summary>
    public partial class UCProductDisplay : UserControl, IMainLoader
    {
        #region 成员变量
        /// <summary>
        /// 子控件列表
        /// </summary>
        public List<UCProductDisplayRecordItem> _childControls = new List<UCProductDisplayRecordItem>();

        /// <summary>
        /// 绑定到显示控制器
        /// </summary>
        private readonly DisplayController _displayController = DisplayController.Instance;

        /// <summary>
        /// 缺陷图像标题标签
        /// </summary>
        private AntdUI.Label _defectTitleLabel;

        /// <summary>
        /// 当前打开的大图查看器
        /// </summary>
        private Form _currentLargeViewForm = null;

        /// <summary>
        /// 节点更新状态跟踪
        /// Key: Tuple(TaskName, NodeName)
        /// Value: 本次更新中出现的记录名称集合
        /// </summary>
        private readonly Dictionary<Tuple<string, string>, HashSet<string>> _nodeUpdateStatus =
            new Dictionary<Tuple<string, string>, HashSet<string>>();

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCProductDisplay()
        {
            InitializeComponent();
        }

        public void InitializeUI()
        {
            // 初始化控制器
            _displayController.Initialize();

            // 订阅事件
            SubscribeToEvents();

            // 获取初始布局配置并生成布局
            var (rows, columns) = _displayController.GetCurrentLayout();
            GenerateLayoutUI(rows, columns);

            // 根据配置设置Segmented1的默认选中状态
            SetSegmentedDefaultSelection(rows, columns);

            // 订阅任务数量改变事件
            _displayController.TaskNumberChanged += OnTaskNumberChanged;
            // 加载绑定配置
            LoadBindingConfigsUI();

            // 初始化缺陷显示面板
            InitializeDefectDisplayPanel();

            AppModuleSingleton.LanguageSwitchEvent += (o, e) => SetLocalizedTexts();
            // 设置本地化文本
            SetLocalizedTexts();
        }

        /// <summary>
        /// 根据配置设置Segmented1的默认选中状态
        /// </summary>
        private void SetSegmentedDefaultSelection(int rows, int columns)
        {
            try
            {
                if (segmented1 != null && segmented1.Items.Count >= 3)
                {
                    // 根据行数和列数设置默认选中项
                    if (rows == 1 && columns == 1)
                    {
                        // 1X1布局
                        segmented1.SelectIndex = 0;
                    }
                    else if (rows == 2 && columns == 2)
                    {
                        // 2X2布局
                        segmented1.SelectIndex = 1;
                    }
                    else
                    {
                        // 自定义布局
                        segmented1.SelectIndex = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToSetSegmentedDefaultSelection", "Failed to set segmented default selection"), ex);
            }
        }

        /// <summary>
        /// 设置本地化文本
        /// </summary>
        private void SetLocalizedTexts()
        {
            // 设置标题标签文本
            if (label1 != null)
            {
                label1.Text = Localizer.GetString("ProductDisplay_ProductionDisplay", "Production display");
            }

            // 设置分段控件文本
            if (segmented1 != null && segmented1.Items.Count >= 3)
            {
                segmented1.Items[2].Text = Localizer.GetString("ProductDisplay_CustomLayout", "Custom Layout");
            }
        }

        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;
        /// <summary>
        /// 订阅控制器事件
        /// </summary>
        private void SubscribeToEvents()
        {
            _displayController.RecordUpdated += OnRecordUpdated;
            _displayController.DefectImagesUpdated += OnDefectImagesUpdated;
            _displayController.BindingUpdated += OnBindingUpdated;
            _displayController.TaskCompleted += OnTaskCompleted;
        }
        /// <summary>
        /// 处理任务数量改变事件
        /// </summary>
        private void OnTaskNumberChanged(object sender, EventArgs e)
        {
            try
            {
                // 重新订阅控制器事件
                SubscribeToEvents();
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToResubscribeToEventsOnTaskNumberChange", "Failed to resubscribe to events on task number change"), ex);
            }
        }
        #endregion

        #region UI布局管理

        /// <summary>
        /// 生成布局UI
        /// </summary>
        public void GenerateLayoutUI(int rows, int columns)
        {
            try
            {
                // 清除现有布局和控件
                ClearLayoutUI();

                // 设置表格布局的行列数
                tableLayoutPanel1.RowCount = rows;
                tableLayoutPanel1.ColumnCount = columns;

                // 设置行列样式
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.ColumnStyles.Clear();
                for (int i = 0; i < rows; i++)
                {
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
                }

                for (int i = 0; i < columns; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
                }

                // 添加子控件
                var controlIds = _displayController.GenerateLayoutControlIds(rows, columns);
                int index = 0;
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        AddChildControlUI(row, col, controlIds[index]);
                        index++;
                    }
                }
                // 调整其他控件位置
                AdjustOtherControlsPosition();
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToGenerateLayout", "Failed to generate layout"), ex);
                throw;
            }
        }

        /// <summary>
        /// 添加子控件UI
        /// </summary>
        private void AddChildControlUI(int row, int col, string controlId)
        {
            try
            {
                var childControl = new UCProductDisplayRecordItem();
                childControl.Dock = DockStyle.Fill;
                childControl.Margin = new Padding(5);
                childControl.Name = controlId;
                childControl.TabIndex = _childControls.Count;

                // 设置父控件引用
                childControl.ParentDisplay = this;

                tableLayoutPanel1.Controls.Add(childControl, col, row);
                _childControls.Add(childControl);

                // 检查是否有绑定配置
                var binding = _displayController.GetControlBinding(controlId);
                if (binding != null)
                {
                    UpdateChildControlBindingUI(childControl, binding);
                }
                else
                {
                    // 设置默认显示
                    childControl.SetDefaultDisplay();
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToAddChildControlToPosition", "Failed to add child control to position ({0}, {1})"), row, col), ex);
            }
        }

        /// <summary>
        /// 清除布局UI
        /// </summary>
        private void ClearLayoutUI()
        {
            try
            {
                // 清理子控件引用
                foreach (var child in _childControls)
                {
                    if (child != null && !child.IsDisposed)
                    {
                        child.Dispose();
                    }
                }
                _childControls.Clear();

                // 清理表格布局
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowCount = 0;
                tableLayoutPanel1.ColumnCount = 0;
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToClearLayout", "Failed to clear layout"), ex);
            }
        }

        #endregion

        #region 绑定管理UI

        /// <summary>
        /// 加载绑定配置UI
        /// </summary>
        private void LoadBindingConfigsUI()
        {
            try
            {
                var bindings = _displayController.GetAllBindings();
                foreach (var binding in bindings)
                {
                    var childControl = _childControls.FirstOrDefault(c => c.Name == binding.ControlId);
                    if (childControl != null)
                    {
                        UpdateChildControlBindingUI(childControl, binding);
                    }
                    else
                    {
                        Log.Warn(string.Format(Localizer.GetString("Message_ControlInBindingConfigurationDoesNotExist", "Control in binding configuration does not exist: {0}"), binding.ControlId));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToLoadBindingConfigurations", "Failed to load binding configurations"), ex);
            }
        }

        /// <summary>
        /// 更新子控件绑定UI
        /// </summary>
        private void UpdateChildControlBindingUI(UCProductDisplayRecordItem childControl, BindingConfig binding)
        {
            if (childControl == null || binding == null)
                return;

            try
            {
                var record = _displayController.GetRecord(
                    binding.TaskName, binding.NodeName, binding.RecordName);

                childControl.SetBindingConfig(
                    record,
                    binding.TaskName,
                    binding.NodeName,
                    binding.RecordName,
                    binding.RecordDisplayName);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToUpdateChildControlBinding", "Failed to update child control binding: {0}"), childControl.Name), ex);
            }
        }

        #endregion

        #region 缺陷图像显示UI

        /// <summary>
        /// 初始化缺陷显示面板
        /// </summary>
        private void InitializeDefectDisplayPanel()
        {
            try
            {
                if (tableLayoutPanel3 == null)
                {
                    // 创建缺陷图像显示面板
                    tableLayoutPanel3 = new TableLayoutPanel
                    {
                        Dock = DockStyle.Top,
                        AutoScroll = true,
                        RowCount = 1,
                        ColumnCount = 0,
                        Height = 0, // 初始高度为0
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.WhiteSmoke,
                        Padding = new Padding(10),
                        Name = "tableLayoutPanel3"
                    };

                    tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                    tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                    // 添加面板到父容器
                    this.Controls.Add(tableLayoutPanel3);

                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToInitializeDefectDisplayPanel", "Failed to initialize defect display panel"), ex);
            }
        }

        /// <summary>
        /// 添加缺陷小图到显示面板
        /// </summary>
        private void AddDefectImagesToTableLayoutPanel(List<Image> defectImages, ProductionDataEvent productionData)
        {
            try
            {
                if (tableLayoutPanel3 == null || defectImages == null || defectImages.Count == 0)
                {
                    // 如果没有缺陷图像，隐藏面板
                    if (tableLayoutPanel3 != null)
                    {
                        tableLayoutPanel3.Controls.Clear();
                        tableLayoutPanel3.Height = 0;
                    }
                    AdjustOtherControlsPosition();
                    return;
                }

                if (tableLayoutPanel3.InvokeRequired)
                {
                    tableLayoutPanel3.Invoke(new Action(() =>
                        AddDefectImagesToTableLayoutPanel(defectImages, productionData)));
                    return;
                }


                // 限制缺陷图像显示数量（最多显示10个，避免过于拥挤）
                int maxDefectImages = 10;
                int imageCount = Math.Min(defectImages.Count, maxDefectImages);

                // 清除现有控件
                tableLayoutPanel3.Controls.Clear();

                // 设置列数（每行最多显示5个图像）
                int columnsPerRow = 10;
                int rows = (int)Math.Ceiling((double)imageCount / columnsPerRow);

                tableLayoutPanel3.ColumnCount = columnsPerRow;
                tableLayoutPanel3.RowCount = rows;

                // 设置列样式（平均分配宽度）
                tableLayoutPanel3.ColumnStyles.Clear();
                for (int i = 0; i < columnsPerRow; i++)
                {
                    tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columnsPerRow));
                }

                // 设置行样式
                tableLayoutPanel3.RowStyles.Clear();
                for (int i = 0; i < rows; i++)
                {
                    tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 120f));
                }

                for (int i = 0; i < imageCount; i++)
                {
                    Image defectImage = defectImages[i];
                    int row = i / columnsPerRow;
                    int column = i % columnsPerRow;

                    // 创建图像容器（使用圆角面板效果）
                    var imageContainer = new System.Windows.Forms.Panel
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(8),
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.White,
                        Padding = new Padding(4)
                    };

                    // 添加阴影效果（使用Panel嵌套）
                    var shadowPanel = new System.Windows.Forms.Panel
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.LightGray,
                        Padding = new Padding(1)
                    };
                    imageContainer.Controls.Add(shadowPanel);

                    // 创建图像显示面板
                    var imagePanel = new System.Windows.Forms.Panel
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.White,
                        Padding = new Padding(2)
                    };
                    shadowPanel.Controls.Add(imagePanel);

                    // 创建图像 PictureBox
                    var pictureBox = new PictureBox
                    {
                        Image = defectImage,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        BorderStyle = BorderStyle.None,
                        Tag = new
                        {
                            ProductionName = productionData?.ProductionName,
                            ProductionTime = productionData?.ProductionTime,
                            SN = productionData?.SN,
                            Status = productionData?.Status,
                            AddedTime = DateTime.Now,
                            Index = i + 1
                        },
                        Cursor = Cursors.Hand
                    };

                    // 创建信息面板
                    var infoPanel = new System.Windows.Forms.Panel
                    {
                        Dock = DockStyle.Bottom,
                        Height = 30,
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.Transparent
                    };

                    // 创建序号标签
                    var indexLabel = new Label
                    {
                        Text = $"#{i + 1}",
                        Dock = DockStyle.Left,
                        Width = 25,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Microsoft YaHei", 9f, FontStyle.Bold),
                        ForeColor = Color.White,
                        BackColor = Color.IndianRed,
                        //BorderStyle = BorderStyle.None
                    };

                    // 创建尺寸标签
                    var sizeLabel = new Label
                    {
                        Text = $"{defectImage.Width}×{defectImage.Height}",
                        Dock = DockStyle.Right,
                        Width = 60,
                        TextAlign = ContentAlignment.MiddleRight,
                        Font = new Font("Microsoft YaHei", 8f),
                        ForeColor = Color.Gray,
                        BackColor = Color.Transparent,
                        Padding = new Padding(0, 0, 5, 0)
                    };

                    infoPanel.Controls.Add(indexLabel);
                    infoPanel.Controls.Add(sizeLabel);

                    // 将控件添加到容器
                    imagePanel.Controls.Add(pictureBox);
                    imagePanel.Controls.Add(infoPanel);

                    // 添加点击事件查看大图
                    pictureBox.Click += PictureBox_Click;

                    // 添加悬停效果
                    imageContainer.MouseEnter += (sender, e) =>
                    {
                        var panel = sender as System.Windows.Forms.Panel;
                        if (panel != null)
                        {
                            panel.BackColor = Color.LightBlue;
                            panel.Padding = new Padding(3); // 轻微缩小，产生凹陷效果
                        }
                    };

                    imageContainer.MouseLeave += (sender, e) =>
                    {
                        var panel = sender as System.Windows.Forms.Panel;
                        if (panel != null)
                        {
                            panel.BackColor = Color.White;
                            panel.Padding = new Padding(4);
                        }
                    };

                    // 添加到表格布局
                    tableLayoutPanel3.Controls.Add(imageContainer, column, row);
                }

                // 调整布局
                AdjustOtherControlsPosition();

            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToAddDefectImages", "Failed to add defect images"), ex);
            }
        }

        /// <summary>
        /// 图片点击事件处理
        /// </summary>
        private void PictureBox_Click(object sender, EventArgs e)
        {
            try
            {
                var pictureBox = sender as PictureBox;
                if (pictureBox == null || pictureBox.Image == null)
                {
                    return;
                }

                // 关闭已打开的大图查看器
                CloseCurrentLargeView();

                // 使用UI线程打开大图查看器
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                        ShowImageInLargeView(pictureBox.Image, pictureBox.Tag)));
                }
                else
                {
                    ShowImageInLargeView(pictureBox.Image, pictureBox.Tag);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToOpenLargeImageViewer", "Failed to open large image viewer"), ex);
            }
        }

        /// <summary>
        /// 显示大图查看窗口
        /// </summary>
        private void ShowImageInLargeView(Image image, object tagData)
        {
            try
            {
                // 创建大图查看器窗口
                _currentLargeViewForm = new Form
                {
                    Text = Localizer.GetString("Message_DefectImageDetails", "Defect Image Details"),
                    Size = new Size(900, 700),
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.Sizable,
                    MaximizeBox = true,
                    MinimizeBox = true,
                    ShowInTaskbar = true
                };

                // 窗口关闭事件处理
                _currentLargeViewForm.FormClosing += (sender, e) =>
                {
                    try
                    {
                        // 清理当前引用
                        _currentLargeViewForm = null;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Localizer.GetString("Message_FailedToReleaseResourcesWhenClosingLargeImageViewer", "Failed to release resources when closing large image viewer"), ex);
                    }
                };

                // 创建图片容器
                var pictureContainer = new System.Windows.Forms.Panel
                {
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.White,
                    Padding = new Padding(20)
                };

                var insDisplayControl = new Insnex.Vision2D.Controls.InsDisplay
                {
                    Dock = DockStyle.Fill,
                    CanvasColor = Color.White,
                    MouseMode = InsDisplayMouseModeConstants.Pointer,
                    ImageMouseZoomEnabled = true,
                    ImageDoubleClickFitEnabled = true
                };
                if (image != null)
                {
                    IInsImage insImage = null;

                    if (image is IInsImage directInsImage)
                    {
                        insImage = directInsImage;
                    }
                    else if (image is System.Drawing.Image drawingImage)
                    {
                        using (var bitmap = new Bitmap(drawingImage))
                        {
                            insImage = new InsImage24PlanarColor(bitmap);
                        }
                    }

                    if (insImage != null)
                    {
                        insDisplayControl.Image = insImage;
                        insDisplayControl.AutoFitImage();
                    }
                }

                pictureContainer.Controls.Add(insDisplayControl);

                _currentLargeViewForm.Controls.Add(pictureContainer);

                // 显示窗口
                _currentLargeViewForm.ShowDialog();

            }
            catch (OutOfMemoryException ex)
            {
                Log.Error(Localizer.GetString("Message_InsufficientMemoryWhenOpeningLargeImageViewer", "Insufficient memory when opening large image viewer"), ex);
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToOpenLargeImageViewer", "Failed to open large image viewer"), ex);
            }
        }

        /// <summary>
        /// 关闭当前打开的大图查看器
        /// </summary>
        private void CloseCurrentLargeView()
        {
            try
            {
                if (_currentLargeViewForm != null && !_currentLargeViewForm.IsDisposed)
                {
                    _currentLargeViewForm.Close();
                    _currentLargeViewForm.Dispose();
                    _currentLargeViewForm = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToCloseLargeImageViewer", "Failed to close large image viewer"), ex);
            }
        }

        #endregion

        #region 事件处理UI

        /// <summary>
        /// 记录更新事件处理
        /// </summary>
        private void OnRecordUpdated(object sender, RecordUpdatedEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => OnRecordUpdated(sender, e)));
                    return;
                }

                if (e == null || string.IsNullOrEmpty(e.TaskName) ||
                    string.IsNullOrEmpty(e.NodeName) || string.IsNullOrEmpty(e.RecordName) || e.Record == null)
                {
                    return;
                }

                var nodeKey = Tuple.Create(e.TaskName, e.NodeName);

                // 记录本次更新的记录
                lock (_nodeUpdateStatus)
                {
                    if (!_nodeUpdateStatus.ContainsKey(nodeKey))
                    {
                        _nodeUpdateStatus[nodeKey] = new HashSet<string>();
                    }
                    _nodeUpdateStatus[nodeKey].Add(e.RecordName);
                }

                var allBindings = _displayController.GetAllBindings().ToList();

                var matchedBindings = allBindings
                    .Where(b => b.TaskName == e.TaskName &&
                               b.NodeName == e.NodeName &&
                               b.RecordName == e.RecordName)
                    .ToList();

                var sameNodeBindings = allBindings
                    .Where(b => b.TaskName == e.TaskName &&
                               b.NodeName == e.NodeName)
                    .ToList();

                if (matchedBindings.Count == 0)
                {
                    foreach (var binding in sameNodeBindings)
                    {
                        var childControl = _childControls.FirstOrDefault(c => c.Name == binding.ControlId);
                        if (childControl != null && !childControl.IsDisposed)
                        {
                            //childControl.UpdateRefreshStatus(false);
                        }
                    }
                    return;
                }
                else
                {
                    foreach (var binding in matchedBindings)
                    {
                        var childControl = _childControls.FirstOrDefault(c => c.Name == binding.ControlId);
                        if (childControl == null || childControl.IsDisposed)
                        {
                            continue;
                        }

                        try
                        {
                            childControl.RefreshDisplaySafe(e.Record);
                            childControl.UpdateRefreshStatus(true);
                        }
                        catch (Exception ex)
                        {
                            childControl.UpdateRefreshStatus(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToProcessRecordUpdateEvent", "Failed to process record update event: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 任务完成事件处理
        /// </summary>
        private void OnTaskCompleted(object sender, EventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => OnTaskCompleted(sender, e)));
                    return;
                }

                // 高亮未绑定控件
                HighlightUnboundControls();

                // 检查并标记未更新的绑定控件
                CheckForUnupdatedBindings();
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToHandleTaskCompletionEvent", "Failed to handle task completion event"), ex);
            }
        }

        /// <summary>
        /// 检查未更新的绑定
        /// </summary>
        private void CheckForUnupdatedBindings()
        {
            try
            {
                var allBindings = _displayController.GetAllBindings().ToList();

                foreach (var binding in allBindings)
                {
                    var nodeKey = Tuple.Create(binding.TaskName, binding.NodeName);

                    // 检查该记录是否在本次更新中出现
                    bool wasUpdated = false;
                    lock (_nodeUpdateStatus)
                    {
                        if (_nodeUpdateStatus.TryGetValue(nodeKey, out var updatedRecords))
                        {
                            wasUpdated = updatedRecords.Contains(binding.RecordName);
                        }
                    }

                    if (!wasUpdated)
                    {
                        var childControl = _childControls.FirstOrDefault(c => c.Name == binding.ControlId);
                        if (childControl != null && !childControl.IsDisposed)
                        {
                            childControl.UpdateRefreshStatus(false);
                        }
                    }
                }

                // 清空更新状态，为下一次任务做准备
                lock (_nodeUpdateStatus)
                {
                    _nodeUpdateStatus.Clear();
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToCheckUnupdatedBindings", "Failed to check unupdated bindings: {0}"), ex.Message), ex);
            }
        }


        /// <summary>
        /// 缺陷图像更新事件处理
        /// </summary>
        private void OnDefectImagesUpdated(object sender, DefectImagesUpdatedEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => OnDefectImagesUpdated(sender, e)));
                    return;
                }

                AddDefectImagesToTableLayoutPanel(e.DefectImages, e.ProductionData);
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_FailedToUpdateDefectImageDisplay", "Failed to update defect image display"), ex);
            }
        }

        /// <summary>
        /// 绑定更新事件处理
        /// </summary>
        private void OnBindingUpdated(object sender, BindingUpdatedEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => OnBindingUpdated(sender, e)));
                    return;
                }

                var childControl = _childControls.FirstOrDefault(c => c.Name == e.ControlId);
                if (childControl != null)
                {
                    if (!string.IsNullOrEmpty(e.TaskName) &&
                        !string.IsNullOrEmpty(e.NodeName) &&
                        !string.IsNullOrEmpty(e.RecordName))
                    {
                        var binding = new BindingConfig(e.TaskName, e.NodeName, e.RecordName, e.ControlId, e.DisplayName);
                        UpdateChildControlBindingUI(childControl, binding);
                    }
                    else
                    {
                        // 解绑
                        childControl.ClearBinding();
                        childControl.SetDefaultDisplay();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToHandleBindingUpdateEvent", "Failed to handle binding update event: {0}"), ex.Message), ex);
            }
        }


        #endregion

        #region 辅助方法UI

        /// <summary>
        /// 高亮所有未绑定的控件
        /// </summary>
        public void HighlightUnboundControls(int durationMs = 0)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<int>(HighlightUnboundControls), durationMs);
                    return;
                }

                foreach (var childControl in _childControls)
                {
                    if (childControl != null && !childControl.IsBound())
                    {
                        childControl.Highlight(durationMs);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToHighlightUnboundControls", "Failed to highlight unbound controls: {0}"), ex.Message), ex);
            }
        }

        /// <summary>
        /// 调整其他控件的位置
        /// </summary>
        private void AdjustOtherControlsPosition()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(AdjustOtherControlsPosition));
                    return;
                }
                if (tableLayoutPanel1 != null)
                    tableLayoutPanel1.Dock = DockStyle.Fill;
                if (tableLayoutPanel3 != null)
                    tableLayoutPanel3.Dock = DockStyle.Fill;
                if (tableLayoutPanel3 != null)
                    tableLayoutPanel3.SendToBack();
                if (tableLayoutPanel1 != null)
                    tableLayoutPanel1.SendToBack();

                if (tableLayoutPanel3 != null && tableLayoutPanel3.Controls.Count > 0)
                {
                    int rowHeight = 120;
                    int rows = tableLayoutPanel3.RowCount;
                    int totalHeight = rows * rowHeight + 20;
                    tableLayoutPanel3.Height = totalHeight > 360 ? 360 : totalHeight;
                    tableLayoutPanel3.AutoScroll = true;
                }
                else if (tableLayoutPanel3 != null)
                {
                    tableLayoutPanel3.Height = 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format(Localizer.GetString("Message_FailedToAdjustControlPositions", "Failed to adjust control positions: {0}"), ex.Message), ex);
            }
        }

        private void Segmented1_ItemClick(object sender, SegmentedItemEventArgs e)
        {
            // 使用本地化文本进行比较
            string customLayoutText = Localizer.GetString("ProductDisplay_CustomLayout", "Custom Layout");
            if (e.Item.Text == customLayoutText)
            {
                var layoutWindow = new VDEApp.UCProductDisplaystructure();
                layoutWindow.LayoutSelected += (s, args) =>
                {
                    _displayController.CreateLayout(args.Item1, args.Item2);
                    GenerateLayoutUI(args.Item1, args.Item2);
                };
                layoutWindow.ShowDialog(this);
            }
        }

        private void Segmented1_SelectIndexChanged(object sender, IntEventArgs e)
        {
            switch (e.Value)
            {
                case 0: // segmentedItem1（1X1）
                    _displayController.CreateLayout(1, 1);
                    GenerateLayoutUI(1, 1);
                    break;
                case 1: // segmentedItem2（2X2）
                    _displayController.CreateLayout(2, 2);
                    GenerateLayoutUI(2, 2);
                    break;
            }
        }

        /// <summary>
        /// 根据任务名称和节点名称获取记录名称列表（UI层调用）
        /// </summary>
        public List<string> GetRecordNamesByTaskAndNode(string taskName, string nodeName)
        {
            return _displayController.GetRecordNames(taskName, nodeName);
        }

        #endregion
    }
}
