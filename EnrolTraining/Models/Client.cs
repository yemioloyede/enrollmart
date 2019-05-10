using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnrolTraining.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public int CompanyID { get; set; }

        [Required]
        [Display(Name = "Company")]
        public string ClientCompany { get; set; }

        public string Abbreviation { get; set; }

        [Display(Name = "Contact First Name")]
        public string ContactFirstName { get; set; }

        [Display(Name = "Contact Last Name")]
        public string ContactLastName { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Display(Name = "Contact Date")]
        public Nullable<DateTime> ContactDate { get; set; }

        [Url]
        public string WebSite { get; set; }

        [Display(Name = "Main Phone")]
        public string MainPhone { get; set; }

        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        [Display(Name = "Shared Notes")]
        [AllowHtml]
        public string SharedNotes { get; set; }

        [Display(Name = "Internal Notes")]
        [AllowHtml]
        public string InternalNotes { get; set; }
    }
}