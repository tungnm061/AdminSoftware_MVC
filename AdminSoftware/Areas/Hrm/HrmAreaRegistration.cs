using System.Web.Mvc;

namespace AdminSoftware.Areas.Hrm
{
    public class HrmAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Hrm";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Hrm_default",
                "Hrm/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}