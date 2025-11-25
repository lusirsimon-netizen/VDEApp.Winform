using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VDEApp.Commons;
using VDEApp.Configs.Display;
using VDEApp.LogModule;
using VDEApp.Models;

namespace VDEApp.Configs.Display
{
    /// <summary>
    /// 显示配置管理类（单例模式）
    /// 专门负责显示配置的加载、保存和管理
    /// </summary>
    public class DisplayConfigManager
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static readonly Lazy<DisplayConfigManager> _instance = new Lazy<DisplayConfigManager>(() => new DisplayConfigManager());
        public static DisplayConfigManager Instance => _instance.Value;

        private DisplayConfigManager()
        {
            LoadConfig();
        }

        private Localizer Localizer => GlobalConfig.Instance.GlobalLocalizer;
        /// <summary>
        /// 全局访问点
        /// </summary> 
        private DisplayConfig _config;

        /// <summary>
        /// 当前配置
        /// </summary>
        public DisplayConfig CurrentConfig => _config;

        /// <summary>
        /// 加载配置
        /// </summary>
        public bool LoadConfig(bool showMessage = true)
        {
            try
            {
                if (GlobalConfig.Instance.CurrentProject == null)
                {
                    if (showMessage)
                        MessageBox.Show(Localizer.GetString("Message_ProjectNotLoadedCannotLoadDisplayConfiguration", "Project not loaded, cannot load display configuration!"), Localizer.GetString("Message_LoadFailed", "Load Failed"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                var _configPath = AppPathRouter.GetDisplayConfigPath("DisplayConfig");
                EnsureDirectoryExists(false);

                if (File.Exists(_configPath))
                {
                    try
                    {
                        string json = File.ReadAllText(_configPath);
                        _config = JsonConvert.DeserializeObject<DisplayConfig>(json);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format(Localizer.GetString("Message_FailedToParseConfigurationFile", "Failed to parse configuration file: {0}"), _configPath), ex);
                        _config = new DisplayConfig();
                        return false;
                    }
                }
                else
                {
                    _config = new DisplayConfig();
                    if (EnsureDirectoryExists(false))
                    {
                        try
                        {
                            string json = JsonConvert.SerializeObject(_config, Formatting.Indented);
                            File.WriteAllText(_configPath, json);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(string.Format(Localizer.GetString("Message_FailedToCreateDefaultConfigurationFile", "Failed to create default configuration file: {0}"), _configPath), ex);
                        }
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_AnUnhandledErrorOccurredWhileLoadingDisplayConfiguration", "An unhandled error occurred while loading display configuration"), ex);
                _config = new DisplayConfig();
                return false;
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public bool
            SaveConfig(bool showMessage = true)
        {
            try
            {
                // 确保目录存在
                if (!EnsureDirectoryExists(showMessage))
                {
                    return false;
                }

                var _configPath = AppPathRouter.GetDisplayConfigPath("DisplayConfig");

                // 检查项目是否已加载
                if (!IsProjectLoaded())
                {
                    return false;
                }

                if (_config == null)
                {
                    _config = new DisplayConfig();
                    Log.Warn(Localizer.GetString("Message_ConfigurationObjectIsNullUsingDefaultConfiguration", "Configuration object is null, using default configuration"));
                }

                try
                {
                    string json = JsonConvert.SerializeObject(_config, Formatting.Indented);
                    File.WriteAllText(_configPath, json);

                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format(Localizer.GetString("Message_FailedToWriteConfigurationFile", "Failed to write configuration file: {0}"), _configPath), ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_AnUnhandledErrorOccurredWhileSavingDisplayConfiguration", "An unhandled error occurred while saving display configuration"), ex);
                return false;
            }
        }

        /// <summary>
        /// 确保配置目录存在
        /// </summary>
        private bool EnsureDirectoryExists(bool showMessage = true)
        {
            try
            {
                var _configPath = AppPathRouter.GetDisplayConfigPath("DisplayConfig");
                string directory = Path.GetDirectoryName(_configPath);

                if (string.IsNullOrEmpty(directory))
                {
                    return false;
                }

                if (!Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);

                        return true;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Log.Error(string.Format(Localizer.GetString("Message_InsufficientPermissionsToCreateDirectory", "Insufficient permissions to create directory: {0}"), directory), ex);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format(Localizer.GetString("Message_FailedToCreateDirectory", "Failed to create directory: {0}"), directory), ex);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(Localizer.GetString("Message_ErrorOccurredWhileCheckingDirectory", "Error occurred while checking directory"), ex);
                return false;
            }
        }

        /// <summary>
        /// 检查项目是否已加载
        /// </summary>
        private bool IsProjectLoaded()
        {
            return GlobalConfig.Instance != null &&
                   GlobalConfig.Instance.CurrentProject != null &&
                   !string.IsNullOrEmpty(GlobalConfig.Instance.CurrentProject.Path);
        }

        /// <summary>
        /// 添加绑定配置
        /// </summary>
        public void AddBinding(BindingConfig config)
        {
            RemoveBinding(config.ControlId, false);
            CurrentConfig.BindingConfigs.Add(config);

            SaveConfig(false);
        }

        /// <summary>
        /// 移除绑定配置
        /// </summary>
        public void RemoveBinding(string controlId, bool showMessage = true)
        {
            var existing = CurrentConfig.BindingConfigs.FirstOrDefault(x => x.ControlId == controlId);
            if (existing != null)
            {
                CurrentConfig.BindingConfigs.Remove(existing);
                SaveConfig(false);
            }
        }

        /// <summary>
        /// 更新布局配置
        /// </summary>
        public void UpdateLayout(int rows, int columns, bool showMessage = true)
        {
            CurrentConfig.LayoutRows = rows;
            CurrentConfig.LayoutColumns = columns;
            SaveConfig(showMessage);
        }

        /// <summary>
        /// 获取所有绑定配置
        /// </summary>
        public List<BindingConfig> GetAllBindings()
        {
            return CurrentConfig.BindingConfigs.ToList();
        }

        /// <summary>
        /// 获取控件的绑定配置
        /// </summary>
        public BindingConfig GetBindingByControlId(string controlId)
        {
            return CurrentConfig.BindingConfigs.FirstOrDefault(x => x.ControlId == controlId);
        }
    }

    /// <summary>
    /// 显示配置类
    /// </summary>
    [Serializable]
    public class DisplayConfig
    {
        /// <summary>
        /// 布局行数
        /// </summary>
        [JsonProperty("layoutRows")]
        public int LayoutRows { get; set; } = 1;

        /// <summary>
        /// 布局列数
        /// </summary>
        [JsonProperty("layoutColumns")]
        public int LayoutColumns { get; set; } = 1;

        /// <summary>
        /// 绑定配置列表
        /// </summary>
        [JsonProperty("bindingConfigs")]
        public List<BindingConfig> BindingConfigs { get; set; } = new List<BindingConfig>();

    }

    /// <summary>
    /// 绑定配置类
    /// </summary>
    [Serializable]
    public class BindingConfig
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [JsonProperty("taskName")]
        public string TaskName { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        [JsonProperty("nodeName")]
        public string NodeName { get; set; }

        /// <summary>
        /// 记录名称
        /// </summary>
        [JsonProperty("recordName")]
        public string RecordName { get; set; }

        /// <summary>
        /// Record显示名称（重命名后的名称）
        /// </summary>
        [JsonProperty("recordDisplayName")]
        public string RecordDisplayName { get; set; } = "";

        /// <summary>
        /// 控件ID
        /// </summary>
        [JsonProperty("controlId")]
        public string ControlId { get; set; }

        /// <summary>
        /// 绑定时间
        /// </summary>
        [JsonProperty("bindingTime")]
        public DateTime BindingTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BindingConfig() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BindingConfig(string taskName, string nodeName, string recordName, string controlId, string recordDisplayName = "")
        {
            TaskName = taskName;
            NodeName = nodeName;
            RecordName = recordName;
            ControlId = controlId;
            RecordDisplayName = !string.IsNullOrEmpty(recordDisplayName) ? recordDisplayName : recordName;
        }
    }
}
