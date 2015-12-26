using System;
using System.Collections.Generic;

using MVCModel.Models;
using MVCCore.Repositories.CommonTasks;


namespace MVCCore.Repositories.CommonTasks
{
    public interface ICommodityRepository : IGenericRepository<Commodity>
    {
        bool InitOfficialCode22DEC15();

        IList<Commodity> SearchCommoditiesByName(string searchText, string commodityTypeIDList, bool? isOnlyAlphaNumericString);
        IList<Commodity> SearchCommoditiesByIndex(int commodityCategoryID, int commodityTypeID);
        IList<CommoditiesInGoodsReceipt> GetCommoditiesInGoodsReceipts(int? locationID, string searchText, int? salesInvoiceID, int? stockTransferID, int? stockAdjustID);
        IList<CommoditiesInWarehouse> GetCommoditiesInWarehouses(int? locationID, DateTime? entryDate, string searchText, int? salesInvoiceID, int? stockTransferID, int? stockAdjustID);

        IList<CommoditiesAvailable> GetCommoditiesAvailables(int? locationID, DateTime? entryDate, string searchText);
        IList<VehicleAvailable> GetVehicleAvailables(int? locationID, DateTime? entryDate, string searchText);
        IList<PartAvailable> GetPartAvailables(int? locationID, DateTime? entryDate, string searchText);
    }
    
}
