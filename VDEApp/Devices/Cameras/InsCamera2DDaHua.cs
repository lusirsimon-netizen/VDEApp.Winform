using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using Newtonsoft.Json;

using MVSDK_Net;

using VDEApp.Devices.Attributes;
using VDEApp.Devices.Datas;
using VDEApp.Devices.Enums;
using VDEApp.Utils.Exceptions;
using VDEApp.Utils.Memory;

using Insnex.Vision2D.Core;

namespace VDEApp.Devices.Cameras
{
    [Camera("DaHua Camera", typeof(VDEApp.Views.Devices.Cameras.UCDaHuaConfigure))]
    public class InsCamera2DDaHua : ICamera
    {
        private IMVDefine.IMV_DeviceInfo _device; // this is detailed info
        private DeviceInfo _briefInfo;
        private string _modelName;
        private MyCamera _camera = new MyCamera();

        private bool _isTriggerModeOn;
        private InsImageType _imageFormat = InsImageType.Grey8;
        private InsCameraTriggerMode _triggerMode;
        private bool _isConnected = false;
        private double _exposureTime;
        private double _gain;
        private int _batchSize = 1;

        private Thread _grabbingThread = null;
        private bool _isGrabbing = false;

        IMVDefine.IMV_FrameCallBack _frameCallback = null;
        IMVDefine.IMV_ConnectCallBack _connectCallback = null;

        public InsCamera2DDaHua()
        {
            _frameCallback = new IMVDefine.IMV_FrameCallBack(callbackImageGrabbed);
            _connectCallback = new IMVDefine.IMV_ConnectCallBack(callbackConnectionChanged);
        }

        private static Dictionary<int, string> getReturnCodeMap()
        {
            var ret = new Dictionary<int, string>();
            foreach (var field in typeof(IMVDefine).GetFields())
                ret.Add((int)field.GetValue(null), field.Name);

            // these return codes are not included in C# SDK, but can be found in C API
            ret.Add(-114, "IMV_RESTORE_STREAM");
            ret.Add(-115, "IMV_RECONNECT_DEVICE");
            ret.Add(-116, "IMV_NOT_AVAILABLE");
            ret.Add(-117, "IMV_NOT_GRABBING");
            ret.Add(-118, "IMV_NOT_CONNECTED");
            ret.Add(-119, "IMV_TIMEOUT");
            ret.Add(-120, "IMV_IS_CONNECTED");
            ret.Add(-121, "IMV_IS_GRABBING");
            ret.Add(-122, "IMV_INVOCATION_ERROR");
            ret.Add(-123, "IMV_SYSTEM_ERROR");
            ret.Add(-124, "IMV_OPENFILE_ERROR");

            return ret;
        }
        private static Dictionary<int, string> _ReturnCodeMap = getReturnCodeMap();

        public string Name { get; set; }
        public string Guid { get; set; }
        public string SN { get => _briefInfo?.SerialNumber; }
        public string ModelName { get => _briefInfo?.ModelName ?? "DaHua Camera"; }
        public InsCameraType CameraType { get => _briefInfo?.Connection.CameraType ?? InsCameraType.Camera2DAreaGige; }

        /// <summary>
        /// Basic device information
        /// </summary>
        public DeviceInfo Info => _briefInfo;
        public int StitchingLines { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut { get; set; }
        /// <summary>
        /// 图像格式
        /// </summary>
        public InsImageType ImageFormat
        {
            get => _imageFormat;
            set { }
        }
        /// <summary>
        /// 取像方式
        /// </summary>
        public InsCamAcqMode AcquisitionMode { get; set; }

        public bool IsTriggerModeOn
        {
            get => _isTriggerModeOn;
            set
            {
                _isTriggerModeOn = value;
                if (_isTriggerModeOn)
                    TriggerModeOn();
                else
                    TriggerModeOff();
            }
        }
        /// <summary>
        /// 触发方式
        /// </summary>
        public InsCameraTriggerMode TriggerMode
        {
            get => _triggerMode;
            set
            {
                SetTriggerMode(value);
            }
        }

        private readonly static List<InsCameraTriggerMode> _supportedTriggerModes = new List<InsCameraTriggerMode>() {
            InsCameraTriggerMode.Software,
            InsCameraTriggerMode.HardwareRisingEdge,
            InsCameraTriggerMode.HardwareFallingEdge,
            InsCameraTriggerMode.HardwareLevelHigh,
            InsCameraTriggerMode.HardwareLevelLow,
            InsCameraTriggerMode.HardwareAnyEdge
        };
        public List<InsCameraTriggerMode> SupportedTriggerModes => _supportedTriggerModes;

        private readonly static List<InsImageType> _supportedImageFormats = new List<InsImageType>() {
            InsImageType.Grey8
        };
        public List<InsImageType> SupportedImageFormats => _supportedImageFormats;

        /// <summary>
        /// 在线状态
        /// </summary>
        public bool IsConnected { get => _isConnected; }
        /// <summary>
        /// 相机掉线重连使能
        /// </summary>
        public bool DisConnectedEnable { get; set; }
        /// <summary>
        ///当前曝光时间
        /// </summary>
        public double ExposureTime { get => _exposureTime; set => SetExposureTime(value); }
        public int LineScanHeight { get; set; }
        public int BatchSize
        {
            get => _batchSize;
            set { _batchSize = Math.Min(Math.Max(1, value), 99); }
        }
        /// <summary>
        /// 增益
        /// </summary>
        public double Gain
        {
            get => _gain;
            set
            {
                // NOTE: some how this cannot set gain value into camera and there's no error....
                checkReturnCode<ConfigException>(_camera.IMV_SetDoubleFeatureValue("GainRaw", _gain));
            }
        }
        public void SetScanDirection(InsCameraScanDirection direction) => throw new NotImplementedException("DaHua");
        public void SetScanLines(int lines) => throw new NotImplementedException("DaHua");
        /// <summary>
        /// 设置曝光
        /// </summary>
        public void SetExposureTime(double timeInUs)
        {
            checkReturnCode<ConfigException>(_camera.IMV_SetDoubleFeatureValue("ExposureTime", timeInUs));
            _exposureTime = timeInUs;
        }
        /// <summary>
        /// 打开触发模式
        /// </summary>
        public void TriggerModeOn()
        {
            checkReturnCode<ConfigException>(_camera.IMV_SetEnumFeatureValue("TriggerMode", 1));
            _isTriggerModeOn = true;
        }
        /// <summary>
        /// 关闭触发模式
        /// </summary>
        public void TriggerModeOff()
        {
            checkReturnCode<ConfigException>(_camera.IMV_SetEnumFeatureValue("TriggerMode", 0));
            _isTriggerModeOn = false;
        }

        /// <summary>
        /// 设置触发模式
        /// </summary>
        /// <param name="triggerMode"></param>
        /// <returns></returns>
        public void SetTriggerMode(InsCameraTriggerMode mode)
        {
            Action<int> check_retcode = (int res) => checkReturnCode<ConfigException>(res);
            if (mode == InsCameraTriggerMode.Software)
            {
                check_retcode(_camera.IMV_SetEnumFeatureValue("TriggerSource", 0));
                _triggerMode = mode;
                return;
            }

            // TODO: following code is not strong exception guaranteed, parameters are set by several steps,
            // if one of them fails, the state is not restored to previous state, which may cause
            // inconsistant param state inside camera.
            // However, IMAF seems to be OK, so I'll leave it for now.
            if (_camera.IMV_SetEnumFeatureValue("TriggerSource", 2) != IMVDefine.IMV_OK)
                check_retcode(_camera.IMV_SetEnumFeatureValue("TriggerSource", 4));

            switch (mode)
            {
                case InsCameraTriggerMode.HardwareRisingEdge:
                    check_retcode(_camera.IMV_SetEnumFeatureValue("TriggerActivation", 0));
                    check_retcode(_camera.IMV_SetEnumFeatureValue("LineSelector", 3));
                    check_retcode(_camera.IMV_SetBoolFeatureValue("LineInverter", false));
                    break;
                case InsCameraTriggerMode.HardwareFallingEdge:
                    check_retcode(_camera.IMV_SetEnumFeatureValue("TriggerActivation", 1));
                    check_retcode(_camera.IMV_SetEnumFeatureValue("LineSelector", 3));
                    check_retcode(_camera.IMV_SetBoolFeatureValue("LineInverter", true));
                    break;
                case InsCameraTriggerMode.HardwareLevelHigh:
                    check_retcode(_camera.IMV_SetEnumFeatureValue("TriggerActivation", 2));
                    break;
                case InsCameraTriggerMode.HardwareLevelLow:
                    check_retcode(_camera.IMV_SetEnumFeatureValue("TriggerActivation", 3));
                    break;
                case InsCameraTriggerMode.HardwareAnyEdge:
                    check_retcode(_camera.IMV_SetEnumFeatureValue("TriggerActivation", 4));
                    break;
                default:
                    throw new ArgumentException("Unsupported trigger mode");
            }
            ;
            _triggerMode = mode;
        }

        public static object ByteToStruct(Byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
                return null;

            // 分配结构体内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);

            // 将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);

            // 将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);

            // 释放内存空间
            Marshal.FreeHGlobal(structPtr);

            return obj;
        }

        [CameraSearchMethod]
        public static List<DeviceInfo> SearchCameras()
        {
            var ret = new List<DeviceInfo>();
            var device_lst = new IMVDefine.IMV_DeviceList();
            try
            {
                checkReturnCode<OperationFailureException>(
                    MyCamera.IMV_EnumDevices(ref device_lst, (uint)IMVDefine.IMV_EInterfaceType.interfaceTypeAll)
                );
            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "DaHua SDK Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ret;
            }

            for (int i = 0; i < device_lst.nDevNum; i++)
            {
                var dev_info = (IMVDefine.IMV_DeviceInfo)Marshal.PtrToStructure(
                    device_lst.pDevInfo + Marshal.SizeOf(typeof(IMVDefine.IMV_DeviceInfo)) * i,
                    typeof(IMVDefine.IMV_DeviceInfo)
                );
                if (!(dev_info.manufactureInfo.Contains("Tianjin Microview") ||
                      dev_info.manufactureInfo == "Machine Vision" ||
                      dev_info.manufactureInfo == "Dahua Technology" ||
                      dev_info.manufactureInfo == "Huaray Technology"))
                    continue;
                var info = new DeviceInfo()
                {
                    Key = dev_info.cameraKey,
                    SerialNumber = dev_info.serialNumber,
                    ModelName = dev_info.modelName,
                    VendorName = dev_info.vendorName,
                    DeviceVersion = dev_info.deviceVersion,
                };

                if (dev_info.nCameraType == IMVDefine.IMV_ECameraType.typeGigeCamera)
                {
                    var gige_dev_info = (IMVDefine.IMV_GigEDeviceInfo)ByteToStruct(
                        dev_info.deviceSpecificInfo.gigeDeviceInfo, typeof(IMVDefine.IMV_GigEDeviceInfo)
                    );
                    info.Connection = new ConnectionConfig()
                    {
                        CameraType = InsCameraType.Camera2DAreaGige,
                        Value = gige_dev_info.ipAddress
                    };
                }
                else if (dev_info.nCameraType == IMVDefine.IMV_ECameraType.typeU3vCamera)
                {
                    info.Connection = new ConnectionConfig()
                    {
                        CameraType = InsCameraType.Camera2DAreaGige,
                        Value = dev_info.serialNumber
                    };
                }
                else
                { continue; } // other types is currently not supported
                ret.Add(info);
            }
            return ret;
        }

        /// <summary>
        /// connect callback
        /// </summary>
        private void callbackConnectionChanged(ref IMVDefine.IMV_SConnectArg connectArg, IntPtr pUser)
        {
            return;
            if (connectArg.EvType == IMVDefine.IMV_EVType.offLine)
            {
                _isConnected = false;
                //掉线事件
                OnCamConnectionChange?.Invoke(this, new CamConnectChangeEventArgs(false, Guid));

                _camera?.IMV_StopGrabbing();
            }
            else if (connectArg.EvType == IMVDefine.IMV_EVType.onLine)
            {
                _camera.IMV_Close();
                while (true)
                {
                    ConnectCamera();
                    if (_isConnected)
                    {
                        OnCamConnectionChange?.Invoke(this, new CamConnectChangeEventArgs(true, Guid));
                        break;
                    }
                    Thread.Sleep(5000);
                }
            }
        }
        /// <summary>
        /// connect callback
        /// </summary>
        private void callbackImageGrabbed(ref IMVDefine.IMV_Frame frame, IntPtr pUser)
        {
            // TODO
        }


        public void SetDeviceInfo(DeviceInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("DaHua: Device info cannot be null");
            _briefInfo = info;
        }

        /// <summary>
        /// 连接相机
        /// </summary>
        public void ConnectCamera()
        {
            if (_briefInfo == null)
                throw new OperationFailureException("DaHua: Can not connect to camera because device info is null");

            var mode = IMVDefine.IMV_ECreateHandleMode.modeByIPAddress;
            if ((_briefInfo.Connection.CameraType & InsCameraType.CameraGige) != 0)
                mode = IMVDefine.IMV_ECreateHandleMode.modeByIPAddress;
            else if ((_briefInfo.Connection.CameraType & InsCameraType.CameraUSB) != 0)
                mode = IMVDefine.IMV_ECreateHandleMode.modeByCameraKey;
            else
                throw new OperationFailureException(
                    $"DaHua: Unsupported camera type: expecting {InsCameraType.CameraGige.ToString()} or {InsCameraType.CameraUSB.ToString()}" +
                    $", got {_briefInfo.Connection.CameraType.ToString()}"
                );

            _isConnected = false;

            // a helper to check return code
            bool should_destroy_handle = false;
            Action fixer = () =>
            {
                if (should_destroy_handle)
                {
                    _camera.IMV_DestroyHandle();
                }
                OnCamConnectionChange?.Invoke(this, new CamConnectChangeEventArgs(_isConnected, Guid));
            };

            checkReturnCode<OperationFailureException>(_camera.IMV_CreateHandle(mode, 0, _briefInfo.Connection.Value), fixer);

            should_destroy_handle = true;
            checkReturnCode<OperationFailureException>(_camera.IMV_Open(), fixer);

            _isConnected = true;
            OnCamConnectionChange?.Invoke(this, new CamConnectChangeEventArgs(_isConnected, Guid));

            checkReturnCode<OperationFailureException>(_camera.IMV_SubscribeConnectArg(_connectCallback, IntPtr.Zero), fixer);

            checkReturnCode<OperationFailureException>(_camera.IMV_GetDeviceInfo(ref _device), fixer);
            _briefInfo.ModelName = _device.modelName;
            _briefInfo.SerialNumber = _device.serialNumber;
            _briefInfo.Key = _device.cameraKey;
            _briefInfo.VendorName = _device.vendorName;
            _briefInfo.DeviceVersion = _device.deviceVersion;

            _camera.IMV_SetBoolFeatureValue("GevGVCPHeartbeatDisable", false);
            _camera.IMV_SetIntFeatureValue("GevHeartbeatTimeout", 5000);

            syncParamsFromCamera();
            checkReturnCode<OperationFailureException>(_camera.IMV_AttachGrabbing(_frameCallback, IntPtr.Zero), fixer);
        }
        /// <summary>
        /// 关闭相机
        /// </summary>
        public void CloseCamera()
        {
            StopAcquireImage();
            _camera?.IMV_Close();
            _camera?.IMV_DestroyHandle();
            _isConnected = false;

            OnCamConnectionChange?.Invoke(this, new CamConnectChangeEventArgs(_isConnected, Guid));
        }

        public void StartAcquireImage()
        {
            if (_isGrabbing)
                return;
            checkReturnCode<OperationFailureException>(_camera.IMV_StartGrabbing());
            _isGrabbing = true;
        }
        public void StopAcquireImage()
        {
            _camera?.IMV_StopGrabbing();
            _isGrabbing = false;
        }

        /// <param name="timeoutInMs"></param>
        /// <param name="exposureTime"></param>
        /// <param name="onReadyAction"></param>
        /// <returns></returns>
        public List<AcquiredImageInfo> AcquireImageOnce(double timeoutInMs = int.MaxValue, double exposureTime = -1, Action onReadyAction = null)
        {
            if (_camera == null)
                throw new OperationFailureException("DaHua: Camera is not connected");

            // TODO: trigger mode is not supported for now
            this.TriggerModeOff();

            var frame = new IMVDefine.IMV_Frame();
            StartAcquireImage();
            var ret = new List<AcquiredImageInfo>();

            using (var stop_acquire_guard = new ScopeGuard(() => StopAcquireImage()))
            {
                for (int i = 0; i < BatchSize; i++)
                {
                    try
                    {
                        checkReturnCode<OperationFailureException>(_camera.IMV_GetFrame(ref frame, (uint)timeoutInMs));
                        ret.Add(readImageFromFrame(frame));
                    }
                    finally
                    {
                        _camera.IMV_ReleaseFrame(ref frame);
                    }
                }
            }

            return ret;
        }

        private AcquiredImageInfo readImageFromFrame(IMVDefine.IMV_Frame frame)
        {
            if (frame.frameInfo.pixelFormat == IMVDefine.IMV_EPixelType.gvspPixelMono8)
            {
                var image = new InsImage8Grey((int)frame.frameInfo.width, (int)frame.frameInfo.height);
                IInsImage8PixelMemory mem = image.Get8GreyPixelMemory(
                    InsImageDataModeConstants.ReadWrite,
                    0, 0,
                    image.Width, image.Height
                );
                using (var guard = new ScopeGuard(() => mem.Dispose()))
                {
                    IntPtr ImagePtr = mem.Scan0;
                    // if memory is aligned
                    if (image.Width == mem.Stride)
                        MemoryOperator.CopyMemory(mem.Scan0, frame.pData, image.Width * image.Height);
                    else
                        for (int i = 0; i < image.Height; i++)
                        {
                            MemoryOperator.CopyMemory(mem.Scan0 + mem.Stride * i, frame.pData + image.Width * i, image.Width);
                        }
                }
                return new AcquiredImageInfo() { Image = image, FrameID = (int)frame.frameInfo.blockId };
            }
            else
                throw new NotImplementedException("DaHua: Pixel type other than mono8 is not supported");
        }


        #region Old Config Interface
        /// <summary>
        /// 读取相机内部参数
        /// </summary>
        public void GetParams() => throw new NotImplementedException("DaHua");
        /// <summary>
        /// 写入参数到相机内部
        /// </summary>
        //void SetParams(InsCamera2DSetting setting);


        public double GainMax
        {
            get
            {
                double gain_max = 0.0;
                _camera.IMV_GetDoubleFeatureMax("GainRaw", ref gain_max);
                return gain_max;
            }
        }
        public double GainMin
        {
            get
            {
                double gain_min = 0.0;
                _camera.IMV_GetDoubleFeatureMin("GainRaw", ref gain_min);
                return gain_min;
            }
        }
        public double ExposureTimeMax
        {
            get
            {
                double max = 0.0;
                _camera.IMV_GetDoubleFeatureMax("ExposureTime", ref max);
                return max;
            }
        }
        public double ExposureTimeMin
        {
            get
            {
                double min = 0.0;
                _camera.IMV_GetDoubleFeatureMin("ExposureTime", ref min);
                return min;
            }
        }

        /// <summary>
        /// Read params from camera and update member variable
        /// </summary>
        void syncParamsFromCamera()
        {
            long value = 0;
            ulong uvalue = 0;

            Action<int> check_retcode = (int res) =>
            {
                checkReturnCode<ConfigException>(res);
            };

            check_retcode(_camera.IMV_GetIntFeatureValue("PayloadSize", ref value));
            check_retcode(_camera.IMV_GetIntFeatureValue("Height", ref value));
            check_retcode(_camera.IMV_GetIntFeatureValue("Width", ref value));

            check_retcode(_camera.IMV_GetEnumFeatureValue("AcquisitionMode", ref uvalue));
            check_retcode(_camera.IMV_GetEnumFeatureValue("TriggerMode", ref uvalue));
            _isTriggerModeOn = uvalue == 1;

            check_retcode(_camera.IMV_GetEnumFeatureValue("TriggerSource", ref uvalue));
            if (uvalue == 0)
            {
                _triggerMode = InsCameraTriggerMode.Software;
            }
            else
            {
                check_retcode(_camera.IMV_GetEnumFeatureValue("TriggerActivation", ref uvalue));
                switch (uvalue)
                {
                    case 0:
                        _triggerMode = InsCameraTriggerMode.HardwareRisingEdge;
                        break;
                    case 1:
                        _triggerMode = InsCameraTriggerMode.HardwareFallingEdge;
                        break;
                    case 2:
                        _triggerMode = InsCameraTriggerMode.HardwareLevelHigh;
                        break;
                    case 3:
                        _triggerMode = InsCameraTriggerMode.HardwareLevelLow;
                        break;
                    case 4:
                        _triggerMode = InsCameraTriggerMode.HardwareAnyEdge;
                        break;
                    default:
                        SetTriggerMode(InsCameraTriggerMode.Software);
                        break;
                }
            }

            check_retcode(_camera.IMV_GetEnumFeatureValue("PixelFormat", ref uvalue));

            check_retcode(_camera.IMV_GetDoubleFeatureValue("ExposureTime", ref _exposureTime));
            check_retcode(_camera.IMV_GetDoubleFeatureValue("GainRaw", ref _gain));
        }
        /// <summary>
        /// Write config related member variable to camera
        /// </summary>
        void syncParamsToCamera()
        {
        }

        public void LoadCameraConfigFile(string configPath) => throw new NotImplementedException("DaHua");
        public void LoadBoardConfigFile(string configPath) => throw new NotImplementedException("DaHua");

        /// <summary>
        /// 从本地文件读取配置
        /// </summary>
        public void LoadSetting() => throw new NotImplementedException("DaHua");
        /// <summary>
        /// 写入配置到本地文件
        /// </summary>
        public void SaveSetting() => throw new NotImplementedException("DaHua");
        #endregion

        #region Config
        /// <summary>
        /// Not sure what this is, probably used to switch config preset within camera.
        /// Such feature is supported by SSZN cameras, not sure if other camera has this.
        /// </summary>
        public void SwitchProgramNo(int programNo) => throw new NotImplementedException("DaHua");

        /// <summary>
        /// Config file extension, with dot. e.g. ".txt"
        /// </summary>
        public string ConfigFileExtension { get => ".mvcfg"; }
        /// <summary>
        /// Load config from file
        /// </summary>
        public void LoadConfig(string path)
        {
            _camera?.IMV_SaveDeviceCfg(path);
        }
        /// <summary>
        /// Export config to file
        /// </summary>
        public void ExportConfig(string path)
        {

        }
        #endregion

        #region State
        public Type StateObjectType => typeof(DeviceInfo);
        public object ExportStateToSerializable()
            => _briefInfo;
        public void LoadStateFromSerializable(object stateObject)
        {
            if (stateObject is DeviceInfo info)
                _briefInfo = info;
        }
        #endregion

        #region Image Queue
        public int BufferedImageCount { get; }
        public int ImageQueueSize { get; set; }

        /// <summary>
        /// Removes all images from the processing queue.
        /// </summary>
        public void ClearImageQueue() => throw new NotImplementedException("DaHua");
        #endregion

        /// <summary>
        /// 取像准备
        /// </summary>
        public event CamReadyHandler OnCamAcqReady;
        /// <summary>
        /// 取像完成
        /// </summary>
        public event CamCompleteHandler OnCamComplete;
        /// <summary>
        /// 相机掉线
        /// </summary>
        public event CamConnectionChangedHandler OnCamConnectionChange;
        /// <summary>
        /// 实时
        /// </summary>
        public event CamLiveHandler OnCamLive;

        private static void checkReturnCode<T>(int res, Action action = null) where T : Exception
        {
            if (res == IMVDefine.IMV_OK)
                return;

            action?.Invoke();
            var msg = $"IMV Unkown Error Code: {res}";
            if (_ReturnCodeMap.ContainsKey(res))
                msg = _ReturnCodeMap[res];
            throw Activator.CreateInstance(typeof(T), msg) as T;
        }

        [Serializable]
        public class ConnectionConfig
        {
            [JsonProperty("CameraType")]
            public InsCameraType CameraType;
            [JsonProperty("Value")]
            public string Value; // ip address    if CameraType & CameraGige
                                 // serial number if CameraType & CameraUSB
        }
        [Serializable]
        public class DeviceInfo
        {
            [JsonProperty("Connection")]
            public ConnectionConfig Connection;
            [JsonProperty("Key")]
            public string Key;
            [JsonProperty("VendorName")]
            public string VendorName;
            [JsonProperty("ModelName")]
            public string ModelName;
            [JsonProperty("SerialNumber")]
            public string SerialNumber;
            [JsonProperty("DeviceVersion")]
            public string DeviceVersion;
        }
    }
}
