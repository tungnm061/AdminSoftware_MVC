using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class HolidayController : BaseController
    {
        private readonly HolidayBll _holidayBll;

        public HolidayController()
        {
            _holidayBll = SingletonIpl.GetInstance<HolidayBll>();
        }

        [HrmFilter]
        public ActionResult Index(int? year)
        {
            var years = new List<KendoForeignKeyModel>();
            for (var i = ((year ?? DateTime.Now.Year) - 2); i <= ((year ?? DateTime.Now.Year) + 2); i++)
            {
                years.Add(new KendoForeignKeyModel("Năm " + i, i.ToString()));
            }
            var holidays = new Dictionary<int, List<Holiday>>();
            for (var i = 1; i <= 12; i++)
            {
                holidays.Add(i, _holidayBll.GetHolidays(year ?? DateTime.Now.Year, i).ToList());
            }
            ViewBag.Years = years;
            ViewBag.Year = year ?? DateTime.Now.Year;
            return View(holidays);
        }

        public JsonResult Save(List<DateTime> models, int year)
        {
            try
            {
                var holidays =
                    models.Select(
                        x =>
                            new Holiday
                            {
                                HolidayId = Guid.NewGuid().ToString(),
                                HolidayDate = DateTime.Parse(x.ToShortDateString() + " 02:00 PM")
                            }).ToList();
                if (_holidayBll.Insert(holidays, year))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}