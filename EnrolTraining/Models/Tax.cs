using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class Tax
    {
        public int TaxID { get; set; }
        public int CompanyID { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 99.9, ErrorMessage = "Please enter a valid percent value")]
        public double Percentage { get; set; }
    }
}