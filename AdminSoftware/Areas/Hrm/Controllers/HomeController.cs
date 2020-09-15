
using System.Web.Mvc;
using AdminSoftware.Controllers;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController() { }


        [HrmFilter]
        public ActionResult Index()
        {

            
            return View();
        }

      
    }
}