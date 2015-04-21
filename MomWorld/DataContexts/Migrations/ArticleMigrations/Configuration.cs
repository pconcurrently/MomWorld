namespace MomWorld.DataContexts.Migrations.ArticleMigrations
{
    using MomWorld.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MomWorld.DataContexts.ArticleDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataContexts\Migrations\ArticleMigrations";
        }

        protected override void Seed(MomWorld.DataContexts.ArticleDb context)
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

            var db = new ArticleDb();
            db.Categories.AddOrUpdate(
                new Category("Dinh dưỡng", "Dinh dưỡng", "MongCon"),
                new Category("Tiêm phòng", "Tiêm phòng", "MongCon"),
                new Category("Dinh dưỡng", "Dinh dưỡng", "MangThai"),
                new Category("Tiêm phòng", "Tiêm phòng", "MangThai"),
                new Category("Dinh dưỡng", "Dinh dưỡng", "TreSoSinh"),
                new Category("Tiêm phòng", "Tiêm phòng", "TreSoSinh"),
                new Category("Dinh dưỡng", "Dinh dưỡng", "NuoiDayTre"),
                new Category("Tiêm phòng", "Tiêm phòng", "NuoiDayTre")
                );
            db.Tags.AddOrUpdate(
                new Tag("Tuần 1", "Tuần 1"),
                new Tag("Tuần 2", "Tuần 2"),
                new Tag("Dinh dưỡng", "Dinh dưỡng"),
                new Tag("Tiêm phòng", "Tiêm phòng")
                );

            db.SaveChanges();
        }
    }
}
