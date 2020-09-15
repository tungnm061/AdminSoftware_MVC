//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using BusinessLogic.Hrm;
//using BusinessLogic.Kpi;
//using Core.Helper.Logging;
//using Core.Singleton;
//using AdminSoftware.Controllers;
//using AdminSoftware.Models;
//using Entity.Hrm;

//namespace AdminSoftware.Areas.Hrm.Controllers
//{
//    public class SalaryTimeSheetController : BaseController
//    {
//        private readonly DepartmentBll _departmentBll;
//        private readonly EmployeeBll _employeeBll;
//        private readonly EmployeeHolidayBll _employeeHolidayBll;
//        private readonly HolidayBll _holidayBll;
//        private readonly IncurredSalaryBll _incurredSalaryBll;
//        private readonly SalaryBll _salaryBll;
//        private readonly ShiftWorkBll _shiftWorkBll;
//        private readonly TimeSheetBll _timeSheetBll;
//        private readonly TimeSheetOtBll _timeSheetOtBll;
//        private readonly WorkDetailBll _workDetailBll;

//        public SalaryTimeSheetController()
//        {
//            _timeSheetBll = SingletonIpl.GetInstance<TimeSheetBll>();
//            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
//            _holidayBll = SingletonIpl.GetInstance<HolidayBll>();
//            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
//            _employeeHolidayBll = SingletonIpl.GetInstance<EmployeeHolidayBll>();
//            _shiftWorkBll = SingletonIpl.GetInstance<ShiftWorkBll>();
//            _timeSheetOtBll = SingletonIpl.GetInstance<TimeSheetOtBll>();
//            _salaryBll = SingletonIpl.GetInstance<SalaryBll>();
//            _incurredSalaryBll = SingletonIpl.GetInstance<IncurredSalaryBll>();
//            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
//        }

//        [HrmFilter]
//        public ActionResult Index()
//        {
//            ViewBag.Departments =
//                _departmentBll.GetDepartments(null)
//                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
//            return View();
//        }

//        public JsonResult SalaryTimeSheets(DateTime date)
//        {
//            var fromDate = new DateTime(date.Year, date.Month, 1);
//            var toDate = fromDate.AddMonths(1).AddDays(-1);
//            var employees = _employeeBll.GetEmployeesSalary(true);
//            var count = Math.Round(
//                (decimal) employees.Count/10
//                );
//            var employeeTimeSheets = new List<EmployeeTimeSheet>();
//            var holidayNumbers = _holidayBll.GetHolidayByDates(fromDate, toDate).Count();
//            var dayDates = (int)
//                (DateTime.Parse(toDate.ToShortDateString()) - DateTime.Parse(fromDate.ToShortDateString()))
//                    .TotalDays -
//                           holidayNumbers + 1;
//            foreach (var item in employees)
//            {
//                var holidayDetails = _employeeHolidayBll.GetHolidayDetails(fromDate, toDate, item.EmployeeId);
//                var factorKpi = _workDetailBll.GetFactorWorkKpi(fromDate, toDate, item.EmployeeId, toDate.Day);
//                var totalDayHoliday = holidayDetails.Select(x => x.ToTalDays).Sum();
//                var dayPointOt =
//                    _timeSheetOtBll.GetTimeSheetOts(fromDate, toDate, item.EmployeeId).Select(x => x.DayPoints).Sum();
//                var employeeTimeSheet = new EmployeeTimeSheet
//                {
//                    EarlyNumber = 0,
//                    LateNumber = 0,
//                    FullName = item.FullName,
//                    DepartmentId = item.DepartmentId,
//                    DepartmentName = item.DepartmentName,
//                    EmployeeCode = item.EmployeeCode,
//                    LateMinutes = 0,
//                    EarlyMinutes = 0,
//                    EmployeeId = item.EmployeeId,
//                    RealPoint = dayDates,
//                    TotalPoint = 0,
//                    TotalDayHoliday = totalDayHoliday,
//                    DayPointOt = dayPointOt
//                };
//                decimal requirementTime = 0;
//                var lisCheckTimeSheets = new List<CheckTimeSheet>();
//                for (var start = fromDate; start <= toDate; start = start.AddDays(1))
//                {
//                    var timeSheet = _timeSheetBll.GetTimeSheetByEmployeeId(start, item.EmployeeId);

//                    var checkTimeSheet = new CheckTimeSheet
//                    {
//                        TimeSheetDate = start,
//                        CheckOut = "",
//                        CheckIn = ""
//                    };
//                    if (timeSheet != null)
//                    {
//                        var shiftWork = _shiftWorkBll.GetShiftWork(timeSheet.ShiftWorkId);
//                        requirementTime += shiftWork == null ? 0 : shiftWork.ToTalTimeHours;
//                        if (timeSheet.Checkout != null)
//                            checkTimeSheet.CheckOut = timeSheet.Checkout.Value.ToString("hh:mm");
//                        if (timeSheet.Checkin != null)
//                            checkTimeSheet.CheckIn = timeSheet.Checkin.Value.ToString("hh:mm");
//                        if (timeSheet.EarlyMinutes > 0)
//                        {
//                            employeeTimeSheet.EarlyMinutes = employeeTimeSheet.EarlyMinutes + timeSheet.EarlyMinutes;
//                            employeeTimeSheet.EarlyNumber = employeeTimeSheet.EarlyNumber + 1;
//                        }
//                        if (timeSheet.LateMinutes > 0)
//                        {
//                            employeeTimeSheet.LateMinutes = employeeTimeSheet.LateMinutes + timeSheet.LateMinutes;
//                            employeeTimeSheet.LateNumber = employeeTimeSheet.LateNumber + 1;
//                        }
//                        if (timeSheet.RealDays > 0)
//                        {
//                            employeeTimeSheet.TotalPoint = employeeTimeSheet.TotalPoint + timeSheet.RealDays;
//                        }
//                    }
//                    lisCheckTimeSheets.Add(checkTimeSheet);
//                }
//                employeeTimeSheet.CheckTimeSheets = lisCheckTimeSheets;
//                employeeTimeSheet.RealPoint = dayDates;
//                if (employeeTimeSheet.TotalPoint == 0)
//                {
//                    employeeTimeSheet.ActualHour = 0;
//                }
//                else
//                {
//                    if (employeeTimeSheet.TotalPoint != null)
//                        employeeTimeSheet.ActualHour = (decimal)(requirementTime - ((employeeTimeSheet.LateMinutes + employeeTimeSheet.EarlyMinutes) / 60));
//                }

//                employeeTimeSheet.FixedHour = requirementTime;
//                employeeTimeSheet.TotalDayPoint = employeeTimeSheet.TotalPoint + employeeTimeSheet.DayPointOt + employeeTimeSheet.TotalDayHoliday;
//                var salary = _salaryBll.GetSalaryByEmployeeId(item.EmployeeId);
//                employeeTimeSheet.BasicSalary = salary.BasicCoefficient*salary.BasicSalary;
//                employeeTimeSheet.ProfessionalSalary = salary.ProfessionalCoefficient*employeeTimeSheet.BasicSalary*
//                                                       (salary.PercentProfessional/100);
//                employeeTimeSheet.BaseSalary = salary.BasicSalary;
//                if (dayDates == 0)
//                {
//                    employeeTimeSheet.SalaryHoliday = 0;
//                    employeeTimeSheet.SalaryOt = 0;
//                }
//                else
//                {
//                    employeeTimeSheet.SalaryHoliday = salary.BasicSalary*(holidayDetails.Sum(x => x.ToTalDays)/dayDates);
//                    employeeTimeSheet.SalaryOt = (decimal) (salary.BasicSalary*employeeTimeSheet.DayPointOt/dayDates);
//                }
//                employeeTimeSheet.ResponsibilitySalary = salary.ResponsibilityCoefficient*employeeTimeSheet.BasicSalary;
//                var incurredSalary = _incurredSalaryBll.GetIncurredSalaries(item.EmployeeId, fromDate, toDate, true);
//                employeeTimeSheet.IncurredSalary = incurredSalary.Select(x => x.Amount).Sum();
//                employeeTimeSheet.FactorPoint = factorKpi.FactorPoint;
//                employeeTimeSheets.Add(employeeTimeSheet);
//            }
//            var a = 0;
//            foreach (
//                var item in
//                    employeeTimeSheets.Where(y => y.TotalEarlyLateMinutes < 60 && y.TotalEarlyLateMinutes > 0)
//                        .OrderByDescending(x => x.TotalEarlyLateMinutes)
//                        .ToList()
//                )
//            {
//                a = a + 1;
//                if (a <= count)
//                {
//                    item.TotalDaySalary60Minutes = item.TotalDayPoint ?? 0 - 1;
//                }
//                else
//                {
//                    item.TotalDaySalary60Minutes = item.TotalDayPoint;
//                }
//            }
//            return Json(employeeTimeSheets, JsonRequestBehavior.AllowGet);
//        }

//        public JsonResult ClearSession()
//        {
//            return null;
//        }
//    }
//}