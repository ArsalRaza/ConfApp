using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferenceWebApp.Models.FormModels.MembershipModel
{
    public class RegistrationModel
    {
        [Display(Name = "Profile Image")]
        public HttpPostedFileBase ProfileImage { get; set; }

        public int ID { get; set; }
        public string Photo { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "Must be less than {1} characters.")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Remote("IsEmailExists", "Membership", ErrorMessage = "There is already an account with this Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [Display(Name = "Role")]
        [DataType(DataType.Text)]
        public string Role { get; set; }

        [Display(Name = "Profile")]
        [DataType(DataType.Text)]
        public string Profile { get; set; }

        [Display(Name = "Designation")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "(Required)")]
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters.")]
        public string Designation { get; set; }

        [Display(Name = "Organization")]
        [Required(ErrorMessage = "(Required)")]
        [StringLength(200, ErrorMessage = "The {0} must be less than {1} characters.")]
        [DataType(DataType.Text)]
        public string Organization { get; set; }

        //[Display(Name = "Profile Picture")]
        
        //public HttpPostedFileBase ProfileImage { get; set; }
        
        
    }
}