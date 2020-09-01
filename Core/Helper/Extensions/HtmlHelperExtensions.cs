using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Newtonsoft.Json;

namespace Core.Helper.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string Image(this HtmlHelper helper, string name, string url)
        {
            return Image(helper, name, url, null);
        }

        public static string Image(this HtmlHelper helper, string name, string url, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("img");

            tagBuilder.GenerateId(name);
            tagBuilder.Attributes["src"] = new UrlHelper(helper.ViewContext.RequestContext).Content(url);
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return tagBuilder.ToString();
        }

        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string strBuilder = "", object obj = null)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var css = "";
            if (actionName == currentAction && controllerName == currentController)
            {
                css = "class='active'";
                //return htmlHelper.ActionLink(linkText, actionName, controllerName, null, new { @class = "selected" });
            }

            return new MvcHtmlString(string.Format(strBuilder, htmlHelper.ActionLink(linkText, actionName, controllerName), css));
        }

        public static IHtmlString ObjectToStringJson(this HtmlHelper htmlHelper, object json)
        {
            return htmlHelper.Raw(JsonConvert.SerializeObject(json, Formatting.None,
                new JsonSerializerSettings
                    {
                        //NullValueHandling = NullValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                        //Formatting = Newtonsoft.Json.Formatting.Indented,
                    }));
        }

    }
}
