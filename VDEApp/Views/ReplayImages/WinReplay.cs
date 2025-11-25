using AntdUI;
using Insnex.Utility;
using Insnex.Vision2D.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Devices.Cameras;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Models.Product;
using VDEApp.Models.Project;
using VDEApp.Models.TaskNodes;

namespace VDEApp.Views.ReplayImages
{
    public partial class WinReplay : AntdUI.Window, ILocalizableForm
    {
        #region 字段与变量
        string imagesFolderPath = "";
        ImageFileTool imageFileTool;
        string message = "";
        private string lastSelectedPath = null; // 用户上一次选择的路径
        private Dictionary<string, List<string>> groupedImagePaths = new Dictionary<string, List<string>>();
        private List<string> imageFiles = new List<string>();
        private Dictionary<string, string> _guidMap = new Dictionary<string, string>();
        private int selectedRowIndex = 1;//运行组
        private int RunCount = 0; //运行次数
        private bool isRunning = false; //是否正在运行
        private CancellationTokenSource continueRunCTS;
        private ProjectModel currentProject = GlobalConfig.Instance.CurrentProject;
        private static Dictionary<string, string> LastSelectedPathMap= new Dictionary<string, string>();
        #endregion

        #region 构造与初始化
        public WinReplay()
        {
            InitializeComponent();
            RefreshLanguage();
            InitUI();
            LoadImagePathsStart();
        }

        /// <summary>
        /// 初始化界面和事件
        /// </summary>
        private void InitUI()
        {
            this.lblNowTask.Text = GlobalConfig.Instance.CurrentProject.CurrentTask.Name;
            RunCount = this.InputRunCount.Text.Count();
            table1.Columns = new ColumnCollection
            {
                new Column("Id", GlobalConfig.Localizer.GetString("Replay_Table_Column_Id", "ID")).SetFixed().SetWidth("50").SetAlign(ColumnAlign.Center),
                new Column("Guid", GlobalConfig.Localizer.GetString("Replay_Table_Column_Guid", "GUID")),
                new Column("SaveTime", GlobalConfig.Localizer.GetString("Replay_Table_Column_SaveTime", "保存时间")).SetFixed().SetWidth("180").SetAlign(ColumnAlign.Center),
                new Column("Quantity", GlobalConfig.Localizer.GetString("Replay_Table_Column_Quantity", "数量")).SetFixed().SetWidth("50").SetAlign(ColumnAlign.Center),
                new Column("StdStatus", GlobalConfig.Localizer.GetString("Replay_Table_Column_StdStatus", "预期状态")).SetFixed().SetWidth("80").SetAlign(ColumnAlign.Center),
                new Column("Status", GlobalConfig.Localizer.GetString("Replay_Table_Column_Status", "状态")).SetFixed().SetWidth("80").SetAlign(ColumnAlign.Center),
            };
            table1.VisibleHeader = true;
            table1.FixedHeader = true;
            table1.EmptyHeader = true;
            table1.Bordered = true;
            table1.AutoSizeColumnsMode = ColumnsMode.Fill;
            table1.EditMode = TEditMode.None;
            table1.EditInputStyle = TEditInputStyle.Default;
            btnSelectFloder.Click += BtnSelectFolder_Click;
            table1.CellClick += Table1_CellClick;
            currentProject.CurrentTask.TaskRunAfterEvent -= CurrentTask_TaskRunAfterEvent;
            currentProject.CurrentTask.TaskRunAfterEvent += CurrentTask_TaskRunAfterEvent;

            this.FormClosed += WinReplay_FormClosed;
        }

        /// <summary>
        /// 窗体关闭事件，清理资源
        /// </summary>
        private void WinReplay_FormClosed(object sender, FormClosedEventArgs e)
        {
            currentProject.CurrentTask.TaskRunAfterEvent -= CurrentTask_TaskRunAfterEvent;
            groupedImagePaths.Clear();
            imageFiles.Clear();
            _guidMap.Clear();
        }
        #endregion

        #region 文件夹选择与加载
        /// <summary>
        /// 选择图片文件夹
        /// </summary>
        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.Description = GlobalConfig.Localizer.GetString("Replay_Dialog_SelectFolderDesc", "请选择图片文件夹");
                dialog.SelectedPath = Directory.Exists(InputImageFloder.Text) ? InputImageFloder.Text : AppPathRouter.CurrentProjectPath;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    InputImageFloder.Text = dialog.SelectedPath;
                    imagesFolderPath = dialog.SelectedPath;
                    lastSelectedPath = dialog.SelectedPath;
                    var task = currentProject?.CurrentTask;
                    if (task!= null)
                    {
                        string taskName = task.Name;
                        LastSelectedPathMap[taskName] = dialog.SelectedPath;
                    }
                    ReloadImagesFromFolder(InputImageFloder.Text);
                    SetFirstGroupAsCurrent();
                }
            }
        }
        /// <summary>
        /// 初始化加载图片路径（默认当前任务的原图文件夹）
        /// </summary>
        private void LoadImagePathsStart()
        {
            try
            {
                var task = currentProject?.CurrentTask;
                if(task==null) return;
                string taskName = task.Name;
                if (LastSelectedPathMap.ContainsKey(taskName) && Directory.Exists(LastSelectedPathMap[taskName]))
                {
                    imagesFolderPath = LastSelectedPathMap[taskName];
                }
                else
                {
                    imagesFolderPath = SaveImageController.GetRawSaveDir(task);
                }
                InputImageFloder.Text = imagesFolderPath;
                lastSelectedPath = imagesFolderPath;

                ReloadImagesFromFolder(imagesFolderPath);
                SetFirstGroupAsCurrent();
            }
            catch (Exception ex)
            {
                message = GlobalConfig.Localizer.GetString("Replay_Log_LoadPathFailed", "加载图片路径失败");
                Log.Error(message, ex);
            }
        }
        #endregion

        #region 图片加载与分组
        /// <summary>
        /// 通用图片加载逻辑
        /// </summary>
        private bool ReloadImagesFromFolder(string folderPath)
        {
            try
            {
                string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tif", ".tiff" };
                imageFiles = Directory.GetFiles(folderPath)
                    .Where(f => imageExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                    .OrderBy(f => f)
                    .ToList();

                if (imageFiles.Count == 0)
                {
                    message = GlobalConfig.Localizer.GetString("Replay_Msg_NoImagesFound", "该文件夹中未找到图片文件");
                    MessageBox.Show(message, GlobalConfig.Localizer.GetString("Common_Tip", "提示"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    table1.DataSource = null;
                    return false;
                }
                if (currentProject == null || currentProject.CurrentTask == null)
                {
                    message = GlobalConfig.Localizer.GetString("Replay_Msg_SelectTaskFirst", "请先选择任务");
                    MessageBox.Show(message, GlobalConfig.Localizer.GetString("Common_Tip", "提示"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                TaskModel task = currentProject.CurrentTask;
                int batchSize = ServiceLocator.TaskController.GetAcqCameraBatchSize(task);
                if (batchSize <= 0)
                {
                    message = GlobalConfig.Localizer.GetString("Replay_Log_GetBatchSizeFailed", "批次大小获取失败");
                    Log.Error(message);
                    return false;
                }

                groupedImagePaths = GroupImagePaths(imageFiles, batchSize);

                var tableData = groupedImagePaths
                    .OrderBy(p => p.Key)
                    .Select((p, idx) =>
                    {
                        string firstImagePath = p.Value.First();
                        string saveTime = "";
                        string StdStatus = "";
                        if (!string.IsNullOrEmpty(firstImagePath) && File.Exists(firstImagePath))
                        {
                            string fileName = Path.GetFileNameWithoutExtension(firstImagePath);
                            string[] parts = fileName.Split('_');
                            if (parts.Length >= 2)
                            {
                                if (DateTime.TryParseExact(parts[1], "yyyyMMddHHmmssfff",
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
                                {
                                    saveTime = parsedTime.ToString("yyyy-MM-dd HH:mm:ss.fff"); // 带毫秒的格式化
                                }
                            }
                            if (parts.Length >= 4)
                            {
                                StdStatus = parts[3];
                            }
                        }
                        return new ImageItem
                        {
                            Id = string.Format(GlobalConfig.Localizer.GetString("Replay_Table_GroupId", "{0}"), idx + 1),
                            Guid = p.Key,
                            Quantity = p.Value.Count.ToString(),
                            StdStatus = StdStatus,
                            Status = GlobalConfig.Localizer.GetString("Replay_Status_NotRun", "未运行"),
                            SaveTime = saveTime
                        };
                    }).ToList();

                BindTableData(tableData);
                if (groupedImagePaths.Count == 0 && imageFiles.Count > 0)
                {
                    message = GlobalConfig.Localizer.GetString("Replay_Msg_InvalidImageNaming", "文件夹中的图片命名不符合要求，无法分组！请选择包含以下格式命名图片的文件夹：\n示例：{guid}/{sn}_{yyyyMMddHHmmssfff}_{expTag}_{status}_1.{ext}");
                    MessageBox.Show(message, GlobalConfig.Localizer.GetString("Common_Tip", "提示"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                message = string.Format(GlobalConfig.Localizer.GetString("Replay_Log_ImagesLoaded", "加载完成,共找到 {0} 张图片，分成 {1} 组。"), imageFiles.Count, groupedImagePaths.Count);
                Log.Info(message);
            }
            catch (Exception ex)
            {
                message = GlobalConfig.Localizer.GetString("Replay_Log_LoadPathFailed", "加载图片路径失败");
                Log.Error(message, ex);
            }
            return true;
        }

        /// <summary>
        /// 解析图片路径并分组
        /// </summary>
        private Dictionary<string, List<string>> GroupImagePaths(List<string> files, int batchSize)
        {
            var temp = new Dictionary<string, List<Tuple<int, string>>>();
            foreach (var path in files)
            {
                try
                {
                    string name = Path.GetFileNameWithoutExtension(path);
                    string[] parts = name.Split('_');
                    if (parts.Length < 5)
                        continue;
                    string guid = parts[0];
                    int.TryParse(parts.Last(), out int num);
                    DateTime? saveTime = null;
                    if (DateTime.TryParseExact(parts[1], "yyyyMMddHHmmssfff",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
                    {
                        saveTime = parsedTime;
                    }
                    string Stdstatus = parts.Length>=4? parts[3] : "";

                    if (!temp.ContainsKey(guid))
                        temp[guid] = new List<Tuple<int, string>>();

                    if (temp[guid].Count < batchSize)
                        temp[guid].Add(Tuple.Create(num, path));
                }
                catch (Exception ex)
                {
                    message = string.Format(GlobalConfig.Localizer.GetString("Replay_Log_ParsePathError", "解析图片路径出错:{0}"), path);
                    Log.Error(message, ex);
                }
            }
            var result = new Dictionary<string, List<string>>();
            foreach (var g in temp)
            {
                var sorted = g.Value.OrderBy(t => t.Item1).Select(t => t.Item2).ToList();
                result[g.Key] = sorted;
            }
            return result;
        }
        #endregion

        #region 表格绑定与状态
        /// <summary>
        /// 绑定表格数据
        /// </summary>
        private void BindTableData(List<ImageItem> data)
        {
            table1.DataSource = null;
            table1.DataSource = data;
            if (data.Count > 0)
            {
                selectedRowIndex = Math.Min(selectedRowIndex, data.Count);
                if (selectedRowIndex < 0)
                    selectedRowIndex = 1;
                table1.SelectedIndex = selectedRowIndex; 
            }
            else
            {
                selectedRowIndex = 0;
                table1.SelectedIndex = selectedRowIndex;
            }
            table1.ScrollLine(selectedRowIndex);
            table1.Refresh();
        }
        /// <summary>
        /// 重置当前组之后的状态（保留当前组及之前的状态）
        /// </summary>
        private void ResetSubsequentGroupsStatus(int startGroupIndex)
        {
            if (!(table1.DataSource is List<ImageItem> items))
                return;
            for (int i = 0; i < items.Count; i++)
            {
                if (i >= startGroupIndex)
                {
                    items[i].Status = GlobalConfig.Localizer.GetString("Replay_Status_NotRun", "未运行");
                }
            }
            BindTableData(items);
        }
        /// <summary>
        /// 任务完成事件回调，用于更新表格状态
        /// </summary>
        private void CurrentTask_TaskRunAfterEvent(object sender, ProductionDataEvent e)
        {
            if (GlobalConfig.Instance.CurrentRunType != RunType.Replay)
                return;

            BeginInvoke(new Action(() =>
            {
                var items = table1.DataSource as List<ImageItem>;
                if (items == null)
                    return;

                string groupGuid = _guidMap.ContainsKey(e.Guid) ? _guidMap[e.Guid] : null;
                if (groupGuid == null)
                    return;

                var target = items.FirstOrDefault(x => x.Guid == groupGuid);
                if (target != null)
                {
                    target.Status = e.Status.ToString();
                }
                BindTableData(items.ToList());
            }));
        }
        #endregion

        #region 单步运行逻辑
        /// <summary>
        /// 单步运行按钮点击事件
        /// </summary>
        private async void btnOneRun_Click(object sender, EventArgs e)
        {
            try
            {
                btnOneRun.Enabled = false;
                btnContinueRun.Enabled = false;
                if (currentProject == null || currentProject.CurrentTask == null)
                {
                    message = GlobalConfig.Localizer.GetString("Replay_Msg_SelectTaskFirst", "请先选择任务");
                    MessageBox.Show(message, GlobalConfig.Localizer.GetString("Common_Tip", "提示"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!Directory.Exists(lastSelectedPath))
                {
                    message = GlobalConfig.Localizer.GetString("Replay_Msg_InvalidFolderPath", "请选择或输入一个有效的图片文件夹路径");
                    MessageBox.Show(message, GlobalConfig.Localizer.GetString("Common_Tip", "提示"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var pair = groupedImagePaths.ElementAt(selectedRowIndex-1);
                string groupGuid = pair.Key;
                
                var images = LoadImagesFromPaths(pair.Value);
                var input = images.Select((img, idx) => ($"InputImage_{idx + 1}", img)).ToList();

                GlobalConfig.Instance.CurrentRunType = RunType.Replay;
                //映射guid
                EventHandler<ProductionDataEvent> handler = null;
                handler = (s, e2) =>
                {
                    if (!_guidMap.ContainsKey(e2.Guid))
                        _guidMap[e2.Guid] = groupGuid;
                    currentProject.CurrentTask.TaskRunAfterEvent -= handler;
                };
                currentProject.CurrentTask.TaskRunAfterEvent += handler;
                currentProject.CurrentTask.InputImages = input;
                await ServiceLocator.TaskController.RunAsyncTask(currentProject.CurrentTask);
                images.Clear();

                selectedRowIndex++;
                if (selectedRowIndex >= groupedImagePaths.Count+1)
                {
                    selectedRowIndex = 1;
                }
                // 更新表格选中状态
                table1.SelectedIndex = selectedRowIndex;
            }
            catch (Exception ex)
            {
                message = GlobalConfig.Localizer.GetString("Replay_Log_SingleStepFailed", "单步运行失败");
                Log.Error(message, ex);
            }
            finally
            { 
                btnOneRun.Enabled = true;
                btnContinueRun.Enabled = true;
            }
        }
        #endregion

        #region 持续运行逻辑
        /// <summary>
        /// 持续运行按钮点击事件
        /// </summary>
        private async void btnContinueRun_Click(object sender, EventArgs e)
        {

            if (!isRunning)
            {
                isRunning = true;
                btnContinueRun.Text = GlobalConfig.Localizer.GetString("Replay_Btn_Stop", "停止");
                btnOneRun.Enabled = false;
                continueRunCTS = new CancellationTokenSource();
                var token = continueRunCTS.Token;
                try
                {
                    await RunContinuousAsync(token);
                }
                catch (OperationCanceledException)
                {
                    Log.Info("已停止持续运行");
                }
                catch (Exception ex)
                {
                    message = GlobalConfig.Localizer.GetString("Replay_Log_ContinuousFailed", "持续运行失败");
                    Log.Error(message, ex);
                }
                finally
                {
                    isRunning = false;
                    btnContinueRun.Text = "持续运行";
                    btnOneRun.Enabled = true;
                }
                return;
            }
            else
            {
                continueRunCTS.Cancel();
            }

            
        }
        private async  System.Threading.Tasks.Task RunContinuousAsync(CancellationToken token)
        {
            if (!double.TryParse(txtBoxTime.Text, out double intervalMs) || intervalMs*1000 <= 0)
            {
                message = GlobalConfig.Localizer.GetString("Replay_Msg_InvalidTime", "请输入正确的运行时间");
                MessageBox.Show(message, GlobalConfig.Localizer.GetString("Common_Tip", "提示"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(InputRunCount.Text, out int runCount) || runCount <= 0)
            {
                message = GlobalConfig.Localizer.GetString("Replay_Msg_InvalidRunCount", "请输入正确的运行次数");
                MessageBox.Show(message, GlobalConfig.Localizer.GetString("Common_Tip", "提示"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (currentProject == null || currentProject.CurrentTask == null)
            {
                message = GlobalConfig.Localizer.GetString("Replay_Msg_SelectTaskFirst", "请先选择任务");
                MessageBox.Show(message, GlobalConfig.Localizer.GetString("Common_Tip", "提示"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Directory.Exists(lastSelectedPath))
            {
                message = GlobalConfig.Localizer.GetString("Replay_Msg_InvalidFolderPath", "请选择或输入一个有效的图片文件夹路径");
                MessageBox.Show(message, GlobalConfig.Localizer.GetString("Common_Tip", "提示"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            GlobalConfig.Instance.CurrentRunType = RunType.Replay;
            TaskModel task = currentProject.CurrentTask;
            int groupCount = groupedImagePaths.Count;
            if (groupCount == 0)
                return;
            for (int runIndex = 0; runIndex < runCount; runIndex++)
            {
                ResetSubsequentGroupsStatus(selectedRowIndex);
                int num = 0;
                if (runIndex == 0)
                    num = groupCount - selectedRowIndex+1;
                else
                    num = groupCount;
                for (int i = 0; i < num; i++)
                {

                    token.ThrowIfCancellationRequested();
                    int idx = selectedRowIndex-1;
                    var pair = groupedImagePaths.ElementAt(idx);
                    string groupGuid = pair.Key;
                    var images = LoadImagesFromPaths(pair.Value);
                    var input = images.Select((img, idx) => ($"InputImage_{idx + 1}", img)).ToList();

                    EventHandler<ProductionDataEvent> handler = null;
                    handler = (s, e2) =>
                    {
                        if (!_guidMap.ContainsKey(e2.Guid))
                            _guidMap[e2.Guid] = groupGuid;
                        currentProject.CurrentTask.TaskRunAfterEvent -= handler;
                    };
                    currentProject.CurrentTask.TaskRunAfterEvent += handler;
                    task.InputImages = input;
                    await ServiceLocator.TaskController.RunAsyncTask(task);
                    images.Clear();
                    //运行完更新currentGroupIndex
                    selectedRowIndex++;
                    if (selectedRowIndex >= groupedImagePaths.Count+1)
                    {
                        selectedRowIndex = 1;
                    }
                    table1.SelectedIndex = selectedRowIndex;
                    await System.Threading.Tasks.Task.Delay(Convert.ToInt32(intervalMs * 1000), token);
                }
            }
        }
        #endregion

        #region 工具方法
        /// <summary>
        /// 根据路径加载图片
        /// </summary>
        private List<IInsImage> LoadImagesFromPaths(List<string> filePaths)
        {
            var images = new List<IInsImage>();
            foreach (var path in filePaths)
            {
                try
                {
                    imageFileTool = new ImageFileTool();
                    imageFileTool.Path = path;
                    var img = imageFileTool.ReadNext();
                    if (img != null)
                        images.Add(img);
                }
                catch (Exception ex)
                {
                    message = string.Format(GlobalConfig.Localizer.GetString("Replay_Log_LoadImageFailed", "加载图片失败：{0}"), path);
                    Log.Error(message, ex);
                }
            }
            return images;
        }

        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        private void Table1_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.RowIndex >= 0 && table1.DataSource is List<ImageItem> items && e.RowIndex < items.Count)
            {
                selectedRowIndex = e.RowIndex;
                table1.SelectedIndex = selectedRowIndex;
            }

        }
        private void SetFirstGroupAsCurrent()
        {
            if (table1.DataSource is List<ImageItem> items && items.Count > 0)
            {
                selectedRowIndex = 1;
                table1.SelectedIndex = selectedRowIndex;
            }
            else
            {
                selectedRowIndex = 0;
            }
        }
   
    }
    public class ImageItem
    {
        public string Id { get; set; }
        public string Guid { get; set; }
        public string Quantity { get; set; }
        public string StdStatus { get; set; }
        public string Status { get; set; }
        public string SaveTime { get; set; }
    }
}
