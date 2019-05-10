using EnrolTraining.Service.Migrations;
using EnrolTraining.Service.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EnrolTraining.Service
{
    public class Context: DbContext
    {
        public Context() : base(System.Configuration.ConfigurationManager.AppSettings["ConnectionDB"])
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            //base.OnModelCreating(modelBuilder);
        }
        public DbSet<Models.RegistrationAdOn> RegistrationAdOn { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<KeycodeBank> KeycodeBank { get; set; }
        public DbSet<CourseType> CourseType { get; set; }
        public DbSet<PromoCode> PromoCode { get; set; }
        public DbSet<CardSetting> CardSetting { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<EmailCampaignProfile> CampaignProfile { get; set; }
        public DbSet<EmailCampaignConfigure> CampaignConfigure { get; set; }
    }
}