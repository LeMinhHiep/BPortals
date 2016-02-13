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

            this.SalesInvoiceByServiceContract();

            this.GoodsReceiptJournal();
        }

        
        #region SalesInvoiceJournal
        private void SalesInvoiceJournal()
        {
            string queryString = " @LocationID int, @SalesInvoiceTypeID int, @FromDate DateTime, @ToDate DateTime, @WithAccountInvoice bit " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalLocationID int, @LocalSalesInvoiceTypeID int, @LocalFromDate DateTime, @LocalToDate DateTime, @LocalWithAccountInvoice bit " + "\r\n";
            queryString = queryString + "       SET         @LocalLocationID = @LocationID  SET @LocalSalesInvoiceTypeID = @SalesInvoiceTypeID  SET @LocalFromDate = @FromDate  SET @LocalToDate = @ToDate  SET @LocalWithAccountInvoice = @WithAccountInvoice " + "\r\n";

            queryString = queryString + "       IF          (@LocalSalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.AllInvoice + ") " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.AllInvoice) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalSalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice + ") " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceJournalBuild(GlobalEnums.SalesInvoiceTypeID.VehiclesInvoice) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalSalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.PartsInvoice + ")  " + "\r\n";
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
            queryString = queryString + "       IF          (@LocalLocationID = 0) " + "\r\n";
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
            queryString = queryString + "       IF          (@LocalWithAccountInvoice = 0) " + "\r\n";
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
            queryString = queryString + "                   INNER JOIN Commodities ON SalesInvoiceDetails.EntryDate >= @LocalFromDate AND SalesInvoiceDetails.EntryDate <= @LocalToDate " + (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice ? "" : " AND SalesInvoiceDetails.SalesInvoiceTypeID = @LocalSalesInvoiceTypeID") + (locationFilter ? " AND SalesInvoiceDetails.LocationID = @LocalLocationID" : "") + " AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";

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


        #endregion SalesInvoiceJournal








        

        #region SalesInvoiceByServiceContract

        private void SalesInvoiceByServiceContract()
        {
            string queryString = " @LocationID int, @SalesInvoiceTypeID int, @FromDate DateTime, @ToDate DateTime, @IsRegularCheckUps bit " + "\r\n";
            
            //queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalLocationID int, @LocalSalesInvoiceTypeID int, @LocalFromDate DateTime, @LocalToDate DateTime, @LocalIsRegularCheckUps bit " + "\r\n";
            queryString = queryString + "       SET         @LocalLocationID = @LocationID  SET @LocalSalesInvoiceTypeID = @SalesInvoiceTypeID  SET @LocalFromDate = @FromDate  SET @LocalToDate = @ToDate  SET @LocalIsRegularCheckUps = @IsRegularCheckUps " + "\r\n";

            queryString = queryString + "       IF          (@LocalIsRegularCheckUps = 1) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceByServiceContractBuild(GlobalEnums.SalesInvoiceTypeID.AllInvoice, true) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalSalesInvoiceTypeID = " + (int)GlobalEnums.SalesInvoiceTypeID.AllInvoice + ") " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceByServiceContractBuild(GlobalEnums.SalesInvoiceTypeID.AllInvoice, false) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n"; //LAY BAT KY GlobalEnums.SalesInvoiceTypeID LAM DAI DIEN, KHONG CO Y NGHIA GI (LAY GlobalEnums.SalesInvoiceTypeID.ServicesInvoice LAM DAI DIEN)
            queryString = queryString + "                   " + this.SalesInvoiceByServiceContractBuild(GlobalEnums.SalesInvoiceTypeID.ServicesInvoice, false) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("SalesInvoiceByServiceContract", queryString);
        }

        private string SalesInvoiceByServiceContractBuild(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool isRegularCheckUps)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocalLocationID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceByServiceContractBuildDetail(salesInvoiceTypeID, false, isRegularCheckUps) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.SalesInvoiceByServiceContractBuildDetail(salesInvoiceTypeID, true, isRegularCheckUps) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }

        private string SalesInvoiceByServiceContractBuildDetail(GlobalEnums.SalesInvoiceTypeID salesInvoiceTypeID, bool locationFilter, bool isRegularCheckUps)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      SalesInvoiceDetails.SalesInvoiceID, SalesInvoiceDetails.SalesInvoiceDetailID, SalesInvoiceDetails.EntryDate, SalesInvoiceDetails.LocationID, Locations.Code AS LocationCode, SalesInvoiceDetails.SalesInvoiceTypeID, " + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails.CommodityID, Commodities.Code, Commodities.Name, SalesInvoiceDetails.Quantity, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, " + "\r\n";
            queryString = queryString + "                   ServiceContracts.CommodityID AS VehicleID, Vehicles.Code AS VehicleCode, Vehicles.Name AS VehicleName, ServiceContracts.ChassisCode, ServiceContracts.EngineCode, ServiceContracts.ColorCode, ServiceContracts.LicensePlate, SalesInvoiceDetails.CurrentMeters, " + "\r\n";
            queryString = queryString + "                   ServiceContracts.PurchaseDate, ServiceContracts.AgentName, Customers.Name AS CustomerName, Customers.AddressNo AS CustomerAddressNo " + "\r\n";

            queryString = queryString + "       FROM        SalesInvoiceDetails " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities ON SalesInvoiceDetails.EntryDate >= @LocalFromDate AND SalesInvoiceDetails.EntryDate <= @LocalToDate " + (isRegularCheckUps ? " AND Commodities.IsRegularCheckUps = 1" : "") + (salesInvoiceTypeID == GlobalEnums.SalesInvoiceTypeID.AllInvoice ? "" : " AND SalesInvoiceDetails.SalesInvoiceTypeID = @LocalSalesInvoiceTypeID") + (locationFilter ? " AND SalesInvoiceDetails.LocationID = @LocalLocationID" : "") + " AND SalesInvoiceDetails.CommodityID = Commodities.CommodityID " + "\r\n";

            queryString = queryString + "                   INNER JOIN ServiceContracts ON SalesInvoiceDetails.ServiceContractID = ServiceContracts.ServiceContractID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Commodities AS Vehicles ON ServiceContracts.CommodityID = Vehicles.CommodityID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Customers ON ServiceContracts.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "                   INNER JOIN Locations ON SalesInvoiceDetails.LocationID = Locations.LocationID " + "\r\n";

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }

        #endregion SalesInvoiceByServiceContract





















        #region GoodsReceiptJournal

        private void GoodsReceiptJournal()
        {
            string queryString = " @LocationID int, @GoodsReceiptTypeID int, @FromDate DateTime, @ToDate DateTime " + "\r\n";

            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE     @LocalLocationID int, @LocalGoodsReceiptTypeID int, @LocalFromDate DateTime, @LocalToDate DateTime " + "\r\n";
            queryString = queryString + "       SET         @LocalLocationID = @LocationID  SET @LocalGoodsReceiptTypeID = @GoodsReceiptTypeID  SET @LocalFromDate = @FromDate  SET @LocalToDate = @ToDate " + "\r\n";

            queryString = queryString + "       IF  (@LocalGoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt + ") " + "\r\n";
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuild(GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt) + "\r\n";
            queryString = queryString + "       ELSE    IF  (@LocalGoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + ") " + "\r\n";
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuild(GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n"; //GlobalEnums.GoodsReceiptTypeID.StockTransfer
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuild(GlobalEnums.GoodsReceiptTypeID.StockTransfer) + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GoodsReceiptJournal", queryString);
        }

        private string GoodsReceiptJournalBuild(GlobalEnums.GoodsReceiptTypeID goodsReceiptTypeID)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";
            queryString = queryString + "       IF          (@LocalLocationID = 0) " + "\r\n";
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuildDetail(goodsReceiptTypeID, false) + "\r\n";
            queryString = queryString + "       ELSE        " + "\r\n";
            queryString = queryString + "                   " + this.GoodsReceiptJournalBuildDetail(goodsReceiptTypeID, true) + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            return queryString;

        }

        private string GoodsReceiptJournalBuildDetail(GlobalEnums.GoodsReceiptTypeID goodsReceiptTypeID, bool locationFilter)
        {
            string queryString = "";

            queryString = queryString + "   BEGIN " + "\r\n";

            if (goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt || goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice)
            {
                queryString = queryString + "       SELECT      GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.GoodsReceiptTypeID, GoodsReceiptDetails.VoucherID, GoodsReceiptDetails.LocationID, Locations.Code AS LocationCode, GoodsReceiptDetails.CommodityID, Commodities.Code, Commodities.Name, Commodities.CommodityTypeID, CommodityTypes.Name AS CommodityTypeName, " + "\r\n";
                queryString = queryString + "                   GoodsReceiptDetails.Quantity, GoodsReceiptDetails.UnitPrice, GoodsReceiptDetails.VATPercent, GoodsReceiptDetails.GrossPrice, GoodsReceiptDetails.Amount, GoodsReceiptDetails.VATAmount, GoodsReceiptDetails.GrossAmount, " + "\r\n";
                queryString = queryString + "                   PurchaseInvoices.SupplierID AS VoucherOwnerID, Suppliers.OfficialName AS VoucherOwner, PurchaseInvoices.VATInvoiceNo AS VoucherReference, PurchaseInvoices.VATInvoiceDate AS VoucherDate, PurchaseOrders.ConfirmReference AS RootVoucherReference, PurchaseOrders.ConfirmDate AS RootVoucherDate, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode " + "\r\n";

                queryString = queryString + "       FROM        GoodsReceiptDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON GoodsReceiptDetails.EntryDate >= @LocalFromDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.PurchaseInvoice + (locationFilter ? " AND GoodsReceiptDetails.LocationID = @LocalLocationID" : "") + " AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";

                queryString = queryString + "                   INNER JOIN PurchaseInvoices ON GoodsReceiptDetails.VoucherID = PurchaseInvoices.PurchaseInvoiceID " + "\r\n";
                queryString = queryString + "                   INNER JOIN CommodityTypes ON Commodities.CommodityTypeID = CommodityTypes.CommodityTypeID  " + "\r\n";
                queryString = queryString + "                   INNER JOIN Customers Suppliers ON PurchaseInvoices.SupplierID = Suppliers.CustomerID  " + "\r\n";
                queryString = queryString + "                   INNER JOIN Locations ON GoodsReceiptDetails.LocationID = Locations.LocationID " + "\r\n";
                queryString = queryString + "                   INNER JOIN PurchaseOrders ON PurchaseInvoices.PurchaseOrderID = PurchaseOrders.PurchaseOrderID  " + "\r\n";
            }

            if (goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt)
                queryString = queryString + "       UNION ALL   " + "\r\n";

            if (goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.AllGoodsReceipt || goodsReceiptTypeID == GlobalEnums.GoodsReceiptTypeID.StockTransfer)
            {
                queryString = queryString + "       SELECT      GoodsReceiptDetails.GoodsReceiptID, GoodsReceiptDetails.GoodsReceiptDetailID, GoodsReceiptDetails.EntryDate, GoodsReceiptDetails.GoodsReceiptTypeID, GoodsReceiptDetails.VoucherID, GoodsReceiptDetails.LocationID, Locations.Code AS LocationCode, GoodsReceiptDetails.CommodityID, Commodities.Code, Commodities.Name, Commodities.CommodityTypeID, CommodityTypes.Name AS CommodityTypeName, " + "\r\n";
                queryString = queryString + "                   GoodsReceiptDetails.Quantity, GoodsReceiptDetails.UnitPrice, GoodsReceiptDetails.VATPercent, GoodsReceiptDetails.GrossPrice, GoodsReceiptDetails.Amount, GoodsReceiptDetails.VATAmount, GoodsReceiptDetails.GrossAmount, " + "\r\n";
                queryString = queryString + "                   StockTransfers.LocationID AS VoucherOwnerID, StockTransferLocations.OfficialName AS VoucherOwner, StockTransfers.Reference AS VoucherReference, StockTransfers.EntryDate AS VoucherDate, TransferOrders.Reference AS RootVoucherReference, TransferOrders.EntryDate AS RootVoucherDate, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode " + "\r\n";

                queryString = queryString + "       FROM        GoodsReceiptDetails " + "\r\n";
                queryString = queryString + "                   INNER JOIN Commodities ON GoodsReceiptDetails.EntryDate >= @LocalFromDate AND GoodsReceiptDetails.EntryDate <= @LocalToDate AND GoodsReceiptDetails.GoodsReceiptTypeID = " + (int)GlobalEnums.GoodsReceiptTypeID.StockTransfer + (locationFilter ? " AND GoodsReceiptDetails.LocationID = @LocalLocationID" : "") + " AND GoodsReceiptDetails.CommodityID = Commodities.CommodityID " + "\r\n";

                queryString = queryString + "                   INNER JOIN StockTransfers ON GoodsReceiptDetails.VoucherID = StockTransfers.StockTransferID " + "\r\n";
                queryString = queryString + "                   INNER JOIN CommodityTypes ON Commodities.CommodityTypeID = CommodityTypes.CommodityTypeID  " + "\r\n";
                queryString = queryString + "                   INNER JOIN Locations StockTransferLocations ON StockTransfers.LocationID = StockTransferLocations.LocationID  " + "\r\n";
                queryString = queryString + "                   INNER JOIN Locations ON GoodsReceiptDetails.LocationID = Locations.LocationID " + "\r\n";
                queryString = queryString + "                   LEFT JOIN TransferOrders ON StockTransfers.TransferOrderID = TransferOrders.TransferOrderID " + "\r\n";
            }

            queryString = queryString + "   END " + "\r\n";

            return queryString;
        }
        #endregion GoodsReceiptJournal

    }
}
