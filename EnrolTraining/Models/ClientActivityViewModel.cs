using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class ClientActivityViewModel
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CourseName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string ScheduleTime { get; set; }
        public double Price { get; set; }
        public double Due { get; set; }
        public int? Status { get; set; }
        public string Discipline { get; set; }
        public int RegisteredStudents { get; set; }
    }
}