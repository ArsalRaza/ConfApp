using ConferenceWebApp.BL.Helper;
using ConferenceWebApp.Models;
using ConferenceWebApp.Models.FormModels.AppModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using ConferenceWebApp.BL.Constants;

namespace ConferenceWebApp.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class AppController : AsyncController
    {
        //
        // GET: /App/

        public async Task<ActionResult> Index()
        {
            //if (!AuthenticationHelper.IsUserLogin || AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin) )
            //    return RedirectToAction("Index", "Login");

            using  (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int ConferenceId = Helper.GetConferenceId();

                Conference Conference = await DBContext.Conference.FirstOrDefaultAsync(x => x.ID == ConferenceId);

                return View(Conference);
            }

        }

        public async Task<ActionResult> ListVideos()
        {
            string dirPath = Path.Combine(Server.MapPath("~/App_Data/ProfileImages"));
            List<string> files = new List<string>();
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            /*string fullPath = Request.MapPath("~/Images/Cakes/" + photoName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }*/
            

            //foreach (FileInfo fInfo in dirInfo.GetFiles())
            //{
            //    fInfo.
            //    files.Add(fInfo.Name);
            //}
            // files.ToArray();


            return  View(dirInfo);
        }

        public async Task<ActionResult> SpeakerCategories()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int ConferenceId = Helper.GetConferenceId();

                var ProgramPeople = await DBContext.ProgramPeople.ToListAsync();
                var ProgramCategoryIds = ProgramPeople.Select(x => x.CategoryId);

                List<ProgramMemberCategories> PMC = await DBContext.ProgramMemberCategories.Include(x => x.ProgramPeople).Where(x => ProgramCategoryIds.Contains(x.ID)).ToListAsync();

                return View(PMC);
            }

            return View();
        }



    }
}