using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EnrolTraining.Models
{
    public class CourseType
    {
        [Display(Name ="Course ID")]
        public int CourseTypeID { get; set; }

        public int CompanyID { get; set; }

        [Required]
        [Display(Name ="Course Name")]
        public string CourseName { get; set; }

        public string Type { get; set; }

        [Display(Name = "Discipline")]
        public int Discipline { get; set; }

        [Display(Name ="Price Options")]
        public bool PriceOptions_IsAllowedRegistrationWithDeposit { get; set; }
        public bool PriceOptions_IsAllowedMultiplePricingLevel { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.00")]
        public double Price { get; set; }

        [Display(Name ="Add-on Prompt")]
        public string AddonPrompt { get; set; }

        [NotMapped]
        [Display(Name = "Add-ons")]
        public string[] AddOnIDs { get; set; }

        public string AddOns { get; set; }

        [Required (ErrorMessage ="Shipping Price is Required. Enter 0 if not Applicable")]
        [Display(Name ="Shipping Price")]
        [Range(0.00, double.MaxValue, ErrorMessage = "Invalid Shipping Price")]
        public double ShippingPrice { get; set; }

        [Display(Name ="Use Keycode Bank")]
        public Nullable<int> KeycodeBankID { get; set; }

        [Display(Name = "Card Options")]
        public Nullable<int> CardOptionsID { get; set; }

        public string Image { get; set; }

        [Display(Name ="Options")]
        public bool CourseOptions_PromptForCertificationDuringRegistration { get; set; }
        public bool CourseOptions_IncludeStudentToInnstructorRatioOnRoster { get; set; }
        public bool CourseOptions_IncludeStudentToManikinRatioOnRoster { get; set; }
        public bool CourseOptions_IncludeElectronicSignatureForAHARoster { get; set; }
        public bool CourseOptions_UseCertificateNoInsteadofTestScore { get; set; }
        public bool CourseOptions_ShowNumberOfSeatsRemainingOnSchedulePage { get; set; }
        public bool CourseOptions_AllowStudentToSelectWillCallSchedule { get; set; }
        public bool CourseOptions_PromptForLicenceNumberDuringRegistration { get; set; }
        public bool CourseOptions_AllowAnAlternateDateTimeDescription { get; set; }

        [Display(Name = "Calendar Icon Color")]
        public string CalendarIconColor { get; set; }

        [Display(Name = "CEU Credits")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid number")]
        public Nullable<int> CEUCredits { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [AllowHtml]
        [Display(Name ="Confirmation Email")]
        public string ConfirmationEmail { get; set; }

        [Display(Name ="Archive")]
        public bool IsArchive { get; set; }

    }
}
