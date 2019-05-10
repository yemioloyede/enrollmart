using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class ScheduleClass
    {
        [Key]
        public int ClassID { get; set; }

        public int CompanyID { get; set; }

        [Required(ErrorMessage ="Please Select a Course")]
        [Display(Name = "Course")]
        public int CourseID { get; set; }


        [Display(Name = "Client")]
        public Nullable<int> ClientID { get; set; }

        [Required(ErrorMessage ="Please provide the location")]
        [Display(Name = "Location")]
        public int LocationID { get; set; }


        [Display(Name = "Instructor")]
        public Nullable<int> InstructorID { get; set; }

        [Display(Name = "Assistant")]
        public Nullable<int> AssistantID { get; set; }


        [Required(ErrorMessage = "Please Provide the maximum enrollments")]
        [Display(Name = "Max Students")]
        public int MaxStudents { get; set; }


        [Display(Name = "Student/Manikin Ratio")]
        public int StudentManikinRatio { get; set; }


        [Required(ErrorMessage = "Please enter a price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.00")]
        public double Price { get; set; }


        [Display(Name = "Total Hours")]
        public string ClassDuration { get; set; }

        [Display(Name = "Options")]
        public bool IsAllowedRegistrationWithoutPayment { get; set; }


        [Display(Name = "Listing")]
        public bool IncludeOnline { get; set; }

        [NotMapped]
        [Display(Name = "Registration Links")]
        public string RegistrationLink { get; set; }


        [Display(Name = "Public Notes")]
        public string PublicNotes { get; set; }

        [Display(Name = "Class ID #")]
        public Nullable<int> ClassNo { get; set; }

        public string ExtraClassTimes { get; set; }

        [Display(Name = "Internal Notes")]
        public string InternalNotes { get; set; }

        [Column(TypeName = "date")]
        public DateTime ScheduleDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        [Display(Name = "Class Times")]
        [Required(ErrorMessage = "Please select a start date")]
        public DateTime[] ClassDate { get; set; }


        [Required(ErrorMessage = "Please select Time From")]
        [Display(Name = "From")]
        public string[] TimeFrom { get; set; }


        [Required(ErrorMessage = "Please select Time To")]
        [Display(Name = "To")]
        public string[] TimeTo { get; set; }

        [NotMapped]
        public string CourseName { get; set; }

        [NotMapped]
        public string Location { get; set; }

        [NotMapped]
        public string[] RepeatDates { get; set; }



    }
}