using EnrolTraining.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnrolTraining.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [UniqueSubDomain(ErrorMessage = "This Scheduling domain has already been taken")]
        //[Remote("IsUniqueSubDomain", "Access", ErrorMessage = "This Scheduling domain has already been taken")]
        [Display(Name = "Scheduling Domain")]
        [NotMapped]
        public string SubDomain { get; set; }

        [Display(Name = "Tag Line")]
        public string TagLine { get; set; }

        [Display(Name = "Contact First Name")]

        public string ContactFirstName { get; set; }

        [Display(Name = "Contact Last Name")]
        public string ContactLastName { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        public string Website { get; set; }

        [Required]
        public string Phone { get; set; }
        public string Fax { get; set; }

        [Required]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        [Display(Name = "Time Zone")]
        public string TimeZone { get; set; }

        [Display(Name = "Training Center")]
        public string TrainingCenter { get; set; }



        [NotMapped]
        public User User { get; set; }

    }
}