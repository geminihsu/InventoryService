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
    
    public partial class History
    {
        public string SN { get; set; }
        public string SalesOrder { get; set; }
        public System.DateTime Date { get; set; }
        public string TrackingNo { get; set; }
        public string BillTo { get; set; }
        public string ShipVia { get; set; }
        public string ShipState { get; set; }
        public string ShipCity { get; set; }
        public string CustPO { get; set; }
        public Nullable<bool> IsPurchaseImported { get; set; }
        public string ModelNo { get; set; }
        public string Location { get; set; }
        public int Seq { get; set; }
    }
}
