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
            string queryString = " @WithAccountInvoice bit, @LocationID int, @SalesInvoiceTypeID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF          (@SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.AllInvoice + ") " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.AllInvoice) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice + ") " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.PartsInvoice + ")  " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.PartsInvoice) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n"; //GlobalEnums.SalesInvoiceTypeID.ServicesInvoice
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.ServicesInvoice) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("SalesInvoiceJournal", queryString);
        }


        private string SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocationID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildMaster(salesInvoiceTypeID, false) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildMaster(salesInvoiceTypeID, true) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }

        private string SalesInvoiceJournalBuildMaster(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool locationFilter)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@WithAccountInvoice = 0) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetail(salesInvoiceTypeID, locationFilter, false) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuildDetail(salesInvoiceTypeID, locationFilter, true) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }

        private string SalesInvoiceJournalBuildDetail(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool locationFilter, bool withAccountInvoice)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoiceDetails.SalesInvoiceDetailID, SalesInvoiceDetails.EntryDate, Customers.CustomerID, Customers.Name AS CustomerName, Customers.Birthday, Customers.IsFemale, Customers.Telephone, Customers.Facsimile, Customers.AddressNo, EntireTerritories.Name2 AS DistrictName, EntireTerritories.Name1 AS ProvinceName, CustomerCategories.Name AS CustomerCategoryName, " + "\r\n";
            queryString = queryString + "                   Commodities.CommodityID, Commodities.Code, Commodities.Name, SalesInvoiceDetails.CommodityTypeID, SalesInvoiceDetails.WarehouseID, IIF(SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.ServicesInvoice + ", SalesInvoiceDetails.SalesInvoiceID, SalesInvoiceDetails.ServiceInvoiceID) AS ServiceInvoiceID, " + "\r\n";
            queryString = queryString + "                   Locations.Code AS LocationCode, VWCommodityCategories.CommodityCategoryID, VWCommodityCategories.Name1 AS CommodityCategory1, VWCommodityCategories.Name2 AS CommodityCategory2, VWCommodityCategories.Name3 AS CommodityCategory3, " + "\r\n";


            if (withAccountInvoice)
                queryString = queryString + "               AccountInvoices.AccountInvoiceID, VATCustomers.Name AS VATCustomerName, VATCustomers.VATCode, AccountInvoices.VATInvoiceNo, AccountInvoices.VATInvoiceDate, AccountInvoices.VATInvoiceSeries, AccountInvoices.Description AS VATDescription, " + "\r\n";
            else
                queryString = queryString + "               NULL AS AccountInvoiceID, NULL AS VATCustomerName, NULL AS VATCode, NULL AS VATInvoiceNo, NULL AS VATInvoiceDate, NULL AS VATInvoiceSeries, NULL AS VATDescription, " + "\r\n";



            if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice || salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice)
                queryString = queryString + "               GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            else
                queryString = queryString + "               NULL AS ChassisCode, NULL AS EngineCode, NULL AS ColorCode, " + "\r\n";



            queryString = queryString + "                   SalesInvoiceDetails.Quantity, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, " + "\r\n";



            if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice)
                queryString = queryString + "               IIF(SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice + ", GoodsReceiptDetails.UnitPrice, (IIF(SalesInvoiceDetails.SalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.PartsInvoice + ", WarehouseBalancePrice.UnitPrice, 0))) AS CostPrice " + "\r\n";
            else
                if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice)
                    queryString = queryString + "           GoodsReceiptDetails.UnitPrice AS CostPrice " + "\r\n";
                else
                    if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.PartsInvoice)
                        queryString = queryString + "       WarehouseBalancePrice.UnitPrice AS CostPrice " + "\r\n";
                    else //salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.ServicesInvoice
                        queryString = queryString + "       0 AS CostPrice " + "\r\n";




            queryString = queryString + "       FROM        SalesInvoiceDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON SalesInvoiceDetails.EntryDate >= @FromDate AND SalesInvoiceDetails.EntryDate <= @ToDate " + (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice ? "" : " AND SalesInvoiceDetails.SalesInvoiceTypeID = @SalesInvoiceTypeID") + (locationFilter ? " AND SalesInvoiceDetails.LocationID = @LocationID" : "") + " AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            if (withAccountInvoice)
            {
                queryString = queryString + "               INNER JOIN AccountInvoiceDetails ON SalesInvoiceDetails.SalesInvoiceDetailID = AccountInvoiceDetails.SalesInvoiceDetailID " + "\r\n";
                queryString = queryString + "               INNER JOIN AccountInvoices ON AccountInvoiceDetails.AccountInvoiceID = AccountInvoices.AccountInvoiceID " + "\r\n";
                queryString = queryString + "               INNER JOIN Customers VATCustomers ON AccountInvoices.CustomerID = VATCustomers.CustomerID " + "\r\n";
            }

            queryString = queryString + "                   INNER JOIN Customers ON SalesInvoiceDetails.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN CustomerCategories ON Customers.CustomerCategoryID = CustomerCategories.CustomerCategoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN EntireTerritories ON Customers.TerritoryID = EntireTerritories.TerritoryID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SalesInvoiceDetails.LocationID = Locations.LocationID " + "\r\n";
            queryString = queryString + "                   INNER JOIN VWCommodityCategories ON Commodities.CommodityCategoryID = VWCommodityCategories.CommodityCategoryID " + "\r\n";

            if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice)
            {
                queryString = queryString + "               LEFT JOIN GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
                queryString = queryString + "               LEFT JOIN WarehouseBalancePrice ON SalesInvoiceDetails.CommodityID = WarehouseBalancePrice.CommodityID AND MONTH(SalesInvoiceDetails.EntryDate) = MONTH(WarehouseBalancePrice.EntryDate) AND YEAR(SalesInvoiceDetails.EntryDate) = YEAR(WarehouseBalancePrice.EntryDate) " + "\r\n";
            }
            else
                if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice)
                    queryString = queryString + "           INNER JOIN GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID " + "\r\n";
                else
                    if (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.PartsInvoice)
                        queryString = queryString + "       INNER JOIN WarehouseBalancePrice ON SalesInvoiceDetails.CommodityID = WarehouseBalancePrice.CommodityID AND MONTH(SalesInvoiceDetails.EntryDate) = MONTH(WarehouseBalancePrice.EntryDate) AND YEAR(SalesInvoiceDetails.EntryDate) = YEAR(WarehouseBalancePrice.EntryDate) " + "\r\n";



            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

    }
}
