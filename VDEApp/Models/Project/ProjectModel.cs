using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using VDEApp.Models.Product;
using VDEApp.Models.TaskNodes;

namespace VDEApp.Models.Project
{
    public class ProjectModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public string CurrentTaskGuid { get; set; }
        /// <summary>
        /// 回复模式
        /// </summary>
        public ResponseType CurrentResponseType { get; set; } = ResponseType.ALL;
        /// <summary>
        /// 当前选中的任务
        /// </summary>
        public TaskModel CurrentTask
        {
            get
            {
                if (string.IsNullOrEmpty(CurrentTaskGuid) || TaskGroup == null)
                {
                    return null;
                }
                // 实时查找！这确保了我们拿到的永远是 TaskGroup 中那个最新的对象
                return TaskGroup.FirstOrDefault(t => t.Guid == CurrentTaskGuid);
            }
            set
            {
                CurrentTaskGuid = value?.Guid;
                //_currentTask = value; // 2. 在这一行设置断点！
            }
        }
        private TaskModel _currentTask;
        /// <summary>
        /// 任务组
        /// </summary>
        public List<TaskModel> TaskGroup { get; set; } = new List<TaskModel>();

        /// <summary>
        /// 存图配置
        /// </summary>
        public ImageSaveOptions ImageSave { get; } = new ImageSaveOptions();

        private bool isModified = false;
        /// <summary>
        /// 项目修改状态
        /// </summary>
        [JsonIgnore]
        public bool IsModified
        {
            get { return isModified; }
            set
            {
                isModified = value;
                ProjectChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }
        [JsonIgnore]
        public bool IsRunning { get; set; } = false;
        [JsonIgnore]
        public bool IsValid => File.Exists(Path);
        [JsonIgnore]
        public string Path { get; set; }
        [JsonIgnore]
        public string Description { get; set; }
        /// <summary>
        /// 服务器端口号
        /// </summary>
        public int Port { get; set; } = 8888;
        #region 事件
        public event EventHandler ProjectChangedEvent;
        #endregion
    }
}
