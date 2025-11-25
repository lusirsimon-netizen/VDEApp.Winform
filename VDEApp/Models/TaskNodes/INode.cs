using Insnex.Vision2D;
using Insnex.Vision2D.ToolBlock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VDEApp.Models.TaskNodes.TaskModel;

namespace VDEApp.Models.TaskNodes
{
    public interface INode
    {
        public event EventHandler<TaskArgEvent> NodeIntoEvent;
        public event EventHandler<TaskArgEvent> NodeAfterEvent;
        public void Execute();
        public void Load(string fileName);
        public void Save(string fileName);
        public string NodeName { get; }
        /// <summary>
        /// 设置任务模型
        /// </summary>
        /// <param name="taskModel"></param>
        public void SetTaskModel(TaskModel taskModel);
        public InsRecords NodeRecords { get; }
    }
    /// <summary>
    /// 任务信息
    /// </summary>
    public class TaskArgEvent
    {
        private TaskModel _task { get; set; }
        private InsRecords _nodeRecords { get; set; }
        public TaskModel Task { get => _task; }
        public InsRecords NodeRecords { get => _nodeRecords; }
        public TaskArgEvent(TaskModel task, InsRecords nodeRecodes)
        {
            _task = task;
            _nodeRecords = nodeRecodes;
        }
    }
}
