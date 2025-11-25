using Insnex.Vision2D;
using Insnex.Vision2D.Core;
using Newtonsoft.Json;

using System;
using System.IO;
using System.Collections.Generic;

using VDEApp.Devices;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Utils.Exceptions;

namespace VDEApp.Models.TaskNodes
{
    public class AcquireNode : INode
    {
        private TaskModel _task;
        private List<(string Name, IInsImage Image)> _images = new List<(string, IInsImage)>();
        private InsRecords _records = new InsRecords();

        [JsonIgnore]
        public TaskModel Task { get => _task; }

        [JsonIgnore]
        public InsRecords NodeRecords { get => _records; }
        public event EventHandler<TaskArgEvent> NodeIntoEvent;
        public event EventHandler<TaskArgEvent> NodeAfterEvent;

        [JsonIgnore]
        public ICamera Camera{ get; set; }
        [JsonIgnore]
        public List<(string Name, IInsImage Image)> Images => _images;

        public string NodeName { get => "Acquire"; }
        public int BatchSize{ get; set; } = 1;
        public int Timeout{ get; set; }
        public double ExposureTime{ get; set; }

        public AcquireNode(TaskModel task)
        {
            _task = task;
        }

        public void Load(string fileName)
        {
            if (!File.Exists(fileName)) return;
            var _ = ServiceLocator.GlobalConfig.GlobalLocalizer.GetString;
            using (StreamReader reader = new StreamReader(fileName))
            {
                var line = reader.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(line))
                {
                    Log.Error(
                        _("log_error_empty_acquire_node_config", "Invalid acquire node config: empty config file")
                    );
                    return;
                }

                Camera = ServiceLocator.DeviceController.FindCameraByGuid(line);
                if (Camera == null)
                {
                    Log.Error(string.Format(
                        _("log_error_invalid_acquire_node_camera", "Invalid acquire node config: can not find camera with guid: \"{0}\""),
                        line
                    ));
                }
            }
        }

        public void Save(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(Camera?.Guid ?? "");
            }
        }

        public void Execute()
        {
            var _ = ServiceLocator.GlobalConfig.GlobalLocalizer.GetString;
            if (Camera == null)
            {
                throw new OperationFailureException(string.Format(
                    _("acquire_error_no_camera", "{0}: No camera selected"),
                    _task.Name
                ));
            }

            lock (ServiceLocator.DeviceController.GetCameraMutex(Camera))
            {
                _images.Clear();
                _records.Clear();
                NodeIntoEvent?.Invoke(this, new TaskArgEvent(this._task, this.NodeRecords));

                Camera.BatchSize = BatchSize;
                Camera.ExposureTime = ExposureTime;
                Camera.TimeOut = Timeout;

                int idx = 1;
                foreach (var img in Camera.AcquireImageOnce())
                {
                    string name = $"InputImage_{idx++}";
                    _images.Add((name, img.Image));
                    _records.Add(new InsRecord(
                        name,
                        img.Image.GetType(), false, img.Image,
                        name
                    ));
                }
                if (_images.Count == 0)
                {
                    throw new OperationFailureException(string.Format(
                        _("acquire_error_no_image_acquired", "{0}: No image acquired"),
                        _task.Name
                    ));
                }

                NodeAfterEvent?.Invoke(this, new TaskArgEvent(this._task, this.NodeRecords));
            }
        }
        /// <summary>
        /// 设置任务模型
        /// </summary>
        /// <param name="taskModel"></param>
        public void SetTaskModel(TaskModel taskModel)
        {
            _task = taskModel;
        }

        public void ResetCamera()
        {
            Camera = null;
        }
    }
}
