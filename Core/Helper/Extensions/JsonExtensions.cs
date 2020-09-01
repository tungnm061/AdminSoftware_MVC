using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Core.Helper.Extensions
{
    public static class JsonExtensions
    {
        //http://iterativo.wordpress.com/2013/04/04/converting-c-razor-models-into-javascript-objects/
        public static IHtmlString ToJson(this HtmlHelper helper, object obj)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            return helper.Raw(JsonConvert.SerializeObject(obj, settings));
        }
    }
}
