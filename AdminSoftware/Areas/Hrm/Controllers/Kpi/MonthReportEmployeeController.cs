using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class MonthReportEmployeeController : BaseController
    {
        private readonly ComplainBll _complainBll;
        private readonly WorkDetailBll _workDetailBll;
        private readonly WorkPlanBll _workPlanBll;

        public MonthReportEmployeeController()
        {
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _workPlanBll = SingletonIpl.GetInstance<WorkPlanBll>();
            _complainBll = SingletonIpl.GetInstance<ComplainBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.CalcType = from CalTypeNum s in Enum.GetValues(typeof (CalTypeNum))
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

        public JsonResult WeekReports(DateTime monthlyDate)
        {
            var startWeek1 = new DateTime(monthlyDate.Year, monthlyDate.Month, 1);
            var endWeek1 = startWeek1.AddDays(6);
            var startWeek2 = endWeek1.AddDays(1);
            var endWeek2 = startWeek2.AddDays(6);
            var startWeek3 = endWeek2.AddDays(1);
            var endWeek3 = startWeek3.AddDays(6);
            var startWeek4 = endWeek3.AddDays(1);
            var endWeek4 = startWeek1.AddMonths(1).AddDays(-1);
            var monthReport = new List<KpiMonthReportModel>();
            var workedTasks = new List<KpiMonthReportModel>();
            var planTasks = new List<KpiMonthReportModel>();

            #region tuần 1

            var workDetailWeek1 =
                _workDetailBll.GetWorkDetails(UserLogin.UserId, 1, startWeek1, endWeek1)
                    .OrderBy(x => x.TaskCode)
                    .ToList();
            if (workDetailWeek1.Any())
            {
                foreach (var item in workDetailWeek1)
                {
                    workedTasks.Add(new KpiMonthReportModel
                    {
                        Title = "I.BÁO CÁO THÀNH TÍCH CÔNG VIỆC TRONG TUẦN",
                        EndTaskWeek1 = item.ToDate.ToString("dd/MM/yyyy"),
                        StartTaskWeek1 = item.FromDate.ToString("dd/MM/yyyy"),
                        EndTaskWeek2 = "",
                        EndTaskWeek3 = "",
                        EndTaskWeek4 = "",
                        FullName = UserLogin.FullName,
                        PointTaskWeek1 = item.WorkPoint ?? 0,
                        PointTaskWeek2 = 0,
                        PointTaskWeek3 = 0,
                        PointTaskWeek4 = 0,
                        StartTaskWeek2 = "",
                        StartTaskWeek3 = "",
                        StartTaskWeek4 = "",
                        TaskCode = item.TaskCode,
                        TaskNameX = item.TaskName,
                        Description = item.Description,
                        TimeTaskWeek1 = item.UsefulHours ?? 0,
                        TimeTaskWeek2 = 0,
                        TimeTaskWeek3 = 0,
                        TimeTaskWeek4 = 0,
                        Total = "",
                        Tag = "1"
                    });
                }
                var planWeek2 =
                    _workDetailBll.GetWorkDetailsNextWeek(UserLogin.UserId, startWeek2, endWeek2)
                        .OrderBy(x => x.TaskCode)
                        .ToList();
                if (planWeek2.Any())
                {
                    foreach (var item in planWeek2)
                    {
                        planTasks.Add(new KpiMonthReportModel
                        {
                            Title = "II.KẾ HOẠCH CÔNG VIỆC TUẦN TIẾP THEO",
                            EndTaskWeek1 = item.ToDate.ToString("dd/MM/yyyy"),
                            StartTaskWeek1 = item.FromDate.ToString("dd/MM/yyyy"),
                            EndTaskWeek2 = "",
                            EndTaskWeek3 = "",
                            EndTaskWeek4 = "",
                            FullName = UserLogin.FullName,
                            PointTaskWeek1 = 0,
                            PointTaskWeek2 = 0,
                            PointTaskWeek3 = 0,
                            PointTaskWeek4 = 0,
                            StartTaskWeek2 = "",
                            StartTaskWeek3 = "",
                            StartTaskWeek4 = "",
                            TaskCode = item.TaskCode,
                            TaskNameX = item.TaskName,
                            Description = item.Description,
                            TimeTaskWeek1 = item.UsefulHours ?? 0,
                            TimeTaskWeek2 = 0,
                            TimeTaskWeek3 = 0,
                            TimeTaskWeek4 = 0,
                            Total = "",
                            Tag = "2"
                        });
                    }
                }
            }

            #endregion

            #region tuần 2

            var workDetailWeek2 =
                _workDetailBll.GetWorkDetails(UserLogin.UserId, 1, startWeek2, endWeek2)
                    .OrderBy(x => x.TaskCode)
                    .ToList();
            if (workDetailWeek2.Any())
            {
                foreach (var item in workDetailWeek2)
                {
                    workedTasks.Add(new KpiMonthReportModel
                    {
                        Title = "I.BÁO CÁO THÀNH TÍCH CÔNG VIỆC TRONG TUẦN",
                        EndTaskWeek1 = "",
                        StartTaskWeek1 = "",
                        EndTaskWeek2 = item.ToDate.ToString("dd/MM/yyyy"),
                        EndTaskWeek3 = "",
                        EndTaskWeek4 = "",
                        FullName = UserLogin.FullName,
                        PointTaskWeek1 = 0,
                        PointTaskWeek2 = item.WorkPoint ?? 0,
                        PointTaskWeek3 = 0,
                        PointTaskWeek4 = 0,
                        StartTaskWeek2 = item.FromDate.ToString("dd/MM/yyyy"),
                        StartTaskWeek3 = "",
                        StartTaskWeek4 = "",
                        TaskCode = item.TaskCode,
                        TaskNameX = item.TaskName,
                        Description = item.Description,
                        TimeTaskWeek1 = 0,
                        TimeTaskWeek2 = item.UsefulHours ?? 0,
                        TimeTaskWeek3 = 0,
                        TimeTaskWeek4 = 0,
                        Total = "",
                        Tag = "1"
                    });
                }
                var planWeek3 =
                    _workDetailBll.GetWorkDetailsNextWeek(UserLogin.UserId, startWeek3, endWeek3)
                        .OrderBy(x => x.TaskCode)
                        .ToList();
                if (planWeek3.Any())
                {
                    foreach (var item in planWeek3)
                    {
                        planTasks.Add(new KpiMonthReportModel
                        {
                            Title = "II.KẾ HOẠCH CÔNG VIỆC TUẦN TIẾP THEO",
                            EndTaskWeek2 = item.ToDate.ToString("dd/MM/yyyy"),
                            StartTaskWeek2 = item.FromDate.ToString("dd/MM/yyyy"),
                            EndTaskWeek3 = "",
                            EndTaskWeek4 = "",
                            EndTaskWeek1 = "",
                            FullName = UserLogin.FullName,
                            PointTaskWeek1 = 0,
                            PointTaskWeek2 = 0,
                            PointTaskWeek3 = 0,
                            PointTaskWeek4 = 0,
                            StartTaskWeek3 = "",
                            StartTaskWeek4 = "",
                            StartTaskWeek1 = "",
                            TaskCode = item.TaskCode,
                            TaskNameX = item.TaskName,
                            Description = item.Description,
                            TimeTaskWeek2 = item.UsefulHours ?? 0,
                            TimeTaskWeek1 = 0,
                            TimeTaskWeek3 = 0,
                            TimeTaskWeek4 = 0,
                            Total = "",
                            Tag = "2"
                        });
                    }
                }
            }

            #endregion

            #region tuần 3

            var workDetailWeek3 =
                _workDetailBll.GetWorkDetails(UserLogin.UserId, 1, startWeek3, endWeek3)
                    .OrderBy(x => x.TaskCode)
                    .ToList();
            if (workDetailWeek3.Any())
            {
                foreach (var item in workDetailWeek3)
                {
                    workedTasks.Add(new KpiMonthReportModel
                    {
                        Title = "I.BÁO CÁO THÀNH TÍCH CÔNG VIỆC TRONG TUẦN",
                        EndTaskWeek1 = "",
                        StartTaskWeek1 = "",
                        EndTaskWeek2 = "",
                        EndTaskWeek3 = item.ToDate.ToString("dd/MM/yyyy"),
                        EndTaskWeek4 = "",
                        FullName = UserLogin.FullName,
                        PointTaskWeek1 = 0,
                        PointTaskWeek2 = 0,
                        PointTaskWeek3 = item.WorkPoint ?? 0,
                        PointTaskWeek4 = 0,
                        StartTaskWeek2 = "",
                        StartTaskWeek3 = item.FromDate.ToString("dd/MM/yyyy"),
                        StartTaskWeek4 = "",
                        TaskCode = item.TaskCode,
                        TaskNameX = item.TaskName,
                        Description = item.Description,
                        TimeTaskWeek1 = 0,
                        TimeTaskWeek2 = 0,
                        TimeTaskWeek3 = item.UsefulHours ?? 0,
                        TimeTaskWeek4 = 0,
                        Total = "",
                        Tag = "1"
                    });
                }
                var planWeek4 =
                    _workDetailBll.GetWorkDetailsNextWeek(UserLogin.UserId, startWeek4, endWeek4)
                        .OrderBy(x => x.TaskCode)
                        .ToList();
                if (planWeek4.Any())
                {
                    foreach (var item in planWeek4)
                    {
                        planTasks.Add(new KpiMonthReportModel
                        {
                            Title = "II.KẾ HOẠCH CÔNG VIỆC TUẦN TIẾP THEO",
                            EndTaskWeek3 = item.ToDate.ToString("dd/MM/yyyy"),
                            StartTaskWeek3 = item.FromDate.ToString("dd/MM/yyyy"),
                            EndTaskWeek2 = "",
                            EndTaskWeek4 = "",
                            EndTaskWeek1 = "",
                            FullName = UserLogin.FullName,
                            PointTaskWeek1 = 0,
                            PointTaskWeek2 = 0,
                            PointTaskWeek3 = 0,
                            PointTaskWeek4 = 0,
                            StartTaskWeek2 = "",
                            StartTaskWeek4 = "",
                            StartTaskWeek1 = "",
                            TaskCode = item.TaskCode,
                            TaskNameX = item.TaskName,
                            Description = item.Description,
                            TimeTaskWeek3 = item.UsefulHours ?? 0,
                            TimeTaskWeek2 = 0,
                            TimeTaskWeek1 = 0,
                            TimeTaskWeek4 = 0,
                            Total = "",
                            Tag = "2"
                        });
                    }
                }
            }

            #endregion

            #region tuần 4

            var workDetailWeek4 =
                _workDetailBll.GetWorkDetails(UserLogin.UserId, 1, startWeek4, endWeek4)
                    .OrderBy(x => x.TaskCode)
                    .ToList();
            if (workDetailWeek4.Any())
            {
                foreach (var item in workDetailWeek4)
                {
                    workedTasks.Add(new KpiMonthReportModel
                    {
                        Title = "I.BÁO CÁO THÀNH TÍCH CÔNG VIỆC TRONG TUẦN",
                        EndTaskWeek1 = "",
                        StartTaskWeek1 = "",
                        EndTaskWeek2 = "",
                        EndTaskWeek4 = item.ToDate.ToString("dd/MM/yyyy"),
                        EndTaskWeek3 = "",
                        FullName = UserLogin.FullName,
                        PointTaskWeek1 = 0,
                        PointTaskWeek2 = 0,
                        PointTaskWeek4 = item.WorkPoint ?? 0,
                        PointTaskWeek3 = 0,
                        StartTaskWeek2 = "",
                        StartTaskWeek4 = item.FromDate.ToString("dd/MM/yyyy"),
                        StartTaskWeek3 = "",
                        TaskCode = item.TaskCode,
                        TaskNameX = item.TaskName,
                        Description = item.Description,
                        TimeTaskWeek1 = 0,
                        TimeTaskWeek2 = 0,
                        TimeTaskWeek4 = item.UsefulHours ?? 0,
                        TimeTaskWeek3 = 0,
                        Total = "",
                        Tag = "1"
                    });
                }
            }

            #endregion

            if (workedTasks.Any())
                monthReport.AddRange(workedTasks);
            if (planTasks.Any())
                monthReport.AddRange(planTasks);
            if (monthReport.Any())
            {
                monthReport.Add(new KpiMonthReportModel
                {
                    Title = "III.TỔNG KẾT",
                    EndTaskWeek1 = "",
                    StartTaskWeek1 = "",
                    EndTaskWeek2 = "",
                    EndTaskWeek3 = "",
                    EndTaskWeek4 = "",
                    FullName = UserLogin.FullName,
                    PointTaskWeek1 = 0,
                    PointTaskWeek2 = 0,
                    PointTaskWeek3 = 0,
                    PointTaskWeek4 = 0,
                    StartTaskWeek2 = "",
                    StartTaskWeek3 = "",
                    StartTaskWeek4 = "",
                    TaskCode = "",
                    Description = "",
                    TimeTaskWeek1 = 0,
                    TimeTaskWeek2 = 0,
                    TimeTaskWeek3 = 0,
                    TimeTaskWeek4 = 0,
                    Total = ""
                });
                monthReport.Add(new KpiMonthReportModel
                {
                    Title = "III.TỔNG KẾT",
                    EndTaskWeek1 =
                        !workDetailWeek1.Any()
                            ? ""
                            : string.Format("{0:###,###.##}", workDetailWeek1.Sum(x => x.UsefulHours)),
                    StartTaskWeek1 = "",
                    EndTaskWeek2 =
                        !workDetailWeek2.Any()
                            ? ""
                            : string.Format("{0:###,###.##}", workDetailWeek2.Sum(x => x.UsefulHours)),
                    EndTaskWeek3 =
                        !workDetailWeek3.Any()
                            ? ""
                            : string.Format("{0:###,###.##}", workDetailWeek3.Sum(x => x.UsefulHours)),
                    EndTaskWeek4 =
                        !workDetailWeek4.Any()
                            ? ""
                            : string.Format("{0:###,###.##}", workDetailWeek4.Sum(x => x.UsefulHours)),
                    FullName = UserLogin.FullName,
                    PointTaskWeek1 = 0,
                    PointTaskWeek2 = 0,
                    PointTaskWeek3 = 0,
                    PointTaskWeek4 = 0,
                    StartTaskWeek2 = "",
                    StartTaskWeek3 = "",
                    StartTaskWeek4 = "",
                    TaskCode = "",
                    Description = "Giờ hữu ích",
                    TimeTaskWeek1 = 0,
                    TimeTaskWeek2 = 0,
                    TimeTaskWeek3 = 0,
                    TimeTaskWeek4 = 0,
                    Total =
                        string.Format("{0:###,###.##}",
                            ((!workDetailWeek1.Any() ? 0 : workDetailWeek1.Sum(x => x.UsefulHours)) +
                             (!workDetailWeek2.Any() ? 0 : workDetailWeek2.Sum(x => x.UsefulHours)) +
                             (!workDetailWeek3.Any() ? 0 : workDetailWeek3.Sum(x => x.UsefulHours)) +
                             (!workDetailWeek4.Any() ? 0 : workDetailWeek4.Sum(x => x.UsefulHours))))
                });
                monthReport.Add(new KpiMonthReportModel
                {
                    Title = "III.TỔNG KẾT",
                    EndTaskWeek1 =
                        !workDetailWeek1.Any()
                            ? ""
                            : string.Format("{0:###,###.##}", workDetailWeek1.Sum(x => x.WorkPoint)),
                    StartTaskWeek1 = "",
                    EndTaskWeek2 =
                        !workDetailWeek2.Any()
                            ? ""
                            : string.Format("{0:###,###.##}", workDetailWeek2.Sum(x => x.WorkPoint)),
                    EndTaskWeek3 =
                        !workDetailWeek3.Any()
                            ? ""
                            : string.Format("{0:###,###.##}", workDetailWeek3.Sum(x => x.WorkPoint)),
                    EndTaskWeek4 =
                        !workDetailWeek4.Any()
                            ? ""
                            : string.Format("{0:###,###.##}", workDetailWeek4.Sum(x => x.WorkPoint)),
                    FullName = UserLogin.FullName,
                    PointTaskWeek1 = 0,
                    PointTaskWeek2 = 0,
                    PointTaskWeek3 = 0,
                    PointTaskWeek4 = 0,
                    StartTaskWeek2 = "",
                    StartTaskWeek3 = "",
                    StartTaskWeek4 = "",
                    TaskCode = "",
                    Description = "Điểm công việc",
                    TimeTaskWeek1 = 0,
                    TimeTaskWeek2 = 0,
                    TimeTaskWeek3 = 0,
                    TimeTaskWeek4 = 0,
                    Total =
                        string.Format("{0:###,###.##}",
                            ((!workDetailWeek1.Any() ? 0 : workDetailWeek1.Sum(x => x.WorkPoint)) +
                             (!workDetailWeek2.Any() ? 0 : workDetailWeek2.Sum(x => x.WorkPoint)) +
                             (!workDetailWeek3.Any() ? 0 : workDetailWeek3.Sum(x => x.WorkPoint)) +
                             (!workDetailWeek4.Any() ? 0 : workDetailWeek4.Sum(x => x.WorkPoint))))
                });
                var suggesWork1 =
                    _workDetailBll.GetSuggesWorkKpis(startWeek1, endWeek1, null, UserLogin.UserId).FirstOrDefault();
                decimal kpiFinish1 = 0;
                if (suggesWork1 != null)
                {
                    if (suggesWork1.TotalQuantity != 0)
                    {
                        kpiFinish1 = Math.Round(((suggesWork1.TotalFinish / suggesWork1.TotalQuantity) * 100), 1,
                            MidpointRounding.AwayFromZero);
                    }
                }
                var suggesWork2 =
                    _workDetailBll.GetSuggesWorkKpis(startWeek2, endWeek2, null, UserLogin.UserId).FirstOrDefault();
                decimal kpiFinish2 = 0;
                if (suggesWork2 != null)
                {
                    if (suggesWork2.TotalQuantity != 0)
                    {
                        kpiFinish2 = Math.Round(((suggesWork2.TotalFinish / suggesWork2.TotalQuantity) * 100), 1,
                            MidpointRounding.AwayFromZero);
                    }
                }
                var suggesWork3 =
                    _workDetailBll.GetSuggesWorkKpis(startWeek3, endWeek3, null, UserLogin.UserId).FirstOrDefault();
                decimal kpiFinish3 = 0;
                if (suggesWork3 != null)
                {
                    if (suggesWork3.TotalQuantity != 0)
                    {
                        kpiFinish3 = Math.Round(((suggesWork3.TotalFinish / suggesWork3.TotalQuantity) * 100), 1,
                            MidpointRounding.AwayFromZero);
                    }
                }
                var suggesWork4 =
                    _workDetailBll.GetSuggesWorkKpis(startWeek4, endWeek4, null, UserLogin.UserId).FirstOrDefault();
                decimal kpiFinish4 = 0;
                if (suggesWork4 != null)
                {
                    if (suggesWork4.TotalQuantity != 0)
                    {
                        kpiFinish4 = Math.Round(((suggesWork4.TotalFinish / suggesWork4.TotalQuantity) * 100), 1,
                            MidpointRounding.AwayFromZero)
                            ;
                    }
                }
                var suggesWork =
                    _workDetailBll.GetSuggesWorkKpis(startWeek1, endWeek4, null, UserLogin.UserId).FirstOrDefault();
                decimal kpiFinish = 0;
                if (suggesWork != null)
                {
                    if (suggesWork.TotalQuantity != 0)
                    {
                        kpiFinish = Math.Round(((suggesWork.TotalFinish / suggesWork.TotalQuantity) * 100), 1,
                            MidpointRounding.AwayFromZero);
                    }
                }
                monthReport.Add(new KpiMonthReportModel
                {
                    Title = "III.TỔNG KẾT",
                    EndTaskWeek1 = kpiFinish1 + " %",
                    StartTaskWeek1 = "",
                    EndTaskWeek2 = kpiFinish2 + " %",
                    EndTaskWeek3 = kpiFinish3 + " %",
                    EndTaskWeek4 = kpiFinish4 + " %",
                    FullName = UserLogin.FullName,
                    PointTaskWeek1 = 0,
                    PointTaskWeek2 = 0,
                    PointTaskWeek3 = 0,
                    PointTaskWeek4 = 0,
                    StartTaskWeek2 = "",
                    StartTaskWeek3 = "",
                    StartTaskWeek4 = "",
                    TaskCode = "",
                    Description = "Tỷ lệ hoàn thành công việc tuần",
                    TimeTaskWeek1 = 0,
                    TimeTaskWeek2 = 0,
                    TimeTaskWeek3 = 0,
                    TimeTaskWeek4 = 0,
                    Total = kpiFinish + " %"
                });
                var comPlain1 = _complainBll.GetComplains_AccusedBy(startWeek1, endWeek1, UserLogin.UserId, 2).Count;
                var comPlain2 = _complainBll.GetComplains_AccusedBy(startWeek2, endWeek2, UserLogin.UserId, 2).Count;
                var comPlain3 = _complainBll.GetComplains_AccusedBy(startWeek3, endWeek3, UserLogin.UserId, 2).Count;
                var comPlain4 = _complainBll.GetComplains_AccusedBy(startWeek4, endWeek4, UserLogin.UserId, 2).Count;
                monthReport.Add(new KpiMonthReportModel
                {
                    Title = "III.TỔNG KẾT",
                    EndTaskWeek1 = comPlain1 + " nhắc nhở",
                    StartTaskWeek1 = "",
                    EndTaskWeek2 = comPlain2 + " nhắc nhở",
                    EndTaskWeek3 = comPlain3 + " nhắc nhở",
                    EndTaskWeek4 = comPlain4 + " nhắc nhở",
                    FullName = UserLogin.FullName,
                    PointTaskWeek1 = 0,
                    PointTaskWeek2 = 0,
                    PointTaskWeek3 = 0,
                    PointTaskWeek4 = 0,
                    StartTaskWeek2 = "",
                    StartTaskWeek3 = "",
                    StartTaskWeek4 = "",
                    TaskCode = "",
                    Description = "Mức độ hài lòng KH/LĐ/ĐN của tuần",
                    TimeTaskWeek1 = 0,
                    TimeTaskWeek2 = 0,
                    TimeTaskWeek3 = 0,
                    TimeTaskWeek4 = 0,
                    Total = (comPlain1 + comPlain2 + comPlain3 + comPlain4) + " nhắc nhở"
                });
            }
            return Json(monthReport, JsonRequestBehavior.AllowGet);
        }
    }
}