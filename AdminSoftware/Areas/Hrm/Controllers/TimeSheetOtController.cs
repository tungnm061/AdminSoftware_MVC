using System;
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
    public class TimeSheetOtController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly ShiftWorkBll _shiftWorkBll;
        private readonly TimeSheetOtBll _timeSheetOtBll;

        public TimeSheetOtController()
        {
            _timeSheetOtBll = SingletonIpl.GetInstance<TimeSheetOtBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _shiftWorkBll = SingletonIpl.GetInstance<ShiftWorkBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            return View();
        }

        public JsonResult TimeSheetOts(DateTime? fromDate, DateTime? toDate)
        {
            return Json(_timeSheetOtBll.GetTimeSheetOts(fromDate, toDate, null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Employees()
        {
            return PartialView(_employeeBll.GetEmployees(true));
        }

        public ActionResult TimeSheetOt(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new TimeSheetOt
                {
                    TimeSheetOtId = Guid.Empty.ToString(),
                    CreateDate = DateTime.Now,
                    DayDate = DateTime.Now
                });
            }
            return PartialView(_timeSheetOtBll.GetTimeSheetOt(id));
        }

        public JsonResult Save(TimeSheetOtModel model)
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
            model.DayPoints = model.CoefficientPoint*(model.Hours/8);
            if (string.IsNullOrEmpty(model.TimeSheetOtId) || model.TimeSheetOtId == Guid.Empty.ToString())
            {
                model.TimeSheetOtId = Guid.NewGuid().ToString();
                model.CreateDate = DateTime.Now;
                if (_timeSheetOtBll.Insert(model.ToObject()))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            if (_timeSheetOtBll.Update(model.ToObject()))
            {
                return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);

                if (_timeSheetOtBll.Delete(id))
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
            return null;
        }
    }
}