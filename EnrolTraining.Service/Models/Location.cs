using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Service.Models
{
    public class Location
    {
        public int LocationID { get; set; }

        [Required]
        [Display(Name ="Name")]
        public string Name { get; set; }


        [Display(Name = "Abbreviation")]
        public string Abbreviation { get; set; }


        [Display(Name = "Directions")]
        public string Directions { get; set; }

        [Display(Name = "Options")]
        public bool IsDefaultSelectionForClasses { get; set; }


        [Display(Name = "Print On Card")]
        public string LocationPrint1 { get; set; }

        public string LocationPrint2 { get; set; }
    }
}
