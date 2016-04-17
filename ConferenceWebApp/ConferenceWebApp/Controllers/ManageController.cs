﻿using ConferenceWebApp.BL.Helper;
using ConferenceWebApp.Models;
using ConferenceWebApp.Models.FormModels.ManageModel;
using ConferenceWebApp.Models.FormModels.ManageModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using ConferenceWebApp.BL.Constants;
using ConferenceWebApp.Models.FormModels.MembershipModel;

namespace ConferenceWebApp.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /Manage/
        public ActionResult Index()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            return View();
        }

        public async Task<ActionResult> EditConferenceDetails()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                Conference EditConference = await DBContext.Conference.FirstOrDefaultAsync();
                if (EditConference == null)
                {
                    return HttpNotFound();
                }


                ConferenceModel EditConferenceModel = new ConferenceModel()
                {
                    Address = Convert.ToString(EditConference.Address),
                    City = EditConference.City,

                    ConferenceDate = Convert.ToDateTime(EditConference.ConferenceDate).Date,
                    ConferenceDescription = EditConference.ConferenceDescription,
                    ConferenceName = EditConference.ConferenceName,
                    EndDate = Convert.ToDateTime(EditConference.EndDate).Date,
                    ID = EditConference.ID,
                    Organizer = EditConference.Organizer
                };

                return View(EditConferenceModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditConferenceDetails(ConferenceModel model)
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                if (ModelState.IsValid)
                {

                    int ConferenceId = Helper.GetConferenceId();
                    Conference EditConference = DBContext.Conference.FirstOrDefault(x => x.ID == model.ID);

                    EditConference.Address = model.Address;
                    EditConference.City = model.City;
                    EditConference.ConferenceDate = model.ConferenceDate.Date;
                    EditConference.ConferenceDescription = model.ConferenceDescription;
                    EditConference.ConferenceName = model.ConferenceName;
                    EditConference.EndDate = model.EndDate.Date;
                    EditConference.Organizer = model.Organizer;

                    DBContext.SaveChanges();

                    return RedirectToAction("Index", "Manage");

                }


                Conference LoadEditConference = await DBContext.Conference.FirstOrDefaultAsync();
                if (LoadEditConference == null)
                {
                    return HttpNotFound();
                }


                ConferenceModel EditConferenceModel = new ConferenceModel()
                {
                    Address = Convert.ToString(LoadEditConference.Address),
                    City = LoadEditConference.City,
                    ConferenceDate = Convert.ToDateTime(LoadEditConference.ConferenceDate).Date,
                    ConferenceDescription = LoadEditConference.ConferenceDescription,
                    ConferenceName = LoadEditConference.ConferenceName,
                    EndDate = Convert.ToDateTime(LoadEditConference.EndDate).Date,
                    ID = LoadEditConference.ID,
                    Organizer = LoadEditConference.Organizer
                };

                return View(EditConferenceModel);
            }


        }

        public ActionResult AddProgram()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            ViewBag.Days = Helper.GetProgramDays();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProgram(AddProgramModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    int ConferenceId = Helper.GetConferenceId();
                    Conference Confrenence = DBContext.Conference.FirstOrDefault(x => x.ID == ConferenceId);
                    DateTime ConferenceDate = (DateTime)Confrenence.ConferenceDate;


                    Program NewProgram = new Program();
                    NewProgram.ProgrameName = model.ProgramName;
                    NewProgram.StartTime = model.StartTime;
                    NewProgram.EndTime = model.EndTime;
                    NewProgram.ProgramDescription = model.ProgramDescription;
                    NewProgram.ProgramDay = model.ProgramDay;
                    NewProgram.ConferenceID = Helper.GetConferenceId();

                    if (!(model.ProgramDay <= 1))
                    {
                        NewProgram.ProgramDate = ConferenceDate.AddDays(model.ProgramDay);
                    }
                    else
                    {
                        NewProgram.ProgramDate = ConferenceDate;
                    }

                    DBContext.Program.Add(NewProgram);
                    DBContext.SaveChanges();

                    return RedirectToAction("ViewPrograms", "Manage");
                }
            }
            ViewBag.Days = Helper.GetProgramDays();
            return View();
        }

        public ActionResult EditProgram(int ProgramId = 0)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin) || ProgramId == 0)
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                Program EditProgram = DBContext.Program.Find(ProgramId);
                if (EditProgram == null)
                {
                    return HttpNotFound();
                }

                AddProgramModel AddProgramModel = new AddProgramModel()
                {
                    ProgramId = EditProgram.ID,
                    ProgramDate = (DateTime)EditProgram.ProgramDate,
                    ProgramDescription = EditProgram.ProgramDescription,
                    StartTime = (TimeSpan)EditProgram.StartTime,
                    EndTime = (TimeSpan)EditProgram.EndTime,
                    ProgramName = EditProgram.ProgrameName,
                    ProgramDay = Convert.ToInt32(EditProgram.ProgramDay)
                };

                ViewBag.Days = Helper.GetProgramDays();

                return View(AddProgramModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProgram(AddProgramModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    int ConferenceId = Helper.GetConferenceId();
                    Conference Confrenence = DBContext.Conference.FirstOrDefault(x => x.ID == ConferenceId);
                    DateTime ConferenceDate = (DateTime)Confrenence.ConferenceDate;

                    Program UpdateProgram = DBContext.Program.First(x => x.ID == model.ProgramId);
                    UpdateProgram.ProgrameName = model.ProgramName;
                    UpdateProgram.StartTime = model.StartTime;
                    UpdateProgram.EndTime = model.EndTime;
                    UpdateProgram.ProgramDescription = model.ProgramDescription;

                    if (!(model.ProgramDay <= 1))
                    {
                        UpdateProgram.ProgramDate = ConferenceDate.AddDays(model.ProgramDay);
                    }
                    else
                    {
                        UpdateProgram.ProgramDate = ConferenceDate;
                    }

                    DBContext.SaveChanges();

                    return RedirectToAction("ViewPrograms", "Manage");
                }
            }
            ViewBag.Days = Helper.GetProgramDays();
            return View();
        }

        public ActionResult ViewPrograms()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            ManageListsModel ProgramDetails = new ManageListsModel();
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                ProgramDetails.Programs = DBContext.Program.OrderBy(x => x.ProgramDate).ThenBy(i => i.StartTime).ToList();
                ProgramDetails.ProgramDates = ProgramDetails.Programs.Select(x => x.ProgramDate).Distinct().ToList();

            }
            return View(ProgramDetails);
        }

        public async Task<ActionResult> ProgramDetails(int? ProgramId = 0)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin) || ProgramId == 0)
                return RedirectToAction("Index", "Login");


            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {

                ManageListsModel model = new ManageListsModel();
                model.Program = await DBContext.Program.Include(x => x.ProgramPeople).FirstOrDefaultAsync(x => x.ID == ProgramId);

                var CategoryIDs = model.Program.ProgramPeople.Select(x => x.CategoryId);

                model.Categories = await DBContext.ProgramMemberCategories.Where(x => CategoryIDs.Contains(x.ID)).ToListAsync();

                var UserProfileIds = model.Program.ProgramPeople.Select(x => x.UserId);
                model.UserProfiles = await DBContext.UserProfile.Where(x => UserProfileIds.Contains(x.ID)).ToListAsync();

                //var userProfiles = DBContext.Program.Include();


                //var Param_ProgramId = new SqlParameter("@ProgramId", ProgramId);
                //model.UserProfiles = DBContext.UserProfile.SqlQuery("Select * from UserProfile Where ID in (Select UserId from ProgramPeople where ProgramId = @ProgramId)", Param_ProgramId).ToList();


                return View(model);

            }
        }

        public async Task<ActionResult> DeleteProgram(int ProgramId = 0)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin) || ProgramId == 0)
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                Program Program = await DBContext.Program.FirstOrDefaultAsync(x => x.ID == ProgramId);

                List<ProgramPeople> ProgramPeoples = Program.ProgramPeople.ToList();

                DBContext.ProgramPeople.RemoveRange(ProgramPeoples);
                DBContext.Program.Remove(Program);

                await DBContext.SaveChangesAsync();

                return RedirectToAction("ViewPrograms", "Manage");
            }
        }
        public ActionResult AddMemberToProgram(int ProgramId = 0)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin) || ProgramId == 0)
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                ViewBag.UserProfiles = DBContext.UserProfile.ToList();
                ViewBag.Categories = DBContext.ProgramMemberCategories.ToList();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMemberToProgram(AddMemberToProgramModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    ProgramPeople NewProgramPeople = new ProgramPeople();
                    NewProgramPeople.CategoryId = model.CategoryId;
                    NewProgramPeople.UserId = model.MemberId;
                    NewProgramPeople.ProgramId = Convert.ToInt32(Request["ProgramId"]);
                    DBContext.ProgramPeople.Add(NewProgramPeople);
                    DBContext.SaveChanges();

                    return RedirectToAction("ProgramDetails", "Manage", new { ProgramId = NewProgramPeople.ProgramId });

                }
            }
            return View();
        }

        public ActionResult RemoveMemberFromProgram(int UserId, int ProgramId = 0)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");


            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                var ProgramPeople = DBContext.ProgramPeople.FirstOrDefault(x => x.UserId == UserId && x.ProgramId == ProgramId);
                DBContext.ProgramPeople.Remove(ProgramPeople);
                DBContext.SaveChanges();

                return RedirectToAction("ProgramDetails", "Manage", new { ProgramId = ProgramId });
            }

            return View();
        }

        public async Task<ActionResult> AddDocument()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");


            return View();
        }


        [HttpPost]
        public async Task<ActionResult> AddDocument(DocumentModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");
            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    ConferenceWebApp.Models.File NewFile = new ConferenceWebApp.Models.File();

                    NewFile.Title = model.Title;
                    NewFile.Description = model.Description;
                    NewFile.FileName = model.UploadFile.FileName;
                    NewFile.FileType = Constants.FileTypes.Document;

                    if (model.UploadFile != null && model.UploadFile.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath(Constants.FilePaths.DocumentServerRelativePath),
                                       Path.GetFileName(model.UploadFile.FileName));
                        model.UploadFile.SaveAs(path);
                    }

                    DBContext.File.Add(NewFile);
                    await DBContext.SaveChangesAsync();

                    return RedirectToAction("ViewDocument", "Manage");
                }
            }
            return View();
        }
        public async Task<ActionResult> ViewDocument(DocumentModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                var Files = await DBContext.File.Where(x => x.FileType == Constants.FileTypes.Document).ToListAsync();

                ViewBag.PresentationFilesPath = Path.Combine(Server.MapPath(Constants.FilePaths.DocumentServerRelativePath));

                return View(Files);
            }
        }

        public async Task<ActionResult> DeleteDocument(int DocumentId)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                ConferenceWebApp.Models.File Document = await DBContext.File.FirstOrDefaultAsync(x => x.ID == DocumentId);

                string DocumentFullPath = Request.MapPath(Constants.FilePaths.DocumentServerRelativePath + Document.FileName);
                if (System.IO.File.Exists(DocumentFullPath))
                {
                    System.IO.File.Delete(DocumentFullPath);
                }

                DBContext.File.Remove(Document);
                await DBContext.SaveChangesAsync();
                return RedirectToAction("ViewDocument", "Manage");

            }
            return View();
        }

        public async Task<ActionResult> AddVideo()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddVideo(AddVideoModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    ConferenceWebApp.Models.File NewFile = new ConferenceWebApp.Models.File();

                    NewFile.Description = model.Description;
                    NewFile.FileUrl = model.FileUrl;
                    NewFile.FileType = Constants.FileTypes.Video;

                    DBContext.File.Add(NewFile);

                    await DBContext.SaveChangesAsync();

                    return RedirectToAction("Videos", "Manage");
                }
            }

            return View();
        }

        public async Task<ActionResult> Videos()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<ConferenceWebApp.Models.File> Videos = await DBContext.File.Where(x => x.FileType == Constants.FileTypes.Video).OrderBy(x => x.ID).ToListAsync();

                return View(Videos);
            }
        }

        public async Task<ActionResult> DeleteVideo(int VideoId)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                ConferenceWebApp.Models.File Video = await DBContext.File.FirstOrDefaultAsync(x => x.ID == VideoId);

                DBContext.File.Remove(Video);

                await DBContext.SaveChangesAsync();

                return RedirectToAction("Videos", "Manage");
            }


        }

        public async Task<ActionResult> AddPictures()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddPictures(UploadPictureModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                foreach (var file in model.UploadFiles)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath(Constants.FilePaths.PictureServerRelativePath),
                                       Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                        using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                        {
                            ConferenceWebApp.Models.File File = new ConferenceWebApp.Models.File();

                            File.FileName = file.FileName;
                            File.FileType = Constants.FileTypes.Picture;

                            DBContext.File.Add(File);
                            await DBContext.SaveChangesAsync();


                        }
                    }
                }
                return RedirectToAction("ViewPictures", "Manage");
            }

            return View();
        }


        public async Task<ActionResult> ViewPictures()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<ConferenceWebApp.Models.File> Pictures = await DBContext.File.Where(x => x.FileType == Constants.FileTypes.Picture).ToListAsync();

                return View(Pictures);
            }

        }

        #region City Info
        public async Task<ActionResult> EditCityInfo()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                CityInformationModel model = new CityInformationModel();

                var Conference = await DBContext.Conference.FirstOrDefaultAsync();
                model.CityInformation = Conference.CityInformation;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditCityInfo(CityInformationModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    var Conference = await DBContext.Conference.FirstOrDefaultAsync();

                    Conference.CityInformation = model.CityInformation;

                    await DBContext.SaveChangesAsync();

                    return RedirectToAction("Index", "Manage");
                }
            }
            return View();
        }
        #endregion

        #region Fee Structure

        public async Task<ActionResult> EditFeeStructure()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                FeeStructureModel model = new FeeStructureModel();
                var Conference = await DBContext.Conference.FirstOrDefaultAsync();
                model.FeeStructure = Conference.FeeStructure;

                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditFeeStructure(FeeStructureModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    var Conference = await DBContext.Conference.FirstOrDefaultAsync();
                    Conference.FeeStructure = model.FeeStructure;
                    await DBContext.SaveChangesAsync();

                    return RedirectToAction("Index", "Manage");
                }
            }

            return View();
        }

        #endregion

        public async Task<ActionResult> ViewUsers()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");


            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<UserProfile> UserProfile = await DBContext.UserProfile.Where(x => x.Role != Constants.Roles.SiteAdmin).ToListAsync();

                return View(UserProfile);
            }
        }


        public async Task<ActionResult> DeleteUser(int UserId)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");


            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                UserProfile UserProfile = await DBContext.UserProfile.FirstOrDefaultAsync(x => x.ID == UserId);

                if (!string.IsNullOrEmpty(UserProfile.Photo))
                {
                    string ProfileImageFullPath = Request.MapPath(Constants.FilePaths.ProfileImagesServerRelativePath + UserProfile.Photo);
                    if (System.IO.File.Exists(ProfileImageFullPath))
                    {
                        System.IO.File.Delete(ProfileImageFullPath);
                    }
                }

                DBContext.UserProfile.Remove(UserProfile);
                await DBContext.SaveChangesAsync();

                return RedirectToAction("ViewUsers", "Manage");
            }
        }


        public async Task<ActionResult> EditUserDetails(int UserId)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");


            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                UserProfile UserProfile = await DBContext.UserProfile.FirstOrDefaultAsync(x => x.ID == UserId);

                RegistrationModel Model = new RegistrationModel();
                Model.ID = UserProfile.ID;
                Model.Name = UserProfile.Name;
                Model.Organization = UserProfile.Organization;
                Model.Password = UserProfile.Password;
                Model.Profile = UserProfile.Profile;
                Model.Role = UserProfile.Role;
                Model.Designation = UserProfile.Designation;
                Model.Email = UserProfile.Email;
                Model.Photo = UserProfile.Photo;

                return View(Model);
            }
        }


        [HttpPost]
        public async Task<ActionResult> EditUserDetails(RegistrationModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                UserProfile UserProfile = await DBContext.UserProfile.FirstOrDefaultAsync(x => x.ID == model.ID);

                if (ModelState.IsValid)
                {
                    UserProfile.Name = model.Name;
                    UserProfile.Organization = model.Organization;
                    UserProfile.Profile = model.Profile;
                    UserProfile.Role = model.Role;
                    UserProfile.Designation = model.Designation;
                    UserProfile.Email = model.Email;


                    if (model.ProfileImage != null && model.ProfileImage.ContentLength > 0)
                    {
                        if (!string.IsNullOrEmpty(UserProfile.Photo))
                        {
                            string ProfileImageFullPath = Request.MapPath(Constants.FilePaths.ProfileImagesServerRelativePath + UserProfile.Photo);
                            if (System.IO.File.Exists(ProfileImageFullPath))
                            {
                                System.IO.File.Delete(ProfileImageFullPath);
                            }
                        }

                        string path = Path.Combine(Server.MapPath(Constants.FilePaths.ProfileImagesServerRelativePath),
                                       Path.GetFileName(UserProfile.ID.ToString() + Path.GetExtension(model.ProfileImage.FileName)));
                        model.ProfileImage.SaveAs(path);
                        UserProfile.Photo = UserProfile.ID.ToString() + Path.GetExtension(model.ProfileImage.FileName);

                    }

                    await DBContext.SaveChangesAsync();

                    return RedirectToAction("ViewUsers", "Manage");
                }
                else
                {
                    return View(model);
                }
            }
            return View();
        }

        public async Task<ActionResult> AddSponsorCategory()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddSponsorCategory(SponsorAndPartnerCategoryModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {

                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    SponsorPartnerCategory newSPC = new SponsorPartnerCategory();
                    newSPC.CategoryName = model.CategoryName;
                    newSPC.CategoryType = Constants.SponsorAndPartnerCategoryTypes.Sponsor;

                    DBContext.SponsorPartnerCategory.Add(newSPC);
                    await DBContext.SaveChangesAsync();

                    return RedirectToAction("AddSponsor", "Manage");
                }
            }
            return View();
        }

        public async Task<ActionResult> AddPartnerCategory()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AddPartnerCategory(SponsorAndPartnerCategoryModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {

                using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
                {
                    SponsorPartnerCategory newSPC = new SponsorPartnerCategory();
                    newSPC.CategoryName = model.CategoryName;
                    newSPC.CategoryType = Constants.SponsorAndPartnerCategoryTypes.Partner;

                    DBContext.SponsorPartnerCategory.Add(newSPC);
                    await DBContext.SaveChangesAsync();

                    return RedirectToAction("AddPartner", "Manage");
                }
            }
            return View();
        }



        public async Task<ActionResult> AddSponsor()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                ViewBag.SponsorCategories = await DBContext.SponsorPartnerCategory.Where(x => x.CategoryType == Constants.SponsorAndPartnerCategoryTypes.Sponsor).ToListAsync();
            }


            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddSponsor(SponsorAndPartnerModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                if (ModelState.IsValid)
                {
                    SponsorsAndPartners Sponsors = new SponsorsAndPartners();
                    Sponsors.CategoryID = model.CategoryId;
                    Sponsors.SponsorName = model.SponsorName;

                    DBContext.SponsorsAndPartners.Add(Sponsors);
                    await DBContext.SaveChangesAsync();

                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath(Constants.FilePaths.SponsorAndPartnerImagesServerRelativePath),
                                       Path.GetFileName(Sponsors.ID.ToString() + Path.GetExtension(model.Image.FileName)));
                        model.Image.SaveAs(path);
                        Sponsors.Photo = Sponsors.ID.ToString() + Path.GetExtension(model.Image.FileName);
                        await DBContext.SaveChangesAsync();
                    }

                    return RedirectToAction("ViewSponsors", "Manage");
                }

                ViewBag.SponsorCategories = await DBContext.SponsorPartnerCategory.Where(x => x.CategoryType == Constants.SponsorAndPartnerCategoryTypes.Sponsor).ToListAsync();
            }



            return View();
        }

        public async Task<ActionResult> AddPartner(SponsorAndPartnerCategoryModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                ViewBag.SponsorCategories = await DBContext.SponsorPartnerCategory.Where(x => x.CategoryType == Constants.SponsorAndPartnerCategoryTypes.Partner).ToListAsync();
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddPartner(SponsorAndPartnerModel model)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                if (ModelState.IsValid)
                {
                    SponsorsAndPartners Sponsors = new SponsorsAndPartners();
                    Sponsors.CategoryID = model.CategoryId;
                    Sponsors.SponsorName = model.SponsorName;

                    DBContext.SponsorsAndPartners.Add(Sponsors);
                    await DBContext.SaveChangesAsync();

                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath(Constants.FilePaths.SponsorAndPartnerImagesServerRelativePath),
                                       Path.GetFileName(Sponsors.ID.ToString() + Path.GetExtension(model.Image.FileName)));
                        model.Image.SaveAs(path);
                        Sponsors.Photo = Sponsors.ID.ToString() + Path.GetExtension(model.Image.FileName);
                        await DBContext.SaveChangesAsync();
                    }
                    return RedirectToAction("ViewPartners", "Manage");
                }

                ViewBag.SponsorCategories = await DBContext.SponsorPartnerCategory.Where(x => x.CategoryType == Constants.SponsorAndPartnerCategoryTypes.Partner).ToListAsync();
            }

            return View();
        }

        public async Task<ActionResult> ViewSponsors()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<SponsorPartnerCategory> SponsorCategory = await DBContext.SponsorPartnerCategory.Include(y => y.SponsorsAndPartners).Where(x => x.CategoryType == Constants.SponsorAndPartnerCategoryTypes.Sponsor).ToListAsync();

                return View(SponsorCategory);
            }

        }

        public async Task<ActionResult> ViewPartners()
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                List<SponsorPartnerCategory> PartnerCategory = await DBContext.SponsorPartnerCategory.Include(y => y.SponsorsAndPartners).Where(x => x.CategoryType == Constants.SponsorAndPartnerCategoryTypes.Partner).ToListAsync();

                return View(PartnerCategory);
            }

        }

        public async Task<ActionResult> DeleteSponsor(int SponsorId)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                SponsorsAndPartners Sponsor = await DBContext.SponsorsAndPartners.FirstOrDefaultAsync(x => x.ID == SponsorId);

                if (!string.IsNullOrEmpty(Sponsor.Photo))
                {
                    string ProfileImageFullPath = Request.MapPath(Constants.FilePaths.SponsorAndPartnerImagesServerRelativePath + Sponsor.Photo);
                    if (System.IO.File.Exists(ProfileImageFullPath))
                    {
                        System.IO.File.Delete(ProfileImageFullPath);
                    }
                }

                DBContext.SponsorsAndPartners.Remove(Sponsor);

                await DBContext.SaveChangesAsync();

                return RedirectToAction("ViewSponsors", "Manage");
            }
        }

        public async Task<ActionResult> DeletePartner(int PartnerId)
        {
            if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
                return RedirectToAction("Index", "Login");

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                SponsorsAndPartners Partner = await DBContext.SponsorsAndPartners.FirstOrDefaultAsync(x => x.ID == PartnerId);

                if (!string.IsNullOrEmpty(Partner.Photo))
                {
                    string ProfileImageFullPath = Request.MapPath(Constants.FilePaths.SponsorAndPartnerImagesServerRelativePath + Partner.Photo);
                    if (System.IO.File.Exists(ProfileImageFullPath))
                    {
                        System.IO.File.Delete(ProfileImageFullPath);
                    }
                }

                DBContext.SponsorsAndPartners.Remove(Partner);

                await DBContext.SaveChangesAsync();

                return RedirectToAction("ViewPartners", "Manage");
            }
        }


        public async Task<ActionResult> AddHighlights()
        {
            //    if (!AuthenticationHelper.IsUserLogin || !AuthenticationHelper.GetRole().Equals(Constants.Roles.SiteAdmin))
            //        return RedirectToAction("Index", "Login");


            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {

                return View();
            }
            return View();
        }

    }
}