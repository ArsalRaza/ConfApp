using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.AppModel
{
    public class MyAgendaModel
    {

        public int ID { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(500, ErrorMessage = "Must be less than {1} characters.")]
        [Display(Name = "Agenda")]
        public string Agenda { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "Program Day")]
        public int ProgramDay { get; set; }

    }
}