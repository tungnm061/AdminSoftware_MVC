using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.System.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.System;

namespace AdminSoftware.Areas.System.Controllers
{
    public class AccountController : BaseController
    {
        private readonly EmployeeBll _employeeBll;
        private readonly ModuleGroupBll _moduleGroupBll;
        private readonly UserBll _userBll;
        private readonly UserGroupBll _userGroupBll;

        public AccountController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _moduleGroupBll = SingletonIpl.GetInstance<ModuleGroupBll>();
            _userGroupBll = SingletonIpl.GetInstance<UserGroupBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
        }

        [SystemFilter]
        public ActionResult Index()
        {
            ViewBag.UserGroups =
                _userGroupBll.GetUserGroups()
                    .Select(x => new KendoForeignKeyModel(x.GroupName, x.UserGroupId.ToString()));
            ViewBag.ModuleGroups =
                _moduleGroupBll.GetModuleGroups()
                    .Select(x => new KendoForeignKeyModel(x.GroupName, x.ModuleGroupId.ToString()));
            ViewBag.Employees =
                _employeeBll.GetEmployees(null)
                    .Select(x => new KendoForeignKeyModel(x.EmployeeCode + "-" + x.FullName, x.EmployeeId.ToString()));
            ViewBag.Roles = from Role s in Enum.GetValues(typeof (Role))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            return View();
        }

        public JsonResult Accounts(bool isActive)
        {
            return Json(_userBll.GetUsers(isActive), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Account(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new User
                {
                    IsActive = true
                });
            }
            return PartialView(_userBll.GetUser(int.Parse(id)));
        }

        public ActionResult UserRight(int userId)
        {
            var user = _userBll.GetUser(userId);
            ViewBag.UserId = userId;
            return PartialView(_userBll.GetRightsAuthority(user.UserId, user.ModuleGroupId ?? 0));
        }

        [HttpPost]
        public JsonResult Save(UserModel model)
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
                if (model.RoleId != (byte) Role.Admin && (model.ModuleGroupId == null || model.ModuleGroupId == 0))
                {
                    return Json(new {Status = 0, Message = "Bạn phải chọn phân hệ!"}, JsonRequestBehavior.AllowGet);
                }
                if (model.EmployeeId != null && model.RoleId == (byte) Role.Admin)
                {
                    return Json(new {Status = 0, Message = "Không thể gán nhân viên cho tài khoản quản trị!"},
                        JsonRequestBehavior.AllowGet);
                }
                var user = _userBll.GetUser(model.UserName);
                var employeeAccount = _userBll.GetUserByEmployeeId(model.EmployeeId ?? 0);
                model.CreateDate = DateTime.Now;

                if (model.UserId <= 0)
                {
                    if (user != null)
                    {
                        return Json(new {Status = 0, Message = "Tên đăng nhập đã tồn tại trong hệ thống!"},
                            JsonRequestBehavior.AllowGet);
                    }
                    if (employeeAccount != null)
                    {
                        return
                            Json(
                                new
                                {
                                    Status = 0,
                                    Message =
                                        "Nhân viên bạn chọn đã có tài khoản. Bạn không thể tạo 2 tài khoản cho 1 nhân viên!"
                                },
                                JsonRequestBehavior.AllowGet);
                    }
                    return Json(_userBll.Insert(model.ToObject()) > 0
                        ? new {Status = 1, Message = MessageAction.MessageCreateSuccess}
                        : new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                if (user != null && user.UserId != model.UserId)
                {
                    return Json(new {Status = 0, Message = "Tên đăng nhập đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (employeeAccount != null && model.EmployeeId != employeeAccount.EmployeeId)
                {
                    return
                        Json(
                            new
                            {
                                Status = 0,
                                Message =
                                    "Nhân viên bạn chọn đã có tài khoản. Bạn không thể tạo 2 tài khoản cho 1 nhân viên!"
                            },
                            JsonRequestBehavior.AllowGet);
                }
                return Json(_userBll.Update(model.ToObject())
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

        public JsonResult ResetPassWord(int id)
        {
            try
            {
                if (_userBll.UpdatePassword(id, "123456"))
                {
                    return Json(new {Status = 1, Message = "Reset mật khẩu về '123456' thành công!"},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveUserFunction(List<Rights> model, int userId)
        {
            try
            {
                if (model == null || !model.Any())
                    return Json(new {Status = 0, Message = "Bạn chưa phân quyền cho tài khoản"},
                        JsonRequestBehavior.AllowGet);
                if (_userBll.UpdateRights(userId, model))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageUpdateSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Employee(string id)
        {
            var employee = _employeeBll.GetEmployee(long.Parse(id));
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}