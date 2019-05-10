using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class CertificatePrintViewModel
    {
        [Required(ErrorMessage = "Upload Certificate Template in Setting>>Certificates")]
        public string Template { get; set; }
        public int[] StudentID { get; set; }
        public bool[] SelectedStudent { get; set; }
        public string[] FirstName { get; set; }
        public string[] LastName { get; set; }

        public int ClassID { get; set; }
        public string CourseName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string Instructor { get; set; }
        public string InstructorID { get; set; }
        public string IssueDate { get; set; }
        public string Expires { get; set; }
        public string ClassHours { get; set; }
        public string CEUCredits { get; set; } 

    }
}