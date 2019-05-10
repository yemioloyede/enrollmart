using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class CalendarEvent
    {
        public int id { get; set; }
        public string CourseName { get; set; }
        public bool IsShowRemainingSeatsLeft { get; set; }
        public int MaxStudents { get; set; }
        public int enrolledStudents { get; set; }
        public DateTime ClassDate { get; set; }
        public string ClassTime { get; set; }
        public string ClassEndTime { get; set; }
        public string title
        {
            get
            {
                return "<b>" + this.ClassTime + "</b >" + (this.ClassDate < DateTime.Today || this.IsShowRemainingSeatsLeft == false ? "" : string.Format(" ({0} left)", this.MaxStudents - this.enrolledStudents)) + "<br/>" + this.CourseName;
            }
        }
        public bool allDay { get; set; }
        public string start { get; set; }
        public string url { get; set; }
        public string className { get; set; }
        public string color { get; set; }
        public string textColor { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public int courseID { get; set; }
    }
}