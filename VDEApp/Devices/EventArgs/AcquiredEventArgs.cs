using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDEApp.Devices.Datas;

namespace VDEApp.Devices
{
    public class AcquiredEventArgs : EventArgs
    {
        public AcquiredEventArgs(List<AcquiredImageInfo> imageList)
        {
            ImageList = imageList;
        }
        public AcquiredEventArgs()
        {
            ImageList = new List<AcquiredImageInfo>();
        }

        public List<AcquiredImageInfo> ImageList { get; set; }
    }
}
