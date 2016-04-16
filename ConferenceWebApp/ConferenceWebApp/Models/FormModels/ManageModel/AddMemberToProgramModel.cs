using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.ManageModel
{
    public class AddMemberToProgramModel
    {
        [Required(ErrorMessage = "(Required)")]
        public int? ProgramId { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "Select Member")]
        [DataType(DataType.Text)]
        public int? MemberId  { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "Category")]
        [DataType(DataType.Text)]
        public int CategoryId { get; set; }

    }
}