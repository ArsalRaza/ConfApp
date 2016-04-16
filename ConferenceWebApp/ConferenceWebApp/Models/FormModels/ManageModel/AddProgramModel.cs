using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferenceWebApp.Models.FormModels.ManageModel
{
    public class AddProgramModel
    {
        public int ProgramId { get; set; }


        [Required(ErrorMessage = "(Required)")]
        [StringLength(300, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "Program Name")]
        public string ProgramName { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "Program Description")]
        public string ProgramDescription { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "Program Day")]
        public int ProgramDay { get; set; }


        [Required(ErrorMessage = "(Required)")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public Nullable<DateTime> ProgramDate { get; set; }
        
        //[Display(Name = "Sequence")]
        //public int? Sequence { get; set; }
    }
}