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
    public class InsuranceController : BaseController
    {
        private readonly AddressBll _addressBll;
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly InsuranceBll _insuranceBll;
        private readonly UserBll _userBll;

        public InsuranceController()
        {
            _insuranceBll = SingletonIpl.GetInstance<InsuranceBll>();
            _addressBll = SingletonIpl.GetInstance<AddressBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            ViewBag.Cities =
                _addressBll.GetCities().Select(x => new KendoForeignKeyModel(x.CityName, x.CityId.ToString()));
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.FullName, x.UserId.ToString()));
            return View();
        }

        public JsonResult Insurances()
        {
            return Json(_insuranceBll.GetInsurances(null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Insurance(string id)
        {
            var employees = _employeeBll.GetEmployeesForInsurance(false);
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Employees = employees
                    .Select(x => new KendoForeignKeyModel(x.EmployeeCode + "-" + x.FullName, x.EmployeeId.ToString()));
                return PartialView(new Insurance
                {
                    SubscriptionDate = DateTime.Now,
                    IsActive = true,
                    MonthBefore = 0
                });
            }
            var insurance = _insuranceBll.GetInsurance(long.Parse(id));
            employees.Add(_employeeBll.GetEmployee(insurance.EmployeeId));
            ViewBag.Employees = employees
                .Select(x => new KendoForeignKeyModel(x.EmployeeCode + "-" + x.FullName, x.EmployeeId.ToString()));
            return PartialView(insurance);
        }

        [HttpPost]
        public JsonResult Save(InsuranceModel model)
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
                if (model.InsuranceId <= 0)
                {
                    if (_insuranceBll.Insert(model.ToObject()) > 0)
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_insuranceBll.Update(model.ToObject()))
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
                if (_insuranceBll.Delete(long.Parse(id)))
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