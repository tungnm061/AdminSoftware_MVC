using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class EmployeeTimeSheetController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly EmployeeHolidayBll _employeeHolidayBll;
        private readonly HolidayBll _holidayBll;
        private readonly ShiftWorkBll _shiftWorkBll;
        private readonly TimeSheetBll _timeSheetBll;
        private readonly TimeSheetOtBll _timeSheetOtBll;

        public EmployeeTimeSheetController()
        {
            _timeSheetBll = SingletonIpl.GetInstance<TimeSheetBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _holidayBll = SingletonIpl.GetInstance<HolidayBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _employeeHolidayBll = SingletonIpl.GetInstance<EmployeeHolidayBll>();
            _shiftWorkBll = SingletonIpl.GetInstance<ShiftWorkBll>();
            _timeSheetOtBll = SingletonIpl.GetInstance<TimeSheetOtBll>();
        }

        [HrmFilter]
        public ActionResult Index(DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            var listDay = new List<String>();
            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);
            if (toDate == null)
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);
            ViewBag.FromDate = fromDate.Value.ToString("dd/MM/yyyy");
            ViewBag.ToDate = toDate.Value.ToString("dd/MM/yyyy");
            for (var start = fromDate; start <= toDate; start = start.Value.AddDays(1))
            {
                listDay.Add(start.Value.ToString("dd/MM/yyyy"));
            }
            ViewBag.Days = listDay;
            return View();
        }

        public JsonResult EmployeeTimeSheets(DateTime fromDate, DateTime toDate)
        {
            var employees = _employeeBll.GetEmployeesTimeSheet(true);
            var employeeTimeSheets = new List<EmployeeTimeSheet>();
            var holidayNumbers = _holidayBll.GetHolidayByDates(fromDate, toDate).Count();
            var dayDates =
                (int)
                    (DateTime.Parse(toDate.ToShortDateString()) - DateTime.Parse(fromDate.ToShortDateString()))
                        .TotalDays -
                holidayNumbers;
            foreach (var item in employees)
            {
                var holidayDetails = _employeeHolidayBll.GetHolidayDetails(fromDate, toDate, item.EmployeeId);
                var totalDayHoliday = holidayDetails.Select(x => x.ToTalDays).Sum();
                var dayPointOt =
                   _timeSheetOtBll.GetTimeSheetOts(fromDate, toDate, item.EmployeeId).Select(x => x.DayPoints).Sum();
                decimal requirementTime = 0;
                var employeeTimeSheet = new EmployeeTimeSheet
                {
                    EarlyNumber = 0,
                    LateNumber = 0,
                    FullName = item.FullName,
                    DepartmentId = item.DepartmentId,
                    DepartmentName = item.DepartmentName,
                    EmployeeCode = item.EmployeeCode,
                    LateMinutes = 0,
                    EarlyMinutes = 0,
                    EmployeeId = item.EmployeeId,
                    RealPoint = 0,
                    TotalPoint = 0,
                    TotalDayHoliday = totalDayHoliday,
                    DayPointOt = dayPointOt
                };
                var lisCheckTimeSheets = new List<CheckTimeSheet>();
               
                for (var start = fromDate; start <= toDate; start = start.AddDays(1))
                {
                    var timeSheet = _timeSheetBll.GetTimeSheetByEmployeeId(start, item.EmployeeId);
                    var checkTimeSheet = new CheckTimeSheet
                    {
                        TimeSheetDate = start,
                        CheckOut = "",
                        CheckIn = ""
                    };
                    if (timeSheet != null)
                    {
                        var shiftWork = _shiftWorkBll.GetShiftWork(timeSheet.ShiftWorkId);
                        requirementTime += shiftWork==null ? 0 : shiftWork.ToTalTimeHours;
                        if (timeSheet.Checkout != null)
                            checkTimeSheet.CheckOut = timeSheet.Checkout.Value.ToString("HH:mm");
                        if (timeSheet.Checkin != null)
                            checkTimeSheet.CheckIn = timeSheet.Checkin.Value.ToString("HH:mm");
                        if (timeSheet.EarlyMinutes > 0)
                        {
                            employeeTimeSheet.EarlyMinutes = employeeTimeSheet.EarlyMinutes + timeSheet.EarlyMinutes;
                            employeeTimeSheet.EarlyNumber = employeeTimeSheet.EarlyNumber + 1;
                        }
                        if (timeSheet.LateMinutes > 0)
                        {
                            employeeTimeSheet.LateMinutes = employeeTimeSheet.LateMinutes + timeSheet.LateMinutes;
                            employeeTimeSheet.LateNumber = employeeTimeSheet.LateNumber + 1;
                        }
                        if (timeSheet.RealDays > 0)
                        {
                            employeeTimeSheet.TotalPoint = employeeTimeSheet.TotalPoint + timeSheet.RealDays;
                        }
                    }
                    lisCheckTimeSheets.Add(checkTimeSheet);
                }
                employeeTimeSheet.CheckTimeSheets = lisCheckTimeSheets;
                employeeTimeSheet.RealPoint = dayDates + 1;
                if (employeeTimeSheet.TotalPoint == 0)
                {
                    employeeTimeSheet.ActualHour = 0;
                }
                else
                {
                    if (employeeTimeSheet.TotalPoint != null)
                        employeeTimeSheet.ActualHour = (decimal)(requirementTime -((employeeTimeSheet.LateMinutes + employeeTimeSheet.EarlyMinutes)/60));
                }

                employeeTimeSheet.FixedHour = requirementTime;
                employeeTimeSheet.TotalDayPoint = employeeTimeSheet.TotalPoint+ employeeTimeSheet.DayPointOt +employeeTimeSheet.TotalDayHoliday;
                employeeTimeSheets.Add(employeeTimeSheet);
            }

            return Json(employeeTimeSheets, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}