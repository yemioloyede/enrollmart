using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Models
{
    public class KeycodeBank
    {
        public int KeycodeBankID { get; set; }

        public int CompanyID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [DisplayFormat ()]
        public string Name { get; set; }

        
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }

        [NotMapped]
        [Display(Name = "Add New Keys\n(one per line)")]
        public string Keys { get; set; }
    }
}
