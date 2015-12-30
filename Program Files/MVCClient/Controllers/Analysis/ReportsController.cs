using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVCCore.Repositories.Analysis;
using MVCModel.Models;
using RequireJsNet;
using MVCClient.Api.SessionTasks;
using System.Net;
using MVCClient.ViewModels.Helpers;

namespace MVCClient.Controllers.Analysis
{
    public class ReportsController : CoreController
    {

        private IReportRepository reportRepository;
        public ReportsController(IReportRepository reportRepository)
        {
            this.reportRepository = reportRepository;
        }

        public ActionResult Index()
        {
            MenuSession.SetModuleID(this.HttpContext, 9);                

            //RequireJsOptions.Add("LocationID", this.baseService.LocationID, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnModuleID", 9, RequireJsOptionsScope.Page);
            RequireJsOptions.Add("NmvnTaskID", 0, RequireJsOptionsScope.Page);

            return View(this.reportRepository.GetReports());
        }



        public ActionResult Open(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Report report = this.reportRepository.GetReports().Where(w => w.ReportUniqueID == id).FirstOrDefault();
            if (report == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            PrintViewModel printViewModel = new PrintViewModel() { Id = 1, ReportPath = report.ReportURL};

            return View(viewName: "Open", model: printViewModel);
        }



    }
}