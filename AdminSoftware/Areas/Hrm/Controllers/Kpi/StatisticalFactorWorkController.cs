using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Kpi;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class StatisticalFactorWorkController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly FactorConfigBll _factorConfigBll;
        private readonly HolidayBll _holidayBll;
        private readonly WorkDetailBll _workDetailBll;

        public StatisticalFactorWorkController()
        {
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _holidayBll = SingletonIpl.GetInstance<HolidayBll>();
            _factorConfigBll = SingletonIpl.GetInstance<FactorConfigBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            var years = new List<KendoForeignKeyModel>();
            for (var i = ((DateTime.Now.Year) - 10); i <= (DateTime.Now.Year); i++)
            {
                years.Add(new KendoForeignKeyModel("Năm " + i, i.ToString()));
            }
            var months = new List<KendoForeignKeyModel>();
            for (var i = 1; i <= 12; i++)
            {
                months.Add(new KendoForeignKeyModel("Tháng " + i, i.ToString()));
            }
            ViewBag.Months = months;
            ViewBag.Years = years;
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            return View();
        }

        public JsonResult StatisticalFactorWorks(int? monthDate, int? yearDate)
        {
            var fromDate = DateTime.Now;
            var toDate = DateTime.Now;
            var totalDay = 1;

            if (monthDate == null && yearDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, date.AddMonths(+1).AddDays(-1).Day);

                totalDay = toDate.Day;
            }
            if (monthDate == null && yearDate != null)
            {
                fromDate = new DateTime((int) yearDate, 1, 1);
                var date = new DateTime((int) yearDate + 1, 1, 1);
                toDate = new DateTime((int) yearDate, 12, date.AddDays(-1).Day);
                totalDay = 365;
            }
            if (monthDate != null && yearDate == null)
            {
                fromDate = new DateTime(DateTime.Now.Year, (int) monthDate, 1);
                var date = new DateTime(DateTime.Now.Year, (int) monthDate, 1);
                toDate = new DateTime(DateTime.Now.Year, (int) monthDate, date.AddMonths(+1).AddDays(-1).Day);
                totalDay = toDate.Day;
            }
            if (monthDate != null && yearDate != null)
            {
                fromDate = new DateTime((int) yearDate, (int) monthDate, 1);
                var date = new DateTime((int) yearDate, (int) monthDate, 1);
                toDate = new DateTime((int) yearDate, (int) monthDate, date.AddMonths(+1).AddDays(-1).Day);
                totalDay = toDate.Day;
            }
            var factorWorks = _workDetailBll.GetFactorWorkKpisNew(fromDate, toDate, null, totalDay);
            var employees = new List<StatisticalFactorWork>();
            foreach (var item in factorWorks)
            {
                item.NumberEmployee = 0;
                if (item.DepartmentCompany == 1)
                {
                    employees = factorWorks.Where(x => x.DepartmentCompany != 1).ToList();
                }
                if (item.DepartmentCompany == 2)
                {
                    employees =
                        factorWorks.Where(
                            x =>
                                 x.DepartmentCompany != 2 &&
                                x.DepartmentCompany != 1).ToList();
                }
                if (item.DepartmentCompany == 3)
                {
                    var factorWorks2 = new List<StatisticalFactorWork>();
                    employees =
                        factorWorks.Where(
                            x =>
                                x.ViceDirectorManagement == true).ToList();
                    var numberEmployees = employees.Count();
                    foreach (var item2 in employees.Where(x=>x.DepartmentCompany==4))
                    {

                        var factorWork2 = _workDetailBll.GetFactorWorkKpisNew(fromDate, toDate, null, totalDay);
                        var employees2 =
                        factorWorks.Where(x => x.DepartmentId == item2.DepartmentId && x.DepartmentCompany == 5)
                            .ToList();
                        var numberEmployees2 = employees2.Count();
                        item2.UsefulHoursTask =
                        employees2.Sum(y => y.UsefulHoursTask);
                        item2.UsefulHoursSuggesWork =
                            employees2.Sum(y => y.UsefulHoursSuggesWork);
                        item2.UsefullHourMin = item2.UsefullHourMin * numberEmployees;

                        item2.NumberEmployee = numberEmployees2;
                      
                        factorWorks2.Add(item2);
                    }
                    item.AvgPointTt = factorWorks2.Sum(x => x.AvgPoint) +
                                      employees.Where(x => x.DepartmentCompany != 4).Sum(x => x.AvgPoint);
                    item.NumberEmployee = numberEmployees;
                    var factorConfigs = _factorConfigBll.GetFactorConfigs();
                    var factorConfig =
                        factorConfigs.Where(
                            x => x.FactorConditionMin <= item.AvgPoint && x.FactorConditionMax > item.AvgPoint)
                            .FirstOrDefault();
                    if (factorConfig != null)
                    {
                        item.FactorPoint = factorConfig.FactorPointMax;
                        item.FactorType = factorConfig.FactorType;
                    }
                    if (item.AvgPoint <= 0)
                    {
                        item.FactorPoint = 0;
                        item.FactorType = "";
                    }
                }
                if (item.DepartmentCompany == 4)
                {
                    employees =
                        factorWorks.Where(x => x.DepartmentId == item.DepartmentId && x.DepartmentCompany == 5)
                            .ToList();
                }
    
       
                if (item.DepartmentCompany != 5 && item.DepartmentCompany != 3)
                {
                    var numberEmployees = employees.Count();
                    var employeeKpis = employees.Where(x => x.DepartmentCompany == 5).ToList();
                    item.UsefulHoursTask =
                        employeeKpis.Sum(y => y.UsefulHoursTask);
                    item.UsefulHoursSuggesWork =
                        employeeKpis.Sum(y => y.UsefulHoursSuggesWork);
                    item.UsefullHourMin = item.UsefullHourMin*numberEmployees;

                    item.NumberEmployee = numberEmployees;
                    var factorConfigs = _factorConfigBll.GetFactorConfigs();
                    var factorConfig =
                        factorConfigs.Where(
                            x => x.FactorConditionMin <= item.AvgPoint && x.FactorConditionMax > item.AvgPoint)
                            .FirstOrDefault();
                    if (factorConfig != null)
                    {
                        item.FactorPoint = factorConfig.FactorPointMax;
                        item.FactorType = factorConfig.FactorType;
                    }
                    if (item.AvgPoint <= 0)
                    {
                        item.FactorPoint = 0;
                        item.FactorType = "";
                    }
                }
            }
            return Json(factorWorks, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}