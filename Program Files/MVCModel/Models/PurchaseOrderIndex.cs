//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCModel.Models
{
    using System;
    
    public partial class PurchaseOrderIndex
    {
        public int PurchaseOrderID { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string Reference { get; set; }
        public string ConfirmReference { get; set; }
        public Nullable<System.DateTime> ConfirmDate { get; set; }
        public string LocationCode { get; set; }
        public string SupplierDescription { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalGrossAmount { get; set; }
        public string Description { get; set; }
    }
}
