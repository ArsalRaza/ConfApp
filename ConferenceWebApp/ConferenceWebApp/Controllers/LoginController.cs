using ConferenceWebApp.BL.Constants;
using ConferenceWebApp.BL.Helper;
using ConferenceWebApp.Models;
using ConferenceWebApp.Models.FormModels.MembershipModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferenceWebApp.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index(string ControllerName, string ActionName)
        {
            ViewBag.Title = PageTitles.LoginPage + PageTitles.AppTitle;
            if (AuthenticationHelper.IsUserLogin)
            {
                string Role = AuthenticationHelper.GetRole();

                if (Role.Equals(Constants.Roles.SiteAdmin))
                {
                    return RedirectToAction("Index", "Manage");
                }
                else if (Role.Equals(Constants.Roles.User) || Role.Equals(Constants.Roles.Speaker))
                {
                    if (!string.IsNullOrEmpty(ControllerName) && !string.IsNullOrEmpty(ActionName))
                    {
                        
                            return RedirectToAction(ActionName, ControllerName);
                        
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

                else
                {
                    return RedirectToAction("PageNotFound", "Home");
                }
            }

            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model, string ControllerName, string ActionName)
        {
            ViewBag.Title = PageTitles.LoginPage + PageTitles.AppTitle;
            UserProfile user = new ConferenceAppEntities().UserProfile.FirstOrDefault(i => i.Username == model.Username && i.Password == model.Password);

            if (ModelState.IsValid && user != null)
            {
                if (model.RememberMe == true && Request.Browser.Cookies)
                    AuthenticationHelper.CreateLoginCookie(user.Username, user.Name, user.Role, user.Email, user.ID);
                else
                    AuthenticationHelper.CreateLoginSession(user.Username, user.Name, user.Role, user.Email, user.ID);

                string Role = AuthenticationHelper.GetRole();

                if (Role.Equals(Constants.Roles.SiteAdmin))
                {
                    return RedirectToAction("Index", "Manage");
                }

                else if (Role.Equals(Constants.Roles.User) || Role.Equals(Constants.Roles.Speaker))
                {
                    if (user.IsReset == 1)
                    {
                        return RedirectToAction("ChangePassword", "Home");
                    }
                    if (!string.IsNullOrEmpty(ControllerName) && !string.IsNullOrEmpty(ActionName))
                    {

                        return RedirectToAction(ActionName, ControllerName);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    //TO-BE Done again
                    return RedirectToAction("PageNotFound", "Home");
                }
            }
            ViewBag.Error = "The username or password is incorrect.";

            return View(model);

        }

        public ActionResult Logout()
        {
            AuthenticationHelper.LogoutUser();
            return RedirectToAction("Index", "Login");
        }



    }
}