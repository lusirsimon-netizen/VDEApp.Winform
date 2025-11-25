using System;
using System.Collections.Generic;
using VDEApp.Views.Main;

namespace VDEApp.Commons
{
    public static class AppModuleSingleton
    {
        #region ViewInstance 
        /// <summary>
        /// MainForm Instance 
        /// </summary> 
        private static readonly Lazy<MainForm> MainForm = new Lazy<MainForm>(() => new MainForm());
        public static MainForm MainFormInstance => MainForm.Value;

        /// <summary>
        /// TitleBar
        /// </summary> 
        private static readonly Lazy<UCTitleBar> UCTitleBar = new Lazy<UCTitleBar>(() => new UCTitleBar());
        public static UCTitleBar TitleBarInstance => UCTitleBar.Value;

        /// <summary>
        /// Gets or sets the shared toolbar control used throughout the application.
        /// </summary> 
        private static readonly Lazy<UCToolBar> UCToolBar = new Lazy<UCToolBar>(() => new UCToolBar());
        public static UCToolBar ToolBarInstance => UCToolBar.Value;

        /// <summary>
        /// Gets or sets the shared product display control instance used for rendering product information in the user interface.
        /// </summary>  
        private static readonly Lazy<UCProductDisplay> UCProductDisplay = new Lazy<UCProductDisplay>(() => new UCProductDisplay());
        public static UCProductDisplay ProductDisplay => UCProductDisplay.Value;

        /// <summary>
        /// Gets or sets the current production record used by the application.
        /// </summary> 
        private static readonly Lazy<UCProductionData> UCProductionRecord = new Lazy<UCProductionData>(() => new UCProductionData());
        public static UCProductionData ProductionRecord => UCProductionRecord.Value;

        /// <summary>
        /// Gets or sets the global production log instance used to record production-related events and data.
        /// </summary>  
        private static readonly Lazy<UCProductionLog> UCProductionLog = new Lazy<UCProductionLog>(() => new UCProductionLog());
        public static UCProductionLog ProductionLog => UCProductionLog.Value;

        /// <summary>
        /// Gets or sets the shared status bar user control for the application.
        /// </summary>  
        private static readonly Lazy<UCStatusBar> UCStatusBar = new Lazy<UCStatusBar>(() => new UCStatusBar());
        public static UCStatusBar StatusBarInstance => UCStatusBar.Value;

        #endregion

        #region
        /// <summary>
        /// Main UI Loaders
        /// </summary>
        private static List<IMainLoader> Loaders = new List<IMainLoader>() { TitleBarInstance, ToolBarInstance, StatusBarInstance, ProductionLog, ProductDisplay, ProductionRecord };
        /// <summary>
        /// 重载UI
        /// </summary>
        public static void ReloadUI()
        {
            foreach (var loader in Loaders)
            {
                loader.InitializeUI();
            }
        }
        #endregion

        #region ViewRefresh
        public static void StatusPrompt(string message)
        {
            StatusBarInstance.Prompt(message);
        }
        public static void ReadyStatusPrompt()
        {
            StatusBarInstance.Prompt("Ready.");
        }
        /// <summary>
        /// 切换语言事件
        /// </summary>
        public static EventHandler LanguageSwitchEvent;
        #endregion
    }
}
