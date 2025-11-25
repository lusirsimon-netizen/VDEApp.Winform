using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDEApp.Models.Product;

namespace VDEApp.Models.Project
{
    /// <summary>
    /// 存图配置
    /// </summary>
    public class ImageSaveOptions
    {
        // 原图
        public bool RawEnabled { get; set; } = true;
        public string RawResult { get; set; } = "ALL";
        public string RawFormat { get; set; } = "png";
        public string RawDir { get; set; } = "";

        // 截图
        public bool ShotEnabled { get; set; } = true;
        public string ShotResult { get; set; } = "ALL";

        public string ShotFormat { get; set; } = "jpg";
        public string ShotDir { get; set; } = "";

        // 解析：拿到 Images 根（若用户未选，则默认 projDir/Images）
        public string GetRawRootOrDefault(string projectDir)
            => string.IsNullOrWhiteSpace(RawDir) ? Path.Combine(projectDir, "Images") : RawDir;

        public string GetShotRootOrDefault(string projectDir)
            => string.IsNullOrWhiteSpace(ShotDir) ? Path.Combine(projectDir, "Images") : ShotDir;
        //截图
        [JsonIgnore]
        public List<ProductionDataStatus> ShotResult_
        {
            get
            {
                switch (ShotResult?.ToUpper())
                {
                    case "OK":
                        return new List<ProductionDataStatus> { ProductionDataStatus.OK };
                    case "NG":
                        return new List<ProductionDataStatus> { ProductionDataStatus.NG };
                    case "ALL":
                    default: // 默认情况，包括 ShotResult 为 null 或其他意外值
                        return new List<ProductionDataStatus> { ProductionDataStatus.OK, ProductionDataStatus.NG, ProductionDataStatus.NA };
                }
            }
        }
        //原图
        [JsonIgnore]
        public List<ProductionDataStatus> RawResult_
        {
            get
            {
                switch (RawResult?.ToUpper())
                {
                    case "OK":
                        return new List<ProductionDataStatus> { ProductionDataStatus.OK };
                    case "NG":
                        return new List<ProductionDataStatus> { ProductionDataStatus.NG };
                    case "ALL":
                    default:
                        return new List<ProductionDataStatus> { ProductionDataStatus.OK, ProductionDataStatus.NG, ProductionDataStatus.NA };
                }
            }
        }

        // 解析：返回“最终目录”= 根/任务名/原图(或截图)
        public string ResolveRawDir(string projectDir, string taskName)
            => Path.Combine(GetRawRootOrDefault(projectDir), Sanitize(taskName), "Raw");

        public string ResolveShotDir(string projectDir, string taskName)
            => Path.Combine(GetShotRootOrDefault(projectDir), Sanitize(taskName), "SnapShot");

        private static string Sanitize(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "DefaultTask";
            foreach (var ch in Path.GetInvalidFileNameChars())
                name = name.Replace(ch, '_');
            return name.Trim();
        }
    }
}

