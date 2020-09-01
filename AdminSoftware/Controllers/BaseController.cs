using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Security.Crypt;
using Core.Singleton;
using AdminSoftware.Areas.System.Models;
using AdminSoftware.Models;
using Entity.System;

namespace AdminSoftware.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserBll _userBll;

        public BaseController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        public User UserLogin
        {
            get { return (User) Session["UserModel"]; }
            set { Session["UserModel"] = value; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserModel"] == null)
            {
                if (Request != null && Request.Cookies["UserId"] != null)
                {
                    int userId;
                    int.TryParse(Request.Cookies["UserId"].Value, out userId);
                    var user = _userBll.GetUser(userId);
                    if (user != null && user.IsActive)
                    {
                        Session["UserModel"] = user;
                        base.OnActionExecuting(filterContext);
                    }
                    else
                    {
                        filterContext.Result = RedirectToAction("Index", "Login", new {Area = ""});
                    }
                }
                else
                {
                    filterContext.Result = RedirectToAction("Index", "Login", new {Area = ""});
                }
            }
            else
                base.OnActionExecuting(filterContext);
        }

        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        public JsonResult SavePassword(string currentPassword, string password)
        {
            try
            {
                if (UserLogin.Password != Md5Util.Md5EnCrypt(currentPassword))
                {
                    return Json(new {Status = 0, Message = "Mật khẩu hiện tại không chính xác!"},
                        JsonRequestBehavior.AllowGet);
                }
                var user = UserLogin;
                user.Password = Md5Util.Md5EnCrypt(password);
                if (_userBll.UpdatePassword(user.UserId,password))
                {
                    UserLogin = user;
                    return Json(new {Status = 1, Message = "Đổi mật khẩu thành công!"}, JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = "Đổi mật khẩu không thành công!"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChangeInfomation()
        {
            return PartialView(UserLogin);
        }

        public JsonResult SaveInfomation(UserModel model)
        {
            try
            {
                ModelState.Remove("model.UserId");
                ModelState.Remove("model.RoleId");
                ModelState.Remove("model.UserGroupId");
                ModelState.Remove("model.UserName");
                ModelState.Remove("model.Password");
                ModelState.Remove("model.IsActive");
                ModelState.Remove("model.ModuleGroupId");
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
                var user = UserLogin;
                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                if (_userBll.Update(user))
                {
                    UserLogin = user;
                    return Json(new {Status = 1, Message = "Cập nhật thông tin thành công!"},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = "Cập nhật thông tin không thành công!"},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}