using AntdUI;
using System.ComponentModel;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs;
using VDEApp.Infrastructure;
using VDEApp.Models;
using VDEApp.Models.Product;

namespace VDEApp.Views.Main
{
    public partial class UCProductionData : UserControl, IMainLoader
    {
        /// <summary>
        /// 生产数据
        /// </summary>
        private readonly BindingList<ProductionDataEvent> _productRecords = new BindingList<ProductionDataEvent>();
        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;
        public UCProductionData()
        {
            InitializeComponent();

            //语言切换事件
            AppModuleSingleton.LanguageSwitchEvent += (o, e) => InitializeUI();
        }
        #region 实现接口
        /// <summary>
        /// Initializes the user interface components and prepares the UI for interaction.
        /// </summary>
        public void InitializeUI()
        {
            //Initialize production data binding.
            tableProductData.Columns = new ColumnCollection
            {
                new  Column("SN", "SN").SetFixed().SetWidth("fill").SetAlign(ColumnAlign.Left).SetEllipsis(),
                new  Column("ProductionTime",Localizer.GetString("UCProductionRecord_ProductionTime","DateTime")).SetFixed().SetWidth("150").SetAlign(ColumnAlign.Left).SetEllipsis() ,
                new  Column("Status",Localizer.GetString("ProductionRecord_Status","Status")).SetFixed().SetWidth("50").SetAlign(ColumnAlign.Center)
            };
            tableProductData.EmptyText = Localizer.GetString("ProductionRecord_NoData", "No production data available.");
            tableProductData.EmptyHeader = true;
            tableProductData.Binding(_productRecords);
        }
        #endregion
        /// <summary>
        /// 绑定事件
        /// </summary>
        public void RefreshBinding()
        {
            foreach (var item in ServiceLocator.ProjectController.CurrentProject.TaskGroup)
            {
                item.TaskRunAfterEvent -= Item_TaskRunAfterEvent;
                item.TaskRunAfterEvent += Item_TaskRunAfterEvent;
            }
        }
        /// <summary>
        /// 新增生产记录 - 简化版，直接添加并限制数量
        /// </summary>
        private void Item_TaskRunAfterEvent(object sender, ProductionDataEvent e)
        {
            tableProductData.BeginInvoke(() =>
            {
                _productRecords.Add(e);
                // 限制最大记录数为1000条
                if (_productRecords.Count > 1000)
                {
                    _productRecords.RemoveAt(0);
                }
            });
        }
    }
}
