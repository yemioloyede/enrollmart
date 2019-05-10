using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class ClassesViewModel
    {
        public int ClassID { get; set; }
        public Nullable<int> ClassNo { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string StartTime { get; set; }
        public string Course { get; set; }
        public string Instructor { get; set;}
        public int MaxStudents { get; set; }
        public int RegisteredStudents { get; set; }
        
    }
}