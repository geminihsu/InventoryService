using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryService.DTOs
{
    public class ModelDto
    {
        public string ModelNo { get; set; }
        public int Version { get; set; }
        public string FG { get; set; }
        public string Model { get; set; }
        public string Desc { get; set; }
    }
}