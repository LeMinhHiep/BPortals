function cancelButton_Click() {
    window.parent.$("#myWindow").data("kendoWindow").close();
}

function handleOKEvent(accountInvoiceGridDataSource, pendingSalesInvoiceGridDataSource) {
    if (accountInvoiceGridDataSource != undefined && pendingSalesInvoiceGridDataSource != undefined) {
        var pendingSalesInvoiceGridDataItems = pendingSalesInvoiceGridDataSource.data();
        for (var i = 0; i < pendingSalesInvoiceGridDataItems.length; i++) {
            if (pendingSalesInvoiceGridDataItems[i].IsSelected === true)
                _setParentInput(accountInvoiceGridDataSource, pendingSalesInvoiceGridDataItems[i]);
        }
        cancelButton_Click();
    }




    function _setParentInput(accountInvoiceGridDataSource, transferOrderGridDataItem) {

        var dataRow = accountInvoiceGridDataSource.add({});

        dataRow.set("SalesInvoiceDetailID", transferOrderGridDataItem.SalesInvoiceDetailID);

        dataRow.set("CustomerID", transferOrderGridDataItem.CustomerID);
        dataRow.set("CommodityID", transferOrderGridDataItem.CommodityID);
        dataRow.set("CommodityName", transferOrderGridDataItem.CommodityName);
        dataRow.set("CommodityCode", transferOrderGridDataItem.CommodityCode);
        dataRow.set("CommodityTypeID", transferOrderGridDataItem.CommodityTypeID);

        

        dataRow.set("Quantity", transferOrderGridDataItem.Quantity);
        dataRow.set("ListedPrice", transferOrderGridDataItem.ListedPrice);
        dataRow.set("DiscountPercent", transferOrderGridDataItem.DiscountPercent);
        dataRow.set("UnitPrice", transferOrderGridDataItem.UnitPrice);
        dataRow.set("VATPercent", transferOrderGridDataItem.VATPercent);
        dataRow.set("GrossPrice", transferOrderGridDataItem.GrossPrice);
        dataRow.set("Amount", transferOrderGridDataItem.Amount);
        dataRow.set("VATAmount", transferOrderGridDataItem.VATAmount);
        dataRow.set("GrossAmount", transferOrderGridDataItem.GrossAmount);

        dataRow.set("IsBonus", transferOrderGridDataItem.IsBonus);
        dataRow.set("IsWarrantyClaim", transferOrderGridDataItem.IsWarrantyClaim);

        dataRow.set("ChassisCode", transferOrderGridDataItem.ChassisCode);
        dataRow.set("EngineCode", transferOrderGridDataItem.EngineCode);
        dataRow.set("ColorCode", transferOrderGridDataItem.ColorCode);
    }
}

