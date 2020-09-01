using System.Web.Mvc;

namespace Core.Helper.Extensions
{
    public static class UrlHelperExtension
    {
        public static string Home(this UrlHelper helper)
        {
            return helper.Content("~/");
        }
    }
}
