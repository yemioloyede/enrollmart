namespace EnrolTraining.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EnrolTraining.Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EnrolTraining.Context context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Discipline.AddOrUpdate(x => x.DisciplineID,
                new Discipline() { DisciplineID = 1, CompanyID = 0, DisciplineName = "Other" },
                new Discipline() { DisciplineID = 2, CompanyID = 0, DisciplineName = "ACLS" },
                new Discipline() { DisciplineID = 3, CompanyID = 0, DisciplineName = "BLS" },
                new Discipline() { DisciplineID = 4, CompanyID = 0, DisciplineName = "PALS" },
                new Discipline() { DisciplineID = 5, CompanyID = 0, DisciplineName = "HeartSaver" },
                new Discipline() { DisciplineID = 6, CompanyID = 0, DisciplineName = "EMS" },
                new Discipline() { DisciplineID = 7, CompanyID = 0, DisciplineName = "NRP" },
                new Discipline() { DisciplineID = 8, CompanyID = 0, DisciplineName = "PEARS" }
            );

        }
    }
}
