using System;
using System.Collections.Concurrent;
using System.Linq;

namespace VDEApp.Devices
{
    public class InsfwDeviceManager
    {
        private static readonly Lazy<InsfwDeviceManager> DeviceManager = new Lazy<InsfwDeviceManager>(() => new InsfwDeviceManager());
        public static InsfwDeviceManager Instance => DeviceManager.Value;

        #region 相机管理
        /// <summary>
        /// 相机列表
        /// </summary>
        public ConcurrentDictionary<string, ICamera> Cameras = new ConcurrentDictionary<string, ICamera>();
        /// <summary>
        /// 初始化
        /// </summary>
        public void InitializeCamera()
        {
            //for (int i = 0; i < InsfwDeviceConfiguration.Default.CameraSettings.Count; ++i)
            //{
            //    try
            //    {
            //        InitCamera(InsfwDeviceConfiguration.Default.CameraSettings[i].Guid);
            //    }
            //    catch (Exception ex)
            //    {
            //        // InsMessageManager.GInstance.Alarm("初始化2D 相机失败：" + ex.Message, Common.InsErrorCode.Err_Unknown);
            //    }
            //}
            NotifyCameraStatusChanged();
        } 
        /// <summary>
        /// 终止
        /// </summary>
        private void ShutDown()
        {
            for (int i = 0; i < Cameras.Count; ++i)
            {
                try
                {
                    Cameras.ElementAt(i).Value.CloseCamera();
                }
                catch (Exception ex)
                {
                    //InsfwGlobalLogger.Error("取像", $"2D相机关闭失败：" + ex.Message);
                }
            }
            Cameras.Clear();
        }
        /// <summary>
        /// 相机状态变化事件
        /// </summary>
        public event EventHandler CameraStatusChanged;
        public void NotifyCameraStatusChanged()
        {
            CameraStatusChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
