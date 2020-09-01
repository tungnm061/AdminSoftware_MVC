using System;
using System.Web;
using log4net;

namespace Core.Helper.Logging
{
    public static class Logging
    {
        private static readonly ILog Log = LogManager.GetLogger("ForAllApplication");

        public static void PutError(string message, Exception e = null)
        {
            if (HttpContext.Current != null)
            {
                message = message + "; Url: " + HttpContext.Current.Request.Url.AbsoluteUri;
            }
            if (e != null)
            {
                Log.Error(message + "; Error: ", e);
            }
            else
            {
                Log.Error(message);
            }
        }

        public static void PutErrorNoContext(string message, Exception e = null)
        {
            if (e != null)
            {
                Log.Error(message + "; Error: ", e);
            }
            else
            {
                Log.Error(message);
            }
        }
        public static void PushString(string message)
        {
            Log.Error(message);
        }
        // Other Custom Logging Functions
    }
}
