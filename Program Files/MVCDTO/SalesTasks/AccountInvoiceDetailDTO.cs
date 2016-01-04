using System;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCModel.Helpers;

namespace MVCDTO.SalesTasks
{
    public class AccountInvoiceDetailDTO : BaseModel, IPrimitiveEntity
    {
        public int GetID() { return this.AccountInvoiceDetailID; }

        public int AccountInvoiceDetailID { get; set; }
        public int AccountInvoiceID { get; set; }

        public int CustomerID { get; set; }
        public int SalesInvoiceDetailID { get; set; }

        public int CommodityID { get; set; }
        [Display(Name = "Hàng hóa")]
        [UIHint("StringReadonly")]
        public virtual string CommodityName { get; set; }
        [UIHint("StringReadonly")]
        public string CommodityCode { get; set; }

        public int CommodityTypeID { get; set; }

        [Display(Name = "SL")]
        [UIHint("DecimalReadonly")]
        public virtual decimal Quantity { get; set; }
        [Display(Name = "Đơn giá")]
        [UIHint("DecimalReadonly")]
        public decimal ListedPrice { get; set; }
        [Display(Name = "CK")]
        [UIHint("DecimalReadonly")]
        public decimal DiscountPercent { get; set; }
        [Display(Name = "Giá bán")]
        [UIHint("DecimalReadonly")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "VAT")]
        [UIHint("DecimalReadonly")]
        public decimal VATPercent { get; set; }
        [Display(Name = "Giá sau thuế")]
        [UIHint("DecimalReadonly")]
        public decimal GrossPrice { get; set; }
        [Display(Name = "Thành tiền")]
        [UIHint("DecimalReadonly")]
        public decimal Amount { get; set; }
        [Display(Name = "Thuế VAT")]
        [UIHint("DecimalReadonly")]
        public decimal VATAmount { get; set; }
        [Display(Name = "Tổng cộng")]
        [UIHint("DecimalReadonly")]
        public decimal GrossAmount { get; set; }
        [Display(Name = "Ghi chú")]
        public string Remarks { get; set; }

        public Nullable<bool> IsBonus { get; set; }
        public Nullable<bool> IsWarrantyClaim { get; set; }

        [Display(Name = "Số khung")]
        [UIHint("StringReadonly")]
        public string ChassisCode { get; set; }
        [UIHint("StringReadonly")]
        [Display(Name = "Số động cơ")]
        public string EngineCode { get; set; }
        [UIHint("StringReadonly")]
        [Display(Name = "Mã màu")]
        public string ColorCode { get; set; }
    }
}
