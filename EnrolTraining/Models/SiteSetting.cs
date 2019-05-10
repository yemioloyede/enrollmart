using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class SiteSetting
    {
        [NotMapped]
        public Company Company { get; set; }
        [NotMapped]
        public EnrollSetting EnrolSetting { get; set; }
        [NotMapped]
        public Discipline Discipline { get; set; }
        [NotMapped]
        public IEnumerable<EnrollQuestion> EnrolQuestions { get; set; }
        [NotMapped]
        public IEnumerable<Discipline> Disciplines { get; set; }
    }
}