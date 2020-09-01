using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessLogic.System;
using Core.Singleton;
using Entity.System;
using ModuleGroup = Core.Enum.ModuleGroup;

namespace AdminSoftware.Areas.Hrm
{
    public class HrmFilter : ActionFilterAttribute, IActionFilter
    {
        private readonly UserBll _userBll;

        public HrmFilter()
        {
            _userBll = SingletonIpl.GetInstance<UserBll>();
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userModel = (User) HttpContext.Current.Session["UserModel"];
            var rights = _userBll.GetRights(userModel.UserId, userModel.RoleId,null);
            var viewRights = rights.Where(x => x.IsView).ToList();
            if (!rights.Any() || !viewRights.Any())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Dashboard"},
                    {"action", "Index"},
                    {"area", ""}
                });
            }
            HttpContext.Current.Session["Rights"] = viewRights;
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var functionRights = viewRights.Find(x => x.Controller.ToLower() == controllerName.ToLower());
            if (functionRights != null ||
                controllerName.ToLower() == "home")
            {
                HttpContext.Current.Session["FunctionRights"] = functionRights;
                base.OnActionExecuting(filterContext);
            }
            else
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Dashboard"},
                    {"action", "Index"},
                    {"area", ""}
                });
        }
    }
}