using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.DTOs
{
    public class FGDailyReplenishment
    {
        public string ModelNo { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int Qty { get; set; }
    }
}