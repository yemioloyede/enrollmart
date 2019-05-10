using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class ScoresViewModel
    {
        public int[] StudentID { get; set; }
        public string[] FirstName { get; set; }
        public string [] LastName { get; set; }
        public Nullable<int>[] Status { get; set; }
        public string[]Score { get; set; }
        public int ClassID { get; set; }
        public string CourseName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}