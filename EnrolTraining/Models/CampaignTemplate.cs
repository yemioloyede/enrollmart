using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EnrolTraining.Models
{
    public class CampaignTemplate
    {
        [Key]
        public int EmailID { get; set; }

        public int ProfileID { get; set; }

        [Required(ErrorMessage ="Please enter a number of days")]
        public int ScheduleDays { get; set; }
        public int ScheduleType { get; set; }

        [Required(ErrorMessage ="A subject is required")]
        [Display(Name ="Subject Line")]
        public string EmailSubject { get; set; }

        [AllowHtml]
        [Display(Name ="Email Body")]
        public string EmailBody { get; set; }


    }
}
