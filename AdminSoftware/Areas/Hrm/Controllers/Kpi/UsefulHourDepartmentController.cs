using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class UsefulHourDepartmentController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly WorkDetailBll _workDetailBll;

        public UsefulHourDepartmentController()
        {
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
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
            ViewBag.StatusWorkDetail = from StatusWorkDetail s in Enum.GetValues(typeof (StatusWorkDetail))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            return View();
        }

        public JsonResult EmployeeUsefulHours(DateTime? fromDate, DateTime? toDate)
        {
            return Json(_workDetailBll.GetEmployeeUsefulHours(fromDate, toDate, UserLogin.DepartmentId ?? 0));
        }

        public ActionResult Employee()
        {
            return PartialView();
        }

        public JsonResult Employees(int id, DateTime fromDate, DateTime toDate)
        {
            var workDetails = _workDetailBll.GetWorkDetails(id, 1, fromDate, toDate);
            return Json(workDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}