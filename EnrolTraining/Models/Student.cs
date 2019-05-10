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
    public class Student
    {
        [Key]
        public int StudentID { get; set; }

        public int CompanyID { get; set; }
        public int ClassID { get; set; }

        
        public DateTime RegisterionDate { get; set; }

        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Primary Phone")]
        public string PrimaryPhone { get; set; }

        [Display(Name = "Alternate Phone")]
        public string AlternatePhone { get; set; }

        [Required]
        [Display(Name = "Address 1")]
        public string MailingAddress1 { get; set; }

        [Display(Name = "Address 2")]
        public string MailingAddress2 { get; set; }

        [Required]
        [Display(Name = "City")]
        public string MailingCity { get; set; }

        [Required]
        [Display(Name = "State")]
        public string MailingState { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public string MailingZip { get; set; }

        [Required(ErrorMessage ="Please select Payment Type")]
        [Display(Name = "Type")]
        public int PaymentType { get; set; }

        [NotMapped]
        [Required]
        [Display(Name = "Card Number")]
        public string CardNo { get; set; }

        [NotMapped]
        [Required]
        [Display(Name = "Expiration")]
        public string ExpirationMonth { get; set; }

        [NotMapped]
        [Required]
        public string ExpirationYear { get; set; }

        [NotMapped]
        [Required]
        [Display(Name = "Card Security Code")]
        public string SecurityCode { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Please agree to our terms & conditions to proceed")]
        public bool TermsAndConditionAgreed { get; set; }

        public bool IsBillingSameAsMailing { get; set; }

        public int LocationID { get; set; }


        public string PromoCode { get; set; }

        public int PromoCodeID { get; set; }

        [NotMapped]
        public bool IsAcceptedPromoCode { get; set; }

        [NotMapped]
        public double DiscountPrice { get; set; }
        public Nullable<int> ClientID { get; set; }

        public Nullable<int> DeliveryID { get; set; }

        public string Comments { get; set; }

        public string Codes { get; set; }

        public string CertificationType { get; set; }

        public Nullable<int> StatusID { get; set; }

        public string CheckedInStatus { get; set; }

        public string CertificateNo { get; set; }

        public string TestScore { get; set; }

        public Nullable<int> Remediation { get; set; }


        [Display (Name ="Delivery")]
        public string DeliveryOption { get; set; }

        [NotMapped]
        public string DeliveryPharase { get; set; }


        [NotMapped]
        [Display(Name = "Course")]
        public string CourseName { get; set; }

        [NotMapped]
        [Display(Name = "Date/Time")]
        public DateTime ScheduleDate { get; set; }

        [NotMapped]
        public string StartTime { get; set; }

        [NotMapped]
        public string EndTime { get; set; }

        [NotMapped]
        [Display(Name = "Location")]
        public string Location { get; set; }

        //[NotMapped]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Class Price")]
        public double ClassPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Selected Options")]
        public double OptionsPrice { get; set; }
        public double ShippingPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double TotalClassPrice { get; set; }

        public string SelectedOptions { get; set; }

        [NotMapped]
        public string ExtraClassTimes { get; set; }


        [NotMapped]
        public bool ShowDeliveryOptions { get; set; }

        [NotMapped]
        public bool showShippingInDeliveryOptions { get; set; }

        [NotMapped]
        public int ShippingDays { get; set; }

        [NotMapped]
        public string PickupPharase { get; set; }

        [NotMapped]
        public string[] QuestionID { get; set; }

        [NotMapped]
        public string[] Answer { get; set; }

        public string QuestionIDs { get; set; }
        public string Answers { get; set; }
        
        [NotMapped]
        public Nullable<double> ShippingCostMaterial { get; set; }

        //[NotMapped]
        //public string CourseImage { get; set; }

        //[NotMapped]
        //public string CourseDescription { get; set; }

        
        [NotMapped]
        public string CourseAddOns { get; set; }

        [NotMapped]
        [Display(Name = "Options")]
        public string[] SelectedAddOns { get; set; }

        
        public bool IsUnSchedule { get; set; }

        [NotMapped]
        public string RescheduleClassID { get; set; }

        [NotMapped]
        public bool PayOnArrival { get; set; }

        [NotMapped]
        public string PaypalEmail { get; set; }

        [NotMapped]
        public string CourseType { get; set; }

        [NotMapped]
        public bool IsPromptCertification { get; set; }

        [NotMapped]
        public Nullable<int> KeycodeBankID { get; set; }

        [NotMapped]
        public double PaymentsTotal { get; set; }


        public string Keycode { get; set; }
        public int KeycodeID { get; set; }


        public string ReceiptCode { get; set; }


    }
}