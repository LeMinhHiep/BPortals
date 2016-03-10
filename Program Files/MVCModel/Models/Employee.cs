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
    
    public partial class Employee
    {
        public Employee()
        {
            this.Quotations = new HashSet<Quotation>();
            this.SalesInvoices = new HashSet<SalesInvoice>();
            this.InventoryAdjustments = new HashSet<InventoryAdjustment>();
        }
    
        public int EmployeeID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int LocationID { get; set; }
        public string Birthday { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual ICollection<Quotation> Quotations { get; set; }
        public virtual ICollection<SalesInvoice> SalesInvoices { get; set; }
        public virtual ICollection<InventoryAdjustment> InventoryAdjustments { get; set; }
    }
}
