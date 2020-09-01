using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class EmployeeHolidayController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly EmployeeHolidayBll _employeeHolidayBll;
        private readonly HolidayBll _holidayBll;
        private readonly HolidayReasonBll _holidayReasonBll;

        public EmployeeHolidayController()
        {
            _employeeHolidayBll = SingletonIpl.GetInstance<EmployeeHolidayBll>();
            _holidayReasonBll = SingletonIpl.GetInstance<HolidayReasonBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _holidayBll = SingletonIpl.GetInstance<HolidayBll>();
        }

        private List<HolidayDetail> HolidayDetailsInMemory
        {
            get
            {
                if (Session["HolidayDetailsInMemory"] == null)
                    return new List<HolidayDetail>();
                return (List<HolidayDetail>) Session["HolidayDetailsInMemory"];
            }
            set { Session["HolidayDetailsInMemory"] = value; }
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            ViewBag.HolidayReason =
                _holidayReasonBll.GetHolidayReasons(true)
                    .Select(x => new KendoForeignKeyModel(x.ReasonName, x.HolidayReasonId.ToString()));
            return View();
        }

        public JsonResult EmployeeHolidays(DateTime fromDate, DateTime toDate)
        {
            return Json(_employeeHolidayBll.GetEmployeeHolidays(fromDate, toDate, null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeHoliday(string id)
        {
            HolidayDetailsInMemory = null;
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new EmployeeHoliday
                {
                    EmployeeHolidayId = Guid.Empty.ToString(),
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(2),
                    CreateDate = DateTime.Now
                });
            }
            var employeeHoliday = (_employeeHolidayBll.GetEmployeeHoliday(id));
            HolidayDetailsInMemory = employeeHoliday.HolidayDetails;
            return PartialView(employeeHoliday);
        }

        public ActionResult HolidayDetail(string id)
        {
            var holidayDetails = HolidayDetailsInMemory;
            return PartialView(holidayDetails.Find(x => x.HolidayDetailId == id));
        }

        public JsonResult HolidayDetails()
        {
            return Json(HolidayDetailsInMemory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Employees()
        {
            return PartialView(_employeeBll.GetEmployees(true));
        }

        public JsonResult Save(EmployeeHolidayModel model)
        {
            if (!ModelState.IsValid)
            {
                var message = ModelState.Where(modelState => modelState.Value.Errors.Count > 0)
                    .Select(x => x.Value)
                    .FirstOrDefault();
                return Json(new
                {
                    Status = 0,
                    Message = message == null ? MessageAction.ModelStateNotValid : message.Errors[0].ErrorMessage
                }, JsonRequestBehavior.AllowGet);
            }
            if (model == null)
            {
                return Json(new
                {
                    Status = 0,
                    Message = MessageAction.DataIsEmpty
                }, JsonRequestBehavior.AllowGet);
            }
            var employeeHoliday = model.ToObject();
            employeeHoliday.HolidayDetails = HolidayDetailsInMemory;
            if (string.IsNullOrEmpty(employeeHoliday.EmployeeHolidayId) ||
                employeeHoliday.EmployeeHolidayId == Guid.Empty.ToString())
            {
                employeeHoliday.EmployeeHolidayId = Guid.NewGuid().ToString();
                employeeHoliday.CreateDate = DateTime.Now;
                if (_employeeHolidayBll.Insert(employeeHoliday))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            if (_employeeHolidayBll.Update(employeeHoliday))
            {
                return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateDetail(DateTime fromDate, DateTime toDate, string holidayReasonId)
        {
            HolidayDetailsInMemory = null;
            var holidayReason = new HolidayReason();
            if (!string.IsNullOrEmpty(holidayReasonId))
            {
                holidayReason = _holidayReasonBll.GetHolidayReason(Int32.Parse(holidayReasonId));
            }
            else
            {
                holidayReason.PercentSalary = 0;
            }
            var holidayDetails = new List<HolidayDetail>();
            var holidays = _holidayBll.GetHolidayByDates(fromDate, toDate);

            for (var start = fromDate; start <= toDate; start = start.AddDays(1))
            {
                if (
                    holidays.Where(x => x.HolidayDate.ToShortDateString() == start.ToShortDateString()).FirstOrDefault() ==
                    null)
                {
                    holidayDetails.Add(new HolidayDetail
                    {
                        HolidayDetailId = Guid.NewGuid().ToString(),
                        EmployeeHolidayId = Guid.Empty.ToString(),
                        NumberDays = 1,
                        Permission = 0,
                        PercentSalary = holidayReason.PercentSalary,
                        ToTalDays = holidayReason.PercentSalary,
                        DateDay = DateTime.Parse(start.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt"))
                    });
                }
            }
            HolidayDetailsInMemory = holidayDetails;
            return Json(HolidayDetailsInMemory, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateDetail(HolidayDetailModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = ModelState.Where(modelState => modelState.Value.Errors.Count > 0)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                    return Json(new
                    {
                        Status = 0,
                        Message = message == null ? MessageAction.ModelStateNotValid : message.Errors[0].ErrorMessage
                    }, JsonRequestBehavior.AllowGet);
                }
                if (model == null)
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = MessageAction.DataIsEmpty
                    }, JsonRequestBehavior.AllowGet);
                }
                var holidayDetails = HolidayDetailsInMemory;
                var holidayDetail = holidayDetails.Find(x => x.HolidayDetailId == model.HolidayDetailId);
                holidayDetail.NumberDays = model.NumberDays;
                holidayDetail.PercentSalary = model.PercentSalary;
                holidayDetail.Permission = model.Permission;
                holidayDetail.ToTalDays = model.PercentSalary*model.NumberDays;
                HolidayDetailsInMemory = holidayDetails;
                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Cập nhật thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);

                if (_employeeHolidayBll.Delete(id))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageDeleteSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        // Holiday Reason
        public ActionResult HolidayReasonSearch()
        {
            return PartialView();
        }

        public JsonResult HolidayReasons()
        {
            return Json(_holidayReasonBll.GetHolidayReasons(null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult HolidayReason(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new HolidayReason {IsActive = true});
            }
            return PartialView(_holidayReasonBll.GetHolidayReason(int.Parse(id)));
        }

        [HttpPost]
        public JsonResult SaveHolidayReason(HolidayReasonModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message =
                        ModelState.Where(modelState => modelState.Value.Errors.Count > 0)
                            .Select(x => x.Value)
                            .FirstOrDefault();
                    return
                        Json(
                            new
                            {
                                Status = 0,
                                Message =
                                    message == null ? MessageAction.ModelStateNotValid : message.Errors[0].ErrorMessage
                            },
                            JsonRequestBehavior.AllowGet);
                }
                var holidayReason = _holidayReasonBll.GetHolidayReason(model.ReasonCode);
                if (model.HolidayReasonId <= 0)
                {
                    if (holidayReason != null)
                    {
                        return Json(new {Status = 0, Message = "Mã lý do đã tồn tại trong hệ thống!"},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (_holidayReasonBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (holidayReason != null && holidayReason.HolidayReasonId != model.HolidayReasonId)
                {
                    return Json(new {Status = 0, Message = "Mã lý do đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_holidayReasonBll.Update(model.ToObject()))
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteHolidayReason(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                if (_holidayReasonBll.Delete(int.Parse(id)))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageDeleteSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ClearSession()
        {
            HolidayDetailsInMemory = null;
            return null;
        }
    }
}