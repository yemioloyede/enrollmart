using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class PromocodeReportFilters
    {
        [Required(ErrorMessage = "Required")]
        public string DateCriteria { get; set; }


        [Required(ErrorMessage = "Required")]
        public DateTime DateForm { get; set; }


        [Required(ErrorMessage = "Required")]
        public DateTime DateTo { get; set; }

        [Required(ErrorMessage = "Required")]
        public int CodeID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string DateType { get; set; }

    }
}
