using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Enum;
using Core.Singleton;
using AdminSoftware.Models;

namespace AdminSoftware.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly ModuleGroupBll _moduleGroupBll;
        private readonly NotificationBll _notificationBll;

        public DashboardController()
        {
            _moduleGroupBll = SingletonIpl.GetInstance<ModuleGroupBll>();
            _notificationBll = SingletonIpl.GetInstance<NotificationBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        public ActionResult Index()
        {
            Session["Rights"] = null;
            return View(_moduleGroupBll.GetModuleGroups(UserLogin.UserId, UserLogin.RoleId));
        }

        public ActionResult DepartmentIndex()
        {
            ViewBag.DepartmentConpanyEnums = from DepartmentConpanyEnum s in Enum.GetValues(typeof(DepartmentConpanyEnum))
                                             let singleOrDefault =
                                                 (DescriptionAttribute)
                                                     s.GetType()
                                                         .GetField(s.ToString())
                                                         .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                         .SingleOrDefault()
                                             where singleOrDefault != null
                                             select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            ViewBag.Sexs = from SexEnum s in Enum.GetValues(typeof (SexEnum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            var departments = _departmentBll.GetDepartments(true);
            ViewBag.DepartmentObjects = departments;
            return PartialView();
        }

        public JsonResult Employees(string path)
        {
            var employees = _employeeBll.GetEmployees(null);
            if (string.IsNullOrEmpty(path))
            {
                return Json(employees, JsonRequestBehavior.AllowGet);
            }
            var employeesBypath = _employeeBll.GetEmployeesByPath(path);
            return Json(employeesBypath, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Notifications()
        {
            var notifications = _notificationBll.GetNotifications(UserLogin.UserId);
            return Json(notifications, JsonRequestBehavior.AllowGet);
        }
    }
}