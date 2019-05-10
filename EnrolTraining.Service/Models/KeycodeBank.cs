using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Service.Models
{
    public class KeycodeBank
    {
        public int KeycodeBankID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [DisplayFormat ()]
        public string Name { get; set; }

        
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }
        
        [Display(Name = "Add New Keys\n(one per line)")]
        public string Keys { get; set; }
    }
}
