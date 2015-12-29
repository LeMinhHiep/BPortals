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
    
    public partial class SalesInvoiceDetail
    {
        public SalesInvoiceDetail()
        {
            this.ServiceContracts = new HashSet<ServiceContract>();
        }
    
        public int SalesInvoiceDetailID { get; set; }
        public System.DateTime EntryDate { get; set; }
        public int SalesInvoiceID { get; set; }
        public Nullable<int> GoodsReceiptDetailID { get; set; }
        public Nullable<int> QuotationDetailID { get; set; }
        public int CommodityID { get; set; }
        public int CommodityTypeID { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityContract { get; set; }
        public decimal QuantityVAT { get; set; }
        public decimal QuantityReturn { get; set; }
        public decimal ListedPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VATPercent { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal VATAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public Nullable<bool> IsWarrantyClaim { get; set; }
        public string Remarks { get; set; }
        public int LocationID { get; set; }
        public Nullable<bool> IsBonus { get; set; }
        public int CustomerID { get; set; }
        public Nullable<int> ServiceInvoiceID { get; set; }
        public int SalesInvoiceTypeID { get; set; }
        public Nullable<int> ServiceContractID { get; set; }
        public Nullable<int> CurrentMeters { get; set; }
    
        public virtual Commodity Commodity { get; set; }
        public virtual GoodsReceiptDetail GoodsReceiptDetail { get; set; }
        public virtual SalesInvoice SalesInvoice { get; set; }
        public virtual ICollection<ServiceContract> ServiceContracts { get; set; }
        public virtual QuotationDetail QuotationDetail { get; set; }
    }
}
