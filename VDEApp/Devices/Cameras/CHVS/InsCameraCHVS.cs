using InsCHVSControl;
using Insnex.Utility;
using Insnex.Vision2D.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using VDEApp.Devices.Datas;
using VDEApp.Devices.Enums;
using VDEApp.Utils.Memory;
using InsCamAcqMode = VDEApp.Devices.Enums.InsCamAcqMode;
using InsImageType = VDEApp.Devices.Enums.InsImageType;

namespace VDEApp.Devices
{
    //public class InsCameraCHVS : ICamera, IDisposable
    //{
    //    #region 私有属性
    //    private const uint PRMTimeOut = 5000;
    //    public AutoResetEvent AcquireEvent = new AutoResetEvent(false);
    //    private const int _imageSliceMaxSize = 60000;
    //    private int _imageSliceSize;
    //    private int _imageSliceCount = 1;
    //    private int _batchSize = 1;
    //    public int BatchSize { get { return _batchSize; } set { _batchSize = value; } }
    //    private static string _errMessage = "";
    //    private string _name = "";
    //    private string _ip = "";
    //    private string _serialNumber = "";
    //    private int _timeOut = 1000;
    //    private int _bufferCount = 16;
    //    private InsDeviceType _deviceType = InsDeviceType.CameraCHVS;
    //    private double _exposureTime; //曝光时间
    //    private double _gain; //增益
    //    private int _CameraID;
    //    private bool _isTriggerOn;
    //    private bool _isConnected = false;
    //    private bool _disconnectedEnable = false;
    //    private bool _acqCompleted = false;
    //    private bool _isLive = false;
    //    private string _mManufacturerName = "";

    //    private object _synObjBuffer = new object();
    //    private ConcurrentQueue<List<AcquiredImageInfo>> _imageList = new ConcurrentQueue<List<AcquiredImageInfo>>();
    //    private int frameID = 0;
    //    //停止取像标记位
    //    private bool m_bStopAcq = false;
    //    private int frameCounter = 0;
    //    public event CamReadyHandler OnCamAcqReady;
    //    public event CamLiveHandler OnCamLive;
    //    public event CamCompleteHandler OnCamComplete;
    //    public event CamConnectionChangedHandler OnCamConnectionChange;

    //    private uint g_nPayloadSize = 0;
    //    private uint nWidth = 0, nHeight = 0;

    //    private bool m_bGrabbing = false;
    //    private Thread m_hReceiveThread = null;

    //    private Mutex m_mutex = new Mutex(); //互斥锁


    //    //public InsCHVS_DeviceInfoList deviceInfoListScanned = new InsCHVS_DeviceInfoList();  //设备信息列表
    //    InsCHVS_DeviceInfo deviceInfoOpened = new InsCHVS_DeviceInfo();
    //    static IntPtr CamHandle = IntPtr.Zero;//相机句柄

    //    IntPtr periodMax = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)));  //数值指针
    //    uint _imageHeight;  //图像高度
    //    uint _lightSourceMode;  //光源模式 
    //    InsCHVS_LineTriggerSource _triggerSource1;  //触发源1
    //    InsCHVS_LineTriggerSource _triggerSource2;  //触发源2
    //    float _maxExposureTime;
    //    InsResult ret = InsResult.INS_OK;
    //    public const uint SAVE_IMAGE_COUNT = 5;  // 保存图片数量
    //    int _index;
    //    public IInsImage Current_Iamge;

    //    //GCHandle dataProcessHandle1;
    //    //GCHandle dataProcessHandle2;
    //    //GCHandle dataProcessHandle3;
    //    //GCHandle dataProcessHandle4;
    //    //GCHandle dataProcessHandle5;
    //    //GCHandle dataProcessHandle6;

    //    #endregion
    //    public InsCameraCHVS()
    //    { }
    //    public InsCameraCHVS(string guid)
    //    {
    //        Guid = guid;
    //    }
    //    #region 公开属性
    //    //拼接的
    //    private int _stitchingLines = 0;
    //    public int StitchingLines
    //    {
    //        get
    //        {
    //            return _stitchingLines;
    //        }
    //        set
    //        {
    //            _stitchingLines = value;
    //        }
    //    }
    //    // 获取或设置相机的ID
    //    public int CameraID
    //    {
    //        get { return _CameraID; }
    //        set { _CameraID = value; }
    //    }
    //    public int BufferedImageCount
    //    {
    //        get
    //        {
    //            return _imageList.Count;
    //        }
    //    }
    //    private int _imageQueueSize = 80;
    //    public int ImageQueueSize
    //    {
    //        get
    //        {
    //            return this._imageQueueSize;
    //        }
    //        set
    //        {
    //            this._imageQueueSize = value;
    //        }
    //    }
    //    // 获取或设置相机的IP地址
    //    public string Ip
    //    {
    //        get { return _ip; }
    //        set { _ip = value; }
    //    }

    //    // 获取或设置相机的序列号
    //    public string SerialNumber
    //    {
    //        get { return _serialNumber; }
    //        set { _serialNumber = value; }
    //    }

    //    // 获取或设置超时时间
    //    public int TimeOut
    //    {
    //        get { return _timeOut; }
    //        set { _timeOut = value; }
    //    }



    //    // 获取相机是否已连接
    //    public bool IsConnected
    //    {
    //        get { return _isConnected; }
    //    }


    //    // 获取或设置触发状态
    //    public bool IsTriggerOn
    //    {
    //        get { return _isTriggerOn; }
    //        set { _isTriggerOn = value; }
    //    }



    //    // 获取或设置断开连接的启用状态
    //    public bool DisConnectedEnable
    //    {
    //        get { return _disconnectedEnable; }
    //        set { _disconnectedEnable = value; }
    //    }

    //    // 获取图像列表
    //    public ConcurrentQueue<List<AcquiredImageInfo>> ImageList { get { return _imageList; } }

    //    // 获取或设置缓冲区数量
    //    public int BufferCount
    //    {
    //        get { return _bufferCount; }
    //        set { _bufferCount = value; }
    //    }

    //    // 获取或设置相机名称
    //    public string Name
    //    {
    //        get { return _name; }
    //        set { _name = value; }
    //    }

    //    // 获取或设置GUID
    //    public string Guid { get; set; }

    //    // 获取或设置当前曝光时间
    //    public double ExposureTime
    //    {
    //        get { return _exposureTime; }
    //        set { _exposureTime = value; }
    //    }


    //    // 获取或设置增益值
    //    public double Gain
    //    {
    //        get { return _gain; }
    //        set { _gain = value; }
    //    }

    //    // 获取或设置错误信息
    //    public string ErrMessage
    //    {
    //        get { return _errMessage; }
    //        set { _errMessage = value; }
    //    }


    //    float _lineFreq;

    //    // 获取或设置行频
    //    public float LineFreq
    //    {
    //        get
    //        {
    //            InsCHVSCamera.InsCHVS_Get_Acq_Intern_TrigPeriod_NET(CamHandle, out _lineFreq, PRMTimeOut);
    //            return _lineFreq;
    //        }
    //        set
    //        {
    //            ret = InsCHVSCamera.InsCHVS_Set_Acq_Intern_TrigPeriod_NET(CamHandle, value, PRMTimeOut);
    //            if (ret == InsResult.INS_OK) _lineFreq = value;
    //        }
    //    }

    //    uint _linenum;

    //    // 获取或设置行数
    //    public uint LineNum
    //    {
    //        get
    //        {
    //            InsCHVSCamera.InsCHVS_Get_Acq_Intern_TrigNums_NET(CamHandle, out _linenum, PRMTimeOut);
    //            return _linenum;
    //        }
    //        set
    //        {
    //            ret = InsCHVSCamera.InsCHVS_Set_Acq_Intern_TrigNums_NET(CamHandle, value, PRMTimeOut);
    //            if (ret == InsResult.INS_OK)
    //            {
    //                _linenum = value;
    //            }
    //        }
    //    }

    //    // 获取或设置LED模式
    //    public InsCHVS_LED_TriggerMode LED_Mode { get; set; }

    //    // 获取或设置配置文件路径
    //    public string ConfigFilePath { get; set; }
    //    InsImageType ICamera.ImageFormat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    //    private InsCamAcqMode _acquisitionMode = InsCamAcqMode.SingleFrame;
    //    public InsCamAcqMode AcquisitionMode
    //    {
    //        get
    //        {
    //            return _acquisitionMode;
    //        }
    //        set
    //        {
    //            if (_acquisitionMode != value)
    //            {
    //                //切换模式

    //                _acquisitionMode = value;
    //            }
    //        }
    //    }
    //    InsCameraTriggerMode ICamera.TriggerMode { get; set; }


    //    int ICamera.LineScanHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    //    #endregion

    //    #region 方法
    //    // 设置曝光时间

    //    public void LoadICF(string filename)
    //    {
    //        if (CamHandle == IntPtr.Zero)
    //            return;
    //        InsResult ret = InsCHVSCamera.InsCHVS_Load_ConfigFile_NET(CamHandle, filename, PRMTimeOut);
    //        ConfigFilePath = filename;
    //    }

    //    public bool SetExposureTime(double value)
    //    {

    //        // 获取最大曝光时间
    //        InsCHVSCamera.InsCHVS_Get_TimeLine_Max_NET(CamHandle, out _maxExposureTime, PRMTimeOut);
    //        // 用户自定义曝光值
    //        List<float> UserControl_Exposure = new List<float> { (float)value, 0, 0 };
    //        UserControl_Exposure[0] = Math.Min(15f, _maxExposureTime);
    //        UserControl_Exposure[1] = Math.Min(10f, _maxExposureTime);
    //        UserControl_Exposure[2] = Math.Min(5f, _maxExposureTime);

    //        // 根据LED模式设置曝光
    //        switch (LED_Mode)
    //        {
    //            case InsCHVS_LED_TriggerMode.LED_SimultaneousDualBrightness:
    //            case InsCHVS_LED_TriggerMode.LED_BacklightOnly:
    //                ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime1_NET(CamHandle, UserControl_Exposure[0], PRMTimeOut);
    //                break;
    //            case InsCHVS_LED_TriggerMode.LED_SeparateTimedFlashingBrightness:
    //                //ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime2_NET(CamHandle, UserControl_Exposure[0], UserControl_Exposure[1], (uint)TimeOut);
    //                //ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_STROBING_MODE, (int)InsCHVSCamera.INS_VAL_BUFFER_STROBING_MULTIPLE);
    //                break;
    //            case InsCHVS_LED_TriggerMode.LED_TripleIndependentBrightness:
    //                //ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime3_NET(CamHandle, UserControl_Exposure[0], UserControl_Exposure[1], UserControl_Exposure[2], (uint)TimeOut);
    //                //ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_STROBING_MODE, (int)InsCHVSCamera.INS_VAL_BUFFER_STROBING_MULTIPLE);
    //                break;
    //            default:
    //                break;
    //        }

    //        if (ret == InsResult.INS_OK)
    //        {
    //            return true;
    //        }
    //        else return false;
    //    }

    //    public bool GetExposureTime()
    //    {
    //        float value;
    //        // 根据LED模式设置曝光
    //        switch (LED_Mode)
    //        {
    //            case InsCHVS_LED_TriggerMode.LED_SimultaneousDualBrightness:
    //            case InsCHVS_LED_TriggerMode.LED_BacklightOnly:
    //                ret = InsCHVSCamera.InsCHVS_Get_LED_ExposureTime1_NET(CamHandle, out value, PRMTimeOut);
    //                break;
    //            case InsCHVS_LED_TriggerMode.LED_SeparateTimedFlashingBrightness:
    //                //ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime2_NET(CamHandle, UserControl_Exposure[0], UserControl_Exposure[1], (uint)TimeOut);
    //                //ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_STROBING_MODE, (int)InsCHVSCamera.INS_VAL_BUFFER_STROBING_MULTIPLE);
    //                break;
    //            case InsCHVS_LED_TriggerMode.LED_TripleIndependentBrightness:
    //                //ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime3_NET(CamHandle, UserControl_Exposure[0], UserControl_Exposure[1], UserControl_Exposure[2], (uint)TimeOut);
    //                //ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_STROBING_MODE, (int)InsCHVSCamera.INS_VAL_BUFFER_STROBING_MULTIPLE);
    //                break;
    //            default:
    //                break;
    //        }

    //        if (ret == InsResult.INS_OK)
    //        {
    //            return true;
    //        }
    //        else return false;
    //    }

    //    public double GetGainValue()
    //    {
    //        double value;
    //        ret = InsCHVSCamera.InsCHVS_Get_Global_User_DGain_NET(CamHandle, out value, PRMTimeOut);

    //        return value;

    //        //else return false;
    //    }

    //    // 设置增益
    //    public bool SetGainValue(double gainValue)
    //    {
    //        ret = InsCHVSCamera.InsCHVS_Set_Global_User_DGain_NET(CamHandle, gainValue, PRMTimeOut);
    //        if (ret == InsResult.INS_OK)
    //        {
    //            return true;
    //        }
    //        else return false;
    //    }

    //    // 设置行频
    //    public bool SetLineFreq(float value)
    //    {
    //        ret = InsCHVSCamera.InsCHVS_Set_Acq_Intern_TrigPeriod_NET(CamHandle, value, PRMTimeOut);
    //        if (ret == InsResult.INS_OK)
    //        {
    //            _lineFreq = value;
    //            return true;
    //        }
    //        else return false;
    //    }

    //    // 设置行数
    //    public bool SetLineNum(uint value)
    //    {
    //        ret = InsCHVSCamera.InsCHVS_Set_Acq_Intern_TrigNums_NET(CamHandle, value, PRMTimeOut);
    //        if (ret == InsResult.INS_OK)
    //        {
    //            return true;
    //        }
    //        else return false;
    //    }

    //    // 设置LED模式
    //    public bool SetLedMode(InsCHVS_LED_TriggerMode value)
    //    {
    //        InsCHVS_RunState state;
    //        InsCHVSCamera.InsCHVS_Get_RunState_NET(CamHandle, out state);

    //        ret = InsCHVSCamera.InsCHVS_Set_LED_TriggerMode_NET(CamHandle, value, PRMTimeOut);

    //        if (ret == InsResult.INS_OK)
    //        {
    //            LED_Mode = value;
    //            return true;
    //        }
    //        else return false;
    //    }

    //    public void LoadSetting()
    //    {
    //        //InsfwDeviceConfiguration.Default.Load();
    //    }
    //    public void SaveSetting()
    //    {
    //        //InsfwDeviceConfiguration.Default.Save();
    //    }
    //    /// <summary>
    //    /// 设置曝光时间
    //    /// </summary> 
    //    bool ICamera.SetExposureTime(double timeInUs)
    //    {
    //        InsCameraSetting cameraSetting = InsfwDeviceConfiguration.Default.GetCameraSetting(Guid);
    //        // 获取最大曝光时间
    //        InsCHVSCamera.InsCHVS_Get_TimeLine_Max_NET(CamHandle, out _maxExposureTime, PRMTimeOut);

    //        // 用户自定义曝光值
    //        List<float> UserControl_Exposure = new List<float> { (float)timeInUs, 0, 0 };
    //        UserControl_Exposure[0] = Math.Min(UserControl_Exposure[0], _maxExposureTime);
    //        UserControl_Exposure[1] = Math.Min(10f, _maxExposureTime);
    //        UserControl_Exposure[2] = Math.Min(5f, _maxExposureTime);

    //        // 根据LED模式设置曝光
    //        switch (LED_Mode)
    //        {
    //            case InsCHVS_LED_TriggerMode.LED_SimultaneousDualBrightness:
    //            case InsCHVS_LED_TriggerMode.LED_BacklightOnly:
    //                ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime1_NET(CamHandle, UserControl_Exposure[0], PRMTimeOut);
    //                break;
    //            case InsCHVS_LED_TriggerMode.LED_SeparateTimedFlashingBrightness:
    //                //ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime2_NET(CamHandle, UserControl_Exposure[0], UserControl_Exposure[1], (uint)TimeOut);
    //                //ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_STROBING_MODE, (int)InsCHVSCamera.INS_VAL_BUFFER_STROBING_MULTIPLE);
    //                break;
    //            case InsCHVS_LED_TriggerMode.LED_TripleIndependentBrightness:
    //                //ret = InsCHVSCamera.InsCHVS_Set_LED_ExposureTime3_NET(CamHandle, UserControl_Exposure[0], UserControl_Exposure[1], UserControl_Exposure[2], (uint)TimeOut);
    //                //ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_STROBING_MODE, (int)InsCHVSCamera.INS_VAL_BUFFER_STROBING_MULTIPLE);
    //                break;
    //            default:
    //                break;
    //        }

    //        if (ret == InsResult.INS_OK)
    //        {
    //            //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Info, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name} {InsfwLangs.TR(1215, "设置曝光")}{timeInUs}us {InsfwLangs.TR(1216, "完成")}!");
    //            return true;
    //        }
    //        else
    //        {
    //            //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Error, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name} {InsfwLangs.TR(1215, "设置曝光")}{timeInUs}us {InsfwLangs.TR(1058, "失败")}!");
    //            return false;
    //        }
    //    }

    //    bool ICamera.TriggerModeOn()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    bool ICamera.TriggerModeOff()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    bool ICamera.SetTriggerMode(InsCameraTriggerMode triggerMode)
    //    {
    //        InsResult ret;
    //        if (triggerMode == InsCameraTriggerMode.FreeRun || triggerMode == InsCameraTriggerMode.Software || triggerMode == InsCameraTriggerMode.Internal)
    //            ret = InsCHVSCamera.InsCHVS_Set_Acq_TrigSource_NET(CamHandle, InsCHVS_LineTriggerSource.Internal_Clock, PRMTimeOut);
    //        else if (triggerMode == InsCameraTriggerMode.Encoder)
    //            ret = InsCHVSCamera.InsCHVS_Set_Acq_TrigSource_NET(CamHandle, InsCHVS_LineTriggerSource.External_Encoder, PRMTimeOut);
    //        else
    //            ret = InsCHVSCamera.InsCHVS_Set_Acq_TrigSource_NET(CamHandle, InsCHVS_LineTriggerSource.External_IO, PRMTimeOut);
    //        return ret == InsResult.INS_OK ? true : false;
    //    }

    //    void ICamera.GetParams()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    bool ICamera.SetScanDirection(InsCameraScanDirection direction)
    //    {
    //        if (direction == InsCameraScanDirection.Foward)
    //        {
    //            InsResult res = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_COLOR_DISPERSION_DIRECTION, 1);
    //            return (res == InsResult.INS_OK) ? true : false;
    //        }
    //        else
    //        {
    //            InsResult res = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_COLOR_DISPERSION_DIRECTION, 0);
    //            return (res == InsResult.INS_OK) ? true : false;
    //        }
    //        return true;

    //    }

    //    bool ICamera.SwitchProgramNo(int programNo)
    //    {
    //        if (InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).CameraType != InsCameraType.Camera3DLine &&
    //            InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).CameraType != InsCameraType.Camera3DArea)
    //            throw new NotImplementedException();
    //        else
    //            throw new NotImplementedException();
    //    }
    //    static bool CheckIKapBoard(int ret)
    //    {
    //        if (ret != (int)IKapBoardClassLibrary.ErrorCode.IK_RTN_OK)
    //        {
    //            return false;

    //        }
    //        return true;
    //    }
    //    bool ICamera.SetScanLines(int lines)
    //    {
    //        //流开启过程中无法设置下列参数
    //        try
    //        {
    //            //判断是否需要切分图像
    //            //如果大于60000行则切分取图然后拼接
    //            int _imageSliceSizeTemp;
    //            int _imageSliceCountTemp;
    //            if (lines > _imageSliceMaxSize)
    //            {
    //                _imageSliceCountTemp = lines / _imageSliceMaxSize + 1;
    //                _imageSliceSizeTemp = (int)(lines * 1.0 / _imageSliceCountTemp);
    //            }
    //            else
    //            {
    //                _imageSliceCountTemp = 1;
    //                _imageSliceSizeTemp = lines;
    //            }
    //            if (_imageSliceSizeTemp != _imageSliceSize)
    //            {
    //                _imageSliceSize = _imageSliceSizeTemp;
    //                //如果是连续模式，关闭流设置参数
    //                if (_acquisitionMode == InsCamAcqMode.Continuous)
    //                    StopAcquirImage();

    //                InsResult ret = InsCHVSCamera.InsCHVS_Set_Img_TransHeight_NET(CamHandle, (uint)_imageSliceSize, PRMTimeOut);
    //                ret = InsCHVSCamera.InsCHVS_Set_Acq_BlendEnd_TriggerNum_NET(CamHandle, (uint)_imageSliceSize, PRMTimeOut);
    //                if (ret != InsResult.INS_OK)
    //                    return false;

    //                if (_acquisitionMode == InsCamAcqMode.Continuous)
    //                    StartAcquirImage();
    //            }
    //            ////相机高度实际可能会小于设置的高度
    //            //long realHeight = 0;
    //            //res = IKapCLib.ItkDevGetInt64(m_hCamera, "Height", ref realHeight);
    //            //if (CheckIKapC(res))
    //            //{
    //            //    _imageSliceSize = (int)realHeight;
    //            //}
    //            if (_imageSliceCountTemp != _imageSliceCount)
    //            {
    //                // 设置图像缓冲区帧数。
    //                // Set frame count of buffer.
    //                _imageSliceCount = _imageSliceCountTemp;
    //                InsCameraSetting cameraSetting = InsfwDeviceConfiguration.Default.GetCameraSetting(Guid);

    //                //如果是连续模式，关闭流设置参数
    //                if (_acquisitionMode == InsCamAcqMode.Continuous)
    //                    StopAcquirImage();
    //                ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_COUNT_CAMERA, _imageSliceCount);
    //                if (ret != InsResult.INS_OK)
    //                {
    //                    //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Warn, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name} {InsfwLangs.TR(1217, "设置图像缓冲区帧数为")}{_imageSliceCount}{InsfwLangs.TR(1058, "失败")}！");
    //                    return false;
    //                }
    //                else
    //                {
    //                    //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Info, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name} {InsfwLangs.TR(1217, "设置图像缓冲区帧数为")}{_imageSliceCount}！");
    //                }
    //                if (_acquisitionMode == InsCamAcqMode.Continuous)
    //                    StartAcquirImage();
    //            }

    //            return true;
    //        }
    //        catch
    //        {
    //            //MessageManager.GetSingleInstance.Alarm(_serialNumber + ":修改高度失败", AlarmMode.Unknown);
    //            return false;
    //        }
    //    }
    //    #region 判断型号
    //    static int extractDPI(InsCHVS_DeviceType type)
    //    {
    //        return ((int)type & 0xF0) >> 4;
    //    }

    //    // 判断DPI是否等于1800或900
    //    static bool is_DPI_EqualTo_1800_900(InsCHVS_DeviceType type)
    //    {
    //        return extractDPI(type) == (int)InsCHVS_DPI_MODULE.DPI900 || extractDPI(type) == (int)InsCHVS_DPI_MODULE.DPI1800 || extractDPI(type) == (int)InsCHVS_DPI_MODULE.DPI3600;
    //    }

    //    static bool isLXMSeriesCamera;

    //    #endregion

    //    #region 内部回调函数
    //    public static void StartCallBack(IntPtr handle, IntPtr pInfo, IntPtr pBuffer, IntPtr pUser)
    //    {
    //        Console.WriteLine("************* START ************* ");
    //    }

    //    // 帧准备完毕回调
    //    public void FrameReadyCallBack(IntPtr handle, IntPtr pInfo, IntPtr pBuffer, IntPtr pUser)
    //    {
    //        IntPtr camera = handle;
    //        InsCHVS_ProcessInfo info = Marshal.PtrToStructure<InsCHVS_ProcessInfo>(pInfo);
    //        InsCHVS_Buffer buffer = Marshal.PtrToStructure<InsCHVS_Buffer>(pBuffer);
    //        if (buffer.effectiveHeight < buffer.height * 0.8)
    //        {
    //            return;
    //        }
    //        // Effective light source flicker count
    //        var (lightCount, result) = InsCHVSCamera.InsCHVS_Get_DevPrm<Int32>(camera, (int)InsCHVSCamera.INS_PRM_LIGHT_COUNT);
    //        int separate = lightCount;
    //        var srcData = new List<IntPtr> { buffer.p_data, buffer.p_data2, buffer.p_data3, buffer.p_data4 };
    //        int current_light_height = (Int32)buffer.height / (isLXMSeriesCamera ? 1 : separate);
    //        int retImage = 0;
    //        List<AcquiredImageInfo> images = new List<AcquiredImageInfo>();
    //        for (int lightNumber = 0; lightNumber < srcData.Count; lightNumber++)
    //        {

    //            if (srcData[lightNumber] == IntPtr.Zero)
    //                continue;
    //            if (buffer.image_type == InsCHVS_PixelFormat.Mono8)
    //            {
    //                AcquiredImageInfo imageInfo = new AcquiredImageInfo
    //                {
    //                    StartEncoderValue = info.EncoderLocationStart,
    //                    EndEncoderValue = info.EncoderLocationEnd,
    //                    EncoderEnable = true,
    //                };
    //                InsImage8Grey image = new InsImage8Grey((int)buffer.width, current_light_height);

    //                IInsImage8PixelMemory mem = image.Get8GreyPixelMemory(InsImageDataModeConstants.Write, 0, 0, image.Width, image.Height);

    //                IntPtr ImagePtr = mem.Scan0;
    //                //如果内存对齐
    //                if (image.Width == mem.Stride)
    //                {
    //                    MemoryOperator.CopyMemory(mem.Scan0, srcData[lightNumber], image.Width * image.Height);
    //                }
    //                else
    //                {
    //                    for (int i = 0; i < image.Height; i++)
    //                    {
    //                        MemoryOperator.CopyMemory(mem.Scan0 + mem.Stride * i, srcData[lightNumber] + image.Width * i, image.Width);
    //                    }
    //                }
    //                mem.Dispose();
    //                imageInfo.Image = image;
    //                images.Add(imageInfo);
    //                //_imageList.Enqueue(image);
    //                retImage++;
    //            }
    //            else if (buffer.image_type == InsCHVS_PixelFormat.RGB888)
    //            {
    //                AcquiredImageInfo imageInfo = new AcquiredImageInfo
    //                {
    //                    StartEncoderValue = info.EncoderLocationStart,
    //                    EndEncoderValue = info.EncoderLocationEnd,
    //                    EncoderEnable = true,
    //                };
    //                Bitmap bmp = new Bitmap((int)buffer.width, current_light_height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
    //                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
    //                BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);


    //                IntPtr ImagePtr = bmpData.Scan0;
    //                //如果内存对齐
    //                if (bmp.Width * 3 == bmpData.Stride)
    //                {
    //                    MemoryOperator.CopyMemory(bmpData.Scan0, srcData[lightNumber], bmp.Width * bmp.Height * 3);
    //                }
    //                else
    //                {
    //                    for (int i = 0; i < bmp.Height; i++)
    //                    {
    //                        MemoryOperator.CopyMemory(bmpData.Scan0 + bmpData.Stride * i, srcData[lightNumber] + bmp.Width * 3 * i, bmp.Width * 3);
    //                    }
    //                }
    //                bmp.UnlockBits(bmpData);

    //                InsImage24PlanarColor image = new InsImage24PlanarColor(bmp);
    //                imageInfo.Image = image;
    //                images.Add(imageInfo);
    //                //_imageList.Enqueue(image);
    //                retImage++;
    //            }
    //        }
    //        if (retImage > 0)
    //        {
    //            if (_imageList.Count < _bufferCount)
    //            {
    //                if (_imageList.Count >= _imageQueueSize)
    //                    _imageList.TryDequeue(out var result1);
    //                _imageList.Enqueue(images);
    //                frameCounter++;
    //            }
    //        }
    //        //满足帧数之后 取图完成
    //        if (frameCounter >= _imageSliceCount)
    //        {
    //            //StopAcquirImage();
    //            if (OnCamComplete != null)
    //            {
    //                AcquiredEventArgs acqEventArgs = new AcquiredEventArgs();
    //                OnCamComplete(this, acqEventArgs);
    //            }
    //        }
    //    }

    //    // 停止回调
    //    public static void StopCallBack(IntPtr handle, IntPtr pInfo, IntPtr pBuffer, IntPtr pUser)
    //    {
    //        Console.WriteLine("************* STOP ************** ");
    //    }

    //    // 帧丢失回调
    //    public static void FrameLostCallBack(IntPtr handle, IntPtr pInfo, IntPtr pBuffer, IntPtr pUser)
    //    {
    //        Console.WriteLine("************* FrameLost ********* ");
    //    }

    //    // 超时回调
    //    public static void TimeOutCallBack(IntPtr handle, IntPtr pInfo, IntPtr pBuffer, IntPtr pUser)
    //    {
    //        Console.WriteLine("************* TimeOut *********** ");
    //    }

    //    // 相机移除回调
    //    public static void CameraRemoveCallback(IntPtr pUser)
    //    {
    //        IntPtr cameraHandle = pUser;

    //        InsCHVS_RunState runState;
    //        InsCHVSCamera.InsCHVS_Get_RunState_NET(cameraHandle, out runState);
    //        if (runState == InsCHVS_RunState.Running)
    //        {
    //            InsCHVSCamera.InsCHVS_Cmd_Stop_NET(cameraHandle);
    //        }
    //        InsCHVSCamera.InsCHVS_Cmd_Close_NET(cameraHandle);
    //    }

    //    #endregion

    //    // 开始采集图像
    //    public bool StartAcquirImage()
    //    {
    //        //清空队列
    //        ClearImageQueue();
    //        // 开始获取图像
    //        m_bStopAcq = false;
    //        ret = InsCHVSCamera.InsCHVS_Cmd_Start_NET(CamHandle);
    //        if (ret != InsResult.INS_OK)
    //            return false;
    //        else
    //            return true;
    //    }

    //    // 停止采集图像
    //    public bool StopAcquirImage()
    //    {
    //        m_bStopAcq = true;
    //        ret = InsCHVSCamera.InsCHVS_Cmd_Stop_NET(CamHandle);
    //        if (ret != InsResult.INS_OK)
    //            return false;
    //        ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_CLEAR, 1); // 清除内部缓冲区
    //        if (ret != InsResult.INS_OK)
    //            return false;
    //        return true;
    //    }

    //    [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
    //    private static extern bool SetDllDirectory(string lpPathName);

    //    // 连接相机
    //    public bool ConnectCamera()
    //    {
    //        // 测试DLL环境是否正确
    //        if (!CheckDll())
    //        {
    //            _isConnected = false;
    //            if (OnCamConnectionChange != null)
    //                OnCamConnectionChange(this, new CamConnectChangeEventArgs(_isConnected, Guid));
    //            InsfwDeviceManager.Instance.NotifyCameraStatusChanged();
    //            return false;
    //        }
    //        InsCameraSetting cameraSetting = InsfwDeviceConfiguration.Default.GetCameraSetting(Guid);
    //        // SDK版本
    //        StringBuilder version = new StringBuilder();
    //        InsCHVSCamera.InsCHVS_SoftWare_Version_NET(version, 20);

    //        // 初始化采集卡
    //        InsCHVSCamera.InsCHVS_Initialize_NET();

    //        // 本地变量
    //        InsCHVS_DeviceInfoList deviceInfoListScanned = new InsCHVS_DeviceInfoList();
    //        InsCHVS_DeviceInfo deviceInfoOpened = new InsCHVS_DeviceInfo();
    //        CamHandle = IntPtr.Zero;
    //        InsCHVSCamera.InsCHVS_CreateHandle_NET(out CamHandle);
    //        InsCHVSCamera.InsCHVS_FindDevice_NET(out deviceInfoListScanned);
    //        int UserControl_ChooseIndex = GetDeviceIDBySN(deviceInfoListScanned, cameraSetting.IPSN);
    //        if (UserControl_ChooseIndex < 0)
    //        {
    //            _isConnected = false;
    //            if (OnCamConnectionChange != null)
    //                OnCamConnectionChange(this, new CamConnectChangeEventArgs(_isConnected, Guid));
    //            InsfwDeviceManager.GInstance.NotifyCameraStatusChanged();
    //            //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Error, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name} {InsfwLangs.TR(1218, "查找相机失败")}...");
    //            return false;
    //        }
    //        ret = InsCHVSCamera.InsCHVS_Cmd_Open_NET(CamHandle, UserControl_ChooseIndex);
    //        if (ret != InsResult.INS_OK)
    //        {
    //            _isConnected = false;
    //            if (OnCamConnectionChange != null)
    //                OnCamConnectionChange(this, new CamConnectChangeEventArgs(_isConnected, Guid));
    //            InsfwDeviceManager.GInstance.NotifyCameraStatusChanged();
    //            //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Error, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name} {InsfwLangs.TR(1219, "打开相机失败")}...");
    //            return false;
    //        }

    //        InsCHVSCamera.InsCHVS_Get_DevInfo_NET(CamHandle, out deviceInfoOpened);
    //        //加载配置文件
    //        string cameraFileFullPath = InsfwGlobalStore.Default.EnsureFilePath(cameraSetting.CameraConfigFilePath);
    //        ret = InsCHVSCamera.InsCHVS_Load_ConfigFile_NET(CamHandle, cameraFileFullPath, PRMTimeOut); // Set as needed
    //        if (ret != InsResult.INS_OK)
    //        {
    //            _isConnected = false;
    //            if (OnCamConnectionChange != null)
    //                OnCamConnectionChange(this, new CamConnectChangeEventArgs(_isConnected, Guid));
    //            InsfwDeviceManager.GInstance.NotifyCameraStatusChanged();
    //            //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Error, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name} {InsfwLangs.TR(1220, "加载配置文件失败")}...");
    //            return false;
    //        }

    //        //获取触发参数
    //        InsCHVS_LineTriggerSource triggerSource;
    //        ret = InsCHVSCamera.InsCHVS_Get_Acq_TrigSource_NET(CamHandle, out triggerSource, PRMTimeOut);
    //        if (ret == InsResult.INS_OK)
    //        {
    //            if (triggerSource == InsCHVS_LineTriggerSource.External_Encoder)
    //            {
    //                InsfwDeviceManager.GInstance.Cameras[Guid].TriggerMode = InsCameraTriggerMode.Encoder; 
    //            }
    //            else if (triggerSource == InsCHVS_LineTriggerSource.Internal_Clock)
    //            {
    //                InsfwDeviceManager.GInstance.Cameras[Guid].TriggerMode = InsCameraTriggerMode.Internal;

    //            }
    //            else if (triggerSource == InsCHVS_LineTriggerSource.External_IO)
    //            {
    //                InsfwDeviceManager.GInstance.Cameras[Guid].TriggerMode = InsCameraTriggerMode.HardwareAnyEdge;

    //            }
    //        }
             
    //        //注册回调函数
    //        RunHookFnPtr MyCallback1 = StartCallBack;
    //        RunHookFnPtr MyCallback2 = FrameReadyCallBack;
    //        RunHookFnPtr MyCallback3 = StopCallBack;
    //        RunHookFnPtr MyCallback4 = FrameLostCallBack;
    //        RunHookFnPtr MyCallback5 = TimeOutCallBack;
    //        RemoveHookFnPtr MyCallback6 = CameraRemoveCallback;
    //        GCHandle dataProcessHandle1 = GCHandle.Alloc(MyCallback1);
    //        GCHandle dataProcessHandle2 = GCHandle.Alloc(MyCallback2);
    //        GCHandle dataProcessHandle3 = GCHandle.Alloc(MyCallback3);
    //        GCHandle dataProcessHandle4 = GCHandle.Alloc(MyCallback4);
    //        GCHandle dataProcessHandle5 = GCHandle.Alloc(MyCallback5);
    //        GCHandle dataProcessHandle6 = GCHandle.Alloc(MyCallback6);
    //        InsCHVSCamera.InsCHVS_RegisterCallback_NET(CamHandle, InsCHVSCamera.INS_Event_GrabStart, MyCallback1, IntPtr.Zero);
    //        ret = InsCHVSCamera.InsCHVS_RegisterCallback_NET(CamHandle, InsCHVSCamera.INS_Event_FrameReady, MyCallback2, IntPtr.Zero);
    //        if (ret != InsResult.INS_OK)
    //        {
    //            _isConnected = false;
    //            if (OnCamConnectionChange != null)
    //                OnCamConnectionChange(this, new CamConnectChangeEventArgs(_isConnected, Guid));
    //            InsfwDeviceManager.GInstance.NotifyCameraStatusChanged();
    //            //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Error, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name}{InsfwLangs.TR(1221, "注册取图回调函数失败")}...");
    //            return false;
    //        }
    //        InsCHVSCamera.InsCHVS_RegisterCallback_NET(CamHandle, InsCHVSCamera.INS_Event_GrabStop, MyCallback3, IntPtr.Zero);
    //        InsCHVSCamera.InsCHVS_RegisterCallback_NET(CamHandle, InsCHVSCamera.INS_Event_FrameLost, MyCallback4, IntPtr.Zero);
    //        InsCHVSCamera.InsCHVS_RegisterCallback_NET(CamHandle, InsCHVSCamera.INS_Event_TimeOut, MyCallback5, IntPtr.Zero);
    //        InsCHVSCamera.InsCHVS_RegisterCallback_DeviceRemove_NET(CamHandle, MyCallback6, CamHandle);


    //        //// 接收图像缓存区
    //        //System.IntPtr bufferHeightPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32))); ;
    //        //System.IntPtr bufferImageWidthPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32))); ;
    //        //InsCHVSCamera.InsCHVS_Get_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_ROI_HEIGHT, bufferHeightPtr);
    //        //InsCHVSCamera.InsCHVS_Get_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_ROI_IMG_WIDTH, bufferImageWidthPtr);

    //        //// 获取图像大小
    //        //int bufferHeight = Marshal.ReadInt32(bufferHeightPtr);
    //        //int bufferImageWidth = Marshal.ReadInt32(bufferImageWidthPtr);
    //        uint height;
    //        ret = InsCHVSCamera.InsCHVS_Get_Img_TransHeight_NET(CamHandle, out height, PRMTimeOut);
    //        if (ret == InsResult.INS_OK)
    //        {
    //            _imageSliceSize = (int)height;
    //        }
    //        System.IntPtr frameBufferCountPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32))); ;
    //        ret = InsCHVSCamera.InsCHVS_Get_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_COUNT_CAMERA, frameBufferCountPtr);
    //        _imageSliceCount = Marshal.ReadInt32(frameBufferCountPtr);

    //        isLXMSeriesCamera = is_DPI_EqualTo_1800_900(deviceInfoOpened.InsType);

    //        InsCHVS_LED_TriggerMode _LedMode;
    //        //
    //        /* Set light source mode */
    //        if (InsCHVSCamera.InsCHVS_Get_LED_TriggerMode_NET(CamHandle, out _LedMode, PRMTimeOut) == InsResult.INS_OK)
    //        {
    //            LED_Mode = _LedMode;
    //        }

    //        _isConnected = true;
    //        if (OnCamConnectionChange != null)
    //            OnCamConnectionChange(this, new CamConnectChangeEventArgs(_isConnected, Guid));
    //        InsfwDeviceManager.GInstance.NotifyCameraStatusChanged();

    //        //_sn = deviceInfoOpened.SpecialInfo.CamLInfo[0].SerialNumber;
    //        //_modelName= _sn = deviceInfoOpened.SpecialInfo.CamLInfo[0].ModelName;
    //        return true;
    //        //InsCHVSCamera.InsCHVS_Get_DevInfo_NET(CamHandle, out deviceInfoOpened);

    //    }
    //    public int GetDeviceIDBySN(InsCHVS_DeviceInfoList devicesInfo, string sn)
    //    {
    //        int ret = -1;
    //        Regex regex = new Regex(@"[0-9]");
    //        if (regex.IsMatch(sn))
    //        {
    //            int id = Convert.ToInt32(sn);
    //            if (id < devicesInfo.DeviceCount)
    //            {
    //                if (devicesInfo.DeviceInfo[id].TransLayerType == InsCHVSCamera.INS_GIGE_DEVICE)
    //                {
    //                    _sn = devicesInfo.DeviceInfo[id].SpecialInfo.GigeInfo[0].SN;
    //                    _modelName = devicesInfo.DeviceInfo[id].InsType.ToString();
    //                }
    //                else if (devicesInfo.DeviceInfo[id].TransLayerType == InsCHVSCamera.INS_CAMERALINK_DEVICE)
    //                {
    //                    _sn = devicesInfo.DeviceInfo[id].SpecialInfo.CamLInfo[0].SN;
    //                    _modelName = devicesInfo.DeviceInfo[id].InsType.ToString();
    //                }
    //                return id;
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < devicesInfo.DeviceCount; i++)
    //            {

    //                if (devicesInfo.DeviceInfo[i].TransLayerType == InsCHVSCamera.INS_GIGE_DEVICE)
    //                {
    //                    for (int j = 0; j < devicesInfo.DeviceInfo[i].SpecialInfo.GigeInfo.Length; j++)
    //                    {
    //                        if (devicesInfo.DeviceInfo[i].SpecialInfo.GigeInfo[j].SN.Trim() == sn.Trim())
    //                        {
    //                            _sn = devicesInfo.DeviceInfo[i].SpecialInfo.GigeInfo[j].SN;
    //                            _modelName = devicesInfo.DeviceInfo[i].InsType.ToString();
    //                            return i;
    //                        }
    //                    }
    //                }

    //                else if (devicesInfo.DeviceInfo[i].TransLayerType == InsCHVSCamera.INS_CAMERALINK_DEVICE)
    //                {

    //                    for (int j = 0; j < devicesInfo.DeviceInfo[i].SpecialInfo.CamLInfo.Length; j++)
    //                    {
    //                        if (devicesInfo.DeviceInfo[i].SpecialInfo.CamLInfo[j].SN.Trim() == sn.Trim())
    //                        {
    //                            _sn = devicesInfo.DeviceInfo[i].SpecialInfo.CamLInfo[j].SN;
    //                            _modelName = devicesInfo.DeviceInfo[i].InsType.ToString();
    //                            return i;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        return ret;
    //    }
    //    // 读取相机内部参数
    //    public void GetParams() { }


    //    // 停止相机
    //    public void StopCamera()
    //    {
    //        if (CamHandle == IntPtr.Zero)
    //            return;
    //        ret = InsCHVSCamera.InsCHVS_Cmd_Stop_NET(CamHandle);
    //        ret = InsCHVSCamera.InsCHVS_Set_DevPrm_NET(CamHandle, (int)InsCHVSCamera.INS_PRM_BUFFER_CLEAR, 1); // 清除内部缓冲区
    //    }

    //    // 关闭相机
    //    public void CloseCamera()
    //    {
    //        if (CamHandle == IntPtr.Zero)
    //            return;

    //        //if (dataProcessHandle1 == null || dataProcessHandle2 == null || dataProcessHandle3 == null || dataProcessHandle4 == null || dataProcessHandle5 == null || dataProcessHandle6 == null)
    //        //{
    //        //    return;
    //        //}

    //        StopCamera();

    //        // 回收回调函数的对象
    //        //dataProcessHandle1.Free();
    //        //dataProcessHandle2.Free();
    //        //dataProcessHandle3.Free();
    //        //dataProcessHandle4.Free();
    //        //dataProcessHandle5.Free();
    //        //dataProcessHandle6.Free();

    //        ret = InsCHVSCamera.InsCHVS_Cmd_Close_NET(CamHandle);

    //        InsCHVSCamera.InsCHVS_DestroyHandle_NET(CamHandle);
    //        CamHandle = IntPtr.Zero;
    //        _isConnected = false;
    //    }

    //    public bool CheckDll()
    //    {
    //        try
    //        {
    //            // 添加 DLL 文件夹的加载路径
    //            if (!SetDllDirectory(@"Assemblies\Cameras\Insnex"))
    //            { 
    //                // 相对路径
    //                //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Error, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name}{InsfwLangs.TR(1222, "SDK运行dll缺失")}");
    //                return false;
    //            }
    //            InsCHVSControlWrapper.Check();

    //        }
    //        catch (Exception ex)
    //        {
    //            //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Error, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name}{InsfwLangs.TR(1222, "SDK运行dll缺失")}");
    //            return false;
    //        }
    //        return true;
    //    }

    //    public void InitFrameGrabber()
    //    {
    //        InsCHVSCamera.InsCHVS_Initialize_NET();
    //    }

    //    public void TerminateFrameGrabber()
    //    {
    //        InsCHVSCamera.InsCHVS_Terminate_NET();
    //    }
    //    public object acqLock = new object();
    //    bool acquireComplete = false;
    //    public void SetTimeout(int timeoutMs)
    //    {
    //        InsCameraSetting cameraSetting = InsfwDeviceConfiguration.Default.GetCameraSetting(Guid);
    //        try
    //        {
    //            InsCHVSCamera.InsCHVS_Set_Img_CombTimeout_NET(CamHandle, (uint)(timeoutMs * 1000), PRMTimeOut);
    //        }
    //        catch { }
    //    }

    //    public void ClearImageQueue()
    //    {
    //        frameID = 0;
    //        //清空图片队列
    //        while (_imageList.Count > 0)
    //        {
    //            if (_imageList.TryDequeue(out List<AcquiredImageInfo> images))
    //            {
    //                images?.Clear();
    //            }
    //        }

    //        //重置拼接图片列表
    //        stitchingImageList?.Clear();
    //        stitchingImageList = null;

    //    }
    //    private List<AcquiredImageInfo> stitchingImageList = null;
    //    public List<AcquiredImageInfo> AcquireImageOnce(double timeoutInMs = int.MaxValue, double exposureTime = -1, Action onReadyAction = null)
    //    {
    //        Monitor.Enter(this);
    //        frameCounter = 0;
    //        //清空图像队列
    //        if (AcquisitionMode != InsCamAcqMode.Continuous)
    //        {
    //            SetTimeout((int)timeoutInMs);
    //            StartAcquirImage();
    //        }
    //        // 设置曝光 
    //        if (exposureTime > 0)
    //            SetExposureTime(exposureTime);
    //        //取像准备事件
    //        if (OnCamAcqReady != null)
    //        {
    //            OnCamAcqReady(this);
    //        }
    //        onReadyAction?.Invoke();

    //        DateTime t1 = DateTime.Now;
    //        TimeSpan timeoutSpan;
    //        if (timeoutInMs > int.MaxValue)
    //        {
    //            timeoutSpan = new TimeSpan(0, 0, 0, 0, int.MaxValue);
    //        }
    //        else
    //        {
    //            timeoutSpan = new TimeSpan(0, 0, 0, 0, (int)timeoutInMs);
    //        }
    //        while (_imageList.Count < _batchSize * _imageSliceCount)
    //        {
    //            if (m_bStopAcq)
    //                break;
    //            DateTime t2 = DateTime.Now;
    //            if (t2 - t1 > timeoutSpan)
    //            {
    //                //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Warn, InsfwLangs.TR(1060, "相机:") + InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name + InsfwLangs.TR(1061, "取图超时"));
    //                break;
    //            }
    //            Thread.Yield();
    //        }
    //        if (AcquisitionMode != InsCamAcqMode.Continuous)
    //            StopAcquirImage();


    //        List<AcquiredImageInfo> retImageList = new List<AcquiredImageInfo>();
    //        if (_imageList.Count >= _batchSize * _imageSliceCount)
    //        {
    //            lock (_synObjBuffer)
    //            {
    //                if (_imageList.Count() == 1)
    //                {
    //                    if (_imageList.TryDequeue(out List<AcquiredImageInfo> imageList))
    //                        retImageList.AddRange(imageList);
    //                }
    //                //拼图
    //                else if (_imageList.Count() > 1)
    //                {
    //                    Stopwatch sw = new Stopwatch();
    //                    sw.Start();
    //                    List<List<AcquiredImageInfo>> imageGroup = new List<List<AcquiredImageInfo>>();
    //                    for (int i = 0; i < _imageSliceCount; i++)
    //                    {
    //                        if (_imageList.TryDequeue(out List<AcquiredImageInfo> imageList))
    //                            imageGroup.Add(imageList);
    //                    }
    //                    for (int j = 0; j < imageGroup[0].Count; j++)
    //                    {
    //                        //拼图的总高度
    //                        if (imageGroup[0][j].Image is InsImage24PlanarColor)
    //                        {
    //                            AcquiredImageInfo imageInfo = new AcquiredImageInfo
    //                            {
    //                                StartEncoderValue = imageGroup[0][j].StartEncoderValue,
    //                                EndEncoderValue = imageGroup[0][j].EndEncoderValue,
    //                                FrameID = frameID++,
    //                                EncoderEnable = true
    //                            };
    //                            InsImage24PlanarColor image = new InsImage24PlanarColor(imageGroup[0][j].Image.Width, imageGroup[0][j].Image.Height * _imageSliceCount);
    //                            IInsImage8PixelMemory mem1;
    //                            IInsImage8PixelMemory mem2;
    //                            IInsImage8PixelMemory mem3;
    //                            image.Get24PlanarColorPixelMemorySubOptimal(InsImageDataModeConstants.ReadWrite, 0, 0, image.Width, image.Height, out mem1, out mem2, out mem3);

    //                            IntPtr ptr1 = mem1.Scan0;
    //                            IntPtr ptr2 = mem2.Scan0;
    //                            IntPtr ptr3 = mem3.Scan0;


    //                            for (int i = 0; i < _imageSliceCount; i++)
    //                            {
    //                                IInsImage insImage = imageGroup[i][j].Image;
    //                                IInsImage8PixelMemory tmem1;
    //                                IInsImage8PixelMemory tmem2;
    //                                IInsImage8PixelMemory tmem3;
    //                                (insImage as InsImage24PlanarColor).Get24PlanarColorPixelMemorySubOptimal(InsImageDataModeConstants.ReadWrite, 0, 0, insImage.Width, insImage.Height, out tmem1, out tmem2, out tmem3);


    //                                MemoryOperator.CopyMemory(ptr1, tmem1.Scan0, tmem1.Stride * tmem1.Height);
    //                                ptr1 += tmem1.Stride * tmem1.Height;
    //                                MemoryOperator.CopyMemory(ptr2, tmem2.Scan0, tmem2.Stride * tmem2.Height);
    //                                ptr2 += tmem2.Stride * tmem2.Height;
    //                                MemoryOperator.CopyMemory(ptr3, tmem3.Scan0, tmem3.Stride * tmem3.Height);
    //                                ptr3 += tmem3.Stride * tmem3.Height;
    //                                tmem1.Dispose();
    //                                tmem2.Dispose();
    //                                tmem3.Dispose();
    //                            }

    //                            mem1.Dispose();
    //                            mem2.Dispose();
    //                            mem3.Dispose();
    //                            imageInfo.Image = image;
    //                            retImageList.Add(imageInfo);
    //                        }
    //                        else if (imageGroup[0][j].Image is InsImage8Grey)
    //                        {
    //                            AcquiredImageInfo imageInfo = new AcquiredImageInfo
    //                            {
    //                                StartEncoderValue = imageGroup[0][j].StartEncoderValue,
    //                                EndEncoderValue = imageGroup[0][j].EndEncoderValue,
    //                                FrameID = frameID++,
    //                                EncoderEnable = true
    //                            };
    //                            InsImage8Grey image = new InsImage8Grey(imageGroup[0][j].Image.Width, imageGroup[0][j].Image.Height * _imageSliceCount);
    //                            IInsImage8PixelMemory mem1 = image.Get8GreyPixelMemory(InsImageDataModeConstants.ReadWrite, 0, 0, image.Width, image.Height);

    //                            IntPtr ptr1 = mem1.Scan0;

    //                            for (int i = 0; i < _imageSliceCount; i++)
    //                            {
    //                                IInsImage insImage = imageGroup[i][j].Image;
    //                                IInsImage8PixelMemory tmem1 = (insImage as InsImage8Grey).Get8GreyPixelMemory(InsImageDataModeConstants.ReadWrite, 0, 0, insImage.Width, insImage.Height);
    //                                MemoryOperator.CopyMemory(ptr1, tmem1.Scan0, tmem1.Stride * tmem1.Height);
    //                                ptr1 += tmem1.Stride * tmem1.Height;
    //                                tmem1.Dispose();
    //                            }
    //                            mem1.Dispose();
    //                            imageInfo.Image = image;
    //                            retImageList.Add(imageInfo);
    //                        }
    //                    }
    //                    sw.Stop();
    //                    InsCameraSetting cameraSetting = InsfwDeviceConfiguration.Default.GetCameraSetting(Guid);
    //                    //InsfwGlobalLogger.CameraLog(InsfwGlobalLogger.Level.Info, $"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name} {_imageSliceCount}{InsfwLangs.TR(1223, "合1拼图耗时")}{sw.ElapsedMilliseconds}ms");
    //                }
    //            }
    //        }
    //        else
    //        {
    //            InsCameraSetting setting = InsfwDeviceConfiguration.Default.GetCameraSetting(Guid);
    //            //InsfwGlobalLogger.CameraError($"{InsfwLangs.TR(1060, "相机:")}{InsfwDeviceConfiguration.Default.GetCameraSetting(Guid).Name}{InsfwLangs.TR(1224, "取图超时,拼接前图像数量")}{_imageList.Count}");

    //        }

    //        List<AcquiredImageInfo> stitched;
    //        //如果需要拼接
    //        if (_stitchingLines > 0)
    //        {
    //            stitched = new List<AcquiredImageInfo>();
    //            for (int i = 0; i < retImageList.Count; i++)
    //            {
    //                if (stitchingImageList.Count > i)
    //                {
    //                    //开始拼接
    //                    AcquiredImageInfo stitchingImageInfo = stitchingImageList[i];
    //                    AcquiredImageInfo retImageInfo = retImageList[i];

    //                    if (stitchingImageInfo.Image is InsImage8Grey && retImageInfo.Image is InsImage8Grey && stitchingImageInfo.Image.Width == retImageInfo.Image.Width)
    //                    {

    //                        InsImage8Grey stitchingImage = stitchingImageInfo.Image as InsImage8Grey;
    //                        InsImage8Grey retImage = retImageInfo.Image as InsImage8Grey;
    //                        int tempStitchingLines = _stitchingLines;
    //                        if (stitchingImage.Height < _stitchingLines)
    //                        {
    //                            tempStitchingLines = stitchingImage.Height;
    //                        }
    //                        IInsImage8PixelMemory memPrevious = stitchingImage.Get8GreyPixelMemory(InsImageDataModeConstants.Read, 0, stitchingImage.Height - tempStitchingLines, stitchingImage.Width, tempStitchingLines);
    //                        IInsImage8PixelMemory mem = retImage.Get8GreyPixelMemory(InsImageDataModeConstants.Read, 0, 0, retImage.Width, retImage.Height);
    //                        InsImage8Grey stitchedImage = new InsImage8Grey(retImage.Width, retImage.Height + tempStitchingLines);

    //                        AcquiredImageInfo stitchedImageInfo = new AcquiredImageInfo(retImageInfo);
    //                        IInsImage8PixelMemory memStitched = stitchedImage.Get8GreyPixelMemory(InsImageDataModeConstants.ReadWrite, 0, 0, stitchedImage.Width, stitchedImage.Height);
    //                        CopyMemory(memStitched.Scan0, memPrevious.Scan0, memStitched.Stride * tempStitchingLines);
    //                        CopyMemory(memStitched.Scan0 + memStitched.Stride * tempStitchingLines, mem.Scan0, memStitched.Stride * retImage.Height);
    //                        stitchedImageInfo.Image = stitchedImage;
    //                        if (stitchedImageInfo.EncoderEnable)
    //                        {
    //                            stitchedImageInfo.StartEncoderValue = stitchingImageInfo.EndEncoderValue - (stitchingImageInfo.EndEncoderValue - stitchingImageInfo.StartEncoderValue) / stitchingImage.Height * tempStitchingLines;
    //                        }
    //                        stitched.Add(stitchedImageInfo);
    //                    }
    //                    else if (stitchingImageInfo.Image is InsImage24PlanarColor && retImageInfo.Image is InsImage24PlanarColor && stitchingImageInfo.Image.Width == retImageInfo.Image.Width)
    //                    {

    //                        InsImage24PlanarColor stitchingImage = stitchingImageInfo.Image as InsImage24PlanarColor;
    //                        InsImage24PlanarColor retImage = retImageInfo.Image as InsImage24PlanarColor;
    //                        int tempStitchingLines = _stitchingLines;
    //                        if (stitchingImage.Height < _stitchingLines)
    //                        {
    //                            tempStitchingLines = stitchingImage.Height;
    //                        }
    //                        IInsImage24PlanarColorPixelMemory memPrevious = stitchingImage.Get24PlanarColorPixelMemory(InsImageDataModeConstants.Read, 0, stitchingImage.Height - tempStitchingLines, stitchingImage.Width, tempStitchingLines);
    //                        IInsImage24PlanarColorPixelMemory mem = retImage.Get24PlanarColorPixelMemory(InsImageDataModeConstants.Read, 0, 0, retImage.Width, retImage.Height);
    //                        InsImage24PlanarColor stitchedImage = new InsImage24PlanarColor(retImage.Width, retImage.Height + tempStitchingLines);

    //                        AcquiredImageInfo stitchedImageInfo = new AcquiredImageInfo(retImageInfo);
    //                        IInsImage24PlanarColorPixelMemory memStitched = stitchedImage.Get24PlanarColorPixelMemory(InsImageDataModeConstants.ReadWrite, 0, 0, stitchedImage.Width, stitchedImage.Height);
    //                        CopyMemory(memStitched.Scan0, memPrevious.Scan0, memStitched.Stride * tempStitchingLines);
    //                        CopyMemory(memStitched.Scan0 + memStitched.Stride * tempStitchingLines, mem.Scan0, memStitched.Stride * retImage.Height);
    //                        stitchedImageInfo.Image = stitchedImage;
    //                        if (stitchedImageInfo.EncoderEnable)
    //                        {
    //                            stitchedImageInfo.StartEncoderValue = stitchingImageInfo.EndEncoderValue - (stitchingImageInfo.EndEncoderValue - stitchingImageInfo.StartEncoderValue) / stitchingImage.Height * tempStitchingLines;
    //                        }
    //                        stitched.Add(stitchedImageInfo);
    //                    }
    //                    else
    //                    {
    //                        stitched.Add(retImageInfo);
    //                    }
    //                }

    //            }
    //        }
    //        else
    //        {
    //            stitched = retImageList;
    //        }
    //        //保存供下次拼接使用
    //        stitchingImageList = stitched;
    //        Monitor.Exit(this);

    //        if (OnCamComplete != null)
    //        {
    //            AcquiredEventArgs acqEventArgs = new AcquiredEventArgs();
    //            acqEventArgs.ImageList.AddRange(stitched);
    //            OnCamComplete(this, acqEventArgs);
    //        }
    //        return stitched;
    //    }
    //    [DllImport("kernel32.dll")]
    //    public static extern void CopyMemory(IntPtr Destination, IntPtr Source, int Length);
    //    // 实时图像
    //    public void LiveImage()
    //    {
    //    }
    //    #endregion

    //    void IDisposable.Dispose()
    //    {
    //        try
    //        {
    //            // 回收回调函数的对象
    //            //dataProcessHandle1.Free();
    //            //dataProcessHandle2.Free();
    //            //dataProcessHandle3.Free();
    //            //dataProcessHandle4.Free();
    //            //dataProcessHandle5.Free();
    //            //dataProcessHandle6.Free(); 
    //            ret = InsCHVSCamera.InsCHVS_Cmd_Close_NET(CamHandle);
    //            InsCHVSCamera.InsCHVS_DestroyHandle_NET(CamHandle);
    //        }
    //        catch (Exception) { }
    //    }

    //    private string _sn = "";
    //    public string SN { get { return _sn; } }
    //    private string _modelName = "";
    //    public string ModelName { get { return _modelName; } }
    //    public bool LoadCameraConfigFile(string configPath)
    //    {
    //        ret = InsCHVSCamera.InsCHVS_Load_ConfigFile_NET(CamHandle, configPath, PRMTimeOut); // Set as needed
    //        if (ret == InsResult.INS_OK)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //    public bool LoadBoardConfigFile(string configPath)
    //    {
    //        throw new NotImplementedException();
    //    } 
    //}
}