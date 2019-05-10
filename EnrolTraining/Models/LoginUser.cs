using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class LoginUser
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string SubDomain { get; set; }
        public int UserID { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsTrainingSiteAdmin { get; set; }
        public bool IsInstructor { get; set; }
        public bool IsInstructorAssistant { get; set; }
        public bool IsWebManager { get; set; }
        public int ClassNumberSetting { get; set; }
    }
}