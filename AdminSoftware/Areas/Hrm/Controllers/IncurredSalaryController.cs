using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class IncurredSalaryController : BaseController
    {
        private readonly EmployeeBll _employeeBll;
        private readonly IncurredSalaryBll _incurredSalaryBll;
        private readonly UserBll _userBll;

        public IncurredSalaryController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _incurredSalaryBll = SingletonIpl.GetInstance<IncurredSalaryBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Employees =
                _employeeBll.GetEmployees(null)
                    .Select(x => new KendoForeignKeyModel(x.EmployeeCode + "-" + x.FullName, x.EmployeeId.ToString()));
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.FullName, x.UserId.ToString()));
            return View();
        }

        public JsonResult IncurredSalaries(DateTime fromDate, DateTime toDate)
        {
            return Json(_incurredSalaryBll.GetIncurredSalaries(null, fromDate, toDate, false),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult IncurredSalary(string id)
        {
            if (string.IsNullOrEmpty(id))
                return
                    PartialView(new IncurredSalary {IncurredSalaryId = Guid.Empty.ToString(), SubmitDate = DateTime.Now});
            return PartialView(_incurredSalaryBll.GetIncurredSalary(id));
        }

        [HttpPost]
        public JsonResult Save(IncurredSalaryModel model)
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
                model.CreateBy = UserLogin.UserId;
                model.CreateDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.IncurredSalaryId) || model.IncurredSalaryId == Guid.Empty.ToString())
                {
                    model.IncurredSalaryId = Guid.NewGuid().ToString();
                    if (_incurredSalaryBll.Insert(model.ToObject()))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_incurredSalaryBll.Update(model.ToObject()))
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
        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                if (_incurredSalaryBll.Delete(id))
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