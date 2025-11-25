using System;
using System.Collections.Generic;
using VDEApp.Devices.Datas;
using VDEApp.Devices.Enums;
using VDEApp.Utils.Exceptions;

namespace VDEApp.Devices
{
    public delegate void CamReadyHandler(object sender);
    public delegate void CamCompleteHandler(object sender, AcquiredEventArgs e);
    public delegate void CamLiveHandler();
    public delegate void CamConnectionChangedHandler(object sender, CamConnectChangeEventArgs e);

    /// <summary>
    /// Base class for all camera types
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown in methods and properties that is not supported in this type of camera
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Thrown in methods and properties that is not implemented yet
    /// </exception>
    /// <exception cref="ConfigException">
    /// Thrown in config related methods and properties when config is invalid
    /// </exception>
    /// <exception cref="OperationFailureException">
    /// Thrown in operation methods when fails
    /// </exception>
    public interface ICamera
    {
        string Name{ get; set; }
        string Guid { get; set; }
        string SN { get; }
        string ModelName { get; }
        InsCameraType CameraType{ get; }

        int StitchingLines { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        int TimeOut { get; set; }
        /// <summary>
        /// 图像格式
        /// </summary>
        InsImageType ImageFormat { get; set; }
        /// <summary>
        /// 取像方式
        /// </summary>
        InsCamAcqMode AcquisitionMode { get; set; }
        /// <summary>
        /// 触发方式
        /// </summary>
        InsCameraTriggerMode TriggerMode { get; set; }
        /// <summary>
        /// 在线状态
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// 相机掉线重连使能
        /// </summary>
        bool DisConnectedEnable { get; set; }
        /// <summary>
        ///当前曝光时间
        /// </summary>
        double ExposureTime { get; set; }
        int LineScanHeight { get; set; }
        int BatchSize { get; set; }
        /// <summary>
        /// 增益
        /// </summary>
        double Gain { get; set; }
        void SetScanDirection(InsCameraScanDirection direction);
        void SetScanLines(int lines);
        /// <summary>
        /// 设置曝光
        /// </summary>
        void SetExposureTime(double timeInUs);
        /// <summary>
        /// 打开触发模式
        /// </summary>
        void TriggerModeOn();
        /// <summary>
        /// 关闭触发模式
        /// </summary>
        void TriggerModeOff();
        /// <summary>
        /// 设置触发模式
        /// </summary>
        /// <param name="triggerMode"></param>
        void SetTriggerMode(InsCameraTriggerMode triggerMode);

        /// <summary>
        /// 连接相机
        /// </summary>
        void ConnectCamera();
        /// <summary>
        /// 关闭相机
        /// </summary>
        void CloseCamera();

        void StartAcquireImage();
        void StopAcquireImage();
        /// <summary>
        ///
        /// </summary>
        /// <param name="timeoutInMs"></param>
        /// <param name="exposureTime"></param>
        /// <param name="onReadyAction"></param>
        /// <returns></returns>
        List<AcquiredImageInfo> AcquireImageOnce(double timeoutInMs = int.MaxValue, double exposureTime = -1, Action onReadyAction = null);


#region Old Config Interface
        /// <summary>
        /// 读取相机内部参数
        /// </summary>
        void GetParams();
        /// <summary>
        /// 写入参数到相机内部
        /// </summary>
        //void SetParams(InsCamera2DSetting setting);

        void LoadCameraConfigFile(string configPath); // ???
        void LoadBoardConfigFile(string configPath);  // used by InsLineScanCameraItek

        /// <summary>
        /// 从本地文件读取配置
        /// </summary>
        void LoadSetting();
        /// <summary>
        /// 写入配置到本地文件
        /// </summary>
        void SaveSetting();
#endregion

#region Config
        /// <summary>
        /// Not sure what this is, probably used to switch config preset within camera.
        /// Such feature is supported by SSZN cameras, not sure if other camera has this.
        /// </summary>
        void SwitchProgramNo(int programNo);

        /// <summary>
        /// Config file extension, with dot. e.g. ".txt"
        /// </summary>
        string ConfigFileExtension{ get; }
        /// <summary>
        /// Load config from file
        /// </summary>
        void LoadConfig(string path);
        /// <summary>
        /// Export config to file
        /// </summary>
        void ExportConfig(string path);
#endregion

#region State
        Type StateObjectType { get; }
        /// <summary>
        /// Export camera states to a serializable object, use this
        /// to save stuff that is related to camera connection
        /// </summary>
        /// <returns>
        /// A serializable object, can be null to indicate no state needs to be saved
        /// </returns>
        object ExportStateToSerializable();
        /// <summary>
        /// Load camera states from a serializable object, use this
        /// to load stuff that is related to camera connection
        /// </summary>
        /// <param name="stateObject">
        /// A serializable object
        /// </param>
        void LoadStateFromSerializable(object stateObject);
#endregion

#region Image Queue
        int BufferedImageCount { get; }
        int ImageQueueSize { get; set; }

        /// <summary>
        /// Removes all images from the processing queue.
        /// </summary>
        void ClearImageQueue();
#endregion

        /// <summary>
        /// 取像准备
        /// </summary>
        event CamReadyHandler OnCamAcqReady;
        /// <summary>
        /// 取像完成
        /// </summary>
        event CamCompleteHandler OnCamComplete;
        /// <summary>
        /// 相机掉线
        /// </summary>
        event CamConnectionChangedHandler OnCamConnectionChange;
        /// <summary>
        /// 实时
        /// </summary>
        event CamLiveHandler OnCamLive;
    }
}
