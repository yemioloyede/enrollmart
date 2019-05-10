using EnrolTraining.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EnrolTraining
{
    public class Context : DbContext
    {
        public Context(): base(System.Configuration.ConfigurationManager.AppSettings["ConnectionDB"])
        {

        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //Database.SetInitializer<Context>(null);
        //    //base.OnModelCreating(modelBuilder);
        //}
        public DbSet<RegistrationAdOn> RegistrationAdOn { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<KeycodeBank> KeycodeBank { get; set; }
        public DbSet<CourseType> CourseType { get; set; }
        public DbSet<PromoCode> PromoCode { get; set; }
        public DbSet<CardSetting> CardSetting { get; set; }
        public DbSet<EmailCampaign> CampaignProfile { get; set; }
        public DbSet<CampaignTemplate> CampaignTemplate { get; set; }
        public DbSet<Discipline> Discipline { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<EnrollSetting> EnrolSetting { get; set; }
        public DbSet<EnrollQuestion> EnrolQuestion { get; set; }
        public DbSet<Keycode> Keycode { get; set; }
        public DbSet<AdOnDeliveryOption> AdOnDeliveryOption { get; set; }
        public DbSet<ScheduleClass> ScheduleClass { get; set; }
        public DbSet<ClassTime> ClassTime { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Tax> Tax { get; set; }
        public DbSet<StudentPayment> StudentPayment { get; set; }
        public DbSet<InstructorCertification> InstructorCertification { get; set; }
        public DbSet<InstructorActivityLog> InstructorActivityLog { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<CourseAdon> CourseAdon { get; set; }
    }
}