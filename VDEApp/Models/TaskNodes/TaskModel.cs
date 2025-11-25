using AntdUI;
using Insnex.Vision2D;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.Core;
using Insnex.Vision2D.ToolBlock;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VDEApp.Commons;
using VDEApp.Configs.Display;
using VDEApp.Controllers;
using VDEApp.Infrastructure;
using VDEApp.LogModule;
using VDEApp.Models.Product;
using VDEApp.Utils.Exceptions;

namespace VDEApp.Models.TaskNodes
{
    public class TaskModel
    {
        private Localizer _ => ServiceLocator.GlobalConfig.GlobalLocalizer;
        /// <summary>
        /// 任务结束事件
        /// </summary>
        public event EventHandler<ProductionDataEvent> TaskRunAfterEvent;
        /// <summary>
        /// GUID
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 取相节点
        /// </summary>
        public AcquireNode AcquireNode { get; set; }
        /// <summary>
        /// 标定节点
        /// </summary>
        public CalibrationNode CalibrationNode { get; set; }
        /// <summary>
        /// 检查节点
        /// </summary>
        public InspectionNode InspectionNode { get; set; }

        /// <summary>
        /// Spec字典
        /// </summary>
        public Dictionary<string, object> Spec { get; set; }
        /// <summary>
        /// 参数字典
        /// </summary>
        public Dictionary<string, object> Parameter { get; set; }
        /// <summary>
        /// 输入图片列表
        /// </summary>
        [JsonIgnore]
        public List<(string Name, IInsImage Image)> InputImages { get; set; }
        /// <summary>
        /// 传入SN
        /// </summary>
        public string SN { get; set; } = "";
        /// <summary>
        /// 任务运行结果
        /// </summary>
        public ProductionDataStatus Status { get; set; } = ProductionDataStatus.NA;
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskModel(string name)
        {
            this.Guid = System.Guid.NewGuid().ToString();
            this.Name = name;
            this.CreatedAt = DateTime.Now;
        }

        public static TaskModel Create(string name)
        {
            var instance = new TaskModel(name);
            instance.AcquireNode = new AcquireNode(instance);
            instance.CalibrationNode = new CalibrationNode(instance);
            instance.InspectionNode = new InspectionNode(instance);
            instance.Spec = new Dictionary<string, object>();
            instance.Parameter = new Dictionary<string, object>();

            ServiceLocator.DeviceController.CameraRemove += instance.DeviceController_CameraRemove;

            return instance;
        }

        private void DeviceController_CameraRemove(DeviceController sender, Controllers.CameraRemoveEventArgs e)
        {
            if (this.AcquireNode.Camera?.Guid == e.Guid)
                this.AcquireNode.ResetCamera();
        }

        private static void fixParamDictionary(Dictionary<string, object> dict, string name)
        {
            if (dict == null)
                return;

            var should_convert = new List<(string key, object value)>();
            foreach (var item in dict)
            {
                if (item.Value is Newtonsoft.Json.Linq.JObject obj)
                {
                    var converted = obj.ToObject<Tuple<double, double>>();
                    if (converted == null)
                    {
                        Log.Warn($"名称为{item.Key}的{name}变量类型错误，已忽略");
                        continue;
                    }
                    should_convert.Add((item.Key, converted));
                }else if (item.Value is long num)
                {
                    var converted = Convert.ToInt32(num);
                    should_convert.Add((item.Key, converted));
                }
            }
            foreach (var (key, value) in should_convert)
            {
                dict[key] = value;
            }

        }

        /// <summary>
        /// 加载任务
        /// </summary>
        /// <param name="path"></param>
        public void Load()
        {
            try
            {
                fixParamDictionary(this.Parameter, "Parameter");
                fixParamDictionary(this.Spec, "Spec");

                AcquireNode.SetTaskModel(this);
                CalibrationNode.SetTaskModel(this);
                InspectionNode.SetTaskModel(this);
                ServiceLocator.DeviceController.CameraRemove += this.DeviceController_CameraRemove;

                // 订阅取相/检测节点事件，用于保存原图/截图
                ServiceLocator.DeviceController.CameraRemove -= this.DeviceController_CameraRemove;
                ServiceLocator.DeviceController.CameraRemove += this.DeviceController_CameraRemove;

                AcquireNode.Load(this.GetAcquireNodeConfigPath());
                CalibrationNode.Load(this.GetCalibrationToolBlockPath());
                InspectionNode.Load(this.GetInspectionToolBlockPath());

                // 1. 为“保存原图”的事件进行安全订阅
                this.AcquireNode.NodeAfterEvent -= OnAcquireNodeAfterEvent; // 先取消
                this.AcquireNode.NodeAfterEvent += OnAcquireNodeAfterEvent; // 再订阅

                // 2. 为“保存截图”的事件进行安全订阅
                this.InspectionNode.NodeAfterEvent -= OnInspectionNodeAfterEvent; // 先取消
                this.InspectionNode.NodeAfterEvent += OnInspectionNodeAfterEvent; // 再订阅
            }
            catch (Exception e)
            {
                throw new OperationFailureException($"Project loading exception, task has been updated, please recreate the project.\n{e.Message}");
            }
        }

        public void Save()
        {
            AcquireNode.Save(this.GetAcquireNodeConfigPath());
            CalibrationNode.Save(this.GetCalibrationToolBlockPath());
            InspectionNode.Save(this.GetInspectionToolBlockPath());
        }

        public void Delete()
        {
            try
            {
                string path = this.GetTaskPath();
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                Log.Error(_.GetString("log_error_task_file_delete_failed"), ex);
            }
        }
        private string _ptGuid;
        public void Run()
        {
            // 取相
            _ptGuid = System.Guid.NewGuid().ToString();//先newGuid
            List<(string Name, IInsImage Image)> images = InputImages;
            try
            {
                if (images == null)
                {
                    AcquireNode.SetTaskModel(this);
                    AcquireNode.Execute();
                    images = AcquireNode.Images;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{Name}:{ex.Message}", ex);
            }
            InsRecords inspectionNodeRecords = null;
            try
            {
                // 标定
                foreach (var (name, img) in images)
                {
                    AddOrUpdateInputTerminal(CalibrationNode.InsToolBlock, name, img, typeof(IInsImage));
                }
                AddOrUpdateInputTerminal(
                    CalibrationNode.InsToolBlock,
                    "parameter",
                    this.Parameter,
                    typeof(Dictionary<string, object>)
                    );
                CalibrationNode.InsToolBlock.Inputs["parameter"].Value = this.Parameter;
                CalibrationNode.SetTaskModel(this);
                CalibrationNode.Execute();
                // 数据联通到检测
                LinkData(CalibrationNode, InspectionNode);
                // 检测
                AddOrUpdateInputTerminal(
                    InspectionNode.InsToolBlock,
                    "parameter",
                    this.Parameter,
                    typeof(Dictionary<string,object>)
                    );
                AddOrUpdateInputTerminal(
                    InspectionNode.InsToolBlock,
                    "Spec",
                    this.Spec,
                    typeof(Dictionary<string, object>)
                    );
                AddOutputTerminalIfNotExists(
                    InspectionNode.InsToolBlock,
                    "DefectImages",
                     null,
                     typeof(ArrayList)
                    );
                InspectionNode.SetTaskModel(this);
                InspectionNode.Execute();
                AddOutputTerminalIfNotExists(
                    InspectionNode.InsToolBlock,
                    "Status",
                    true,
                    typeof(bool)
                    );
                if (InspectionNode.InsToolBlock.Outputs["Status"].Value != null)
                    Status = (bool)InspectionNode.InsToolBlock.Outputs["Status"].Value ? ProductionDataStatus.OK : ProductionDataStatus.NG;

                inspectionNodeRecords = InspectionNode.NodeRecords;
            }
            catch (Exception ex)
            {
                Status = ProductionDataStatus.NA;
                throw new Exception($"{Name}:{ex.Message}", ex);
            }
            finally
            {
                TaskRunAfterEvent -= ServiceLocator.SaveImageController.TaskRunAfterEvent;
                TaskRunAfterEvent += ServiceLocator.SaveImageController.TaskRunAfterEvent;
                var productionData = new ProductionDataEvent()
                {
                    ProductionName = _.GetString("TaskController_ProductionName"),
                    ProductionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Guid = this._ptGuid,
                    SN = SN,
                    Status = Status,
                    DefectImages = GetDefectImagesSafely(),
                    InspectionNodeRecords = inspectionNodeRecords
                };
                SafeInvokeEvent(productionData);
            }
        }
        #region 私有辅助方法
        /// <summary>
        /// 给 InsToolBlock 添加或更新输入终端（不存在则创建，存在则更新值）
        /// </summary>
        /// <param name="toolBlock">目标 InsToolBlock</param>
        /// <param name="terminalName">终端名称</param>
        /// <param name="value">终端值</param>
        /// <param name="terminalType">终端数据类型</param>
        private void AddOrUpdateInputTerminal(InsToolBlock toolBlock, string terminalName, object value, Type terminalType)
        {
            if (toolBlock == null)
                throw new ArgumentNullException(nameof(toolBlock), "InsToolBlock 不能为空");
            if (string.IsNullOrWhiteSpace(terminalName))
                throw new ArgumentException("终端名称不能为空", nameof(terminalName));
            if (terminalType == null)
                throw new ArgumentNullException(nameof(terminalType), "终端类型不能为空");

            // 检查终端是否存在：不存在则创建并添加，存在则更新值
            if (!toolBlock.Inputs.Contains(terminalName))
            {
                toolBlock.Inputs.Add(new InsToolBlockTerminal(terminalName, value, terminalType));
            }
            else
            {
                toolBlock.Inputs[terminalName].Value = value;
            }
        }

        /// <summary>
        /// 给 InsToolBlock 添加输出终端（仅当终端不存在时创建）
        /// </summary>
        /// <param name="toolBlock">目标 InsToolBlock</param>
        /// <param name="terminalName">终端名称</param>
        /// <param name="defaultValue">终端默认值（创建时赋值）</param>
        /// <param name="terminalType">终端数据类型</param>
        private void AddOutputTerminalIfNotExists(InsToolBlock toolBlock, string terminalName, object defaultValue, Type terminalType)
        {
            if (toolBlock == null)
                throw new ArgumentNullException(nameof(toolBlock), "InsToolBlock 不能为空");
            if (string.IsNullOrWhiteSpace(terminalName))
                throw new ArgumentException("终端名称不能为空", nameof(terminalName));
            if (terminalType == null)
                throw new ArgumentNullException(nameof(terminalType), "终端类型不能为空");

            // 仅当终端不存在时添加
            if (!toolBlock.Outputs.Contains(terminalName))
            {
                toolBlock.Outputs.Add(new InsToolBlockTerminal(terminalName, defaultValue, terminalType));
            }
        }
        /// <summary>
        /// 连接两个节点的输入输出
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        public void LinkData(CalibrationNode node1, InspectionNode node2)
        {
            var outputTerminals = node1.InsToolBlock.Outputs;
            var inputTerminals = node2.InsToolBlock.Inputs;
            try
            {
                foreach (var output in outputTerminals)
                {
                    if (inputTerminals.Contains(output.Name))
                        inputTerminals[output.Name].Value = output.Value;
                    else
                        node2.InsToolBlock.Inputs.Add(output);
                }
            }
            catch (Exception ex)
            {
                 Log.Error(_.GetString("log_error_node_data_link_failed"), ex);
            }
        }
        /// <summary>
        /// 安全触发事件
        /// </summary>
        private void SafeInvokeEvent(ProductionDataEvent productionData)
        {
            try
            {
                // 检查是否有订阅者
                if (TaskRunAfterEvent == null)
                {
                    Log.Debug(_.GetString("TaskController_NoTaskRunAfterEventSubscriber"));
                    return;
                }

                TaskRunAfterEvent?.Invoke(this, productionData);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0}: {1}", _.GetString("TaskController_EventTriggerFailed"), ex.Message), ex);
            }
        }
        /// <summary>
        /// 安全获取缺陷图像
        /// </summary>
        private ArrayList GetDefectImagesSafely()
        {
            try
            {
                if (InspectionNode?.InsToolBlock?.Outputs == null)
                {
                    return new ArrayList();
                }

                var outputs = InspectionNode.InsToolBlock.Outputs;

                if (outputs.Contains("DefectImages"))
                {
                    var defectImages = outputs["DefectImages"].Value as ArrayList;
                    return defectImages ?? new ArrayList();
                }

                return new ArrayList();
            }
            catch (Exception ex)
            {
                Log.Warn(string.Format("{0}: {1}", _.GetString("TaskController_GetDefectImagesFailed"), ex.Message), ex);
                return new ArrayList();
            }
        }
        #endregion
        #region 存图事件处理
        private void OnAcquireNodeAfterEvent(object s, TaskArgEvent e)
        {
            try
            {
                if (!ServiceLocator.GlobalConfig.CurrentProject.ImageSave.RawEnabled || ServiceLocator.GlobalConfig.CurrentRunType != RunType.Production)
                {
                    return;
                }
                var node = e.Task.AcquireNode;
                ServiceLocator.SaveImageController.EnqueueRecords(
                    e.Task,
                    e.NodeRecords,
                    runGuid: e.Task._ptGuid,
                    isRawImage: true
                );
            }
            catch (Exception ex)
            {
                try
                { Log.Error($"原图入队保存失败：{ex.Message}"); }
                catch { }
            }
        }

        private void OnInspectionNodeAfterEvent(object s, TaskArgEvent e)
        {
        }
        #endregion
    }
}
