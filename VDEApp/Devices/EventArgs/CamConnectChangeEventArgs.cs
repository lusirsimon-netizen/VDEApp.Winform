using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Devices
{
    public class CamConnectChangeEventArgs : EventArgs
    {
        public CamConnectChangeEventArgs(bool isConnected, string guid)
        {
            IsConnected = isConnected;
            Guid = guid;
        }
        public CamConnectChangeEventArgs()
        { }
        public bool IsConnected { set; get; }
        public string Guid { set; get; }

    }
}
