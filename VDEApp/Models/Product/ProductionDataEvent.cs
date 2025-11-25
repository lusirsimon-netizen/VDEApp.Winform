using System;
using System.Collections.Generic;
using System.Collections;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Insnex.Vision2D;

namespace VDEApp.Models.Product
{
    public class ProductionDataEvent
    {
        public string ProductionTime { get; set; }
        public string Guid { get; set; }
        public string SN { get; set; }
        public string ProductionName { get; set; }
        public ProductionDataStatus Status { get; set; }
        public ArrayList DefectImages { get; set; }
        public InsRecords InspectionNodeRecords { get; set; }
    }
}
