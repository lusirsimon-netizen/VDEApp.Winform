using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VDEApp.Devices.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InsCameraType
    {
        // basic type
        CameraVirtual = 0x0001,
        Camera2D      = 0x0002,
        Camera3D      = 0x0003,
        Camera25D     = 0x0004,
        CameraCHVS    = 0x0005,

        // capture type
        CameraLine    = 0x0010,
        CameraArea    = 0x0020,

        // interface type
        CameraUSB     = 0x0100,
        CameraGige    = 0x0200,
        CameraLink    = 0x0300,

        // not sure what is this
        CameraVisionBoard = 0x1000,

        Camera2DAreaGige = Camera2D | CameraArea | CameraGige,
        Camera2DLineGige = Camera2D | CameraLine | CameraGige,
        Camera2DAreaUSB  = Camera2D | CameraArea | CameraUSB,
        Camera2DLineUSB  = Camera2D | CameraLine | CameraUSB,
        Camera2DLineCameraLink  = Camera2D | CameraLine | CameraLink,
        Camera2DGigeVisionBoard = Camera2D | CameraGige | CameraVisionBoard,
        Camera3DArea  = Camera3D | CameraArea,
        Camera3DLine  = Camera3D | CameraLine,
        Camera25DArea = Camera25D | CameraArea,
        Camera25DLine = Camera25D | CameraLine,
    }
}
