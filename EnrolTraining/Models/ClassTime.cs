using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class ClassTime
    {
        [Key]
        public int TimeID { get; set; }
        public int ClassID { get; set; }
        public DateTime ClassDate { get; set; } 
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }

    }
}