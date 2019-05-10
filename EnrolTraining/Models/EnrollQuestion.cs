using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class EnrollQuestion
    {
        [Key]
        public int QuestionID { get; set; }
        public int CompanyID { get; set; }

        [Required]
        public string Question { get; set; }

        public int Type { get; set; }

        [Display(Name = "Answer Choices")]
        public string Answers { get; set; }

        [Display (Name ="Options")]
        public bool IsRequiredToAnswer { get; set; }
        public bool OnlyDisplayForSpeceficCourse { get; set; }
    }
}