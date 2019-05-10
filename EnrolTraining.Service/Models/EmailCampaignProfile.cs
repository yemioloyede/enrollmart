using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrolTraining.Service.Models
{
    public class EmailCampaignProfile
    {
        [Key]
        public int ProfileID { get; set; }

        [Required(ErrorMessage ="Please enter the name of your campaign")]
        public string Name { get; set; }

        [Required]
        [Display (Name ="Mail From Address")]
        public string MailFrom { get; set; }

        [Required]
        [Display(Name = "Mail From Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "BCC Emails To")]
        public string BCC { get; set; }

        [Display(Name = "Options")]
        public bool IsActive { get; set; }
        public bool StopIfRegister { get; set; }

        [Display(Name = "Send to all Course Types")]
        public string SendToAllCourseTypes { get; set; }
    }
}
