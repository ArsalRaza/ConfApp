using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.ManageModel
{
    public class ConferenceModel
    {

        public int ID { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(200, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "ConferenceName")]
        public string ConferenceName { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "Conference Date")]
        public DateTime ConferenceDate { get; set; }

        [StringLength(300, ErrorMessage = "Must be less than {1} characters.")]
        [Display(Name = "Organizer")]
        public string Organizer { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(8000, ErrorMessage = "Must be less than {1} characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "Conference Description")]
        public string ConferenceDescription { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "Must be less than {1} characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(300, ErrorMessage = "Must be less than {1} characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        public string Address { get; set; }
        
        //[StringLength(300, ErrorMessage = "Must be less than {1} characters.")]
        //[DataType(DataType.Text)]
        //[Display(Name = "Fee Structure")]
        //public string FeeStructure { get; set; }

        //[StringLength(500, ErrorMessage = "Must be less than {1} characters.")]
        //[DataType(DataType.Text)]
        //[Display(Name = "City Information")]
        //public string CityInformation { get; set; }
        

        
    }

    public class CityInformationModel
    {
        [Required(ErrorMessage = "(Required)")]
        [StringLength(500, ErrorMessage = "Must be less than {1} characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "City Information")]
        public string CityInformation { get; set; }
    }

    public class FeeStructureModel
    {
        [Required(ErrorMessage = "(Required)")]
        [StringLength(300, ErrorMessage = "Must be less than {1} characters.")]
        [DataType(DataType.Text)]
        [Display(Name = "Fee Structure")]
        public string FeeStructure { get; set; }
    }
    

}