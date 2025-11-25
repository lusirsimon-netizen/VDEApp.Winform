using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Models.Product
{
    [Flags]
    public enum ResponseType
    {
        AfterAcquireNode     = 0b001,
        AfterCalibrationNode = 0b010,
        AfterInspectionNode  = 0b100,
        ALL = AfterAcquireNode | AfterCalibrationNode | AfterInspectionNode,
    }
}
