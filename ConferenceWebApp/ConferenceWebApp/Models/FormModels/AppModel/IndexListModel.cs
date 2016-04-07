using ConferenceWebApp.BL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.AppModel
{
    public class IndexListModel
    {

        public List<UserProfile> UserProfiles { get; set; }
        //public Program Program { get; set; }
        public List<Program> Programs { get; set; }
        public List<DateAndDay> ProgramDatesAndDays { get; set; }
        //public List<ProgramMemberCategories> Categories { get; set; }
        public List<ProgramPeople> ProgramPeople { get; set; }
        public Conference ConferenceDetail { get; set; }

        public List<File> Pictures { get; set; }
    }



}