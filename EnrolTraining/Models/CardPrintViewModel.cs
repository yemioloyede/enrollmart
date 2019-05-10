using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class CardPrintViewModel
    {
        [Required(ErrorMessage = "Please add profile in Settings>>Card Settings")]
        public int ProfileID { get; set; }
        public int CardType { get; set; }

        public int[] StudentID { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Please select al least one student to proceed")]

        public bool[] SelectedStudent { get; set; }
        public string[] FirstName { get; set; }
        public string[] LastName { get; set; }
        public string[] Address1 { get; set; }
        public string[] Address2 { get; set; }
        public string[] City { get; set; }
        public string[] State { get; set; }
        public string[] Zip { get; set; }

        public string CourseName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string Instructor { get; set; }
        public string IssueDate { get; set; }
        public string Expires { get; set; }


        public int ClassID { get; set; }


        [Display(Name = "Training Ctr")]
        public string TrainingCenter { get; set; }

        [Display(Name = "TC Info")]
        public string TCInfo1 { get; set; }
        public string TCInfo2 { get; set; }

        [Display(Name = "Course Location")]
        public string CourseLocation1 { get; set; }
        public string CourseLocation2 { get; set; }


        public int CardOnPage { get; set; }
    }
}