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
    public class SalaryController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly SalaryBll _salaryBll;
        private readonly UserBll _userBll;

        public SalaryController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _salaryBll = SingletonIpl.GetInstance<SalaryBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.FullName, x.UserId.ToString()));
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            return View();
        }

        public JsonResult Salaries(long? employeeId)
        {
            if (employeeId == 0 || employeeId == null)
                return Json(_salaryBll.GetSalaries(), JsonRequestBehavior.AllowGet);
            return Json(_salaryBll.GetSalaries((long) employeeId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalariesEmployee(long employeeId)
        {
            ViewBag.EmployeeId = employeeId;
            return PartialView();
        }

        public ActionResult Salary(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new Salary {ApplyDate = DateTime.Now, SalaryId = Guid.Empty.ToString()});
            return PartialView(_salaryBll.GetSalary(id));
        }

        [HttpPost]
        public JsonResult Save(SalaryModel model)
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
                model.PercentProfessional = 100;
                if (string.IsNullOrEmpty(model.SalaryId) || model.SalaryId == Guid.Empty.ToString())
                {
                    model.SalaryId = Guid.NewGuid().ToString();
                    if (_salaryBll.Insert(model.ToObject()))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_salaryBll.Update(model.ToObject()))
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
                if (_salaryBll.Delete(id))
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