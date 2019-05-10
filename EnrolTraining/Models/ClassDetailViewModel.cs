using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class ClassDetailViewModel
    {
        public int ClassID { get; set; }
        public string Location { get; set; }
        public string CourseImage { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public double ClassPrice { get; set; }
        public string CourseAddOns { get; set; }
        public string ExtraClasses { get; set; }

        [Display(Name = "Options")]

        public string[] SelectedAddOns { get; set; }

        public int iid { get; set; }

    }
}