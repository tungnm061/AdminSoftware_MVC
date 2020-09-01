using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Hrm.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Controllers
{
    public class DisciplineController : BaseController
    {
        private readonly AutoNumberBll _autoNumberBll;
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly PraiseDisciplineBll _praiseDisciplineBll;
        private readonly string _prefix = "KL" + DateTime.Now.Year.ToString().Substring(2, 2);
        private readonly UserBll _userBll;

        public DisciplineController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _praiseDisciplineBll = SingletonIpl.GetInstance<PraiseDisciplineBll>();
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
        }

        private List<PraiseDisciplineDetail> PraiseDisciplineDetailsInMemory
        {
            get
            {
                if (Session["PraiseDisciplineDetailsInMemory"] == null)
                    return new List<PraiseDisciplineDetail>();
                return (List<PraiseDisciplineDetail>) Session["PraiseDisciplineDetailsInMemory"];
            }
            set { Session["PraiseDisciplineDetailsInMemory"] = value; }
        }

        [HrmFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            ViewBag.Users = users.Select(x => new KendoForeignKeyModel {value = x.UserId.ToString(), text = x.FullName});
            return View();
        }

        public JsonResult Disciplines()
        {
            var praiseDisciplines = _praiseDisciplineBll.GetPraiseDisciplines((byte) PraiseDisciplineType.Discipline);
            return Json(praiseDisciplines, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Discipline(string id)
        {
            var departments = _departmentBll.GetDepartments(null);
            ViewBag.Departments =
                departments.Select(
                    x => new KendoForeignKeyModel {value = x.DepartmentId.ToString(), text = x.DepartmentName});
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new PraiseDiscipline
                {
                    PraiseDisciplineDate = DateTime.Now,
                    PraiseDisciplineId = Guid.Empty.ToString(),
                    PraiseDisciplineCode = _autoNumberBll.GetAutoNumber(_prefix)
                });
            }
            var praiseDiscipline = _praiseDisciplineBll.GetPraiseDiscipline(id);
            PraiseDisciplineDetailsInMemory = praiseDiscipline.PraiseDisciplineDetails;
            return PartialView(praiseDiscipline);
        }

        public JsonResult DisciplineDetails()
        {
            return Json(PraiseDisciplineDetailsInMemory, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(PraiseDisciplineModel model)
        {
            try
            {
                if (!PraiseDisciplineDetailsInMemory.Any() || model == null)
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                }
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
                var praiseDiscipline = model.ToObject();
                praiseDiscipline.PraiseDisciplineType = (byte) PraiseDisciplineType.Discipline;
                praiseDiscipline.PraiseDisciplineDetails = PraiseDisciplineDetailsInMemory;
                if (string.IsNullOrEmpty(praiseDiscipline.PraiseDisciplineId) ||
                    praiseDiscipline.PraiseDisciplineId == Guid.Empty.ToString())
                {
                    praiseDiscipline.CreateDate = DateTime.Now;
                    praiseDiscipline.CreateBy = UserLogin.UserId;
                    praiseDiscipline.PraiseDisciplineId = Guid.NewGuid().ToString();
                    praiseDiscipline.PraiseDisciplineCode = _prefix;
                    var praiseDisciplineCode = "";
                    if (_praiseDisciplineBll.Insert(praiseDiscipline, ref praiseDisciplineCode))
                    {
                        return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (!_praiseDisciplineBll.Update(praiseDiscipline))
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
                var praiseDiscipline = _praiseDisciplineBll.GetPraiseDiscipline(id);
                if (praiseDiscipline == null)
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                if (_praiseDisciplineBll.Delete(id))
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
            PraiseDisciplineDetailsInMemory = null;
            return null;
        }

        public ActionResult SearchEmployee()
        {
            return PartialView();
        }

        public JsonResult Employees(string keyword)
        {
            var employees = new List<Employee>();
            if (!string.IsNullOrEmpty(keyword))
            {
                employees =
                    _employeeBll.GetEmployeesByKeyword(keyword).AsEnumerable()
                        .Where(x => !PraiseDisciplineDetailsInMemory.Select(y => y.EmployeeId).Contains(x.EmployeeId))
                        .ToList();
            }
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEmployee(long employeeId)
        {
            try
            {
                var praiseDisciplineDetails = PraiseDisciplineDetailsInMemory;
                if (praiseDisciplineDetails.Find(x => x.EmployeeId == employeeId) == null)
                {
                    var employee = _employeeBll.GetEmployee(employeeId);
                    praiseDisciplineDetails.Add(new PraiseDisciplineDetail
                    {
                        FullName = employee.FullName,
                        DepartmentId = employee.DepartmentId,
                        EmployeeId = employee.EmployeeId,
                        EmployeeCode = employee.EmployeeCode
                    });
                }
                PraiseDisciplineDetailsInMemory = praiseDisciplineDetails;
                return Json(new {Status = 1, Message = "Chọn nhân viên thành công!"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteEmployee(long employeeId)
        {
            try
            {
                var praiseDisciplineDetails = PraiseDisciplineDetailsInMemory;
                praiseDisciplineDetails.Remove(praiseDisciplineDetails.Find(x => x.EmployeeId == employeeId));
                PraiseDisciplineDetailsInMemory = praiseDisciplineDetails;
                return Json(new {Status = 1, Message = "Xóa nhân viên thành công!"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}