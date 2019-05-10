using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class InstructorActivityLog
    {
        [Key]
        public int ActivityID { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime ActivityDate { get; set; }

        [Required]
        [Display(Name = "Class/ Activity/ Event")]
        public string Activity { get; set; }

        [Required]
        public string Location { get; set; }


        public int InstructorID { get; set; }
    }
}