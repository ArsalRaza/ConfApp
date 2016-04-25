using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.ManageModel
{
    public class DocumentModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "Title")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Display(Name = "Description ")]
        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Display(Name = "Presentation File")]
        [Required(ErrorMessage = "(Required)")]
        public HttpPostedFileBase UploadFile { get; set; }

        [Display(Name = "Speaker")]
        [Required(ErrorMessage = "(Required)")]
        public int SpeakerId { get; set; }

    }
}