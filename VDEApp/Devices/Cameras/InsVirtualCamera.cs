using System;
using System.IO;
using System.Collections.Generic;
using VDEApp.Devices.Datas;
using VDEApp.Devices.Enums;
using VDEApp.Devices.Attributes;
using VDEApp.Utils.Exceptions;

using Insnex.ImageFile;
using Insnex.Vision2D.Core;

namespace VDEApp.Devices.Cameras
{
    internal class ImageFileTool
    {
        private InsImageFile _imgFile;
        private string _path;
        public string Path{
            get => _path;
            set {
                _path = value;
                Reset();
            }
        }

        public int Count { get => _imgFilePaths.Count; }
        public List<string> ImageFilePaths { get => _imgFilePaths; }

        private List<string> _imgFilePaths;
        private int imgIdx;
        private int subImgIdx;

        public ImageFileTool()
        {
            _imgFile = new InsImageFile();
            _path = null;

            _imgFilePaths = new List<string>();
            imgIdx = 0;
            subImgIdx = 0;
        }

        public IInsImage ReadNext()
        {
            if (_imgFilePaths.Count == 0)
                return null;

            subImgIdx++;
            if (subImgIdx >= _imgFile.Count)
            {
                subImgIdx = 0;
                imgIdx = (imgIdx + 1) % _imgFilePaths.Count;

                _imgFile.Open(_imgFilePaths[imgIdx], InsImageFileModeConstants.Read);
            }

            return _imgFile[subImgIdx];
        }

        public void Reset()
        {
            _imgFilePaths.Clear();
            imgIdx = 0;
            subImgIdx = 0;

            var attr = File.GetAttributes(_path);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                foreach (var img_path in Directory.GetFiles(_path))
                {
                    if (!File.Exists(img_path)) continue;

                    var ext = System.IO.Path.GetExtension(img_path).Remove(0, 1);

                    if (_imgFile.SupportedFileTypes.Contains(ext))
                        _imgFilePaths.Add(img_path);
                }
                return;
            }

            var ext_ = System.IO.Path.GetExtension(_path).Remove(0, 1);
            if (!_imgFile.SupportedFileTypes.Contains(ext_))
                return;

            _imgFilePaths.Add(_path);
        }

    }

    [Camera("Insnex Virtual Camera", typeof(VDEApp.Views.Devices.Cameras.UCVirtualConfigure))]
    public class InsVirtualCamera : ICamera
    {
        private ImageFileTool _imgFile;
        private int _batchSize;

        public InsVirtualCamera()
        {
            _imgFile = new ImageFileTool();
            _batchSize = 1;
        }
        public string Name{ get; set; }
        public string Guid { get; set; }
        public string SN => "Virtual";
        public string ModelName => "Insnex Virtual Camera";
        public InsCameraType CameraType => InsCameraType.CameraVirtual;

        public int BufferedImageCount => 0;
        public int ImageQueueSize { get => 0; set {} }

        public InsImageType ImageFormat {
            get => InsImageType.Grey8;
            set { throw new InvalidOperationException("Virtual camera can not set image format"); }
        }
        public InsCamAcqMode AcquisitionMode {
            get => InsCamAcqMode.SingleFrame;
            set { throw new InvalidOperationException("Virtual camera can not set acquisition mode"); }
        }
        public InsCameraTriggerMode TriggerMode {
            get => InsCameraTriggerMode.Software;
            set { throw new InvalidOperationException("Virtual camera can not set trigger mode"); }
        }

        public bool IsConnected => true;
        public bool DisConnectedEnable {
            get => throw new InvalidOperationException("Virtual camera does not support disconnect enabled");
            set => throw new InvalidOperationException("Virtual camera does not support disconnect enabled");
        }

        public int TimeOut { get; set; }
        public int StitchingLines {
            get => throw new InvalidOperationException("Virtual camera can not set stitching lines");
            set => throw new InvalidOperationException("Virtual camera can not set stitching lines");
        }
        public double ExposureTime { get; set; }
        public int LineScanHeight {
            get => throw new InvalidOperationException("Virtual camera can not set line scan height");
            set => throw new InvalidOperationException("Virtual camera can not set line scan height");
        }
        public double Gain {
            get => throw new InvalidOperationException("Virtual camera can not set gain");
            set => throw new InvalidOperationException("Virtual camera can not set gain");
        }
        public int BatchSize {
            get => _batchSize;
            set { _batchSize = Math.Min(Math.Max(1, value), 99); }
        }

        public event CamReadyHandler OnCamAcqReady;
        public event CamCompleteHandler OnCamComplete;
        public event CamConnectionChangedHandler OnCamConnectionChange;
        public event CamLiveHandler OnCamLive;

        public string Path {
            get => _imgFile.Path;
            set => _imgFile.Path = value;
        }

        /// <exception cref="OperationFailureException">
        /// Throws when acquire operation failed, for any reason
        /// </exception>
        public List<AcquiredImageInfo> AcquireImageOnce(double timeoutInMs = 2147483647, double exposureTime = -1, Action onReadyAction = null)
        {
            try
            {
                var ret = new List<AcquiredImageInfo>();
                OnCamAcqReady?.Invoke(this);
                onReadyAction?.Invoke();

                if (_imgFile.Count == 0)
                {
                    OnCamComplete?.Invoke(this, new AcquiredEventArgs(ret));
                    return ret;
                }
                for (int i = 0; i < BatchSize; i++)
                {
                    var img_info = new AcquiredImageInfo();
                    img_info.Image = _imgFile.ReadNext().Copy();
                    ret.Add(img_info);
                }
                var evt = new AcquiredEventArgs(ret);
                OnCamComplete?.Invoke(this, evt);

                return ret;
            }
            catch (Exception ex) {
                throw new OperationFailureException($"Virtual Camera: {Name} acquire image failed: {ex.Message}");
            }
        }

        public void ClearImageQueue()
        {
            _imgFile.Reset();
        }

        public void CloseCamera() {}
        public void ConnectCamera() {}

        public void GetParams() => new InvalidOperationException();
        public void LoadBoardConfigFile(string configPath) => new InvalidOperationException();
        public void LoadCameraConfigFile(string configPath) => new InvalidOperationException();
        public void LoadSetting() => new InvalidOperationException();
        public void SaveSetting() => new InvalidOperationException();


#region Config
        public void SwitchProgramNo(int programNo)
            => new InvalidOperationException("Virtual Camera: SwitchProgramNo is not supproted");

        public string ConfigFileExtension => ".txt";

        /// <exception cref="ConfigException">
        /// Throws when config file is invalid
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throws when path contains no valid image file
        /// </exception>
        public void LoadConfig(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                var path = reader.ReadLine().Trim();
                Path = path;

                int.TryParse(reader.ReadLine(), out _batchSize);
                BatchSize = _batchSize;
            }
        }
        /// <exception cref="OperationFailureException">
        /// Throws when camera's Path is null or empty
        /// </exception>
        public void ExportConfig(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(Path ?? "");
                writer.WriteLine($"{BatchSize}");
            }
        }
#endregion

#region State
        public Type StateObjectType => null;
        public object ExportStateToSerializable() { return null; }
        public void LoadStateFromSerializable(object stateObject) {}
#endregion

        public void SetExposureTime(double timeInUs)  => new InvalidOperationException();

        public void SetScanDirection(InsCameraScanDirection direction)
        {
            throw new NotImplementedException("Virtual Camera");
        }

        public void SetScanLines(int lines)
        {
            throw new NotImplementedException("Virtual Camera");
        }


        public void StartAcquireImage() {}
        public void StopAcquireImage() {}

        public void StartAcquirImage() => StartAcquireImage();
        public void StopAcquirImage() => StopAcquireImage();


        public void SetTriggerMode(InsCameraTriggerMode triggerMode) {}
        public void TriggerModeOff() {}
        public void TriggerModeOn() {}
    }
}
