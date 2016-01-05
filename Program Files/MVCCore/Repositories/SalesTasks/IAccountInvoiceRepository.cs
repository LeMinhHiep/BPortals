using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCBase.Enums;
using System;


namespace MVCCore.Repositories.SalesTasks
{
    public interface IAccountInvoiceRepository : IGenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>
    {
    }

    public interface IAccountInvoiceAPIRepository : IGenericAPIRepository
    {
        IEnumerable<PendingSalesInvoice> GetPendingSalesInvoices(string aspUserID, int locationID, int accountInvoiceID, int commodityTypeID, DateTime fromDate, DateTime toDate);
    }
}
