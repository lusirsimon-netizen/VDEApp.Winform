using Insnex.Vision2D;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.Controls;
using Insnex.Vision2D.Core;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VDEApp.Configs;
using VDEApp.Configs.Display;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Models.Product;
using VDEApp.Models.Project;
using VDEApp.Models.TaskNodes;


namespace VDEApp.Controllers
{
    /// <summary>
    /// 单例队列异步存图控制器
    /// 目录：默认 projDir/Images/{任务名}/原图(截图)；若 RawDir/ShotDir 已配置，则用其作为根目录（后续仍拼 {任务名}/原图 或 {任务名}/截图）
    /// 深拷贝
    /// 公共路径方法：GetRawSaveDir/GetShotSaveDir
    /// </summary>
    public sealed class SaveImageController : IDisposable
    {
        internal class SaveImageJob
        {
            public string Guid { get; set; }
            public string Path { get; set; }
            public IInsImage Image { get; set; }
        }
        private BlockingCollection<SaveImageJob> _saveImageJobQueue;
        private Task _saveImageWorker;

        internal class RenameJob
        {
            public string Guid { get; set; }
            public string Path { get; set; }
        }
        private BlockingCollection<RenameJob> _renameJobQueue;
        private Task _renameWorker;
        private ConcurrentDictionary<string, ProductionDataStatus> _runGuidToStatusMap;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        // 批次日志：开始/完成
        private readonly ConcurrentDictionary<string, BatchCounter> _batches = new ConcurrentDictionary<string, BatchCounter>();

        private readonly ConcurrentDictionary<string, ProductionDataEvent> _runResults = new ConcurrentDictionary<string, ProductionDataEvent>();

        public SaveImageController(int capacity = 500)
        {
            _saveImageJobQueue = new BlockingCollection<SaveImageJob>(capacity);
            _saveImageWorker = Task.Factory.StartNew(SaveImageWorkLoop, TaskCreationOptions.LongRunning);

            _renameJobQueue = new BlockingCollection<RenameJob>(capacity);
            _renameWorker = Task.Factory.StartNew(RenameWorkLoop, TaskCreationOptions.LongRunning);

            _runGuidToStatusMap = new ConcurrentDictionary<string, ProductionDataStatus>();
        }

        private void SaveImageWorkLoop()
        {
            foreach (var job in _saveImageJobQueue.GetConsumingEnumerable(_cts.Token))
            {
                try
                {
                    // 写入
                    var parent = Path.GetDirectoryName(job.Path);
                    if (string.IsNullOrEmpty(parent))
                        throw new Exception($"Invalid path: {job.Path}");
                    if (!Directory.Exists(parent))
                        Directory.CreateDirectory(parent);

                    if (!job.Image.Save(job.Path))
                        throw new Exception($"IInsImage returned false");

                    if (!File.Exists(job.Path))
                        throw new Exception($"File not found after save");

                    _renameJobQueue.Add(new RenameJob
                    {
                        Guid = job.Guid,
                        Path = job.Path
                    });

                    // 更新批次计数
                    OnJobCompleted(job.Guid);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Log.Info(string.Format(GlobalConfig.Localizer.GetString("ImageSave_SaveFailedFmt", "[ImageSave] 保存失败：原因={0} | 尝试路径={1}"),ex.Message, job.Path));
                    OnJobCompleted(job.Guid, failed: true);
                }
            }
        }

        private void RenameWorkLoop()
        {
            foreach (var job in _renameJobQueue.GetConsumingEnumerable(_cts.Token))
            {
                try
                {
                    while (!_runGuidToStatusMap.ContainsKey(job.Guid))
                    {
                        Thread.Sleep(0);
                    }
                    var status = _runGuidToStatusMap[job.Guid];
                    if (status == ProductionDataStatus.NA)
                    {
                        continue;
                    }

                    string newPath = job.Path.Replace("_NA_", $"_{status}_");
                    if (job.Path.Equals(newPath) || File.Exists(newPath))
                    {
                        File.Delete(job.Path);
                    }
                    File.Move(job.Path, newPath);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Log.Info(string.Format(GlobalConfig.Localizer.GetString("ImageSave_SaveFailedFmt", "[ImageSave] 保存失败：原因={0} | 尝试路径={1}"),ex.Message, job.Path));
                    OnJobCompleted(job.Guid, failed: true);
                }
            }
        }

        //队列中存图数量，供监控读取当前队列长度
        public int CurrentQueueLength => _saveImageJobQueue?.Count ?? 0;

        // 对外原图保存公共路径查询
        public static string GetRawSaveDir(TaskModel task)
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            var cfg = proj?.ImageSave ?? new ImageSaveOptions();
            var projDir = GetProjectDir();

            var root = string.IsNullOrWhiteSpace(cfg.RawDir)
                ? Path.Combine(projDir, "Images")
                : cfg.RawDir;

            return Path.Combine(root, Sanitize(task?.Name), "Raw");
        }
        // 对外截图保存公共路径查询
        public static string GetShotSaveDir(TaskModel task)
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            var cfg = proj?.ImageSave ?? new ImageSaveOptions();
            var projDir = GetProjectDir();

            var root = string.IsNullOrWhiteSpace(cfg.ShotDir)
                ? Path.Combine(projDir, "Images")
                : cfg.ShotDir;

            return Path.Combine(root, Sanitize(task?.Name), "SnapShot");
        }

        public void EnqueueRecords(TaskModel task, InsRecords records, string runGuid, bool isRawImage)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            int idx = 0;
            string ext = NormalizeExt(
                isRawImage
                ? ServiceLocator.GlobalConfig.CurrentProject.ImageSave.RawFormat
                : ServiceLocator.GlobalConfig.CurrentProject.ImageSave.ShotFormat
            );
            string status = ProductionDataStatus.NA.ToString();

            foreach (var rec in records)
            {
                var img = rec.Content as IInsImage;
                if (img == null)
                    continue;

                if (isRawImage)
                    img = img.Copy();
                else
                {
                    using (var s = new InsSnapshot())
                    {
                        try
                        {
                            s.SetRecord(rec);
                            img = s.SnapshotImageScale();
                            if (img == null)
                                throw new InvalidOperationException(GlobalConfig.Localizer.GetString("Snapshot_GenerateFailed_NullImage","生成截图失败：返回空图像"));
                        }
                        catch
                        {
                            // 完全独立的快照
                            img = InsSnapshotHelper.GenerateSnapshotImageScale(rec);
                            if (img == null)
                                throw;
                        }
                    }
                }

                string dir = isRawImage ? GetRawSaveDir(task) : GetShotSaveDir(task);
                string path = Path.Combine(dir, $"{runGuid}_{timestamp}_{task.AcquireNode.ExposureTime}_{status}_{idx++}.{ext}");
                _saveImageJobQueue.Add(new SaveImageJob
                {
                    Guid = runGuid,
                    Path = path,
                    Image = img,
                });
            }
        }

        public void TaskRunAfterEvent(object sender, ProductionDataEvent e)
        {
            var task = sender as TaskModel;
            _runGuidToStatusMap.TryAdd(e.Guid, e.Status);

            var allowedResults = new List<string>();
            if (ServiceLocator.GlobalConfig.CurrentProject.ImageSave.ShotResult == "ALL")
                allowedResults.AddRange(Enum.GetNames(typeof(ProductionDataStatus)));
            else
                allowedResults.Add(ServiceLocator.GlobalConfig.CurrentProject.ImageSave.ShotResult);

            if (!allowedResults.Any(s => string.Equals(s, e.Status.ToString(), StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }

            try
            {
                // 获取当前项目的存图配置
                var cfg = ServiceLocator.GlobalConfig.CurrentProject.ImageSave;

                // 检查截图功能总开关是否开启
                if (!cfg.ShotEnabled)
                {
                    return;
                }

                // 从配置中获取文件格式
                string shotFormat = cfg.ShotFormat;

                // 从显示控制器获取用户界面上绑定的、需要截图的 Record 名称列表
                var binds = DisplayConfigManager.Instance.CurrentConfig?.BindingConfigs;
                if (binds == null)
                    return;

                var records = new InsRecords();
                var seen = new HashSet<int>();
                foreach (var b in binds)
                {
                    if (b == null ||
                        !string.Equals(b.TaskName, task.Name, StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(b.NodeName, "Inspection", StringComparison.OrdinalIgnoreCase) ||
                        string.IsNullOrWhiteSpace(b.RecordName))
                    {
                        continue;
                    }
                    int idx = e.InspectionNodeRecords.IndexOfKey(b.RecordName);
                    if (seen.Contains(idx) || idx < 0)
                        continue;
                    records.Add(e.InspectionNodeRecords[idx]);
                    seen.Add(idx);
                }
                ServiceLocator.SaveImageController.EnqueueRecords(
                    task,
                    records,
                    runGuid: e.Guid,
                    isRawImage: false
                );
            }
            catch (Exception ex)
            {
                try
                {
                    Log.Error(string.Format(GlobalConfig.Localizer.GetString("ImageSave_EnqueuePrepErrorFmt", "截图入队准备阶段发生错误：{0}"), ex.Message), ex);
                }
                catch { }
            }
        }

        private void OnJobCompleted(string runGuid, bool failed = false)
        {
            if (runGuid == null)
                return;
            if (!_batches.TryGetValue(runGuid, out var b))
                return;

            int left = Interlocked.Decrement(ref b.Remaining);
            if (left <= 0)
            {
                _batches.TryRemove(runGuid, out _);
                if (failed)
                    Log.Info(string.Format(GlobalConfig.Localizer.GetString("ImageSave_BatchCompleteWithFailFmt", "[ImageSave] 批次完成（含失败）：Task={0}, Category={1}, Total={2}, Dir={3}"),b.TaskName, b.Category, b.Total, b.Dir));
                else
                    Log.Info(string.Format(GlobalConfig.Localizer.GetString("ImageSave_BatchCompleteFmt", "[ImageSave] 批次完成：Task={0}, Category={1}, Total={2}, Dir={3}"),b.TaskName, b.Category, b.Total, b.Dir));
            }
        }

        public void Dispose()
        {
            try
            {
                _cts.Cancel();
            }
            catch { }
            finally
            {
                _cts.Dispose();
            }
        }

        private static string GetProjectDir()
        {
            var proj = GlobalConfig.Instance.CurrentProject;
            if (proj != null && !string.IsNullOrWhiteSpace(proj.Path))
            {
                if (Directory.Exists(proj.Path))
                    return proj.Path;
                var dir = Path.GetDirectoryName(proj.Path);
                if (!string.IsNullOrWhiteSpace(dir))
                    return dir;
            }
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private static string NormalizeExt(string ext)
        {
            if (ext == "tif")
                return "tiff";
            if (ext == "jpeg")
                return "jpg";
            if (string.IsNullOrEmpty(ext))
                return "png";
            return ext;
        }

        private static string Sanitize(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "DefaultTask";
            foreach (var ch in Path.GetInvalidFileNameChars())
                name = name.Replace(ch, '_');
            name = name.Trim();
            return string.IsNullOrWhiteSpace(name) ? "DefaultTask" : name;
        }

        private sealed class BatchCounter
        {
            public int Total;
            public int Remaining;
            public string Category;
            public string Dir;
            public string TaskName;
        }

        private static bool AllowRawFor(RunType rt)
            => rt == RunType.Production;

        private static bool AllowShotFor(RunType rt)
            => rt == RunType.Production || rt == RunType.Replay || rt == RunType.SimulationRun;
    }
}
