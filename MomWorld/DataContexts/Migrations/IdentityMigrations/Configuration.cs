namespace MomWorld.DataContexts.Migrations.IdentityMigrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MomWorld.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MomWorld.DataContexts.IdentityDb>
    {
        // TrungNM
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataContexts\Migrations\IdentityMigrations";
        }

        protected override void Seed(MomWorld.DataContexts.IdentityDb context)
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

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Create Admin Role
            string roleName = "Admins";
            IdentityResult roleResult;

            // Check to see if Role Exists, if not create it
            if (!RoleManager.RoleExists(roleName))
            {
                roleResult = RoleManager.Create(new IdentityRole(roleName));
            }

            // Create Users Role
            roleName = "Users";

            // Check to see if Role Exists, if not create it
            if (!RoleManager.RoleExists(roleName))
            {
                roleResult = RoleManager.Create(new IdentityRole(roleName));
            }

            // Create user Admin
            if (UserManager.FindByName("admin") == null)
            {
                var adminUser = new ApplicationUser { Email = "admin@momworld.com", UserName = "admin", ProfilePicture = "~/App/uploads/avatar/default.png" };
                UserManager.Create(adminUser, "12345678");
                UserManager.AddToRole(adminUser.Id, "Admins");
            }

            // Create normal user
            if (UserManager.FindByName("user1") == null)
            {
                var user1 = new ApplicationUser { Email = "user1@momworld.com", UserName = "user1", DatePregnancy = DateTime.Now, ProfilePicture = "~/App/uploads/avatar/default.png" };
                UserManager.Create(user1, "12345678");
                UserManager.AddToRole(user1.Id, "Users");
            }
        }
    }
}
