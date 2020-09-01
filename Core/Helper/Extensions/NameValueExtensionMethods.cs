using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper.Extensions
{
    //Link ref: http://www.asp.net/ajaxlibrary/jquery_webforms_post_data_to_pagemethod.ashx
    /// <summary>
    /// Name Value Extension
    /// </summary>
    public static class NameValueExtensionMethods
    {
        /// <summary>
        /// Forms the specified form vars.
        /// </summary>
        /// <param name="formVars">The form vars.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string Form(this  NameValue[] formVars, string name)
        {
            if (formVars != null)
            {
                var matches = formVars.FirstOrDefault(nv => nv.name.ToLower() == name.ToLower());
                if (matches != null)
                    return matches.value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Forms the multiple.
        /// </summary>
        /// <param name="formVars">The form vars.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string[] FormMultiple(this  NameValue[] formVars, string name)
        {
            var matches = formVars.Where(nv => nv.name.ToLower() == name.ToLower())
         .Select(nv => nv.value).ToArray();
            if (matches.Length == 0)
                return null;
            return matches;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NameValue
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
