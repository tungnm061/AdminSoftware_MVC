using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class MonthlyReportController : BaseController
    {
        private readonly WorkDetailBll _workDetailBll;
        public MonthlyReportController()
        {
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
        }
        // GET: Hrm/MonthlyReport
        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.CalcType = from CalTypeNum s in Enum.GetValues(typeof(CalTypeNum))
                               let singleOrDefault =
                                   (DescriptionAttribute)
                                       s.GetType()
                                           .GetField(s.ToString())
                                           .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                           .SingleOrDefault()
                               where singleOrDefault != null
                               select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            return View();
        }

        public JsonResult MonthlyReports(DateTime monthlyDate)
        {
            var date = new DateTime(monthlyDate.Year, monthlyDate.Month, 1);
            var newmonthlyDate = date.AddMonths(+1);
            var newmonthlyDay = newmonthlyDate.AddDays(-1);

            var workDetails = _workDetailBll.GetWorkDetails(null, 9, date, newmonthlyDay).OrderBy(x=>x.CreateBy).ThenBy(y=>y.FromDate);
            return Json(workDetails, JsonRequestBehavior.AllowGet);
        }
    }
}