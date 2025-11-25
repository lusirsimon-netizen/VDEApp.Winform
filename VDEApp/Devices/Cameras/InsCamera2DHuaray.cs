// using Insnex.Vision2D.Core;
// using System;
// using System.Collections.Concurrent;
// using System.Collections.Generic;
// using System.Drawing;
// using System.Drawing.Imaging;
// using System.Linq;
// using System.Runtime.InteropServices;
// using System.Text;
// using System.Threading;
// using System.Threading.Tasks;
// using VDEApp.Devices.Datas;
// using VDEApp.Devices.Enums;
//
// namespace VDEApp.Devices.Cameras
// {
//     /// <summary>
//     /// 华睿
//     /// </summary>
//     public class InsCamera2DHuaray : ICamera
//     {
//         public int StitchingLines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//
//         public int BufferedImageCount => throw new NotImplementedException();
//
//         public int ImageQueueSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//         public string Guid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//         public int TimeOut { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//         public InsImageType ImageFormat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//         public InsCamAcqMode AcquisitionMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//         public InsCameraTriggerMode TriggerMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//
//         public bool IsConnected => throw new NotImplementedException();
//
//         public bool DisConnectedEnable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//         public double ExposureTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//         public int LineScanHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//         public int BatchSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//         public double Gain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//
//         public string SN => throw new NotImplementedException();
//
//         public string ModelName => throw new NotImplementedException();
//
//         public event CamReadyHandler OnCamAcqReady;
//         public event CamCompleteHandler OnCamComplete;
//         public event CamConnectionChangedHandler OnCamConnectionChange;
//         public event CamLiveHandler OnCamLive;
//
//         public List<AcquiredImageInfo> AcquireImageOnce(double timeoutInMs = 2147483647, double exposureTime = -1, Action onReadyAction = null)
//         {
//             throw new NotImplementedException();
//         }
//
//         public void ClearImageQueue()
//         {
//             throw new NotImplementedException();
//         }
//
//         public void CloseCamera()
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool ConnectCamera()
//         {
//             throw new NotImplementedException();
//         }
//
//         public void GetParams()
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool LoadBoardConfigFile(string configPath)
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool LoadCameraConfigFile(string configPath)
//         {
//             throw new NotImplementedException();
//         }
//
//         public void LoadSetting()
//         {
//             throw new NotImplementedException();
//         }
//
//         public void SaveSetting()
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool SetExposureTime(double timeInUs)
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool SetScanDirection(InsCameraScanDirection direction)
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool SetScanLines(int lines)
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool SetTriggerMode(InsCameraTriggerMode triggerMode)
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool StartAcquirImage()
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool StopAcquirImage()
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool SwitchProgramNo(int programNo)
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool TriggerModeOff()
//         {
//             throw new NotImplementedException();
//         }
//
//         public bool TriggerModeOn()
//         {
//             throw new NotImplementedException();
//         }
//     }
// }
