using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class ReportFilters
    {
        [Required(ErrorMessage = "Required")]
        public string DateCriteria { get; set; }


        [Required(ErrorMessage = "Required")]
        public DateTime DateForm { get; set; }


        [Required(ErrorMessage = "Required")]
        public DateTime DateTo { get; set; }

        [Required(ErrorMessage = "Required")]
        public int ClinetID { get; set; }
    }
}