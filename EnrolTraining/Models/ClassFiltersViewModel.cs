using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class ClassFiltersViewModel
    {
        public int CourseID { get; set; }
        public int LocationID { get; set; }
        public int InstructorID { get; set; }
        public string DateCriteria { get; set; }

        [Required(ErrorMessage ="Required")]
        public DateTime DateForm { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime DateTo { get; set; }
        
    }
}