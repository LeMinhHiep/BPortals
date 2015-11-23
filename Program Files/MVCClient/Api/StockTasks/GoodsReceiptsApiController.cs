using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using MVCModel.Models;
using MVCDTO.StockTasks;
using MVCCore.Repositories.StockTasks;


using Microsoft.AspNet.Identity;


namespace MVCClient.Api.StockTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class GoodsReceiptsApiController : Controller
    {
        private readonly IGoodsReceiptRepository goodsReceiptRepository;
        private readonly IGoodsReceiptAPIRepository goodsReceiptAPIRepository;

        public GoodsReceiptsApiController(IGoodsReceiptRepository goodsReceiptRepository, IGoodsReceiptAPIRepository goodsReceiptAPIRepository)
        {
            this.goodsReceiptRepository = goodsReceiptRepository;
            this.goodsReceiptAPIRepository = goodsReceiptAPIRepository;
        }

        public JsonResult GetGoodsReceiptIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<GoodsReceiptIndex> goodsReceiptIndexes = this.goodsReceiptAPIRepository.GetEntityIndexes<GoodsReceiptIndex>(User.Identity.GetUserId(), DateTime.Today.AddDays(-1000), DateTime.Today.AddDays(360));

            DataSourceResult response = goodsReceiptIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetPurchaseInvoices([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? goodsReceiptID, string purchaseInvoiceReference)
        {
            ICollection<GoodsReceiptGetPurchaseInvoice> GoodsReceiptGetPurchaseInvoices = this.goodsReceiptRepository.GetPurchaseInvoices(locationID, goodsReceiptID, purchaseInvoiceReference);
            return Json(GoodsReceiptGetPurchaseInvoices.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStockTransfers([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? goodsReceiptID, string stockTransferReference)
        {
            ICollection<GoodsReceiptGetStockTransfer> GoodsReceiptGetStockTransfers = this.goodsReceiptRepository.GetStockTransfers(locationID, goodsReceiptID, stockTransferReference);
            return Json(GoodsReceiptGetStockTransfers.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }      

    }
}