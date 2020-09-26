using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Singleton;

namespace AdminSoftware.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserBll _userBll;

        public LoginController()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Login(string userName, string password)
        {
            try
            {
                var user = _userBll.Login(userName, password);
                if (user == null)
                {
                    return Json(new {Status = 0, Message = "Thông tin đăng nhập không chính xác!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (!user.IsActive)
                {
                    return Json(new {Status = 0, Message = "Tài khoản của bạn đã bị khóa!"},
                        JsonRequestBehavior.AllowGet);
                }

                user = _userBll.GetUser(user.UserId);
                if (user == null)
                {
                    return Json(new { Status = 0, Message = "Thông tin đăng nhập không chính xác!" },
                        JsonRequestBehavior.AllowGet);
                }
                Session["UserModel"] = user;
                var ckUserId = new HttpCookie("UserId")
                {
                    Value = user.UserId.ToString(CultureInfo.InvariantCulture),
                    Expires = DateTime.Now.AddDays(1)
                };
                Response.Cookies.Add(ckUserId);
                return Json(new {Status = 1, Message = "Đăng nhập thành công!"},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Logout()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            FormsAuthentication.SignOut();
            //clear session
            Session.Clear();
            return RedirectToAction("Index", "Login", new {Area = ""});
        }
    }
}