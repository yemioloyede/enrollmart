using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Service.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Address 1")]
        public string Address1{ get; set; }

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
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage ="Minimum 8 Characters Required")]
        public string Password { get; set; }

        [Display(Name ="Options")]
        public bool IsActive { get; set; }

        [Display(Name = "Roles")]
        public bool IsTrainingSiteAdmin { get; set; }

        public bool IsInstructor { get; set; }
        public bool IsInstructorAssistant { get; set; }
    }
}
