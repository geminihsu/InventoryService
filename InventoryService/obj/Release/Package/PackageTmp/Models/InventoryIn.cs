//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InventoryService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InventoryIn
    {
        public int Seq { get; set; }
        public string SN { get; set; }
        public System.DateTime Date { get; set; }
        public string Location { get; set; }
        public string ModelNo { get; set; }
        public string ContainerNo { get; set; }
    }
}