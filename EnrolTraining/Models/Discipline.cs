using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class Discipline
    {
        [Key]
        public int DisciplineID { get; set; }
        public int CompanyID { get; set; }
        [Required]
        public string DisciplineName { get; set; }
    }
}