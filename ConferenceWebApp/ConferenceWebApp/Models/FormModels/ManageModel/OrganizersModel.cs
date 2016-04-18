using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.ManageModel
{
    public class OrganizersModel
    {

        [Display(Name = "Image")]
        [Required(ErrorMessage = "(Required)")]
        public HttpPostedFileBase Image { get; set; }

        [Display(Name = "Organizer Name")]
        [Required(ErrorMessage = "(Required)")]
        [DataType(DataType.Text)]
        public string OrganizerName { get; set; }

    }
}