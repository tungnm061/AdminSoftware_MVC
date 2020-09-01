using System.Configuration;

namespace Core.Configuration
{
    /// <summary>
    /// Get config from .config file dynamic key
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets the config by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetConfigByKey(string key)
        {
            var sconnectionString = ConfigurationManager.AppSettings[key];
            return sconnectionString;
        }
    }
}
