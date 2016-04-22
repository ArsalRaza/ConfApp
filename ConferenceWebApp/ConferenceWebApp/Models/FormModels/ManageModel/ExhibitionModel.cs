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


    public class FloorPlanModel
    {
        public int Id { get; set; }

        
        [Display(Name = "")]
        [DataType(DataType.Text)]
        public string FloorPlanText { get; set; }

        [Display(Name = "FloorPlanImage")]
        public HttpPostedFileBase FloorPlanImage { get; set; }
    }


    

}