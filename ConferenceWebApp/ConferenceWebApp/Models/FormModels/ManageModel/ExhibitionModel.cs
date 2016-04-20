using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.ManageModel
{
    public class ExhibitionModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "Exhibitor Name")]
        [DataType(DataType.Text)]
        public string ExhibitorName { get; set; }

        [Display(Name = "Image")]
        [Required(ErrorMessage = "(Required)")]
        public HttpPostedFileBase Image { get; set; }
    }
}