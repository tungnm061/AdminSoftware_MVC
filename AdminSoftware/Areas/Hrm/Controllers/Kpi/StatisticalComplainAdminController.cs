using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class StatisticalComplainAdminController : BaseController
    {
        private readonly ComplainBll _complainBll;
        private readonly DepartmentBll _departmentBll;
        private readonly WorkDetailBll _workDetailBll;

        public StatisticalComplainAdminController()
        {
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _complainBll = SingletonIpl.GetInstance<ComplainBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.StatusComplain = from StatusComplain s in Enum.GetValues(typeof (StatusComplain))
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

        public JsonResult StatisticalComplains(DateTime? fromDate, DateTime? toDate, long? departmentId)
        {
            var department = _departmentBll.GetDepartment(departmentId ?? 0);
            if (department == null || string.IsNullOrEmpty(department.Path))
            {
                return
                Json(_workDetailBll.GetComplainKpis(fromDate, toDate, ""),
                    JsonRequestBehavior.AllowGet);
            }
            var path = department.Path;
            String[] elements = Regex.Split(path, "/");
            var newPath = elements[0] + "/" + elements[1] + "/" + elements[2] + "/";
            return Json(_workDetailBll.GetComplainKpis(fromDate, toDate, newPath));
        }


        public JsonResult ComplainDetails(DateTime? fromDate, DateTime? toDate, int? id)
        {
            return Json(_complainBll.GetComplains_AccusedBy(fromDate, toDate, id,1));
        }

        public ActionResult ComplainDetail()
        {
            return PartialView();
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}