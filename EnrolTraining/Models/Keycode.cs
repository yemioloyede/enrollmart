using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class Keycode
    {
        [Key]
        public int KeycodeID { get; set; }
        public int KeyBankID { get; set; }
        public string Key { get; set; }
        public int AddonID { get; set; }
        public string Registrant { get; set; }
        public string Email { get; set; }
        public DateTime DateAdded { get; set; }
        public Nullable<DateTime> DateAssigned { get; set; }
        
    }
}