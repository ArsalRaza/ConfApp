using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.BL.Helper
{
    public class AuthenticationHelper
    {
        public static HttpCookie LoginCookie
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["cPmOdEskoUsErkInFoie"] != null)
                    return HttpContext.Current.Request.Cookies["cPmOdEskoUsErkInFoie"];
                else
                    return null;
            }
        }

        public static void CreateLoginCookie(String Username, String Name, String Role, String Email, int UserId)
        {
            String CookieText = EncryptionHelper.Encrypt(Username) + ":"
                + EncryptionHelper.Encrypt(Name) + ":"
                + EncryptionHelper.Encrypt(Role) + ":"
                + EncryptionHelper.Encrypt(Email) + ":"
                + EncryptionHelper.Encrypt(UserId.ToString());

            HttpCookie UserInfo = new HttpCookie("cPmOdEskoUsErkInFoie");
            UserInfo.Value = CookieText;
            // Cookie Expires on Coming Sunday at 12:00:00 AM
            int days = 7 - (int)DateTime.Now.DayOfWeek;
            DateTime dt = DateTime.Now.AddDays(days > 0 ? days : 7);
            DateTime dtcookexp = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            UserInfo.Expires = dtcookexp;
            HttpContext.Current.Response.Cookies.Add(UserInfo);
        }

        public static void CreateLoginSession(String Username, String Name, String Role, String Email, int UserId)
        {
            String SessionText = EncryptionHelper.Encrypt(Username) + ":"
                + EncryptionHelper.Encrypt(Name) + ":"
                + EncryptionHelper.Encrypt(Role) + ":"
                + EncryptionHelper.Encrypt(Email) + ":"
                + EncryptionHelper.Encrypt(UserId.ToString());
            
            HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"] = SessionText;
        }

        //public static void UpdateName(String Name)
        //{
        //    if (AuthenticationHelper.IsLoginCookieExists)
        //    {
        //        String[] userdata = AuthenticationHelper.LoginCookie.Value.Split(':');
        //        RemoveCookie();
        //        CreateLoginCookie(EncryptionHelper.Decrypt(userdata[0]), Name, Convert.ToInt32(EncryptionHelper.Decrypt(userdata[2])),
        //        Convert.ToInt32(EncryptionHelper.Decrypt(userdata[3])), Convert.ToInt32(EncryptionHelper.Decrypt(userdata[4])));
        //    }
        //    else if (AuthenticationHelper.IsLoginSessionExists)
        //    {
        //        String[] userdata = ((String)HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"]).Split(':');
        //        RemoveLoginSession();
        //        CreateLoginSession(EncryptionHelper.Decrypt(userdata[0]), Name, Convert.ToInt32(EncryptionHelper.Decrypt(userdata[2])),
        //        Convert.ToInt32(EncryptionHelper.Decrypt(userdata[3])), Convert.ToInt32(EncryptionHelper.Decrypt(userdata[4])));
        //    }
        //}
        public static string GetUserUsername()
        {
            if (AuthenticationHelper.IsLoginCookieExists)
            {
                String[] userdata = AuthenticationHelper.LoginCookie.Value.Split(':');
                return EncryptionHelper.Decrypt(userdata[0]);
            }
            else if (AuthenticationHelper.IsLoginSessionExists)
            {
                String[] userdata = ((String)HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"]).Split(':');
                return EncryptionHelper.Decrypt(userdata[0]);
            }
            return "";
        }

        public static string GetEmail()
        {
            if (AuthenticationHelper.IsLoginCookieExists)
            {
                String[] userdata = AuthenticationHelper.LoginCookie.Value.Split(':');
                return EncryptionHelper.Decrypt(userdata[3]);
            }
            else if (AuthenticationHelper.IsLoginSessionExists)
            {
                String[] userdata = ((String)HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"]).Split(':');
                return EncryptionHelper.Decrypt(userdata[3]);
            }
            return "";
        }

        public static String GetName()
        {
            if (AuthenticationHelper.IsLoginCookieExists)
            {
                String[] userdata = AuthenticationHelper.LoginCookie.Value.Split(':');
                return EncryptionHelper.Decrypt(userdata[1]);
            }
            else if (AuthenticationHelper.IsLoginSessionExists)
            {
                String[] userdata = ((String)HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"]).Split(':');
                return EncryptionHelper.Decrypt(userdata[1]);
            }
            return "";
        }

        public static int GetUserId()
        {
            if (AuthenticationHelper.IsLoginCookieExists)
            {
                String[] userdata = AuthenticationHelper.LoginCookie.Value.Split(':');
                return Convert.ToInt32(EncryptionHelper.Decrypt(userdata[4]));
            }
            else if (AuthenticationHelper.IsLoginSessionExists)
            {
                String[] userdata = ((String)HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"]).Split(':');
                return Convert.ToInt32(EncryptionHelper.Decrypt(userdata[4]));
            }
            return 0;
        }

        //public static int GetRoleID()
        //{
        //    if (AuthenticationHelper.IsLoginCookieExists)
        //    {
        //        String[] userdata = AuthenticationHelper.LoginCookie.Value.Split(':');
        //        return Convert.ToInt32(EncryptionHelper.Decrypt(userdata[2]));
        //    }
        //    else if (AuthenticationHelper.IsLoginSessionExists)
        //    {
        //        String[] userdata = ((String)HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"]).Split(':');
        //        return Convert.ToInt32(EncryptionHelper.Decrypt(userdata[2]));
        //    }
        //    return -1;
        //}

        public static string GetRole()
        {
            if (AuthenticationHelper.IsLoginCookieExists)
            {
                String[] userdata = AuthenticationHelper.LoginCookie.Value.Split(':');
                return Convert.ToString(EncryptionHelper.Decrypt(userdata[2]));
            }
            else if (AuthenticationHelper.IsLoginSessionExists)
            {
                String[] userdata = ((String)HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"]).Split(':');
                return Convert.ToString(EncryptionHelper.Decrypt(userdata[2]));
            }
            return "";
        }

        //public static int GetProgramID()
        //{
        //    if (AuthenticationHelper.IsLoginCookieExists)
        //    {
        //        String[] userdata = AuthenticationHelper.LoginCookie.Value.Split(':');
        //        return Convert.ToInt32(EncryptionHelper.Decrypt(userdata[3]));
        //    }
        //    else if (AuthenticationHelper.IsLoginSessionExists)
        //    {
        //        String[] userdata = ((String)HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"]).Split(':');
        //        return Convert.ToInt32(EncryptionHelper.Decrypt(userdata[3]));
        //    }
        //    return -1;
        //}

        //public static int GetHopID()
        //{
        //    if (AuthenticationHelper.IsLoginCookieExists)
        //    {
        //        String[] userdata = AuthenticationHelper.LoginCookie.Value.Split(':');
        //        return Convert.ToInt32(EncryptionHelper.Decrypt(userdata[4]));
        //    }
        //    else if (AuthenticationHelper.IsLoginSessionExists)
        //    {
        //        String[] userdata = ((String)HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"]).Split(':');
        //        return Convert.ToInt32(EncryptionHelper.Decrypt(userdata[4]));
        //    }
        //    return -1;
        //}

        /// <summary>
        /// Checks whether login cookie exists in user computer if user checked the remember me checkbox
        /// </summary>
        public static bool IsLoginCookieExists
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["cPmOdEskoUsErkInFoie"] != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Checks whether login session exists if user did not checked the remember me checkbox.
        /// </summary>
        public static bool IsLoginSessionExists
        {
            get
            {
                if (HttpContext.Current.Session["sPmEdEsksUsErSInFoIon"] != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Removes the login cookie if exists.
        /// </summary>
        public static void RemoveCookie()
        {
            if (IsLoginCookieExists)
            {
                HttpCookie UserInfo = new HttpCookie("cPmOdEskoUsErkInFoie");
                UserInfo.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(UserInfo);
            }
        }

        /// <summary>
        /// Abandons the login session if exists.
        /// </summary>
        public static void RemoveLoginSession()
        {
            if (IsLoginSessionExists)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.RemoveAll();
            }
        }

        /// <summary>
        /// Checks whether user is login by looking the the Session or Cookie
        /// </summary>
        public static bool IsUserLogin
        {
            get
            {
                return (AuthenticationHelper.IsLoginCookieExists || AuthenticationHelper.IsLoginSessionExists);
            }
        }

        public static void LogoutUser()
        {
            if (IsUserLogin)
            {
                AuthenticationHelper.RemoveCookie();
                AuthenticationHelper.RemoveLoginSession();
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Session.RemoveAll();

            }
        }

    }
}