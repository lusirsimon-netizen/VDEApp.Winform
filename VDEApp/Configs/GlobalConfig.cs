using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Models;
using VDEApp.Models.Project;
using VDEApp.Models.Product;
using VDEApp.Models.TaskNodes;
using VDEApp.Controllers;

namespace VDEApp.Configs
{
    public class GlobalConfig : IConfig
    {
        private static readonly Lazy<GlobalConfig> GlobalCtrl = new Lazy<GlobalConfig>(() => new GlobalConfig());
        public static GlobalConfig Instance => GlobalCtrl.Value;
        public static Localizer Localizer => Instance.GlobalLocalizer;
        /// <summary>
        /// 全局本地化实例
        /// </summary>
        public Localizer GlobalLocalizer = new Localizer();
        /// <summary>
        ///  当前语言（默认中文）
        /// </summary>
        public string CurrentLang = "zh-CN";
        /// <summary>
        /// user operation
        /// </summary>
        public OperationModel UserOperation { get; set; } = new OperationModel();
        /// <summary>
        /// 当前项目
        /// </summary>
        public ProjectModel CurrentProject { get; set; } = new ProjectModel() { Name = "NewProject", Description = "*", TaskGroup = new List<TaskModel>() { TaskModel.Create("NewTask") } };
        /// <summary>
        /// 当前运行类型
        /// </summary>
        public RunType CurrentRunType { get; set; } = RunType.Production;
        /// <summary>
        /// /Initialization Globalconfig
        /// </summary>
        public void Initialization()
        {
            try
            {
                //initial directory
                var appPaths = new List<string>() { AppPathRouter.AppData, AppPathRouter.WorkRoot };
                foreach (var item in appPaths)
                {
                    if (!Directory.Exists(item))
                        Directory.CreateDirectory(item);
                }
                //initial operations
                if (File.Exists(AppPathRouter.AppOperation))
                {
                    try
                    {
                        var openData = File.ReadAllText(AppPathRouter.AppOperation);
                        UserOperation = JsonConvert.DeserializeObject<OperationModel>(openData);
                        if (!string.IsNullOrEmpty(UserOperation.CurrentLanguage))
                        {
                            CurrentLang = UserOperation.CurrentLanguage;
                            GlobalConfig.Localizer.LoadLanguage(CurrentLang);
                            LanguageController.SwitchLanguage(CurrentLang);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Reading operation file failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Save global config
        /// </summary>
        public void Save()
        {
            try
            {
                SaveOperation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 保存操作文件
        /// </summary>
        /// <exception cref="OperationCanceledException"></exception>
        public void SaveOperation()
        {
            try
            {
                File.WriteAllText(AppPathRouter.AppOperation, JsonConvert.SerializeObject(UserOperation));
            }
            catch (Exception)
            {
                throw new OperationCanceledException("Failed to save operation file.");
            }
        }
    }
}
