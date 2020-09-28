using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity.System;

namespace AdminSoftware.Models
{
    public class SessionData
    {

        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session == null)
                {
                    return null;
                }
                if (HttpContext.Current.Session["UserModel"] == null)
                {
                    return null;
                }
                else
                {
                    return (User)HttpContext.Current.Session["UserModel"];
                }
            }
            set
            {
                HttpContext.Current.Session["Account"] = value;
            }
        }

    }
}