using System.Linq;
using System.Collections.Generic;

using MVCModel.Models;
using MVCBase.Enums;


namespace MVCCore.Repositories.SalesTasks
{
    public interface IAccountInvoiceRepository : IGenericWithDetailRepository<AccountInvoice, AccountInvoiceDetail>
    {
    }

    public interface IAccountInvoiceAPIRepository : IGenericAPIRepository
    {
    }
}
