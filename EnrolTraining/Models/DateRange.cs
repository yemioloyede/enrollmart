using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class DateRange
    {
        [Required]
        public DateTime DateForm { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

    }
}