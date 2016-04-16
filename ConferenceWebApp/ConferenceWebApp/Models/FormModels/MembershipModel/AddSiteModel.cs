using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.MembershipModel
{
    public class AddSiteModel
    {
        [Required(ErrorMessage = "(Required)")]
        [StringLength(300, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "CompanyName:")]
        [RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "{0} is not valid")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Licence Start Date:")]
        public DateTime LicenceStartDate { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Licence End Date:")]
        public DateTime LicenceEndDate { get; set; }


        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "Must be less than {1} characters.")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        //[System.Web.Mvc.Remote("IsEmailExists", "Membership", HttpMethod = "POST", ErrorMessage = "There is already an account with this Email")]
        [Display(Name = "Owner Email:")]
       
        public string OwnerEmail { get; set; }


    }
}