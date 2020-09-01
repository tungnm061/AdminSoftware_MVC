using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Helper;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class TimeSheetController : BaseController
    {
        private readonly EmployeeBll _employeeBll;
        private readonly TimeSheetBll _timeSheetBll;
        private readonly ShiftWorkBll _shiftWorkBll;

        public TimeSheetController()
        {
            _timeSheetBll = SingletonIpl.GetInstance<TimeSheetBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _shiftWorkBll = SingletonIpl.GetInstance<ShiftWorkBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult TimeSheets(DateTime timeSheetDate)
        {
            if (_timeSheetBll.GetTimeSheetCheckDate(timeSheetDate) == null)
            {
                if (!_timeSheetBll.Inserts(timeSheetDate))
                {
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
            }
            return Json(_timeSheetBll.GetTimeSheets(timeSheetDate), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Save(TimeSheetModel model)
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
                return Json(_timeSheetBll.Update(model.ToObject())
                    ? new {Status = 1, Message = MessageAction.MessageUpdateSuccess}
                    : new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Import()
        {
            return PartialView();
        }

        public JsonResult ImportExcel()
        {
            if (Request.Files["files"] == null)
                return Json(new {Status = 0, Message = "Không có dữ liệu để import!"}, JsonRequestBehavior.AllowGet);
            try
            {
                var file = Request.Files["files"];
                var dataList = ExcelHelper.ReadExcelDictionary(file.InputStream);
                var timeSheets = new List<TimeSheet>();
                foreach (var data in dataList)
                {
                    if (data.ContainsKey("Emp No.") && data.ContainsKey("Date") &&
                        data.ContainsKey("Timetable") && data.ContainsKey("Clock In") && data.ContainsKey("Clock Out"))
                    {
                        try
                        {
                            var timeSheetDate = new DateTime(int.Parse(data["Date"].Split('/')[2]), int.Parse(data["Date"].Split('/')[1]), int.Parse(data["Date"].Split('/')[0]));
                            var checkIn = string.IsNullOrEmpty(data["Clock In"]) ? (DateTime?)null :DateTime.Parse(timeSheetDate.ToShortDateString() + " " + data["Clock In"]);
                            var checkOut = string.IsNullOrEmpty(data["Clock Out"]) ? (DateTime?)null : DateTime.Parse(timeSheetDate.ToShortDateString() + " " + data["Clock Out"]);
                            var employee = _employeeBll.GetEmployeeByTimeSheetCode(data["Emp No."].Trim());
                            if (employee == null)
                                return Json(new {Status = 0, Message = "Mã chấm công "+ data["Emp No."] + " không tồn tại trong hệ thống!"},
                                    JsonRequestBehavior.AllowGet);
                            var shiftWork = _shiftWorkBll.GetShiftWork(data["Timetable"].Trim());
                            if (shiftWork == null)
                                return Json(new { Status = 0, Message = "Ca làm việc "+ data["Timetable"] + " không tồn tại trong hệ thống!" },
                                    JsonRequestBehavior.AllowGet);
                            timeSheets.Add(new TimeSheet
                            {
                                EmployeeId = employee.EmployeeId,
                                Checkin = checkIn,
                                Checkout = checkOut,
                                TimeSheetDate = timeSheetDate,
                                TimeSheetId = Guid.NewGuid().ToString(),
                                ShiftWorkId = shiftWork.ShiftWorkId
                            });
                        }
                        catch (Exception)
                        {
                            return Json(new {Status = 0, Message = "Dữ liệu truyền vào không hợp lệ!"},
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new {Status = 0, Message = "File excel không đúng định dạng!"},
                            JsonRequestBehavior.AllowGet);
                    }
                }
                if (timeSheets.Any())
                {
                    if (_timeSheetBll.UpdateList(timeSheets))
                        return Json(new {Status = 1, Message = "Import thành công!"},
                            JsonRequestBehavior.AllowGet);
                    return Json(new {Status = 0, Message = "File không có dữ liệu!"},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = "File không có dữ liệu!"},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new {Status = 0, Message = "Có lỗi xảy ra trong quá trình xử lý!"},
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}