using Insnex.Vision2D.Core;

namespace VDEApp.Devices.Datas
{
    /// <summary>
    /// 取图返回结构
    /// </summary>
    public class AcquiredImageInfo
    {
        public AcquiredImageInfo()
        {
            Image = null;
            StartEncoderValue = -999;
            EndEncoderValue = -999;
            FrameID = -999;
            EncoderEnable = false;
        }
        public AcquiredImageInfo(AcquiredImageInfo acquiredImageInfo)
        {
            Image = acquiredImageInfo.Image;
            StartEncoderValue = acquiredImageInfo.StartEncoderValue;
            EndEncoderValue = acquiredImageInfo.EndEncoderValue;
            FrameID = acquiredImageInfo.FrameID;
            EncoderEnable = acquiredImageInfo.EncoderEnable;
        }

        public IInsImage Image { get; set; }
        public long StartEncoderValue { get; set; }
        public long EndEncoderValue { get; set; }
        public int FrameID { get; set; }
        public bool EncoderEnable { get; set; }
    }
}
