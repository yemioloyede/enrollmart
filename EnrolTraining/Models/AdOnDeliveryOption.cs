using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class AdOnDeliveryOption
    {
        [Key]
        public int ID { get; set; }
        public int CompanyID { get; set; }

        [Required]
        [Display(Name = "Pickup Text")]
        public string PickupText { get; set; }

        [Required]
        [RegularExpression("^(-1|[0-9]*)$", ErrorMessage = "Please enter valid number")]
        [Display(Name ="Shipping Days")]
        public int ShippingDays { get; set; }
    }
}