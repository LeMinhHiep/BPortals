using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;


using MVCBase.Enums;
using MVCModel.Models;
using MVCCore.Repositories.SalesTasks;
using System;



namespace MVCData.Repositories.SalesTasks
{
    public class AccountInvoiceRepository : GenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>, IAccountInvoiceRepository
    {
        public AccountInvoiceRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities) { }
    }


    public class AccountInvoiceAPIRepository : GenericAPIRepository, IAccountInvoiceAPIRepository
    {
        public AccountInvoiceAPIRepository(TotalBikePortalsEntities totalBikePortalsEntities)
            : base(totalBikePortalsEntities, "GetAccountInvoiceIndexes")
        {
        }
        public IEnumerable<PendingSalesInvoice> GetPendingSalesInvoices(string aspUserID, int locationID, int accountInvoiceID, int commodityTypeID, DateTime fromDate, DateTime toDate, string salesInvoiceDetailIDs)
        {
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = false;
            IEnumerable<PendingSalesInvoice> pendingSalesInvoices = base.TotalBikePortalsEntities.GetPendingSalesInvoices(aspUserID, locationID, accountInvoiceID, commodityTypeID, fromDate, toDate, salesInvoiceDetailIDs).ToList();
            this.TotalBikePortalsEntities.Configuration.ProxyCreationEnabled = true;

            return pendingSalesInvoices;
        }
    }

}
