using System;
using System.IO;
using System.Web.Hosting;

namespace Core.Helper
{
    /// <summary>
    /// ApplicationHelper 
    /// </summary>
    public class ApplicationHelper
    {
        /// <summary>
        /// Restarts this instance.
        /// </summary>
        /// <exception cref="System.Exception">
        /// We needs to be restarted due to a configuration change, but was unable to do so. + Environment.NewLine +
        ///                     To prevent this issue in the future, a change to the web server configuration is required: + Environment.NewLine +
        ///                     - run the application in a full trust environment, or + Environment.NewLine +
        ///                     - give the application write access to the 'web.config' file.
        /// or
        /// We needs to be restarted due to a configuration change, but was unable to do so. + Environment.NewLine +
        ///                     To prevent this issue in the future, a change to the web server configuration is required: + Environment.NewLine +
        ///                     - run the application in a full trust environment, or + Environment.NewLine +
        ///                     - give the application write access to the 'Global.asax' file.
        /// </exception>
        public static void Restart()
        {
            bool success = TryWriteWebConfig();
            if (!success)
            {
                throw new Exception("We needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
                    "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
                    "- run the application in a full trust environment, or" + Environment.NewLine +
                    "- give the application write access to the 'web.config' file.");
            }

            success = TryWriteGlobalAsax();
            if (!success)
            {
                throw new Exception("We needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
                    "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
                    "- run the application in a full trust environment, or" + Environment.NewLine +
                    "- give the application write access to the 'Global.asax' file.");
            }
        }

        /// <summary>
        /// Tries the write web config.
        /// </summary>
        /// <returns></returns>
        public static bool TryWriteWebConfig()
        {
            try
            {
                // In medium trust, "UnloadAppDomain" is not supported. Touch web.config
                // to force an AppDomain restart.
                File.SetLastWriteTimeUtc(MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries the write global asax.
        /// </summary>
        /// <returns></returns>
        public static bool TryWriteGlobalAsax()
        {
            try
            {
                //When a new plugin is dropped in the Plugins folder and is installed into nopCommerce, 
                //even if the plugin has registered routes for its controllers, 
                //these routes will not be working as the MVC framework couldn't 
                //find the new controller types and couldn't instantiate the requested controller. 
                //That's why you get these nasty errors 
                //i.e "Controller does not implement IController".
                //The issue is described here: http://www.nopcommerce.com/boards/t/10969/nop-20-plugin.aspx?p=4#51318
                //The solutino is to touch global.asax file
                File.SetLastWriteTimeUtc(MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>
        /// The physical path. E.g. "c:\inetpub\wwwroot\bin"
        /// </returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }
            else
            {
                //not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                return Path.Combine(baseDirectory, path);
            }
        }
    }
}
