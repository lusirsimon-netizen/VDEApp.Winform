using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insnex.Vision2D;
using VDEApp.Models.Product;

namespace VDEApp.Configs.Display
{
    /// <summary>
    /// 布局选择事件参数类
    /// 用于传递用户选择的布局行列数
    /// </summary>
    public class LayoutSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// 行数
        /// </summary>
        public int Rows { get; private set; }

        /// <summary>
        /// 列数
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        /// 提供 Tuple 风格的兼容访问（Item1 = Rows, Item2 = Columns）
        /// </summary>
        public int Item1 => Rows;

        /// <summary>
        /// 提供 Tuple 风格的兼容访问（Item1 = Rows, Item2 = Columns）
        /// </summary>
        public int Item2 => Columns;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        public LayoutSelectedEventArgs(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        /// <summary>
        /// 隐式转换为 Tuple（可选）
        /// </summary>
        public static implicit operator (int, int)(LayoutSelectedEventArgs args)
        {
            return (args.Rows, args.Columns);
        }
    }

    /// <summary>
    /// 记录更新事件参数
    /// </summary>
    public class RecordUpdatedEventArgs : EventArgs
    {
        public string TaskName { get; }
        public string NodeName { get; }
        public string RecordName { get; }
        public InsRecord Record { get; }

        public RecordUpdatedEventArgs(string taskName, string nodeName, string recordName, InsRecord record)
        {
            TaskName = taskName;
            NodeName = nodeName;
            RecordName = recordName;
            Record = record;
        }
    }

    /// <summary>
    /// 缺陷图像更新事件参数
    /// </summary>
    public class DefectImagesUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// 缺陷图像列表
        /// </summary>
        public System.Collections.Generic.List<System.Drawing.Image> DefectImages { get; }

        /// <summary>
        /// 生产数据事件
        /// </summary>
        public ProductionDataEvent ProductionData { get; }

        public DefectImagesUpdatedEventArgs(System.Collections.Generic.List<System.Drawing.Image> defectImages, ProductionDataEvent productionData)
        {
            DefectImages = defectImages;
            ProductionData = productionData;
        }
    }

    /// <summary>
    /// 绑定更新事件参数
    /// </summary>
    public class BindingUpdatedEventArgs : EventArgs
    {
        public string ControlId { get; }
        public string TaskName { get; }
        public string NodeName { get; }
        public string RecordName { get; }
        public string DisplayName { get; }

        public BindingUpdatedEventArgs(string controlId, string taskName, string nodeName, string recordName, string displayName)
        {
            ControlId = controlId;
            TaskName = taskName;
            NodeName = nodeName;
            RecordName = recordName;
            DisplayName = displayName;
        }
    }

    /// <summary>
    /// 绑定确认事件参数
    /// </summary>
    public class BindingConfirmedEventArgs : EventArgs
    {
        public string TaskName { get; }
        public string NodeName { get; }
        public string RecordName { get; }
        public string RecordDisplayName { get; }

        public BindingConfirmedEventArgs(string taskName, string nodeName, string recordName, string recordDisplayName = "")
        {
            TaskName = taskName;
            NodeName = nodeName;
            RecordName = recordName;
            RecordDisplayName = recordDisplayName;
        }
    }
}
