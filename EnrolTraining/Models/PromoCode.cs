using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Models
{
    public class PromoCode
    {
        [Key]
        public int CodeID { get; set; }

        public int CompanyID { get; set; }
        public int ClientID { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a start date")]
        [Display(Name ="Start Date" )]
        public DateTime StartDate { get; set; }

        
        [Required(ErrorMessage = "Please select a end date")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string Type { get; set; }

        //(^100(\.0{1,2})?$)|(^([1-9]([0-9])?|0)(\.[0-9]{1,2})?$) for 0.0 to 99.99
        //^(?:\$(?!.*%))?(?:\d*\.)?\d+%?$ for discount in percent and value (drawback accept more than 100%)
        [Required(ErrorMessage = "Please enter a discount amount")]
        [RegularExpression(@"(^100(\.0*)?\%$)|(^([1-9]([0-9])?|0)(\.[0-9]*)?\%$)|(^\$(?:\$(?!.*%))?(?:\d*\.)?\d+$)", ErrorMessage = "Valid Discount Format 40%, 40.XXXX%, $215, $90.XXXX")]
        public string Discount { get; set; }

        [Required(ErrorMessage = "Please enter the number of uses remaining")]
        [Display(Name = "# of uses")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid Number")]
        public int NumOfUses { get; set; }

        [Display(Name = "Options")]
        public bool IsDiscountForShippingAndAdon { get; set; }
        public bool IsRestrictUseByCourseType { get; set; }
        public bool IsDeferredPaymentsOnly { get; set; }

        [NotMapped]
        [Display(Name = "Courses Allowed")]
        public string[] CoursesAllowedArray { get; set; }

        public double DiscountValue { get; set; }
        public string CoursesAllowed { get; set; }
    }
}
