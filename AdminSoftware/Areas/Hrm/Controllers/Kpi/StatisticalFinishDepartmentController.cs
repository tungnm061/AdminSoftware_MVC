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
    public class StatisticalFinishDepartmentController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly WorkDetailBll _workDetailBll;

        public StatisticalFinishDepartmentController()
        {
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
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

        public JsonResult StatisticalFinishs(DateTime? fromDate, DateTime? toDate)
        {
            var department = _departmentBll.GetDepartment(UserLogin.DepartmentId ?? 0);
            if (department == null || string.IsNullOrEmpty(department.Path))
            {
                return
                Json(_workDetailBll.GetFinishWorkKpis(fromDate, toDate, ""),
                    JsonRequestBehavior.AllowGet);
            }
            var path = department.Path;
            String[] elements = Regex.Split(path, "/");
            var newPath = elements[0] + "/" + elements[1] + "/" + elements[2] + "/";
            return Json(_workDetailBll.GetFinishWorkKpis(fromDate, toDate, newPath),
                JsonRequestBehavior.AllowGet);
        }


        public JsonResult WorkDetails(DateTime? fromDate, DateTime? toDate, int id)
        {
            var workDetails = _workDetailBll.GetWorkDetails(id, 1, fromDate, toDate);
            return Json(workDetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkDetail()
        {
            return PartialView();
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}