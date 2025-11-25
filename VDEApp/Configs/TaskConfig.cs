using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Configs
{
    public class TaskConfig : IConfig
    {
        private static readonly Lazy<TaskConfig> _taskConfig = new Lazy<TaskConfig>(() => new TaskConfig());
        public static TaskConfig Instance = _taskConfig.Value;
        
        /// <exception cref="NotImplementedException"></exception>
        public void Initialization()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
