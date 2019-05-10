using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Models
{
    public class Location
    {
        [Display(Name ="Location ID")]
        public int LocationID { get; set; }

        public int CompanyID { get; set; }

        [Required]
        [Display(Name ="Name")]
        public string Name { get; set; }


        [Display(Name = "Abbreviation")]
        public string Abbreviation { get; set; }


        [Display(Name = "Directions")]
        public string Directions { get; set; }

        [Display(Name = "Options")]
        public bool IsDefaultSelectionForClasses { get; set; }
        public bool IsNotAvailableCallToSchedule { get; set; }
        public bool IsArchive { get; set; }

        [Display(Name = "Print On Card")]
        public string LocationPrint1 { get; set; }

        public string LocationPrint2 { get; set; }
    }
}
