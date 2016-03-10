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
    using System.Collections.Generic;
    
    public partial class InventoryAdjustmentDetail
    {
        public int InventoryAdjustmentDetailID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int LocationID { get; set; }
        public int InventoryAdjustmentID { get; set; }
        public int InventoryAdjustmentTypeID { get; set; }
        public int SupplierID { get; set; }
        public Nullable<int> GoodsReceiptDetailID { get; set; }
        public int CommodityID { get; set; }
        public int CommodityTypeID { get; set; }
        public int WarehouseID { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VATPercent { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string Remarks { get; set; }
    
        public virtual Commodity Commodity { get; set; }
        public virtual GoodsReceiptDetail GoodsReceiptDetail { get; set; }
        public virtual InventoryAdjustment InventoryAdjustment { get; set; }
    }
}
