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
    
    public partial class Report
    {
        public string FG { get; set; }
        public string Model { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> Shipped { get; set; }
        public Nullable<decimal> OnHand { get; set; }
        public Nullable<decimal> Unshippable { get; set; }
        public Nullable<int> Zone1 { get; set; }
        public Nullable<int> Zone2 { get; set; }
        public Nullable<int> ReturnItem { get; set; }
        public Nullable<int> ShowRoom { get; set; }
        public int MinQuantity { get; set; }
    }
}
