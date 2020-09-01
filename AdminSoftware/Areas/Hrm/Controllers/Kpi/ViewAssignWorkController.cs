using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class ViewAssignWorkController : BaseController
    {
        private readonly AssignWorkBll _assignWorkBll;

        public ViewAssignWorkController()
        {
            _assignWorkBll = SingletonIpl.GetInstance<AssignWorkBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.WorkDetailStatusEnum = from WorkDetailStatusEnum s in Enum.GetValues(typeof (WorkDetailStatusEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            return View();
        }

        public JsonResult AssignWorks(DateTime fromDate, DateTime toDate)
        {
            var assignWorks = _assignWorkBll.GetAssignWorks(null, null, UserLogin.UserId, fromDate, toDate);
            return Json(assignWorks, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignWork(string id)
        {
            return PartialView(_assignWorkBll.GetAssignWork(id));
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}