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
using System.Web;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.LogModule;

namespace VDEApp.Models.TaskNodes
{
    public class CalibrationNode : INode
    {
        [JsonIgnore]
        public InsToolBlock InsToolBlock {  get; set; }
        private InsRecords _insRecords;
        private TaskModel _task;
        public string NodeName { get => "Calibration"; }
        [JsonIgnore]
        public InsRecords NodeRecords { get => _insRecords; }

        public event EventHandler<TaskArgEvent> NodeIntoEvent;
        public event EventHandler<TaskArgEvent> NodeAfterEvent;

        public CalibrationNode(TaskModel task)
        {
            _task = task;
            InsToolBlock = new InsToolBlock();
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
                 LogModule.Log.Error($"保存对象到文件 '{fileName}' 时发生错误：{ex.Message}", ex);
            }
        }
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
                    throw new Exception(InsToolBlock.RunStatus.Message);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"执行标定节点时发生错误：{ex.Message}", ex);
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
