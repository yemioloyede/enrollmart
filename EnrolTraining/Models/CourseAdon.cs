using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnrolTraining.Models
{
    public class CourseAdon
    {
        [Key]
        public int ID { get; set; }
        public int AdonID { get; set; }
        public string AdOnName { get; set; }
        public double Price { get; set; }
        public int StudentID { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Type { get; set; }
        public int KeycodeID { get; set; }
        public string Keycode { get; set; }
        public string KeycodeInstructions { get; set; }
        
    }
}