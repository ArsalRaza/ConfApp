using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.BL.Constants
{
    public static class Constants
    {
        public static class Roles
        {
            public static string SiteAdmin = "Site Admin";
            public static string User = "User";
            public static string Speaker = "Speaker";
        }


        public static class FilePaths
        {
            public static string ProfileImagesServerRelativePath = "~/Files/ProfileImages/";
            public static string VideoServerRelativePath = "~/Files/Videos";
            public static string AudioServerRelativePath = "~/Files/Audios";
            public static string DocumentServerRelativePath = "~/Files/Presentations";
            public static string PictureServerRelativePath = "~/Files/Pictures";

            public static string ProfileImagesPath = "/Files/ProfileImages";
            public static string VideoPath = "/Files/Videos";
            public static string AudioPath = "/Files/Audios";
            public static string DocumentPath = "/Files/Presentations";
            public static string PicturePath = "/Files/Pictures";
        }


        public static class FileTypes
        {
            public static string Video = "Video";
            public static string Picture = "Picture";
            public static string Document = "Document";

        }

        //public static class Controllers
        //{
        //    public static string Administration = "Administration";
        //    public static string App = "App";
        //    public static string Name = "Home";

        //}
    }
}