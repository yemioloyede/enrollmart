using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class StudentSearchViewModel
    {
        public string Filter { get; set; }
        public string Search { get; set; }
        public string LastName { get; set; }
    }
}