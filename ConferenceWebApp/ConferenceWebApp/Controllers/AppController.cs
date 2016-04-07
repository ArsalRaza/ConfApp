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

        public ActionResult ViewPrograms()
        {
            

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {


            }

            return View();
        }

        public async Task<ActionResult> EditAgenda(int AgendaId = 0)
        {
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User)) || AgendaId == 0)
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                MyAgenda EditAgenda = await DBContext.MyAgenda.FindAsync(AgendaId);
                if (EditAgenda == null)
                {

                    return HttpNotFound();
                }

                MyAgendaModel EditMyAgendaModel = new MyAgendaModel()
                {
                    ID = EditAgenda.ID,
                    Date = (DateTime)EditAgenda.Date,
                    Agenda = EditAgenda.Agenda,
                    ProgramDay = (int)EditAgenda.Day,
                    Title = EditAgenda.Title,
                    UserId = (int)EditAgenda.UserId
                };

                ViewBag.Days = Helper.GetProgramDays();

                return View(EditMyAgendaModel);
            }

        }

        [HttpPost]
        public async Task<ActionResult> EditAgenda(MyAgendaModel model)
        {
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User)))
                return RedirectToAction("Index", "Login");
            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    MyAgenda EditAgenda = DBContext.MyAgenda.FirstOrDefault(x => x.ID == model.ID);
                    EditAgenda.Day = model.ProgramDay;
                    EditAgenda.Title = model.Title;
                    EditAgenda.Agenda = model.Agenda;

                    int ConferenceId = Helper.GetConferenceId();
                    DateTime ConferenceDate = Helper.getConferenceDate();

                    if (!(model.ProgramDay <= 1))
                    {
                        EditAgenda.Date = ConferenceDate.AddDays(--model.ProgramDay);
                    }
                    else
                    {
                        EditAgenda.Date = ConferenceDate;
                    }

                    await DBContext.SaveChangesAsync();
                    return RedirectToAction("MyAgenda", "App");
                }

            }

            ViewBag.Days = Helper.GetProgramDays();
            return View();
        }


        public async Task<ActionResult> DeleteAgenda(int AgendaId = 0)
        {
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User)) || AgendaId == 0)
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                MyAgenda AgendaToRemove = DBContext.MyAgenda.FirstOrDefault(x => x.ID == AgendaId);

                if (AgendaToRemove == null)
                {

                    return HttpNotFound();
                }

                DBContext.MyAgenda.Remove(AgendaToRemove);

                await DBContext.SaveChangesAsync();
                return RedirectToAction("MyAgenda", "App");
            }
        }


        public async Task<ActionResult> MyAgenda(string From)
        {
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User)))
                return RedirectToAction("Index", "Login", new { From = From });

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int CurrentUserId = AuthenticationHelper.GetUserId();

                return View(await DBContext.MyAgenda.Where(x => x.UserId == CurrentUserId).OrderBy(i => i.Date).ToListAsync());
            }

        }


        public ActionResult AddMyAgenda()
        {
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker)))
                return RedirectToAction("Index", "Login");

            ViewBag.Days = Helper.GetProgramDays();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddMyAgenda(MyAgendaModel model)
        {
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker)))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    MyAgenda NewMyAgenda = new MyAgenda();
                    NewMyAgenda.Title = model.Title;
                    NewMyAgenda.Agenda = model.Agenda;
                    NewMyAgenda.Day = model.ProgramDay;
                    NewMyAgenda.UserId = AuthenticationHelper.GetUserId();
                    int ConferenceID = Helper.GetConferenceId();
                    Conference Conference = await DBContext.Conference.FirstOrDefaultAsync(x => x.ID == ConferenceID);
                    DateTime ConferenceDate = (DateTime)Conference.ConferenceDate;

                    if (!(model.ProgramDay <= 1))
                    {
                        NewMyAgenda.Date = ConferenceDate.AddDays(--model.ProgramDay);
                    }
                    else
                    {
                        NewMyAgenda.Date = ConferenceDate;
                    }

                    DBContext.MyAgenda.Add(NewMyAgenda);
                    await DBContext.SaveChangesAsync();

                    return RedirectToAction("MyAgenda", "App");
                }
            }

            ViewBag.Days = Helper.GetProgramDays();

            return View();
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
        //public IEnumerable<string> Get()
        //{
        //    string dirPath = Path.Combine(ConfigurationManager.AppSettings["MyFilesPath"].ToString());
        //    List<string> files = new List<string>();
        //    DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
        //    foreach (FileInfo fInfo in dirInfo.GetFiles())
        //    {
        //        files.Add(fInfo.Name);
        //    }
        //    return files.ToArray();
        //}




    }
}