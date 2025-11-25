using Insnex.Vision2D;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.ToolBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.LogModule;
using VDEApp.Models.Product;

namespace VDEApp.Models.TaskNodes
{
    public class InspectionNode : INode
    {
        [JsonIgnore]
        public InsToolBlock InsToolBlock { get; set; }
        private InsRecords _insRecords { get; set; }
        private TaskModel _task {get;set;}
        public string NodeName { get => "Inspection"; }
        [JsonIgnore]
        public InsRecords NodeRecords { get => _insRecords; }

        public event EventHandler<TaskArgEvent> NodeIntoEvent;
        public event EventHandler<TaskArgEvent> NodeAfterEvent;
        public InspectionNode(TaskModel task)
        {
            InsToolBlock = new InsToolBlock();
            _task = task;
        }
        /// <summary>
        /// 从本地磁盘加载ToolBlock到程序
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Load(string fileName)
        {
            // 1. 检查输入路径是否合法
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("文件名不能为空或仅包含空白字符。", nameof(fileName));
            }

            // 2. 检查文件是否存在,没有则新建
            if (!File.Exists(fileName))
            {
                try
                {
                    InsToolBlock = new InsToolBlock();
                    InsToolBlock.CreateNewScript(InsScriptLanguageConstants.ScriptCSharp, InsToolBlockScriptTypeConstants.Advanced);
                    InsToolBlock.Script.ReadScriptFromFile(Path.Combine(AppPathRouter.AppPath, "ScriptTemplate.txt"));
                    Save(fileName);
                }
                catch
                {
                    throw new FileNotFoundException($"未找到文件：{fileName}");
                }

            }

            // 3. 尝试加载文件内容
            try
            {
                InsToolBlock = (InsToolBlock)InsUtils.LoadObjectFromFile(fileName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"加载文件 '{fileName}' 时发生错误：{ex.Message}", ex);
            }

        }
        /// <summary>
        /// 程序中的ToolBlock保存到本地磁盘
        /// </summary>
        /// <param name="fileName"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Save(string fileName)
        {
            // 1. 检查输入路径是否合法
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("文件名不能为空或仅包含空白字符。", nameof(fileName));
            }
            // 2. 如果目录路径不为空且目录不存在，则创建目录
            string directoryPath = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            // 2. 尝试保存文件内容
            try
            {
                InsUtils.SaveObjectToFile(InsToolBlock, fileName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"保存对象到文件 '{fileName}' 时发生错误：{ex.Message}", ex);
            }
        }
        // --- 【新增】一个公共方法，用于获取最新的运行状态 ---
        public ProductionDataStatus GetLastRunStatus()
        {
            // 检查工具块的输出引脚中是否存在 "Status"
            if (InsToolBlock != null && InsToolBlock.Outputs.Contains("Status"))
            {
                // 获取 Status 的值
                object statusValue = InsToolBlock.Outputs["Status"].Value;

                // 安全地将 object 转换为 bool，再转换为我们的枚举
                if (statusValue is bool b)
                {
                    return b ? ProductionDataStatus.OK : ProductionDataStatus.NG;
                }
            }

            // 如果找不到 Status 或者类型不正确，返回默认值 NA
            return ProductionDataStatus.NA;
        }
        public ProductionDataStatus LastProductionResult { get; private set; } = ProductionDataStatus.NA;
        /// <summary>
        /// ToolBlock执行
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Execute()
        {
            NodeIntoEvent?.Invoke(this, new TaskArgEvent(_task, NodeRecords));
            try
            {
                InsToolBlock.Run();
                if (InsToolBlock.RunStatus.Result != InsToolResultConstants.Accept)
                {
                    LastProductionResult = ProductionDataStatus.NA;
                    throw new Exception(InsToolBlock.RunStatus.Message);
                }
                if (!InsToolBlock.Outputs.Contains("Status"))
                {
                    LastProductionResult = ProductionDataStatus.NA;
                }
                else if (InsToolBlock.Outputs["Status"].Value is bool b)
                {
                    LastProductionResult = b ? ProductionDataStatus.OK : ProductionDataStatus.NG;
                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"InspectionNode Execute Error: {ex.Message}");
            }
            _insRecords?.Clear();
            _insRecords = InsToolBlock.CreateLastRunRecord().SubRecords;
            NodeAfterEvent?.Invoke(this, new TaskArgEvent(_task, NodeRecords));
        }
        /// <summary>
        /// 设置任务模型
        /// </summary>
        /// <param name="taskModel"></param>
        public void SetTaskModel(TaskModel taskModel)
        {
            _task = taskModel;
        }
    }
}
