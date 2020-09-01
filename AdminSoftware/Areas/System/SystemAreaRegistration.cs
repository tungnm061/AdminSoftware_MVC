using System.Web.Mvc;

namespace AdminSoftware.Areas.System
{
    public class SystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "System";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "System_default",
                "System/{controller}/{action}/{id}",
                new { controller ="Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}