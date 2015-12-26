using System.Text;
using MVCModel.Models;
using MVCBase.Enums;
using System;


namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    public class SalesInvoice
    {

        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public SalesInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.SalesInvoiceJournal();
        }


        private void SalesInvoiceJournal()
        {
            string queryString = " @LocationID int, @CommodityTypeID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF          (@CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Vehicles + ") " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID.Vehicles) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Parts + ")  " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID.Parts) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@CommodityTypeID = " + (int)GlobalEnums.CommodityTypeID.Consumables + ")  " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID.Consumables) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID.Services) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("SalesInvoiceJournal", queryString);
        }


        private string SalesInvoiceJournalBuild(GlobalEnums.CommodityTypeID commodityTypeID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocationID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetail(commodityTypeID, false) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetail(commodityTypeID, true) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }

        private string SalesInvoiceJournalBuildDetail(GlobalEnums.CommodityTypeID commodityTypeID, bool locationFilter)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoiceDetails.EntryDate, Customers.CustomerID, Customers.Name AS CustomerName, Commodities.CommodityID, Commodities.Code, Commodities.Name, SalesInvoiceDetails.CommodityTypeID, SalesInvoiceDetails.WarehouseID, " + "\r\n";
            queryString = queryString + "                   Locations.Code AS LocationCode, VWCommodityCategories.CommodityCategoryID, VWCommodityCategories.Name1 AS CommodityCategory1, VWCommodityCategories.Name2 AS CommodityCategory2, VWCommodityCategories.Name3 AS CommodityCategory3, " + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails.Quantity, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, IIF(SalesInvoiceDetails.SalesInvoiceTypeID ;;;) SalesInvoiceDetails.ServiceInvoiceID, " + "\r\n";

            if (commodityTypeID == GlobalEnums.CommodityTypeID.AllTypes)
                queryString = queryString + "            XX SalesInvoiceDetails.ServiceInvoiceID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.UnitPrice AS CostPrice " + "\r\n";
            else
                if (commodityTypeID == GlobalEnums.CommodityTypeID.Vehicles)
                    queryString = queryString + "           SalesInvoiceDetails.ServiceInvoiceID, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, GoodsReceiptDetails.UnitPrice AS CostPrice " + "\r\n";
                else
                    if (commodityTypeID == GlobalEnums.CommodityTypeID.Parts || commodityTypeID == GlobalEnums.CommodityTypeID.Consumables)
                        queryString = queryString + "       SalesInvoiceDetails.ServiceInvoiceID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, WarehouseBalancePrice.UnitPrice AS CostPrice " + "\r\n";
                    else
                        queryString = queryString + "       SalesInvoiceDetails.SalesInvoiceID AS ServiceInvoiceID, '' AS ChassisCode, '' AS EngineCode, '' AS ColorCode, 0 AS CostPrice " + "\r\n";

            queryString = queryString + "       FROM        SalesInvoiceDetails INNER JOIN " + "\r\n";
            queryString = queryString + "                   Commodities ON SalesInvoiceDetails.EntryDate >= @FromDate AND SalesInvoiceDetails.EntryDate <= @ToDate AND SalesInvoiceDetails.CommodityTypeID = " + (int)commodityTypeID + (locationFilter ? " AND SalesInvoiceDetails.LocationID = @LocationID" : "") + " AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON SalesInvoiceDetails.CustomerID = Customers.CustomerID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Locations ON SalesInvoiceDetails.LocationID = Locations.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   VWCommodityCategories ON Commodities.CommodityCategoryID = VWCommodityCategories.CommodityCategoryID " + "\r\n";

            if (commodityTypeID == GlobalEnums.CommodityTypeID.AllTypes)
            {
                queryString = queryString + "               LEFT JOIN GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
                queryString = queryString + "               LEFT JOIN WarehouseBalancePrice ON SalesInvoiceDetails.CommodityID = WarehouseBalancePrice.CommodityID AND MONTH(SalesInvoiceDetails.EntryDate) = MONTH(WarehouseBalancePrice.EntryDate) AND YEAR(SalesInvoiceDetails.EntryDate) = YEAR(WarehouseBalancePrice.EntryDate) " + "\r\n";
            }
            else
                if (commodityTypeID == GlobalEnums.CommodityTypeID.Vehicles)
                    queryString = queryString + "           INNER JOIN GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
                else
                    if (commodityTypeID == GlobalEnums.CommodityTypeID.Parts || commodityTypeID == GlobalEnums.CommodityTypeID.Consumables)
                        queryString = queryString + "       INNER JOIN WarehouseBalancePrice ON SalesInvoiceDetails.CommodityID = WarehouseBalancePrice.CommodityID AND MONTH(SalesInvoiceDetails.EntryDate) = MONTH(WarehouseBalancePrice.EntryDate) AND YEAR(SalesInvoiceDetails.EntryDate) = YEAR(WarehouseBalancePrice.EntryDate) " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

    }
}
