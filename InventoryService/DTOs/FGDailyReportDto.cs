using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.DTOs
{
    public class FGDailyReportDto
    {
        public string ModelNo{ get; set; }
        public string ModelFG{ get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public int Previous { get; set; }
        public int Shipped { get; set; }
        public int Received { get; set; }
        public int Scrapped { get; set; }
        public int OnHand { get; set; }
        public int ReturnItem { get; set; }
        public int ShowRoom { get; set; }
        public int  Rework { get; set; }
        public int QC { get; set; }
        public int Total { get; set; }
    }
}