using System.Web.Mvc;

namespace AdminSoftware.Areas.Sale
{
    public class SaleAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "Sale";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Sale_default",
                "Sale/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}