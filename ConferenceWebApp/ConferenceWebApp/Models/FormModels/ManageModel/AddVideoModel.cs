using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.ManageModel
{
    public class AddVideoModel
    {
        public int ID { get; set; }

        [Display(Name = "File Type")]
        [DataType(DataType.Text)]
        public string FileType { get; set; }

        //[Display(Name = "Presentation File")]
        //[Required(ErrorMessage = "(Required)")]
        //public IEnumerable<HttpPostedFileBase> UploadFiles { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Display(Name = "Video Url")]
        [DataType(DataType.Url)]
        [Required(ErrorMessage = "(Required)")]
        public string FileUrl { get; set; }
    }
}