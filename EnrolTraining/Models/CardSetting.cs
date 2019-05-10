using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Models
{
    public class CardSetting
    {
        [Key]
        public int ProfileID { get; set; }

        public int CompanyID { get; set; }

        [Required]
        [Display(Name ="ProfileName")]
        public string ProfileName { get; set; }

        [Display(Name ="Card type")]
        public int CardType { get; set; }


        [Display(Name = "Training Ctr")]
        public string TrainingCenter { get; set; }

        [Display(Name = "TC Info")]
        public string TCInfo1 { get; set; }
        public string TCInfo2 { get; set; }

        [Display(Name = "Course Location")]
        public string CourseLocation1 { get; set; }
        public string CourseLocation2 { get; set; }

        [Display(Name = "Card Allignment Adjustments")]
        public Nullable<int> Card1 { get; set; }

        public Nullable<int> Card2 { get; set; }

        public Nullable<int> Card3 { get; set; }

        [Display(Name ="Options")]
        public bool IsDefault{ get; set; }

    }
}
