using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Configs
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public interface IConfig
    {
        public void Save();
        public void Initialization();

    }
}