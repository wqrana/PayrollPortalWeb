using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeAide.Common.Helpers
{
    public class SessionHelper
    {
        // private constructor
        private SessionHelper()
        {
            //Property1 = "default value";
        }

        // Gets the current session.
        public static void SetSessionVariables( string loginEmail, int userId, string userName, bool isAdmin )
        {
            SessionHelper.LoginId = userId;
            SessionHelper.LoginEmail = loginEmail;
            SessionHelper.UserName = userName;
            SessionHelper.IsAdminUser = isAdmin;
           SessionHelper.UserProfilePicture = string.IsNullOrEmpty("") ? "/images/no-profile-image.jpg" : "";


        }
        public static SessionHelper Current
        {
            get
            {
                SessionHelper session =
                  (SessionHelper)HttpContext.Current.Session["__SessionHelper__"];
                if (session == null)
                {
                    session = new SessionHelper();
                    HttpContext.Current.Session["__SessionHelper__"] = session;
                }
                return session;
            }
        }

        // **** add your session properties here, e.g like this:
                   
       
      
        public static int LoginId
        {
            get
            {
                if (HttpContext.Current==null || HttpContext.Current.Session["LoginId"] == null)
                    return 0;
                return Convert.ToInt32(HttpContext.Current.Session["LoginId"].ToString());
            }
            set
            {
                HttpContext.Current.Session["LoginId"] = value;
            }
        }
        public static string UserProfilePicture
        {
            get
            {
                if (HttpContext.Current.Session["UserProfilePicture"] == null)
                    return "";
                return HttpContext.Current.Session["UserProfilePicture"].ToString();
            }
            set
            {
                HttpContext.Current.Session["UserProfilePicture"] = value;
            }
        }
       
        public static string LoginEmail
        {
            get
            {
                if (HttpContext.Current.Session["LoginEmail"] == null)
                    return "";
                return HttpContext.Current.Session["LoginEmail"].ToString();
            }
            set
            {
                HttpContext.Current.Session["LoginEmail"] = value;
            }
        }
        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["LoginEmail"] == null)
                    return "";
                return HttpContext.Current.Session["UserName"].ToString();
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }

        public static bool IsAdminUser
        {
            get
            {
                if (HttpContext.Current.Session["LoginEmail"] == null)
                    return false;
                return bool.Parse(HttpContext.Current.Session["IsAdminUser"].ToString());
            }
            set
            {
                HttpContext.Current.Session["IsAdminUser"] = value;
            }
        }
    }
}
