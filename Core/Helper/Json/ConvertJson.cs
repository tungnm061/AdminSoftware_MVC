using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper.Json
{
    public static class ConvertJson
    {
        public static string ObjectToStringJson(object json)
        {
            return JsonConvert.SerializeObject(json, Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
