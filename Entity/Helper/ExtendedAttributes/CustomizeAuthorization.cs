//using System;
//using System.Web;
//using System.Web.Mvc;

//namespace AdminSoftware.Helper
//{
//    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
//    public class CustomizeAuthorizationAttribute : AuthorizeAttribute
//    {
//        //アクセス不可の時
//        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
//        {
//            string SubWeb = filterContext.HttpContext.Request.ApplicationPath;
//            //Check has Referrer and Referrer is page change password
//            if (filterContext.HttpContext.Request.UrlReferrer != null)
//            {
//                string sPage = filterContext.HttpContext.Request.UrlReferrer.AbsolutePath;
//                if (sPage.IndexOf("PTUR07") > 0)
//                {
//                    //Pass Unauthorized Request
//                    return;
//                }
//            }
//            //ログインしていない時
//            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
//            {
//                /*filterContext.Result = new RedirectToRouteResult(
//                        new RouteValueDictionary(new { controller = "PTUR01", action = "PTUR0101" })
//                );*/
//                string rawUrl = filterContext.HttpContext.Request.RawUrl.Remove(0, (SubWeb.Equals("/") ? 0 : SubWeb.Length));
//                filterContext.HttpContext.Session["find.Login.RedirectURL"] = rawUrl;
//                string url = filterContext.HttpContext.Request.Url.ToString();
//				string redirectURL = "";
//                if (url.IndexOf("FindGateWay") != -1)
//                {
//                    redirectURL = (SubWeb.Equals("/") ? string.Empty : SubWeb) + "/" + "?" + rawUrl;
//                }
//                else {
//                    redirectURL = (SubWeb.Equals("/") ? string.Empty : SubWeb) + "/";
//                }
//				if (filterContext.HttpContext.Request.IsAjaxRequest())
//                {
//                    filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
//                    JsonResult result = new JsonResult
//                    {
//                        Data = new
//                        {
//                            message = LocalizeHelper.GetValue("DION0139")
//                        },
//                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
//                    };
//                    filterContext.Result = result;
//                }
//                else
//                {
//                    filterContext.Result = new RedirectResult(redirectURL);
//                }
//            }
//            //ログイン済み、アクセスできない時
//            else
//            {
//                filterContext.HttpContext.Session["find.Login.RedirectURL"] = filterContext.HttpContext.Request.RawUrl;
//                //filterContext.HttpContext.Response.Redirect((SubWeb.Equals("/") ? string.Empty : SubWeb) + "/");
//                if (filterContext.HttpContext.Request.IsAjaxRequest())
//                {
//                    filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
//                    JsonResult result = new JsonResult
//                    {

//                        Data = new
//                        {
//                            messsage = LocalizeHelper.GetValue("DION0139")
//                        },
//                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
//                    };
//                    filterContext.Result = result;
//                }
//                else
//                {
//                    filterContext.Result = new RedirectResult((SubWeb.Equals("/") ? string.Empty : SubWeb) + "/");
//                }

//                /*filterContext.Result = new RedirectToRouteResult(
//                        new RouteValueDictionary(new { controller = "Account", action = "NotAuthorized" })
//                );*/
//            }
//        }

//        //認証実施
//        protected override bool AuthorizeCore(HttpContextBase httpContext)
//        {
//            SessionManager scM = new SessionManager(httpContext.Session);
//            return scM.IsAuthenticated();
//        }
//    }
//}