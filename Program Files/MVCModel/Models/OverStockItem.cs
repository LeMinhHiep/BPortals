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
    
    public partial class OverStockItem
    {
        public System.DateTime OverStockDate { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseCode { get; set; }
        public int CommodityID { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityName { get; set; }
        public double Quantity { get; set; }
    }
}
