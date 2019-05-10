using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class InstructorClasses
    {
        public int ClassID { get; set; }
        public DateTime ClassDate { get; set; }
        public int CourseID { get; set; }
        public string Course { get; set; }
        public int RegisteredStudents { get; set; }
        public string ClassVenue { get; set; }
        public int? InstructorID { get; set; }
        public int? AssistantID { get; set; }

    }
}