using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Service.Models
{
    public class RegistrationAdOn
    {
        [Key]
        public int AdOnID { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        
        [Display(Name = "Description/ Notes")]
        public string Description { get; set; }

        
        [Display(Name = "Display Order")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid number")]
        public int DisplayOrder { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.00")]
        public double Price { get; set; }


        
        [Display(Name = "Type")]
        public string Type { get; set; }

        
        [Display(Name = "Options")]
        public bool IsDefaultForAllRegistration { get; set; }
    }
}
