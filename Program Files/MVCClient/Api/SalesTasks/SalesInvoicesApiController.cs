using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.UI;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


using MVCBase.Enums;
using MVCModel.Models;

using MVCDTO.SalesTasks;

using MVCCore.Repositories.SalesTasks;
using MVCClient.ViewModels.SalesTasks;
using System.Collections.Generic;


using Microsoft.AspNet.Identity;



namespace MVCClient.Api.SalesTasks
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class VehiclesInvoiceApiController : Controller
    {
        private readonly IVehiclesInvoiceAPIRepository vehiclesInvoiceAPIRepository;

        public VehiclesInvoiceApiController(IVehiclesInvoiceAPIRepository vehiclesInvoiceAPIRepository)
        {
            this.vehiclesInvoiceAPIRepository = vehiclesInvoiceAPIRepository;
        }

        public JsonResult GetVehiclesInvoiceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<VehiclesInvoiceIndex> vehiclesInvoiceIndexes = this.vehiclesInvoiceAPIRepository.GetEntityIndexes<VehiclesInvoiceIndex>(User.Identity.GetUserId(), DateTime.Today.AddDays(-1000), DateTime.Today.AddDays(360));

            DataSourceResult response = vehiclesInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
       
    }



    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class PartsInvoiceApiController : Controller
    {
        private readonly IPartsInvoiceAPIRepository partsInvoiceAPIRepository;

        public PartsInvoiceApiController(IPartsInvoiceAPIRepository partsInvoiceAPIRepository)
        {
            this.partsInvoiceAPIRepository = partsInvoiceAPIRepository;
        }


        public JsonResult GetPartsInvoiceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<PartsInvoiceIndex> partsInvoiceIndexes = this.partsInvoiceAPIRepository.GetEntityIndexes<PartsInvoiceIndex>(User.Identity.GetUserId(), DateTime.Today.AddDays(-1000), DateTime.Today.AddDays(360));

            DataSourceResult response = partsInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }






    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ServicesInvoiceApiController : Controller
    {
        private readonly IServicesInvoiceRepository servicesInvoiceRepository;
        private readonly IServicesInvoiceAPIRepository servicesInvoiceAPIRepository;

        public ServicesInvoiceApiController(IServicesInvoiceRepository servicesInvoiceRepository, IServicesInvoiceAPIRepository servicesInvoiceAPIRepository)
        {
            this.servicesInvoiceRepository = servicesInvoiceRepository;
            this.servicesInvoiceAPIRepository = servicesInvoiceAPIRepository;
        }


        public JsonResult GetServicesInvoiceIndexes([DataSourceRequest] DataSourceRequest request)
        {
            ICollection<ServicesInvoiceIndex> servicesInvoiceIndexes = this.servicesInvoiceAPIRepository.GetEntityIndexes<ServicesInvoiceIndex>(User.Identity.GetUserId(), DateTime.Today.AddDays(-1000), DateTime.Today.AddDays(360));

            DataSourceResult response = servicesInvoiceIndexes.ToDataSourceResult(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetActiveServiceInvoices([DataSourceRequest] DataSourceRequest dataSourceRequest, int locationID, int? serviceInvoiceID, string licensePlate, int isFinished)
        {
            if (licensePlate == "") return Json(null);

            var result = servicesInvoiceRepository.GetActiveServiceInvoices(locationID, serviceInvoiceID, licensePlate, isFinished).Select(s => new
            {
                s.SalesInvoiceID,
                s.Reference,
                s.EntryDate,

                s.QuotationID,
                QuotationReference = s.Quotation != null ? s.Quotation.Reference : null,
                QuotationEntryDate = s.Quotation != null ? s.Quotation.EntryDate : (DateTime?)null,

                s.CustomerID,
                CustomerName = s.Customer.Name,
                CustomerBirthday = s.Customer.Birthday,
                CustomerTelephone = s.Customer.Telephone,
                CustomerAddressNo = s.Customer.AddressNo,
                CustomerEntireTerritoryEntireName = s.Customer.EntireTerritory.Name,

                s.ServiceContractID,
                ServiceContractReference = s.ServiceContract.Reference,
                ServiceContractCommodityID = s.ServiceContract.CommodityID,
                ServiceContractCommodityCode = s.ServiceContract.Commodity.Code,
                ServiceContractCommodityName = s.ServiceContract.Commodity.Name,
                ServiceContractLicensePlate = s.ServiceContract.LicensePlate,
                ServiceContractColorCode = s.ServiceContract.ColorCode,
                ServiceContractChassisCode = s.ServiceContract.ChassisCode,
                ServiceContractEngineCode = s.ServiceContract.EngineCode,
                ServiceContractPurchaseDate = s.ServiceContract.PurchaseDate,
                ServiceContractAgentName = s.ServiceContract.AgentName
            });
            return Json(result.ToDataSourceResult(dataSourceRequest), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 0)]
        public JsonResult GetRelatedPartsInvoiceValue(int serviceInvoiceID)
        {
            try
            {
                var relatedPartsInvoiceValue = this.servicesInvoiceRepository.GetRelatedPartsInvoiceValue(serviceInvoiceID);
                return Json(new
                {
                    NoInvoice = relatedPartsInvoiceValue[0].NoInvoice,
                    TotalPartsAmount = relatedPartsInvoiceValue[0].TotalGrossAmount
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

    }
}