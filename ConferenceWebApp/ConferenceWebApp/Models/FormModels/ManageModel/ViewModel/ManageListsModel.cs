using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.ManageModel.ViewModel
{
    public class ManageListsModel
    {
        public List<UserProfile> UserProfiles { get; set; }

        public Program Program { get; set; }
        public List<Program> Programs { get; set; }
        public List<DateTime?> ProgramDates { get; set; }
        public List<ProgramMemberCategories> Categories { get; set; }
       
    }
}