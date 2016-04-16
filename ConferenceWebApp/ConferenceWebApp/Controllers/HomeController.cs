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
using ConferenceWebApp.BL.Constants;
using ConferenceWebApp.Models.FormModels.HomeModels;

namespace ConferenceWebApp.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {

            IndexListModel IndexListModel = new IndexListModel();
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                IndexListModel.Programs = await DBContext.Program.OrderBy(x => x.ProgramDate).ThenBy(i => i.StartTime).ToListAsync();
                IndexListModel.ProgramDatesAndDays = await Helper.GetProgramDaysWithDate();

                IndexListModel.ProgramPeople = await DBContext.ProgramPeople.Where(x => x.ProgramMemberCategories.CategoryName == "Keynote Speakers").ToListAsync();

                var ProgramPeopleIds = IndexListModel.ProgramPeople.Where(x => x.ProgramMemberCategories.CategoryName == "Keynote Speakers").Select(x => x.UserId).ToList();

                IndexListModel.UserProfiles = await DBContext.UserProfile.Where(x => ProgramPeopleIds.Contains(x.ID)).ToListAsync();

                IndexListModel.ConferenceDetail = await DBContext.Conference.FirstOrDefaultAsync();

                IndexListModel.Pictures = await DBContext.File.Where(x => x.FileType == Constants.FileTypes.Picture).ToListAsync();

                if (AuthenticationHelper.IsUserLogin)
                {
                    int CurrentUserId = AuthenticationHelper.GetUserId();
                    IndexListModel.MyAgendas = await DBContext.MyAgenda.Where(x => x.UserId == CurrentUserId).ToListAsync();
                }


                ViewBag.MemberCategory = "Keynote Speakers";
            }

            return View(IndexListModel);

        }

        public async Task<ActionResult> CityInfo()
        {

            IndexListModel IndexListModel = new IndexListModel();
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {

                IndexListModel.ConferenceDetail = await DBContext.Conference.FirstOrDefaultAsync();

            }

            return View(IndexListModel);

        }

        public async Task<ActionResult> Files()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                return View(await DBContext.File.Where(x => x.FileType == Constants.FileTypes.Document).ToListAsync());
            }

        }

        public async Task<ActionResult> Videos()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                return View(await DBContext.File.Where(x => x.FileType == Constants.FileTypes.Video).ToListAsync());
            }
        }


        public async Task<ActionResult> ImageGallery()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                return View(await DBContext.File.Where(x => x.FileType == Constants.FileTypes.Picture).ToListAsync());
            }
        }

        public async Task<ActionResult> Message()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int CurrentUserId = AuthenticationHelper.GetUserId();
                List<Conversation> Conversations = await DBContext.Conversation.Include(x => x.UserProfile).Include(x => x.UserProfile1).Include(x => x.Conversation_reply).Where(x => x.UserIdOne == CurrentUserId || x.UserIdTwo == CurrentUserId).OrderBy(x => x.Time).ToListAsync();
                return View(Conversations);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SendNewMessage(int ToUserId, string Message)
        {
            //if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker))
            //    return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int CurrentUserId = AuthenticationHelper.GetUserId();

                Conversation NewConversation = new Conversation();
                NewConversation.UserIdOne = CurrentUserId;
                NewConversation.UserIdTwo = ToUserId;
                NewConversation.Time = DateTime.Now.TimeOfDay;
                NewConversation.Status = "Unseen";

                DBContext.Conversation.Add(NewConversation);
                await DBContext.SaveChangesAsync();

                Conversation_reply NewConversationReply = new Conversation_reply();
                NewConversationReply.ConversationID = NewConversation.ID;
                NewConversationReply.Reply = Message;
                NewConversationReply.Time = DateTime.Now;
                NewConversationReply.Status = "Unseen";
                NewConversationReply.UserId = CurrentUserId;

                DBContext.Conversation_reply.Add(NewConversationReply);
                await DBContext.SaveChangesAsync();


                return Json(NewConversation.ID, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ConversationReplies(int ConversationId)
        {
            //if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker))
            //    return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<Conversation_reply> ConversationReplies = await DBContext.Conversation_reply.Where(x => x.ConversationID == ConversationId).Include(x => x.UserProfile).OrderBy(x => x.Time).ToListAsync();

                return View(ConversationReplies);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ReplyToConversation(int ConversationId, string Reply)
        {
            //if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker))
            //    return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int CurrentUserId = AuthenticationHelper.GetUserId();

                Conversation UpdateConversation = await DBContext.Conversation.FirstOrDefaultAsync(x => x.ID == ConversationId);
                UpdateConversation.Time = DateTime.Now.TimeOfDay;
                UpdateConversation.Status = "Unseen";

                await DBContext.SaveChangesAsync();

                Conversation_reply NewConversationReply = new Conversation_reply();
                NewConversationReply.UserId = CurrentUserId;
                NewConversationReply.Time = DateTime.Now;
                NewConversationReply.ConversationID = ConversationId;
                NewConversationReply.Reply = Reply;
                NewConversationReply.Status = "Unseen";

                DBContext.Conversation_reply.Add(NewConversationReply);
                await DBContext.SaveChangesAsync();

                List<Conversation_reply> ConversationReplies = await DBContext.Conversation_reply.Where(x => x.ConversationID == ConversationId).Include(x => x.UserProfile).OrderBy(x => x.Time).ToListAsync();

                return View(ConversationReplies);
            }
        }

        public JsonResult GetMatchingUser(string term)
        {
            Response.CacheControl = "no-cache";
            ConferenceAppEntities DB = new ConferenceAppEntities();
            var usrScreenNamelst = DB.UserProfile.Where(a => (a.Name.StartsWith(term) ||
                   a.Name.StartsWith(term))).ToList();
            string userProfileImagePath = ConferenceWebApp.BL.Constants.Constants.FilePaths.ProfileImagesServerRelativePath + "/";
            var lst = usrScreenNamelst.Select(m => new
            {
                id = m.ID,
                value = m.Name,
                label = m.Name,
                description = m.Profile,
                image = Url.Content(userProfileImagePath + (string.IsNullOrEmpty(m.Photo) ?
                "NoProfileImage.png" : m.Photo))
            }).ToList().Take(5);

            return Json(lst.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AboutSpeaker(int SpeakerId)
        {
            

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                SpeakerDetailsModel SDM = new SpeakerDetailsModel();

                SDM.UserProfile = await DBContext.UserProfile.Include(y => y.ProgramPeople).FirstOrDefaultAsync(x => x.ID == SpeakerId);

                SDM.ProgramPeoples = SDM.UserProfile.ProgramPeople.ToList();

                var ProgramIds = SDM.ProgramPeoples.Select(x => x.ProgramId);

                SDM.Programs = await DBContext.Program.Where(x => ProgramIds.Contains(x.ID)).ToListAsync();

                SDM.ProgramMemberCategories = await DBContext.ProgramMemberCategories.ToListAsync();

                return View(SDM);
            }
        }

        public async Task<ActionResult> Speakers()
        {
            //if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker))
            //    return RedirectToAction("Index", "Login");

            IndexListModel IndexListModel = new IndexListModel();
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                IndexListModel.ProgramPeople = await DBContext.ProgramPeople.Where(x => x.ProgramMemberCategories.CategoryName == "Keynote Speakers").ToListAsync();

                var ProgramPeopleIds = IndexListModel.ProgramPeople.Where(x => x.ProgramMemberCategories.CategoryName == "Keynote Speakers").Select(x => x.UserId).ToList();

                IndexListModel.UserProfiles = await DBContext.UserProfile.Where(x => ProgramPeopleIds.Contains(x.ID)).ToListAsync();
            }

            return View(IndexListModel);
        }

        public async Task<ActionResult> Schedule()
        {
            //if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker))
            //    return RedirectToAction("Index", "Login");

            IndexListModel IndexListModel = new IndexListModel();
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                IndexListModel.Programs = await DBContext.Program.OrderBy(x => x.ProgramDate).ThenBy(i => i.StartTime).ToListAsync();
                IndexListModel.ProgramDatesAndDays = await Helper.GetProgramDaysWithDate();

                IndexListModel.ProgramPeople = await DBContext.ProgramPeople.Where(x => x.ProgramMemberCategories.CategoryName == "Keynote Speakers").ToListAsync();

                var ProgramPeopleIds = IndexListModel.ProgramPeople.Where(x => x.ProgramMemberCategories.CategoryName == "Keynote Speakers").Select(x => x.UserId).ToList();

                IndexListModel.UserProfiles = await DBContext.UserProfile.Where(x => ProgramPeopleIds.Contains(x.ID)).ToListAsync();

                if (AuthenticationHelper.IsUserLogin)
                {
                    int CurrentUserId = AuthenticationHelper.GetUserId();
                    IndexListModel.MyAgendas = await DBContext.MyAgenda.Where(x => x.UserId == CurrentUserId).ToListAsync();
                }
            }

            return View(IndexListModel);
        }


        public async Task<ActionResult> Sponsors()
        {
            //if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker))
            //    return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddToMyAgenda(int ProgramId)
        {
            bool IsAdded = false;
            if (AuthenticationHelper.IsUserLogin && (AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || AuthenticationHelper.GetRole().Equals(Constants.Roles.User)))
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    int CurrentUserId = AuthenticationHelper.GetUserId();
                    if (!DBContext.MyAgenda.Any(x => x.UserId == CurrentUserId && x.ProgramId == ProgramId))
                    {
                        MyAgenda NewMyAgenda = new MyAgenda();
                        NewMyAgenda.UserId = CurrentUserId;
                        NewMyAgenda.ProgramId = ProgramId;
                        DBContext.MyAgenda.Add(NewMyAgenda);
                        await DBContext.SaveChangesAsync();
                        IsAdded = true;
                    }
                }
            }
            return Json(IsAdded, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveFromMyAgenda(int ProgramId)
        {
            bool IsRemoved = false;
            if (AuthenticationHelper.IsUserLogin && (AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || AuthenticationHelper.GetRole().Equals(Constants.Roles.User)))
            {

                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    int CurrentUserId = AuthenticationHelper.GetUserId();

                    MyAgenda NewMyAgenda = await DBContext.MyAgenda.FirstOrDefaultAsync(x => x.UserId == CurrentUserId && x.ProgramId == ProgramId);

                    DBContext.MyAgenda.Remove(NewMyAgenda);
                    await DBContext.SaveChangesAsync();
                    IsRemoved = true;
                }
            }
            return Json(IsRemoved, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> MyAgenda(string From)
        {
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User)))
                return RedirectToAction("Index", "Login", new { From = From });

            IndexListModel IndexListModel = new IndexListModel();
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int CurrentUserId = AuthenticationHelper.GetUserId();

                var ProgramIds = DBContext.MyAgenda.Where(x => x.UserId == CurrentUserId).Select(x => x.ProgramId);

                IndexListModel.Programs = await DBContext.Program.Where(y => ProgramIds.Contains(y.ID)).OrderBy(x => x.ProgramDate).ThenBy(i => i.StartTime).ToListAsync();
                IndexListModel.ProgramDatesAndDays = await Helper.GetProgramDaysWithDate();
                IndexListModel.ProgramPeople = await DBContext.ProgramPeople.Where(x => x.ProgramMemberCategories.CategoryName == "Keynote Speakers").ToListAsync();

                var ProgramPeopleIds = IndexListModel.ProgramPeople.Where(x => x.ProgramMemberCategories.CategoryName == "Keynote Speakers").Select(x => x.UserId).ToList();

                IndexListModel.UserProfiles = await DBContext.UserProfile.Where(x => ProgramPeopleIds.Contains(x.ID)).ToListAsync();

            }

            return View(IndexListModel);
        }

        public async Task<ActionResult> KeynoteMemberByCategoryId(int ProgramMemberCatId)
        {
            //if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker))
            //    return RedirectToAction("Index", "Login");

            IndexListModel IndexListModel = new IndexListModel();
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                ProgramMemberCategories PMC = await DBContext.ProgramMemberCategories.FirstOrDefaultAsync(x => x.ID == ProgramMemberCatId);
                ViewBag.MemberCategory = PMC.CategoryName;
                IndexListModel.ProgramPeople = await DBContext.ProgramPeople.Where(x => x.ProgramMemberCategories.ID == ProgramMemberCatId).ToListAsync();

                var ProgramPeopleIds = IndexListModel.ProgramPeople.Where(x => x.ProgramMemberCategories.ID == ProgramMemberCatId).Select(x => x.UserId).ToList();

                IndexListModel.UserProfiles = await DBContext.UserProfile.Where(x => ProgramPeopleIds.Contains(x.ID)).ToListAsync();

            }

            return View(IndexListModel);
        }







        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {


            return View();
        }

        public ActionResult test()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



    }
}