using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.DTOs
{
    public class FGSNRecordDto
    {
        public string SN { get; set; }
        public string Status { get; set; }
        public System.DateTime Date { get; set; }
        public string Location { get; set; }
        public string ContainNo { get; set; }
        public Nullable<int> ZoneCode { get; set; }
        public string SalesOrder { get; set; }
    }
}