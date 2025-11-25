using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VDEApp.Commons;
using VDEApp.Controllers;
using VDEApp.Devices;
using VDEApp.Infrastructure;

namespace VDEApp.Configs
{
    public class CameraConfiguration : IConfig
    {
        public void Initialization()
        {
            if (!File.Exists(AppPathRouter.CameraListConfigPath))
                return;

            var configs = JsonConvert.DeserializeObject<List<CameraConfigurationItem>>(
                File.ReadAllText(AppPathRouter.CameraListConfigPath)
            );
            if (configs == null)
            {
                // Invalid camera config, failed to load camera information
                LogModule.Log.Warn(
                    GlobalConfig.Localizer.GetString("log_warning_invalid_camera_config")
                );
                return;
            }

            var searched_models = new HashSet<Type>();

            foreach (var conf in configs)
            {
                var type = Type.GetType(conf.Type);
                var camera = Activator.CreateInstance(type) as ICamera;
                if (camera == null)
                {
                    // Failed to create instance for type "{0}"
                    LogModule.Log.Error(string.Format(
                        GlobalConfig.Localizer.GetString("log_error_cannot_create_camera_instance"),
                        conf.Type
                    ));
                    continue;
                }

                Action<string, Exception> log_init_error = (string key, Exception ex) => {
                    LogModule.Log.Error(string.Format(
                        GlobalConfig.Localizer.GetString(key),
                        camera.Name,
                        ex.Message
                    ));
                };

                camera.Name = conf.Name;
                camera.Guid = conf.Guid;
                camera.BatchSize = conf.BatchSize;

                if (conf.StateObject is JObject obj && camera.StateObjectType != null)
                {
                    try {
                        camera.LoadStateFromSerializable(obj.ToObject(camera.StateObjectType));
                    } catch (Exception ex) {
                        // Camera "{0}" failed to restore state: {1}
                        log_init_error("log_error_camera_restore_state_failed", ex);
                    }
                }
                if (conf.IsConnected)
                {
                    if (!searched_models.Contains(type))
                    {
                        var model = ServiceLocator.DeviceController.CameraModels[type];
                        try {
                            model.SearchCameras();
                            searched_models.Add(type);
                        } catch (Exception ex) {
                            // Camera type "{0}" search failed: {1}
                            LogModule.Log.Error(string.Format(
                                GlobalConfig.Localizer.GetString("log_error_camera_search_failed"),
                                model.Name,
                                ex.Message
                            ));
                        }
                    }
                    try {
                        camera.ConnectCamera();
                    } catch (Exception ex) {
                        // Camera "{0}" failed to connect: {1}
                        log_init_error("log_error_camera_init_connect_failed", ex);
                    }
                    try {
                        camera.LoadConfig(AppPathRouter.GetSingleCameraConfigPath(conf.ConfigFileName));
                    } catch (Exception ex) {
                        // Camera "{0}" failed to load config: {1}
                        log_init_error("log_error_camera_init_load_config_failed", ex);
                    }
                }

                ServiceLocator.DeviceController.AddCamera(camera);
            }
        }

        public void Save()
        {
            if (ServiceLocator.DeviceController.Cameras.Count == 0)
                return;
            if (!Directory.Exists(AppPathRouter.CameraConfigPath))
                Directory.CreateDirectory(AppPathRouter.CameraConfigPath);
            var config_list = new List<CameraConfigurationItem>();
            foreach (var camera in ServiceLocator.DeviceController.Cameras)
            {
                var config_file_name = $"{camera.Guid}{camera.ConfigFileExtension}";
                camera.ExportConfig(AppPathRouter.GetSingleCameraConfigPath(config_file_name));
                config_list.Add(new CameraConfigurationItem{
                    Type=camera.GetType().ToString(),
                    Name=camera.Name,
                    IsConnected=camera.IsConnected,
                    Guid=camera.Guid,
                    BatchSize=camera.BatchSize,
                    ConfigFileName=config_file_name,
                    StateObject=camera.ExportStateToSerializable()
                });
            }
            var json = JsonConvert.SerializeObject(config_list, Formatting.Indented);
            File.WriteAllText(AppPathRouter.CameraListConfigPath, json);
        }
    }

    [Serializable]
    internal class CameraConfigurationItem
    {
        [JsonProperty("Type")]
        public string Type{ get; set; }
        [JsonProperty("Name")]
        public string Name{ get; set; }
        [JsonProperty("IsConnected")]
        public bool IsConnected{ get; set; }
        [JsonProperty("Guid")]
        public string Guid{ get; set; }
        [JsonProperty("BatchSize")]
        public int BatchSize{ get; set; }
        [JsonProperty("ConfigFileName")]
        public string ConfigFileName{ get; set; }
        [JsonProperty("StateObject")]
        public object StateObject{ get; set; } = null;
    }
}
