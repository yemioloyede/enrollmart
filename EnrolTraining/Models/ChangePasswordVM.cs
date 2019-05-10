using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class ChangePasswordVM
    {
        [Required]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum 8 Characters Required")]
        public string NewPassword { get; set; }

        [Display(Name = "Repeat Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and repeat password do not match.")]
        public string RepeatPassword { get; set; }

    }
}