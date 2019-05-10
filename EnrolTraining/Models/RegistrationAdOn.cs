using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Models
{
    public class RegistrationAdOn
    {
        [Key]
        public int AdOnID { get; set; }
        public int CompanyID { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        
        [Display(Name = "Description/ Notes")]
        public string Description { get; set; }

        
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.00")]
        public double Price { get; set; }


        
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name ="Keycode Bank")]
        public Nullable<int> KeycodeID { get; set; }

        [Display(Name = "Tax")]
        public Nullable<int> TaxID { get; set; }

        [Display(Name = "Options")]
        public bool IsDefaultForAllRegistration { get; set; }
        public bool IsTaxable { get; set; }

    }
}
