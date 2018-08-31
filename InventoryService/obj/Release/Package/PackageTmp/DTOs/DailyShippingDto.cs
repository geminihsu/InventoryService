using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.DTOs
{
    public class DailyShippingDto
    {
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string SO { get; set; }
        public string ItemID { get; set; }
        public string BillTo { get; set; }
        public string CustPo { get; set; }
        public string FG { get; set; }
        public string Qty { get; set; }
        public string TrackingNo { get; set; }
        public string ShipVia { get; set; }
        public string ShipState { get; set; }
        public string ShipCity { get; set; }
        public System.DateTime ShippingDate { get; set; }
        public string SN { get; set; }
    }
}