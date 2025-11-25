using VDEApp.Configs;
using VDEApp.Controllers;

namespace VDEApp.Views.Tools
{
    public partial class WinEquipmentManagement : AntdUI.Window, ILocalizableForm
    {
        public WinEquipmentManagement()
        {
            InitializeComponent();
        }
        public void RefreshLanguage()
        {
            LanguageController.AutoRefreshLanguage(this, GlobalConfig.Instance.GlobalLocalizer);
        }
    }
}
