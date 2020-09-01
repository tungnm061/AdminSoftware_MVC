using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.System.Models.Kpi;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;
using Entity.Kpi;
using Newtonsoft.Json;

namespace AdminSoftware.Areas.Sale.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController()
        {
        }

        [SaleFilter]
        public ActionResult Index()
        {
            return View();
        }
    }

    //

    //
}