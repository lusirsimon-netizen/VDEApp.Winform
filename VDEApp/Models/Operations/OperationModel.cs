using System.Collections.Generic;
using VDEApp.Models.Operations;

namespace VDEApp.Models
{
    public class OperationModel
    {
        public List<OpenHistory> ProjectHistorys { get; set; } = new List<OpenHistory>();

        public string CurrentProject { get; set; }

        public string CurrentLanguage { get; set; } = "zh-CN";
    }
}
