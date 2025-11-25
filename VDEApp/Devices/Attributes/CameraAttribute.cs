using System;
using System.Windows.Forms;

namespace VDEApp.Devices.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CameraAttribute : Attribute
    {
        public string ReadableName { get; private set; }
        public Type ConfigureCtrlType{ get; private set; }

        public CameraAttribute(string ReadableName, Type ConfigureCtrlType)
        {
            this.ReadableName = ReadableName;
            this.ConfigureCtrlType = ConfigureCtrlType;
        }
    }
}
