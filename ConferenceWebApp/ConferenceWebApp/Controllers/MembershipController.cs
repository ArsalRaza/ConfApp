using ConferenceWebApp.BL.Constants;
using ConferenceWebApp.BL.Helper;
using ConferenceWebApp.Models;
using ConferenceWebApp.Models.FormModels.MembershipModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ConferenceWebApp.Controllers
{
    public class MembershipController : Controller
    {
        //
        // GET: /Membership/
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult RegisterUserFromAdmin()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");



            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterUserFromAdmin(RegistrationModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    UserProfile NewUserProfile = new UserProfile();
                    NewUserProfile.Name = model.Name;
                    NewUserProfile.Email = model.Email;
                    NewUserProfile.Designation = model.Designation;
                    NewUserProfile.Organization = model.Organization;
                    NewUserProfile.Password = model.Password;
                    NewUserProfile.Profile = model.Profile;
                    NewUserProfile.Role = model.Role;
                    NewUserProfile.Username = model.Email;
                    

                    DBContext.UserProfile.Add(NewUserProfile);
                    await DBContext.SaveChangesAsync();

                    if (model.ProfileImage != null && model.ProfileImage.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath(Constants.FilePaths.ProfileImagesServerRelativePath),
                                       Path.GetFileName(NewUserProfile.ID.ToString() + Path.GetExtension(model.ProfileImage.FileName)));
                        model.ProfileImage.SaveAs(path);
                        NewUserProfile.Photo = NewUserProfile.ID.ToString() + Path.GetExtension(model.ProfileImage.FileName);
                        await DBContext.SaveChangesAsync();
                    }

                    
                    ViewBag.Status = "The profile of " + model.Name + " have been successfully created.";
                    return RedirectToAction("ViewUsers", "Manage");
                }
            }

            return View();
        }

        public ActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UserRegistration(RegistrationModel model)
        {
            model.Role = Constants.Roles.User;
            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    UserProfile NewUserProfile = new UserProfile();
                    NewUserProfile.Name = model.Name;
                    NewUserProfile.Email = model.Email;
                    NewUserProfile.Designation = model.Designation;
                    NewUserProfile.Organization = model.Organization;
                    NewUserProfile.Password = model.Password;
                    NewUserProfile.Profile = model.Profile;
                    NewUserProfile.Role = model.Role;
                    NewUserProfile.Username = model.Email;

                    DBContext.UserProfile.Add(NewUserProfile);
                    await DBContext.SaveChangesAsync();

                    if (model.ProfileImage != null && model.ProfileImage.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath(Constants.FilePaths.ProfileImagesServerRelativePath),
                                       Path.GetFileName(NewUserProfile.ID.ToString() + Path.GetExtension(model.ProfileImage.FileName)));
                        model.ProfileImage.SaveAs(path);
                        NewUserProfile.Photo = NewUserProfile.ID.ToString() + Path.GetExtension(model.ProfileImage.FileName);
                        await DBContext.SaveChangesAsync();
                    }

                    return RedirectToAction("Index", "Login", new {status = "Your registration have been succesfully done" });
                    
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]

        public JsonResult IsEmailExists(string Email)
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                bool result = DBContext.UserProfile.Any(i => i.Username == Email);
                return Json(!result, JsonRequestBehavior.AllowGet);
            }
        }
        

    }
}