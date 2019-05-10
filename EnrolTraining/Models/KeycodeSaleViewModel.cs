using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class KeycodeSaleViewModel
    {
        public int ClassID { get; set; }
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<DateTime> Purchased { get; set; }
        public string Phone { get; set; }
        public string Course { get; set; }
        public DateTime ClassDate { get; set; }
        public string ClassTime { get; set; }
        public string Keycode { get; set; }
        public bool IsUnscheduled { get; set; }
    }
}