using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using Core.Enum;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class HolidayConfigController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly HolidayConfigBll _employeeHolidayBll;

        public HolidayConfigController()
        {
            _employeeHolidayBll = SingletonIpl.GetInstance<HolidayConfigBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        [HrmFilter]
        public ActionResult Index(int? year)
        {
            var years = new List<KendoForeignKeyModel>();
            for (var i = ((year ?? DateTime.Now.Year) - 2); i <= ((year ?? DateTime.Now.Year) + 2); i++)
            {
                years.Add(new KendoForeignKeyModel("Năm " + i, i.ToString()));
            }
            ViewBag.Years = years;
            ViewBag.Year = year ?? DateTime.Now.Year;
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            ViewBag.Genders = from SexEnum s in Enum.GetValues(typeof (SexEnum))
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

        public JsonResult HolidayConfigs(int year)
        {
            return Json(_employeeHolidayBll.GetHolidayConfigs(year,1), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(HolidayConfig model)
        {
            try
            {
                _employeeHolidayBll.Update(model);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}