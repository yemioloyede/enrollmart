using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class InstructorCertification
    {
        [Key]
        public int CertificationID { get; set; }

        [Required]
        [Display(Name ="Discipline")]
        public int DisciplineID { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [Display(Name = "Initial")]
        public DateTime Initial { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Expires { get; set; }

        public bool IsTCF { get; set; }
        public bool isRF { get; set; }
        public int InstructorID { get; set; }

    }
}