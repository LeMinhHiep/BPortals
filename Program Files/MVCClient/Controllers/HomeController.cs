using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using MVCClient.Models;
using MVCClient.ViewModels.Home;
using MVCClient.Api.SessionTasks;

namespace MVCClient.Controllers
{
    public class HomeController : CoreController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserGuide()
        {
            if (User.Identity.IsAuthenticated)
            {
                string aspUserID = User.Identity.GetUserId();

                var Db = new ApplicationDbContext();

                var userID = Db.Users.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Options()
        {
            OptionViewModel optionViewModel = new OptionViewModel { GlobalFromDate = HomeSession.GetGlobalFromDate(this.HttpContext), GlobalToDate = HomeSession.GetGlobalToDate(this.HttpContext) };

            return View(optionViewModel);
        }

        [HttpPost]
        public ActionResult Options(OptionViewModel optionViewModel)
        {
            HomeSession.SetGlobalFromDate(this.HttpContext, optionViewModel.GlobalFromDate);
            HomeSession.SetGlobalToDate(this.HttpContext, optionViewModel.GlobalToDate);

            return View("Index");
        }

    }
}