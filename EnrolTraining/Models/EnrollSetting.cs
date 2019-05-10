using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnrolTraining.Models
{
    public class EnrollSetting
    {
        [Key]
        public int ID { get; set; }
        public int CompanyID { get; set; }

        [Display (Name ="Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Schedule Page Format")]
        public int SchedulePageFormat { get; set; }

        [AllowHtml]
        [Display(Name = "Schedule Page Text")]
        public string SchedulePageText { get; set; }

        [Display(Name = "Options")]
        public bool UseAccordianOnSchedule { get; set; }
        public bool AgreeTermsForRegistrants { get; set; }
        public bool DisplayClassEndTime { get; set; }
        public bool ShowPromoCode { get; set; }
        public bool RestrictInstructorsForOthersClasses { get; set; }
        public bool CloseRegistrationBeforeClassStart { get; set; }
        public bool PrintTermsInStudentReceipt { get; set; }

        [Display(Name = "Logo Url")]
        public string LogoUrl { get; set; }


        [EmailAddress]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [Display(Name = "'Contact Us' Email Address")]
        public string ContactUsEmail { get; set; }

        [EmailAddress]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [Display(Name = "Mail From Address")]
        public string MailForm { get; set; }

        [Display(Name = "Mail From Display Name")]
        public string DisplayName { get; set; }

        [EmailAddress]
        [RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        [Display(Name = "Paypal Email Address")]
        public string PaypalEmail { get; set; }

        [Display(Name = "Stripe Live Secrect Key")]
        public string StripeLiveSecretKey { get; set; }

        [Display(Name = "Google Analytics Account")]
        public string GoogleAnalyticsAccount { get; set; }
        public bool UseUniversalAnalyticsTag { get; set; }

        [Display(Name = "Class ID Numbers")]
        public int ClassIDGenerationMethod { get; set; }

        [Display(Name = "Confirmation Script")]
        public string ConfirmationScript { get; set; }

        [AllowHtml]
        [Display(Name = "Terms and Conditions")]
        public string TermsCondition { get; set; }
        

        [AllowHtml]
        [Display(Name = "Custom Sidebar")]
        public string CustomerSidebar { get; set; }

        [Display(Name = "Check-in Password")]
        public string CheckinPassword { get; set; }
          

        public string WebExperienceID { get; set; }
    }
}