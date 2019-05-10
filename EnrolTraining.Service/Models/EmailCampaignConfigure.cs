using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Service.Models
{
    public class EmailCampaignConfigure
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


        [Display(Name ="Email Body")]
        public string EmailBody { get; set; }


    }
}
