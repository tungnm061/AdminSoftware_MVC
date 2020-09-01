using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Core.Configuration;
using Dapper;
using Core.Security.Crypt;

namespace Core.Helper.Cache
{
    /// <summary>
    /// Cache Helper for all class implement cache
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// The _schema
        /// </summary>
        public string _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheHelper"/> class.
        /// </summary>
        /// <param name="schema">The schema.</param>
        public CacheHelper(string schema)
        {
            _schema = schema;
        }


        /// <summary>
        /// Gets a value indicating whether [use cache].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use cache]; otherwise, <c>false</c>.
        /// </value>
        public bool UseCache
        {
            get
            {
                return Config.GetConfigByKey("UseCache").ToLower() == "true";
            }
        }

        /// <summary>
        /// Generates the key cache.
        /// </summary>
        /// <returns></returns>
        public string GenerateKeyCache()
        {
            var stackTrace = new StackTrace(true);
            var methodBase = stackTrace.GetFrame(1).GetMethod();
            var type = methodBase.ReflectedType;
            var className = type.Name;
            var key = string.Format("{0}-{1}_{2}", _schema, className, methodBase.Name);
            //var nameSpace = type.Namespace;
            //var aParams = methodBase.GetParameters();
            return Md5Util.Md5EnCrypt(key);
        }

        /// <summary>
        /// Generates the key cache.
        /// </summary>
        /// <param name="oParams">The o params.</param>
        /// <returns></returns>
        public string GenerateSuffixKeyByParams(object oParams)
        {
            var key = "";
            if (oParams == null)
            {
                return key;
            }
            //if(oParams.Count > 0)
            //Dapper.DynamicParameters
            //oParams.parameters
            var obj = oParams.GetType();
            if (obj.FullName == "Dapper.DynamicParameters")
            {
                var listParams = oParams as DynamicParameters;
                if (listParams != null)
                {
                    var parameters = listParams.ListParameters;
                    if (parameters.Count > 0)
                    {
                        foreach (var parameter in parameters)
                        {
                            key += string.Format(Config.GetConfigByKey("CacheFormatGenerate"), parameter.Key, parameter.Value);
                        }
                    }
                }
            }
            else
            {
                var properties = obj.GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var attributes = propertyInfo.GetCustomAttributes(false);
                    if (propertyInfo.CanRead && !propertyInfo.GetMethod.IsVirtual
                        && !attributes.Any(a => a is KeyAttribute)
                        && !attributes.Any(a => a is NotMappedAttribute)
                        )
                    {
                        var value = propertyInfo.GetValue(oParams, null);
                        var name = propertyInfo.Name;
                        key += string.Format("_{0}_{1}", name, value);
                    }
                }
            }

            //foreach (PropertyInfo propertyInfo in properties)
            //{
            //    var attributes = propertyInfo.GetCustomAttributes(false);
            //    if (propertyInfo.CanRead && !propertyInfo.GetMethod.IsVirtual
            //            && !attributes.Any(a => a is ElasticPropertyAttribute)
            //           && !attributes.Any(a => a is KeyAttribute)
            //           && !attributes.Any(a => a is NotMappedAttribute)
            //    )
            //    {
            //        var value = propertyInfo.GetValue(oParams, null);
            //        var name = propertyInfo.Name;
            //        if (name == "ParameterNames")
            //        {
            //            var listParams = oParams as DynamicParameters;
            //            if (listParams != null)
            //            {
            //                var parameters = listParams.ListParameters;
            //                if (parameters.Count > 0)
            //                {
            //                    foreach (var parameter in parameters)
            //                    {
            //                        key += string.Format("_{0}_{1}", parameter.Key, parameter.Value);
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            key += string.Format("_{0}_{1}", name, value);
            //        }
            //    }
            //}
            return key;
        }

        /// <summary>
        /// Generates the key cache output.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string GenerateKeyCacheOutput(string key, string name)
        {
            return string.Format(Config.GetConfigByKey("CacheFormatGenerateOutputParam"), key, name);
        }
    }
}
