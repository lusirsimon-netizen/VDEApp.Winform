using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VDEApp.Devices.Enums
{
    public enum InsCameraTriggerMode
    {
        Encoder,
        FreeRun,
        Internal,
        Software,
        HardwareRisingEdge,
        HardwareFallingEdge,
        HardwareLevelHigh,
        HardwareLevelLow,
        HardwareAnyEdge,
    }
}
