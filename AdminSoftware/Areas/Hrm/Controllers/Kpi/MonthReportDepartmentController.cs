using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class MonthReportDepartmentController : BaseController
    {
        private readonly WorkDetailBll _workDetailBll;
        private readonly DepartmentBll _departmentBll;
        public MonthReportDepartmentController()
        {
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }
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
        public JsonResult WeekReports(DateTime monthlyDate)
        {
            var date = new DateTime(monthlyDate.Year, monthlyDate.Month, 1);
            var newmonthlyDate = date.AddMonths(+1);
            var newmonthlyDay = newmonthlyDate.AddDays(-1);
            var department = _departmentBll.GetDepartment(UserLogin.DepartmentId ?? 0);
            if (department == null || string.IsNullOrEmpty(department.Path))
            {
                return
                Json(_workDetailBll.GetWorkDetailsByPath("", 6, date, newmonthlyDay),
                    JsonRequestBehavior.AllowGet);
            }
            var workDetails =
                _workDetailBll.GetWorkDetailsByPath(department.Path, 6, date, newmonthlyDay)
                    .OrderBy(x => x.CreateBy)
                    .ThenBy(y => y.TaskCode);
            return Json(workDetails, JsonRequestBehavior.AllowGet);
        }
    }
}