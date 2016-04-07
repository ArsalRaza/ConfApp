using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.BL.Helper
{
    public static class SessionHelper
    {
        public static void CreateSession(string SessionKey, string Object)
        {
            HttpContext.Current.Session[SessionKey] = Object;
        }

        public static int? getSession(string SessionKey)
        {
            return Convert.ToInt32(HttpContext.Current.Session[SessionKey]);
        }
    }
}