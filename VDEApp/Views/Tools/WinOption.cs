using AntdUI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VDEApp.Configs;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.Models.Project;

namespace VDEApp.Views.Tools
{
    public partial class WinOption : AntdUI.Window, ILocalizableForm
    {
        private static readonly string[] ResultOptions = { "ALL", "OK", "NG" };
        private static readonly string[] FormatOptions = { "jpg", "png", "bmp", "tiff" };

        public WinOption()
        {
            InitializeComponent();
            WireEvents();
            InitUI();
            RefreshLanguage();
        }

        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }

        // 初始化 & 事件绑定
        private void WireEvents()
        {
            // 开关
            switch1.CheckedChanged += (_, __) => { UpdatePanelEnabled(panel1, switch1, label1, select1); MarkDirty(); };
            switch2.CheckedChanged += (_, __) => { UpdatePanelEnabled(panel2, switch2, label8, select4); MarkDirty(); };

            // 结果/格式选择变化
            select1.SelectedIndexChanged += (_, __) => MarkDirty();
            select2.SelectedIndexChanged += (_, __) => MarkDirty();
            select3.SelectedIndexChanged += (_, __) => MarkDirty();
            select4.SelectedIndexChanged += (_, __) => MarkDirty();

            // 目录选择按钮（原图/截图）
            YTSelectFloder.Click += (_, __) => PickDirInto(input1);
            JTSelectFloder.Click += (_, __) => PickDirInto(input2);

            // 打开目录按钮（原图/截图）
            YTOpenFloder.Click += (_, __) => OpenDir(GetEffectiveFolder(true));
            JTOpenFloder.Click += (_, __) => OpenDir(GetEffectiveFolder(false));

            // 保存 / 恢复默认并保存
            button1.Click += (_, __) => { if (SaveToProject()) Info(GlobalConfig.Localizer.GetString("WInOption_WireEvents_savetoproj", "已保存到项目")); else { Error(GlobalConfig.Localizer.GetString("WInOption_WireEvents_savefail", "保存失败")); } };
            button2.Click += (_, __) => { ResetDefaults(); if (SaveToProject()) Info(GlobalConfig.Localizer.GetString("WInOption_restored_save", "已恢复默认并保存")); else Error(GlobalConfig.Localizer.GetString("WInOption_restored_savefail", "恢复默认失败")); };
        }

        private void InitUI()
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            var cfg = proj?.ImageSave ?? new ImageSaveOptions();

            // 结果下拉
            SetupSelect(select1, ResultOptions, SafeValue(cfg.RawResult, "ALL"));
            SetReadOnly(select1, true);
            SetupSelect(select4, ResultOptions, SafeValue(cfg.ShotResult, "ALL"));

            // 格式下拉
            SetupSelect(select2, FormatOptions, SafeValue(cfg.RawFormat, "png"));
            SetupSelect(select3, FormatOptions, SafeValue(cfg.ShotFormat, "jpg"));

            // 目录（显示根目录；默认 <projDir>/Images）
            var projDir = GetProjectDir();
            input1.Text = string.IsNullOrWhiteSpace(cfg.RawDir) ? Path.Combine(projDir, "Images") : cfg.RawDir;
            input2.Text = string.IsNullOrWhiteSpace(cfg.ShotDir) ? Path.Combine(projDir, "Images") : cfg.ShotDir;
            SetReadOnly(input1, true);
            SetReadOnly(input2, true);

            // 开关
            switch1.Checked = cfg.RawEnabled;
            switch2.Checked = cfg.ShotEnabled;

            // 面板启用联动
            UpdatePanelEnabled(panel1, switch1, label1, select1);
            UpdatePanelEnabled(panel2, switch2, label8, select4);
        }

        // 保存 / 恢复默认
        private bool SaveToProject()
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            if (proj == null)
                return false;

            // 采集 UI 值
            proj.ImageSave.RawEnabled = switch1.Checked;
            proj.ImageSave.RawResult = GetSelectText(select1);
            proj.ImageSave.RawFormat = GetSelectText(select2);
            proj.ImageSave.RawDir = input1.Text;

            proj.ImageSave.ShotEnabled = switch2.Checked;
            proj.ImageSave.ShotResult = GetSelectText(select4);
            proj.ImageSave.ShotFormat = GetSelectText(select3);
            proj.ImageSave.ShotDir = input2.Text;

            // 准备目录（根）
            TryCreateDir(proj.ImageSave.RawDir);
            TryCreateDir(proj.ImageSave.ShotDir);

            // 标记改动并保存项目
            proj.IsModified = true;
            try
            {
                ServiceLocator.ProjectController.SaveCurrentProject();
                //保存后创建所有任务文件夹
                try
                {
                    if (proj.TaskGroup != null && proj.TaskGroup.Count > 0)
                    {
                        var projectDir = GetProjectDir();
                        var imageSaveOptions = proj.ImageSave;

                        foreach (var task in proj.TaskGroup)
                        {
                            if (task == null || string.IsNullOrWhiteSpace(task.Name))
                                continue;

                            // 解析并创建原图目录
                            string rawDirToCreate = imageSaveOptions.ResolveRawDir(projectDir, task.Name);
                            Directory.CreateDirectory(rawDirToCreate);

                            // 解析并创建截图目录
                            string shotDirToCreate = imageSaveOptions.ResolveShotDir(projectDir, task.Name);
                            Directory.CreateDirectory(shotDirToCreate);
                        }

                        // (可选) 可以添加一个日志记录
                        // Log.Info("已为所有任务预创建存图目录。");
                    }
                }
                catch (Exception ex)
                {
                    var fmt = GlobalConfig.Localizer.GetString("Project_Save_PartialImageDirCreateFailed", "设置已保存，但预创建部分存图目录失败：{0}");
                    Error(string.Format(fmt, ex.Message));
                }
                return true;
            }
            catch (Exception ex)
            {
                var fmt = GlobalConfig.Localizer.GetString("Project_Save_Failed", "保存项目失败：{0}");
                Error(string.Format(fmt, ex.Message));
                return false;
            }
        }

        private void ResetDefaults()
        {
            var projDir = GetProjectDir();

            // 恢复开关默认：都启用
            switch1.Checked = true;
            switch2.Checked = true;

            // 结果默认
            SetupSelect(select1, ResultOptions, "ALL");
            SetupSelect(select4, ResultOptions, "ALL");

            // 格式默认
            SetupSelect(select2, FormatOptions, "bmp");
            SetupSelect(select3, FormatOptions, "jpg");

            // 目录默认（统一为 <projDir>/Images）
            input1.Text = Path.Combine(projDir, "Images");
            input2.Text = Path.Combine(projDir, "Images");

            UpdatePanelEnabled(panel1, switch1, label1, select1);
            UpdatePanelEnabled(panel2, switch2, label8, select4);

            MarkDirty();
        }

        private static void SetupSelect(AntdUI.Select sel, string[] items, string value)
        {
            sel.Items.Clear();
            sel.Items.AddRange(items);
            int idx = Array.FindIndex(items, x => string.Equals(x, value, StringComparison.OrdinalIgnoreCase));
            sel.SelectedIndex = (idx >= 0 ? idx : 0);
        }

        private static string GetSelectText(AntdUI.Select sel)
        {
            if (sel.SelectedIndex >= 0 && sel.SelectedIndex < sel.Items.Count)
                return sel.Items[sel.SelectedIndex]?.ToString() ?? string.Empty;
            return sel.Text ?? string.Empty;
        }

        private static string SafeValue(string v, string fallback) =>
            string.IsNullOrWhiteSpace(v) ? fallback : v;

        private static void SetReadOnly(AntdUI.Input input, bool ro)
        {
            try
            { input.ReadOnly = ro; }
            catch { input.Enabled = !ro; }
        }

        private static void TryCreateDir(string path)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(path))
                    Directory.CreateDirectory(path);
            }
            catch { /* 忽略创建失败，保存流程不中断 */ }
        }

        private string GetProjectDir()
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            if (proj != null && !string.IsNullOrWhiteSpace(proj.Path))
            {
                var dir = Path.GetDirectoryName(proj.Path);
                if (!string.IsNullOrWhiteSpace(dir))
                    return dir;
            }
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private static void UpdatePanelEnabled(AntdUI.Panel panel, AntdUI.Switch toggle, AntdUI.Label title, AntdUI.Select resultSelect)
        {
            bool enabled = toggle?.Checked ?? true;
            if (panel != null)
            {
                foreach (Control c in panel.Controls)
                {
                    if (ReferenceEquals(c, toggle) || ReferenceEquals(c, title))
                        continue;
                    c.Enabled = enabled;
                }
            }
            if (title != null)
            {
                try
                { title.ForeColor = enabled ? SystemColors.ControlText : Color.Gray; }
                catch { }
            }
            if (resultSelect != null)
            {
                try
                {
                    resultSelect.Enabled = enabled;
                    resultSelect.DropDownArrow = true;
                }
                catch { }
            }
        }

        private static void MarkDirty()
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            if (proj != null)
                proj.IsModified = true;
        }

        private static void Info(string msg) { try { MessageBox.Show(msg, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); } catch { } }
        private static void Error(string msg) { try { MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { } }

        // 目录选择 / 打开

        private void PickDirInto(AntdUI.Input target)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                dlg.Description = GlobalConfig.Localizer.GetString("WinOption_PickDirInto_select", "选择存图根目录（程序会在其下自动创建：任务名/原图 或 任务名/截图）");
                dlg.ShowNewFolderButton = true;
                dlg.SelectedPath = Directory.Exists(target.Text) ? target.Text : GetProjectDir();

                if (dlg.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.SelectedPath))
                {
                    target.Text = dlg.SelectedPath;
                    TryCreateDir(target.Text);
                    MarkDirty();
                }
            }
        }

        private string GetEffectiveFolder(bool isRaw)
        {
            // 根目录
            string root = isRaw ? input1.Text : input2.Text;
            if (string.IsNullOrWhiteSpace(root) || !Path.IsPathRooted(root))
                root = Path.Combine(GetProjectDir(), "Images");

            // 任务名
            var proj = GlobalConfig.Instance?.CurrentProject;
            var taskName = proj?.CurrentTask?.Name;
            if (string.IsNullOrWhiteSpace(taskName))
                taskName = "DefaultTask";

            // 统一规则：<根>/任务名/原图(截图)
            var sub = isRaw ? "Raw" : "SnapShot";
            return Path.Combine(root, taskName, sub);
        }

        private void OpenDir(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                Process.Start(new ProcessStartInfo()
                {
                    FileName = path,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            catch (Exception ex)
            {
                var fmt = GlobalConfig.Localizer.GetString("OpenDir_Failed", "无法打开目录：{0}");
                Error(string.Format(fmt, ex.Message));
            }
        }
    }
}
