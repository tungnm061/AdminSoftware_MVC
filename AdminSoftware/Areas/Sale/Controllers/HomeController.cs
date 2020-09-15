using System;
using System.Web.Mvc;
using AdminSoftware.Controllers;

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

}