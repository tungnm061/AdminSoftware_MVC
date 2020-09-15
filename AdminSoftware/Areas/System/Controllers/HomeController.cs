using System.Linq;
using System.Web.Mvc;
using BusinessLogic.System;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;

namespace AdminSoftware.Areas.System.Controllers
{
    public class HomeController : BaseController
    {
        private readonly UserBll _userBll;

        public HomeController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        [SystemFilter]
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString()));
            return View();
        }

    }
}