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
using ConferenceWebApp.Models.FormModels.MembershipModel;
using System.IO;

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
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker)))
                return RedirectToAction("Index", "Login", new { ControllerName = "Home", ActionName = "Files" });

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                var Files = await DBContext.File.Where(x => x.FileType == Constants.FileTypes.Document).Select(x => x.SpeakerId).ToListAsync();

                var SpeakerProfiles = await DBContext.UserProfile.Where(x => Files.Contains(x.ID)).ToListAsync();

                return View(SpeakerProfiles);
            }

        }

        public async Task<ActionResult> FilesOfSpeaker(int SpeakerId = 0)
        {
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker)) || SpeakerId == 0)
                return RedirectToAction("Index", "Login", new { ControllerName = "Home", ActionName = "Files" });

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                return View(await DBContext.File.Include(x => x.UserProfile).Where(x => x.FileType == Constants.FileTypes.Document && x.SpeakerId == SpeakerId).ToListAsync());
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
                //

                ViewBag.speakersUserProfiles = await DBContext.UserProfile.Where(x => x.Role == Constants.Roles.Speaker).ToListAsync();

                ViewBag.ParticipantsUserProfiles = await DBContext.UserProfile.Where(x => x.Role == Constants.Roles.User).ToListAsync();

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
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<SponsorsAndPartners> Sponsors = await DBContext.SponsorsAndPartners.Include(y => y.SponsorPartnerCategory).Where(x => x.SponsorPartnerCategory.CategoryType == Constants.SponsorAndPartnerCategoryTypes.Sponsor).ToListAsync();

                var PartnerCategoryIds = Sponsors.Select(x => x.CategoryID).ToList();
                ViewBag.PartnerCategory = await DBContext.SponsorPartnerCategory.Where(x => PartnerCategoryIds.Contains(x.ID)).OrderBy(x => x.Sequence).ToListAsync();

                return View(Sponsors);
            }
        }

        public async Task<ActionResult> Partners()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<SponsorsAndPartners> Partners = await DBContext.SponsorsAndPartners.Include(y => y.SponsorPartnerCategory).Where(x => x.SponsorPartnerCategory.CategoryType == Constants.SponsorAndPartnerCategoryTypes.Partner).ToListAsync();

                var PartnerCategoryIds = Partners.Select(x => x.CategoryID).ToList();
                ViewBag.PartnerCategory = await DBContext.SponsorPartnerCategory.Where(x => PartnerCategoryIds.Contains(x.ID)).OrderBy(x => x.Sequence).ToListAsync();

                return View(Partners);
            }
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

        public async Task<ActionResult> MyAgenda()
        {
            if (!AuthenticationHelper.IsUserLogin && (!AuthenticationHelper.GetRole().Equals(Constants.Roles.User) || !AuthenticationHelper.GetRole().Equals(Constants.Roles.Speaker)))
                return RedirectToAction("Index", "Login", new { ControllerName = "Home", ActionName = "MyAgenda" });


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

        public async Task<ActionResult> Organizers()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<Organizers> Organizers = await DBContext.Organizers.ToListAsync();
                return View(Organizers);
            }
        }

        public async Task<ActionResult> Exhibition()
        {

            return View();
        }

        public async Task<ActionResult> ExhibitorsList()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<Exhibition> Exhibitors = await DBContext.Exhibition.ToListAsync();
                return View(Exhibitors);
            }
        }

        public async Task<ActionResult> FloorPlan()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                
                return View(await DBContext.Conference.FirstOrDefaultAsync());
            }
        }

        public async Task<ActionResult> Gallery()
        {

            return View();
        }

        public async Task<ActionResult> ForgotPassword()
        {

            return View();

        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    UserProfile user = await DBContext.UserProfile.FirstOrDefaultAsync(i => i.Username == model.Username);

                    if (user != null)
                    {
                        string Password = RandomPassword.Generate(8, 10);
                        user.Password = Password;

                        user.IsReset = 1;

                        //Send Email
                        Helper.SendEmail(user.Email, user.Name, Password);

                        await DBContext.SaveChangesAsync();
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        ViewBag.Error = "The Username/Email do not exist";
                    }


                }
            }
            return View();

        }

        public async Task<ActionResult> ChangePassword()
        {

            //IndexListModel IndexListModel = new IndexListModel();
            //using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            //{

            //    IndexListModel.ConferenceDetail = await DBContext.Conference.FirstOrDefaultAsync();

            //}

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    int CurrentUserId = AuthenticationHelper.GetUserId();
                    UserProfile user = await DBContext.UserProfile.FirstOrDefaultAsync(i => i.ID == CurrentUserId);

                    if (user != null)
                    {
                        if (user.Password.Equals(model.CurrentPassword))
                        {
                            user.Password = model.NewPassword;
                            user.IsReset = 0;

                            await DBContext.SaveChangesAsync();

                            return RedirectToAction("Index", "Login");
                        }
                        else
                        {
                            ViewBag.Error = "Your current password is incorrect.";
                        }
                    }
                    else
                    {
                        ViewBag.Error = "The Username/Email do not exist";
                    }
                }
            }
            return View();

        }

        public FileResult Download(string FileName)
        {
            string path = Path.Combine(Server.MapPath(Constants.FilePaths.DocumentServerRelativePath),
                                       Path.GetFileName(FileName));

            return new FilePathResult(path, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            //return File(path, System.Net.Mime.MediaTypeNames.Application.Octet);
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