using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using VDEApp.Devices;
using VDEApp.Devices.Attributes;

namespace VDEApp.Controllers
{
    public class DeviceController
    {
        private static readonly Lazy<DeviceController> DeviceCtrl = new Lazy<DeviceController>(() => new DeviceController());
        public static DeviceController Instance => DeviceCtrl.Value;

        public DeviceController()
        {
            _cameras = new List<ICamera>();
            _cameraMutexs = new Dictionary<string, Mutex>();
        }

        #region Cameras
        private List<ICamera> _cameras;
        /// <summary>
        /// map from camera guid to its mutex lock
        /// </summary>
        private Dictionary<string, Mutex> _cameraMutexs;

        public List<ICamera> Cameras { get => _cameras; }

        public ICamera GetCamera(string name)
        {
            var ret = FindCamera(name).Camera;
            if (ret == null)
                throw new ArgumentOutOfRangeException($"Camera with name \"{name}\" does not exist");
            return ret;
        }
        public T GetCamera<T>(string name) where T : class, ICamera
        {
            var cam = GetCamera(name) as T;
            return cam;
        }

        public bool HasCamera(string name)
        {
            return FindCamera(name).Camera != null;
        }

        public T AddCamera<T>(string name) where T : class, ICamera, new()
        {
            if (FindCamera(name).Camera != null)
                throw new ArgumentException($"Camera with same name \"{name}\" already exists");

            var cam = new T();
            cam.Name = name;
            return AddCamera<T>(cam);
        }
        public T AddCamera<T>(T camera) where T : class, ICamera
        {
            if (HasCamera(camera.Name))
                throw new ArgumentException($"Camera with same name \"{camera.Name}\" already exists");

            if (string.IsNullOrWhiteSpace(camera.Guid))
                camera.Guid = System.Guid.NewGuid().ToString();
            _cameras.Add(camera);
            _cameraMutexs.Add(camera.Guid, new Mutex());

            return camera;
        }
        /// <exception cref="ArgumentException">
        /// throws when no camera is named "name"
        /// </exception>
        public void RemoveCamera(string name)
        {
            var (idx, cam) = FindCamera(name);
            if (cam == null)
                throw new ArgumentException($"Camera with name \"{name}\" does not exist");
            RemoveCamera(idx);
        }
        /// <exception cref="IndexOutOfRangeException">
        /// throws when index is smaller than 0 or bigger than camera count
        /// </exception>
        public void RemoveCamera(int index)
        {
            if (index < 0 || index >= _cameras.Count)
                throw new IndexOutOfRangeException($"Index out of range");

            var cam = _cameras[index];
            var evt = new CameraRemoveEventArgs()
            {
                Name = cam.Name,
                Guid = cam.Guid,
                Model = cam.ModelName,
                SN = cam.SN
            };
            _cameras.RemoveAt(index);
            CameraRemove?.Invoke(this, evt);
        }
        /// <exception cref="ArgumentException">
        /// throws when a camera named "new_name" already exists
        /// </exception>
        public void RenameCamera(string old_name, string new_name)
        {
            // rename to itself, just ignore
            if (old_name == new_name)
                return;
            var (idx, _) = FindCamera(old_name);
            if (idx == -1)
                throw new ArgumentException($"Can not find camera with name \"{old_name}\"");
            RenameCamera(idx, new_name);
        }
        /// <exception cref="ArgumentException">
        /// throws when a camera named "new_name" already exists
        /// </exception>
        public void RenameCamera(int index, string new_name)
        {
            new_name = new_name.Trim();
            var existing_camera = FindCamera(new_name);

            if (existing_camera.Camera?.Name == new_name && existing_camera.Index != index)
                throw new ArgumentException($"Can not rename camera when a camera named \"{new_name}\" already exists");

            _cameras[index].Name = new_name;
        }

        public ICamera FindCameraByGuid(string guid)
        {
            return _cameras.Find((cam) => cam.Guid == guid);
        }
        public List<T> FindCamerasByType<T>() where T : ICamera
        {
            var ret = new List<T>();
            foreach (var camera in _cameras)
            {
                if (camera is T cam)
                    ret.Add(cam);
            }
            return ret;
        }
        public List<ICamera> FindCamerasByType(Type type)
        {
            var ret = new List<ICamera>();
            foreach (var camera in _cameras)
            {
                if (camera.GetType() == type)
                    ret.Add(camera);
            }
            return ret;
        }

        /// <summary>
        /// find camera by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>if no camera named "name", returns (-1, null)</returns>
        private (int Index, ICamera Camera) FindCamera(string name)
        {
            name = name.Trim();
            int idx = _cameras.FindIndex((cam) => cam.Name == name);
            return (idx, idx == -1 ? null : _cameras[idx]);
        }

        public Dictionary<Type, CameraModelData> CameraModels = buildCameraModelMetaInfo();

        private static Dictionary<Type, CameraModelData> buildCameraModelMetaInfo()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var ICameraType = typeof(ICamera);

            var camera_types = assembly.GetTypes()
                .Where(t =>
                    t.Namespace == $"{ICameraType.Namespace}.Cameras" &&
                    ICameraType.IsAssignableFrom(t) &&
                    !t.IsAbstract && t.IsClass
                );

            var ret = new Dictionary<Type, CameraModelData>();
            foreach (var type in camera_types)
            {
                var class_attr = type.GetCustomAttribute<CameraAttribute>();
                MethodInfo search_method = null;
                foreach (var static_method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    var search_method_attr = static_method.GetCustomAttribute<CameraSearchMethodAttribute>();
                    if (search_method_attr == null)
                        continue;
                    search_method = static_method;
                }

                ret.Add(type, new CameraModelData(search_method, class_attr.ConfigureCtrlType)
                {
                    Name = class_attr.ReadableName,
                });
            }
            return ret;
        }

        public Mutex GetCameraMutex(ICamera camera)
        {
            if (camera == null)
                throw new ArgumentNullException("Can not get mutex for a null camera instance");

            return _cameraMutexs[camera.Guid];
        }

        public event CameraRemove CameraRemove;
        #endregion
    }

    public delegate void CameraRemove(DeviceController sender, CameraRemoveEventArgs e);

    public class CameraRemoveEventArgs : EventArgs
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public string Model { get; set; }
        public string SN { get; set; }
    }

    public class CameraModelData
    {
        private MethodInfo _searchMethod;
        private Type _configureCtrlType;

        public string Name { get; set; }

        public CameraModelData(MethodInfo searchMethod, Type configureCtrlType)
        {
            _searchMethod = searchMethod;
            _configureCtrlType = configureCtrlType;
        }

        public UserControl CreateConfigureControl(ICamera camera)
            => Activator.CreateInstance(_configureCtrlType, camera) as UserControl;

        public void SearchCameras()
            => _searchMethod?.Invoke(null, null);

        public override string ToString() { return Name; }
    }

}
