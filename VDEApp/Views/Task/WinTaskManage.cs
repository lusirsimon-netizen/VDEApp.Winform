using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Controllers.Display;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Models.TaskNodes;
using VDEApp.Views.Main;
using static VDEApp.Program;

namespace VDEApp.Views.Task
{
    public partial class WinTaskManage : AntdUI.Window, ILocalizableForm
    {
        private readonly BindingList<TaskRow> _rows = new BindingList<TaskRow>();

        public WinTaskManage()
        {
            InitializeComponent();
            InitTable();
            RefreshLanguage();

            dgvTasks.Binding(_rows);

            dgvTasks.CellBeginEdit += dgvTasks_CellBeginEdit;
            dgvTasks.CellButtonClick += dgvTasks_CellButtonClick;
            dgvTasks.CellEndEdit += dgvTasks_CellEndEdit;

            // 订阅统一刷新事件
            try
            {
                ServiceLocator.TaskController.TasksChanged -= OnTasksChangedFromController;
                ServiceLocator.TaskController.TasksChanged += OnTasksChangedFromController;
            }
            catch { }
        }

        private void Task_Load(object sender, EventArgs e)
        {
            // 进入窗口时核对一次
            ReconcileTasksWithDiskAndRefresh(bringOrphans: false);
            LoadFromController();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            try
            { ServiceLocator.TaskController.TasksChanged -= OnTasksChangedFromController; }
            catch { }
        }

        private void OnTasksChangedFromController()
        {
            if (IsDisposed)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnTasksChangedFromController));
                return;
            }
            LoadFromController();
        }

        /// <summary>
        /// 初始化表格列与样式
        /// </summary>
        private void InitTable()
        {
            dgvTasks.Columns = new ColumnCollection
            {
                new Column("Index",GlobalConfig.Localizer.GetString("ID", "序号")).SetFixed().SetWidth("60").SetAlign(ColumnAlign.Center),
                new Column("Name", GlobalConfig.Localizer.GetString("TaskName", "任务名")),
                new Column("Created",GlobalConfig.Localizer.GetString("CreateTime", "创建日期")).SetFixed().SetWidth("180").SetAlign(ColumnAlign.Center),
                new Column("Status",GlobalConfig.Localizer.GetString("Status", "状态")).SetFixed().SetWidth("80").SetAlign(ColumnAlign.Center),
                new Column("Ops", GlobalConfig.Localizer.GetString("Option", "操作")).SetFixed().SetWidth("160").SetAlign(ColumnAlign.Center),
            };

            dgvTasks.VisibleHeader = true;
            dgvTasks.FixedHeader = true;
            dgvTasks.EmptyHeader = true;

            dgvTasks.Bordered = true;
            dgvTasks.AutoSizeColumnsMode = ColumnsMode.Fill;
            dgvTasks.EditMode = TEditMode.DoubleClick;
            dgvTasks.EditInputStyle = TEditInputStyle.Default;
        }

        private void LoadFromController()
        {
            _rows.Clear();
            var proj = GlobalConfig.Instance.CurrentProject;
            if (proj?.TaskGroup == null || proj.TaskGroup.Count == 0)
            {
                return;
            }

            for (int i = 0; i < proj.TaskGroup.Count; i++)
            {
                var model = proj.TaskGroup[i];

                string createdText = "-";
                try
                {
                    DateTime created = DateTime.MinValue;
                    if (model != null && model.CreatedAt != default)
                    {
                        created = model.CreatedAt;
                    }
                    else
                    {
                        var path = GetTaskFolderPath(model);
                        if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                            created = Directory.GetCreationTime(path);
                    }
                    if (created != DateTime.MinValue)
                        createdText = created.ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch { }

                _rows.Add(new TaskRow
                {
                    Index = i + 1,
                    Name = model.Name,
                    Created = createdText,
                    Status = IsCurrentTask(model) ? GlobalConfig.Localizer.GetString("Current", "当前任务") : "",
                    Model = model,
                    Ops = CreateOpsButtons()
                });
            }

            RefreshStatusColumn();
            RebuildIndexes();
        }

        private static string GetTaskFolderPath(TaskModel m)
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            if (proj == null || m == null)
                return string.Empty;
            var projectDir = Path.GetDirectoryName(proj.Path) ?? "";
            return Path.Combine(projectDir, "Tasks", m.Name ?? "");
        }

        private static CellLink[] CreateOpsButtons() => new CellLink[]
        {
            new CellButton("copy",TTypeMini.Default).SetIcon("CopyOutlined"),
            new CellButton("delete",TTypeMini.Error  ).SetIcon("DeleteOutlined"),
        };

        private string MakeUniqueName(string baseName = "newTask")
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            if (proj == null)
                return baseName;
            if (string.IsNullOrWhiteSpace(baseName))
                baseName = "newTask";

            // 现有任务名
            var taken = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (proj.TaskGroup != null)
            {
                foreach (var t in proj.TaskGroup)
                {
                    if (!string.IsNullOrWhiteSpace(t?.Name))
                        taken.Add(t.Name.Trim());
                }
            }

            var projectDir = Path.GetDirectoryName(proj.Path) ?? "";
            var tasksRoot = Path.Combine(projectDir, "Tasks");

            int i = 0;
            while (true)
            {
                var name = i == 0 ? baseName : $"{baseName}{i}";
                bool inList = taken.Contains(name);
                bool onDisk = Directory.Exists(Path.Combine(tasksRoot, name));
                if (!inList && !onDisk)
                    return name;
                i++;
            }
        }

        private int GetNameColumnIndex()
        {
            for (int i = 0; i < dgvTasks.Columns.Count; i++)
                if (string.Equals(dgvTasks.Columns[i].Key, "Name", StringComparison.OrdinalIgnoreCase))
                    return i;
            return 1;
        }

        private bool IsNameConflict(string name, string excludeName = null)
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            if (proj == null || string.IsNullOrWhiteSpace(name))
                return false;

            // 列表里是否有重名（忽略大小写）
            var taskGroup = proj.TaskGroup ?? new List<TaskModel>();
            foreach (var t in taskGroup)
            {
                var n = t?.Name?.Trim();
                if (string.IsNullOrEmpty(n))
                    continue;
                if (string.Equals(n, excludeName, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (string.Equals(n, name, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            // 磁盘 Tasks\{name} 是否已存在
            var projectDir = Path.GetDirectoryName(proj.Path) ?? "";
            var tasksRoot = Path.Combine(projectDir, "Tasks");
            var maybeDir = Path.Combine(tasksRoot, name);
            if (Directory.Exists(maybeDir) &&
                !string.Equals(name, excludeName, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        /// <summary>
        /// 新建任务
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            if (proj == null)
                return;
            if (proj.TaskGroup == null)
                proj.TaskGroup = new List<TaskModel>();
            try
            {
                // 自动生成唯一名
                var taskName = MakeUniqueName("newTask");
                ServiceLocator.TaskController.AddTask(taskName);
                SaveProjectSilently("添加", taskName);
                //ResubscribeDisplayControllerEvents();
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, string.Format(GlobalConfig.Localizer.GetString("TaskManage_AddTaskFailedFmt", "添加新任务失败：{0}"), ex.Message
        )
    );
            }
        }

        /// <summary>
        /// 仅允许编辑“任务名”列
        /// </summary>
        private bool dgvTasks_CellBeginEdit(object sender, TableEventArgs e)
        {
            if (e.Column == null)
                return true;
            return e.Column.Key == "Name";
        }

        /// <summary>
        /// 进入任务管理窗口时，核对一次
        /// </summary>
        private void ReconcileTasksWithDiskAndRefresh(bool bringOrphans = false)
        {
            try
            {
                int changed = ServiceLocator.ProjectController.RepairTaskListAgainstDisk(
                    removeMissing: true,
                    bringOrphans: bringOrphans
                );

                if (changed > 0)
                {
                    ServiceLocator.ProjectController.SaveCurrentProject();
                    LoadFromController();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Reconcile tasks failed: " + ex.Message);
            }
        }

        /// <summary>
        /// 重命名校验
        /// </summary>
        private bool dgvTasks_CellEndEdit(object sender, TableEndEditEventArgs e)
        {
            if (e.ColumnIndex != GetNameColumnIndex())
                return true;

            var row = e.Record as TaskRow;
            if (row == null)
                return false;

            var oldName = row.Name?.Trim();
            var newName = (e.Value ?? "").Trim();

            if (string.IsNullOrWhiteSpace(newName))
            {
                AntdUI.Message.error(this, GlobalConfig.Localizer.GetString("TaskManage_NameEmpty", "任务名不能为空"));
                return false;
            }
            if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                AntdUI.Message.error(this, GlobalConfig.Localizer.GetString("TaskManage_NameInvalidChars", "任务名包含非法字符"));
                return false;
            }
            if (string.Equals(newName, oldName, StringComparison.OrdinalIgnoreCase))
                return true;

            if (IsNameConflict(newName, excludeName: oldName))
            {
                AntdUI.Message.error(this, GlobalConfig.Localizer.GetString("TaskManage_NameDuplicate", "任务名不能重名"));
                return false;
            }
            try
            {
                ServiceLocator.TaskController.ReNameTask(row.Model, newName);

                // 仅保存
                SaveProjectSilently(GlobalConfig.Localizer.GetString("TaskManage_Action_Rename", "重命名"), newName);
                return true;
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this,string.Format(GlobalConfig.Localizer.GetString("TaskManage_RenameFailedFmt", "重命名失败：{0}"), ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 操作列：复制 / 删除
        /// </summary>
        private async void dgvTasks_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            var row = e.Record as TaskRow;
            if (row == null)
                return;

            int idx = _rows.IndexOf(row);
            if (idx < 0)
                return;

            switch (e.Btn.Id)
            {
                case "copy":
                {
                    this.Cursor = Cursors.WaitCursor;
                    dgvTasks.Enabled = false;
                    try
                    {
                        var targetModel = row.Model;
                        TaskModel newTask = null;
                        await System.Threading.Tasks.Task.Run(() =>
                        {
                            newTask = ServiceLocator.TaskController.CopyTask(targetModel);
                        });
                        //var newTask = ServiceLocator.TaskController.CopyTask(row.Model);
                        if(newTask != null)
                        {
                            // 复制后不设为当前；仅保存，刷新由 TasksChanged 触发
                            SaveProjectSilently(GlobalConfig.Localizer.GetString("TaskManage_copyTask", "复制"), newTask.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        AntdUI.Message.error(this, GlobalConfig.Localizer.GetString("TaskManage_copyTaskfail", "复制失败：") + ex.Message);
                    }finally
                    {
                        if (!IsDisposed)
                        {
                            this.Cursor = Cursors.Default;
                            dgvTasks.Enabled = true;
                        }
                    }
                    break;
                }
                case "delete":
                {
                    // 判断是否当前任务
                    bool isCurrent = IsCurrentTask(row.Model) || string.Equals(row.Status, GlobalConfig.Localizer.GetString("Current", "当前任务"));

                    string title = isCurrent ? GlobalConfig.Localizer.GetString("TaskManage_DeleteCurrentConfirmTitle", "确定删除 当前任务？") : GlobalConfig.Localizer.GetString("TaskManage_DeleteConfirmTitle", "确认删除");
                    string message = isCurrent ? string.Format(GlobalConfig.Localizer.GetString("TaskManage_DeleteCurrentConfirmMessageFmt", "{0}\n\n注意：这是当前正在执行的任务。"), row.Name) : string.Format(
                    GlobalConfig.Localizer.GetString("TaskManage_DeleteConfirmMessageFmt", "确定要删除任务“{0}”吗？"), row.Name);

                    DialogResult dr = MessageBox.Show(
                        this,
                        message,
                        title,
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning
                    );


                    if (dr == DialogResult.OK)
                    {
                        if (row.Model != null)
                        {
                            ServiceLocator.TaskController.DeleteTask(row.Model);
                        }

                        // 不再由 UI 指定“下一个任务”，控制器内部已处理
                        SaveProjectSilently("删除", row.Name);
                    }
                    break;
                }
            }
        }


        /// <summary>
        /// 重排“序号”
        /// </summary>
        private void RebuildIndexes()
        {
            for (int i = 0; i < _rows.Count; i++)
                _rows[i].Index = i + 1;
        }

        /// <summary>
        /// 规范状态列：最多一个“当前”
        /// </summary>
        private void RefreshStatusColumn()
        {
            var proj = GlobalConfig.Instance?.CurrentProject;
            foreach (var r in _rows)
            {
                r.Status = (proj != null && IsCurrentTask(r.Model)) ? GlobalConfig.Localizer.GetString("Current", "当前任务") : "";
            }
        }

        /// <summary>
        /// 静默保存
        /// </summary>
        private void SaveProjectSilently(string action, string taskName)
        {
            try
            {
                ServiceLocator.ProjectController.SaveCurrentProject();
                var proj = GlobalConfig.Instance.CurrentProject;
                if (proj != null)
                    proj.IsModified = false;

                string tn = string.IsNullOrWhiteSpace(taskName) ? "-" : taskName;
                Log.Info(string.Format(GlobalConfig.Localizer.GetString("TaskManage_AutoSaveSuccessFmt", "【{0}】任务：{1}，自动保存成功。"), action, tn));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Save project failed: " + ex.Message);
            }
        }

        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }

        private static bool IsCurrentTask(TaskModel m)
        {
            var proj = GlobalConfig.Instance?.CurrentProject;
            if (proj?.CurrentTask == null || m == null)
                return false;

            return proj.CurrentTask.Guid == m.Guid;

            //var cur = proj.CurrentTask;
            //if (!string.IsNullOrEmpty(cur.Guid) && !string.IsNullOrEmpty(m.Guid))
            //    return string.Equals(cur.Guid, m.Guid, StringComparison.OrdinalIgnoreCase);

            //return ReferenceEquals(cur, m)
            //    || string.Equals(cur.Name, m.Name, StringComparison.OrdinalIgnoreCase);
        }

        private void panel2_Click(object sender, EventArgs e) { }
        private void taskDivider_Click(object sender, EventArgs e) { }
    }

    public class TaskRow : AntdUI.NotifyProperty
    {
        private int _index;
        public int Index
        {
            get => _index;
            set { if (_index == value) return; _index = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged();
                if (Model != null)
                    Model.Name = _name;
            }
        }

        private string _created;
        public string Created
        {
            get => _created;
            set { if (_created == value) return; _created = value; OnPropertyChanged(); }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set { if (_status == value) return; _status = value; OnPropertyChanged(); }
        }

        private CellLink[] _ops;
        public CellLink[] Ops
        {
            get => _ops;
            set { _ops = value; OnPropertyChanged(); }
        }

        public TaskModel Model { get; set; }
    }
}
