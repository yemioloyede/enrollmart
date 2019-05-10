using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class StudentPayment
    {
        [Key]
        public int PaymentID { get; set; }

        public int StudentID { get; set; }
        public DateTime PaymentDate { get; set; }

        [Required]
        public string type { get; set; }
        public string Detail { get; set; }
        public string TransactionID { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public double Amount { get; set; }
    }
}