using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EnrolTraining.Common
{
    public class SessionWrapper
    {
        // private constructor
        private SessionWrapper()
        {
            CompanyID = 0;
            UserID = 0;

        }

        // Gets the current session.
        public static SessionWrapper Current
        {
            get
            {
                SessionWrapper session =
                  (SessionWrapper)HttpContext.Current.Session["__MySession__"];
                if (session == null)
                {
                    session = new SessionWrapper();
                    HttpContext.Current.Session["__MySession__"] = session;
                }
                return session;
            }
        }

        // **** add your session properties here, e.g like this:
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string SubDomain { get; set; }
        public int UserID { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsTrainingSiteAdmin { get; set; }
        public bool IsInstructor { get; set; }
        public bool IsInstructorAssistant { get; set; }
        public int ClassNumberSetting { get; set; }

    }
}