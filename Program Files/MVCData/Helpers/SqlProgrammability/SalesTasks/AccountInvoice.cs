using MVCBase;
using MVCBase.Enums;
using MVCModel.Models;

namespace MVCData.Helpers.SqlProgrammability.SalesTasks
{
    class AccountInvoice
    {
        private readonly TotalBikePortalsEntities totalBikePortalsEntities;

        public AccountInvoice(TotalBikePortalsEntities totalBikePortalsEntities)
        {
            this.totalBikePortalsEntities = totalBikePortalsEntities;
        }

        public void RestoreProcedure()
        {
            this.GetAccountInvoiceIndexes();

            this.GetAccountInvoiceViewDetails();
            this.AccountInvoiceSaveRelative();
            this.AccountInvoicePostSaveValidate();

            this.AccountInvoiceInitReference();
        }

        private void GetAccountInvoiceIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoices.AccountInvoiceID, CAST(AccountInvoices.EntryDate AS DATE) AS EntryDate, AccountInvoices.Reference, AccountInvoices.VATInvoiceNo, Locations.Code AS LocationCode, Customers.Name + ',    ' + Customers.AddressNo AS CustomerDescription, AccountInvoices.Description, AccountInvoices.TotalGrossAmount " + "\r\n";
            queryString = queryString + "       FROM        AccountInvoices INNER JOIN" + "\r\n";
            queryString = queryString + "                   Locations ON AccountInvoices.EntryDate >= @FromDate AND AccountInvoices.EntryDate <= @ToDate AND AccountInvoices.OrganizationalUnitID IN (SELECT AccessControls.OrganizationalUnitID FROM AccessControls INNER JOIN AspNetUsers ON AccessControls.UserID = AspNetUsers.UserID WHERE AspNetUsers.Id = @AspUserID AND AccessControls.NMVNTaskID = " + (int)MVCBase.Enums.GlobalEnums.NmvnTaskID.AccountInvoice + " AND AccessControls.AccessLevel > 0) AND Locations.LocationID = AccountInvoices.LocationID INNER JOIN " + "\r\n";
            queryString = queryString + "                   Customers ON AccountInvoices.CustomerID = Customers.CustomerID " + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetAccountInvoiceIndexes", queryString);
        }

        private void GetAccountInvoiceViewDetails()
        {
            string queryString;

            queryString = " @AccountInvoiceID Int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      AccountInvoiceDetails.AccountInvoiceDetailID, AccountInvoiceDetails.AccountInvoiceID, AccountInvoiceDetails.SalesInvoiceDetailID, Commodities.CommodityID, Commodities.Code AS CommodityCode, Commodities.Name AS CommodityName, GoodsReceiptDetails.ChassisCode, GoodsReceiptDetails.EngineCode, GoodsReceiptDetails.ColorCode, " + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails.Quantity, SalesInvoiceDetails.ListedPrice, SalesInvoiceDetails.DiscountPercent, SalesInvoiceDetails.UnitPrice, SalesInvoiceDetails.VATPercent, SalesInvoiceDetails.GrossPrice, SalesInvoiceDetails.Amount, SalesInvoiceDetails.VATAmount, SalesInvoiceDetails.GrossAmount, SalesInvoiceDetails.IsBonus, SalesInvoiceDetails.IsWarrantyClaim, AccountInvoiceDetails.Remarks" + "\r\n";
            queryString = queryString + "       FROM        AccountInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                   SalesInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @AccountInvoiceID AND AccountInvoiceDetails.SalesInvoiceDetailID = SalesInvoiceDetails.SalesInvoiceDetailID INNER JOIN" + "\r\n";
            queryString = queryString + "                   Commodities ON SalesInvoiceDetails.CommodityID = Commodities.CommodityID LEFT JOIN" + "\r\n";
            queryString = queryString + "                   GoodsReceiptDetails ON SalesInvoiceDetails.GoodsReceiptDetailID = GoodsReceiptDetails.GoodsReceiptDetailID" + "\r\n";
            queryString = queryString + "       " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("GetAccountInvoiceViewDetails", queryString);
        }


        private void AccountInvoiceSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           UPDATE      SalesInvoiceDetails" + "\r\n";
            queryString = queryString + "           SET         SalesInvoiceDetails.AccountInvoiceID = AccountInvoiceDetails.AccountInvoiceID " + "\r\n";
            queryString = queryString + "           FROM        SalesInvoiceDetails INNER JOIN" + "\r\n";
            queryString = queryString + "                       AccountInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND SalesInvoiceDetails.SalesInvoiceDetailID = AccountInvoiceDetails.SalesInvoiceDetailID " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           UPDATE      SalesInvoiceDetails" + "\r\n";
            queryString = queryString + "           SET         AccountInvoiceID = NULL " + "\r\n";
            queryString = queryString + "           WHERE       AccountInvoiceID = @EntityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalBikePortalsEntities.CreateStoredProcedure("AccountInvoiceSaveRelative", queryString);
        }

        private void AccountInvoicePostSaveValidate()
        {
            string[] queryArray = new string[1];

            queryArray[0] = " SELECT TOP 1 @FoundEntity = N'Ngày bán hàng: ' + CAST(SalesInvoiceDetails.EntryDate AS nvarchar) FROM AccountInvoiceDetails INNER JOIN SalesInvoiceDetails ON AccountInvoiceDetails.AccountInvoiceID = @EntityID AND AccountInvoiceDetails.SalesInvoiceDetailID = SalesInvoiceDetails.SalesInvoiceDetailID AND AccountInvoiceDetails.EntryDate < SalesInvoiceDetails.EntryDate ";

            this.totalBikePortalsEntities.CreateProcedureToCheckExisting("AccountInvoicePostSaveValidate", queryArray);
        }



        private void AccountInvoiceInitReference()
        {
            SimpleInitReference simpleInitReference = new SimpleInitReference("AccountInvoices", "AccountInvoiceID", "Reference", ModelSettingManager.ReferenceLength, ModelSettingManager.ReferencePrefix(GlobalEnums.NmvnTaskID.AccountInvoice));
            this.totalBikePortalsEntities.CreateTrigger("AccountInvoiceInitReference", simpleInitReference.CreateQuery());
        }
    }
}
