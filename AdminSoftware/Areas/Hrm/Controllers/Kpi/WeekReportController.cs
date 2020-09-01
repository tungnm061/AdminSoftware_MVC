using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Kpi;
using BusinessLogic.System;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Kpi;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class WeekReportController : BaseController
    {
        private readonly WorkDetailBll _workDetailBll;
        private readonly WorkPlanBll _workPlanBll;
        private readonly UserBll _userBll;
        private readonly ComplainBll _complainBll;
        public WeekReportController()
        {
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _workPlanBll = SingletonIpl.GetInstance<WorkPlanBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _complainBll = SingletonIpl.GetInstance<ComplainBll>();
        }
        [HrmFilter]
        public ActionResult Index()
        {
            var date = DateTime.Now;
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;
            DayOfWeek firstDay = defaultCultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = date.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);
             date = firstDayInWeek;
            ViewBag.FromDate = date;
            ViewBag.ToDate = date.AddDays(+6);
            ViewBag.Users =
               _userBll.GetUsers(null)
                   .Select(x => new KendoForeignKeyModel(x.FullName, x.UserId.ToString()));
            return View();
        }

        public JsonResult WeekReports(DateTime fromDate, DateTime toDate)
        {

            var workDetailWeek1 = _workDetailBll.GetWorkDetails(UserLogin.UserId, 1, fromDate, toDate);
            foreach (var item in workDetailWeek1)
            {
                item.Title = "I.BÁO CÁO THÀNH TÍCH CÔNG VIỆC TRONG TUẦN : TỪ " + fromDate.ToString("dd/MM/yyyy") + " ĐẾN " + toDate.ToString("dd/MM/yyyy");
            }
            List<WorkDetail> workDetail = new List<WorkDetail>();
            workDetail.AddRange(workDetailWeek1);
            if (workDetailWeek1.Any())
            {
                var workDetailWeek2 = _workDetailBll.GetWorkDetailsNextWeek(UserLogin.UserId, toDate.AddDays(+1), toDate.AddDays(+7));
                foreach (var item in workDetailWeek2)
                {
                    item.Title = "II.KẾ HOẠCH CÔNG VIỆC TUẦN TIẾP THEO TỪ " + toDate.AddDays(+1).ToString("dd/MM/yyyy") + " ĐẾN " +
                                 toDate.AddDays(+7).ToString("dd/MM/yyyy");
                }
                workDetail.AddRange(workDetailWeek2);
            }
            return Json(workDetail.OrderBy(x => x.TaskCode), JsonRequestBehavior.AllowGet);
        }

    }
}