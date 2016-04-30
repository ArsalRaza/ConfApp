using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConferenceWebApp.Models.FormModels.MembershipModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "Email")]
        //[RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "{0} is not valid")]
        public string Username { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "Email")]
        //[RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "{0} is not valid")]
        public string Username { get; set; }

    }


    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "Current Password")]
        //[RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "{0} is not valid")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "New Password")]
        //[RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "{0} is not valid")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again !")]
        //[RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "{0} is not valid")]
        public string ConfirmPassword { get; set; }

    }

    public class ChangePasswordFromAdminModel
    {
        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "New Password")]
        //[RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "{0} is not valid")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "(Required)")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again !")]
        //[RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "{0} is not valid")]
        public string ConfirmPassword { get; set; }
    }
}