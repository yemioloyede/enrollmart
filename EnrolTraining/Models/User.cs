using EnrolTraining.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnrolTraining.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public int CompanyID { get; set; }

        [Required]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [UniqueUser(ErrorMessage ="This user name has already been taken")]
        //[Remote("IsUniqueUserName", "Access", ErrorMessage = "This user name has already been taken")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNo { get; set; }

        [Display(Name = "Name to Print on Card")]
        public string NameOnCardPrint { get; set; }

        [Display(Name = "AHA Instructor ID")]
        public string AHAInstructorID { get; set; }

        [Display(Name = "ASHI Instructor ID")]
        public string ASHIInstructorID { get; set; }

        
        [Required]
        [RegularExpression (@"(\S)+", ErrorMessage ="White space is not allowed")]
        [EmailAddress]
        [UniqueEmail(ErrorMessage = "This email is already in used")]
        //[Remote("IsUniqueEmail", "Access", AdditionalFields ="UserID", ErrorMessage = "This email is already in used")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum 8 Characters Required")]
        public string Password { get; set; }

        [Display(Name = "Options")]
        public bool IsActive { get; set; }

        [Display(Name = "Roles")]
        public bool IsTrainingSiteAdmin { get; set; }

        public bool IsInstructor { get; set; }
        public bool IsInstructorAssistant { get; set; }

        public bool IsWebManager { get; set; }

        public Nullable<DateTime> LastActivity { get; set; }

        public string MonitorDate { get; set; }
        public string DuesPaidThru { get; set; }
        public string Notes { get; set; }

        public bool isSendEmailWhenAssignedClass { get; set; }
        public bool isSendReminderPriorClass { get; set; }
        public bool isDisAbleMobileFriendly { get; set; }
        public bool isDfaultUpcomingClassToShowAll { get; set; }

    }
}