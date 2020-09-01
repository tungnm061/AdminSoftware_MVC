using System;
using System.Collections.Generic;
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
    public class InsuranceMedicalController : BaseController
    {
        private readonly AddressBll _addressBll;
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly InsuranceMedicalBll _insuranceMedicalBll;
        private readonly MedicalBll _medicalBll;
        private readonly UserBll _userBll;

        public InsuranceMedicalController()
        {
            _insuranceMedicalBll = SingletonIpl.GetInstance<InsuranceMedicalBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _addressBll = SingletonIpl.GetInstance<AddressBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _medicalBll = SingletonIpl.GetInstance<MedicalBll>();
        }

        [HrmFilter]
        public ActionResult Index(int? year)
        {
            var years = new List<KendoForeignKeyModel>();
            for (var i = ((year ?? DateTime.Now.Year) - 2); i <= ((year ?? DateTime.Now.Year) + 1); i++)
            {
                years.Add(new KendoForeignKeyModel("Năm " + i, i.ToString()));
            }
            ViewBag.Year = year ?? DateTime.Now.Year;
            ViewBag.Years = years;
            ViewBag.Employees =
                _employeeBll.GetEmployees(null)
                    .Select(x => new KendoForeignKeyModel(x.EmployeeCode + "-" + x.FullName, x.EmployeeId.ToString()));
            ViewBag.Users =
                _userBll.GetUsers(null).Select(x => new KendoForeignKeyModel(x.FullName, x.UserId.ToString()));
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            ViewBag.Cities =
                _addressBll.GetCities().Select(x => new KendoForeignKeyModel(x.CityName, x.CityId.ToString()));
            ViewBag.Medicals =
                _medicalBll.GetMedicals().Select(x => new KendoForeignKeyModel(x.MedicalName, x.MedicalId.ToString()));
            return View();
        }

        public JsonResult InsuranceMedicals(int? year)
        {
            return Json(_insuranceMedicalBll.GetInsuranceMedicals(year ?? DateTime.Now.Year,null),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsuranceMedical(string id)
        {
            if (string.IsNullOrEmpty(id))
                return PartialView(new InsuranceMedical
                {
                    InsuranceMedicalId = Guid.Empty.ToString(),
                    StartDate = DateTime.Now,
                    ExpiredDate = DateTime.Now.AddYears(1)
                });
            return PartialView(_insuranceMedicalBll.GetInsuranceMedical(id));
        }

        [HttpPost]
        public JsonResult Save(InsuranceMedicalModel model)
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
                if (string.IsNullOrEmpty(model.InsuranceMedicalId) || model.InsuranceMedicalId == Guid.Empty.ToString())
                {
                    model.InsuranceMedicalId = Guid.NewGuid().ToString();
                    if (_insuranceMedicalBll.Insert(model.ToObject()))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_insuranceMedicalBll.Update(model.ToObject()))
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
                if (_insuranceMedicalBll.Delete(id))
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