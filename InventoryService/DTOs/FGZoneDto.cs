using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.DTOs
{
    public class FGZoneDto
    {
        public string SN { get; set; }
        public System.DateTime Date { get; set; }
        public string Location { get; set; }
        public string ModelNo { get; set; }
        public Nullable<short> ZoneCode { get; set; }
    }
}