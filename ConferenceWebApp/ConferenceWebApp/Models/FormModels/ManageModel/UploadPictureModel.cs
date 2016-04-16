using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.ManageModel
{
    public class UploadPictureModel
    {
        public int ID { get; set; }

        [Display(Name = "File Type")]
        [DataType(DataType.Text)]
        public string FileType { get; set; }

        [Display(Name = "Pictures")]
        [Required(ErrorMessage = "(Required)")]
        public IEnumerable<HttpPostedFileBase> UploadFiles { get; set; }

        [Display(Name = "File Name")]
        [DataType(DataType.Text)]
        public string FileName { get; set; }

    }
}