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
                var adminUser = new ApplicationUser { Email = "admin@momworld.com", UserName = "admin", ProfilePicture = "~/App/uploads/avatar/default.png", Status = 2 };
                UserManager.Create(adminUser, "12345678");
                UserManager.AddToRole(adminUser.Id, "Admins");
            }

            // Create normal user
            if (UserManager.FindByName("phoht") == null)
            {
                var user1 = new ApplicationUser { Email = "phohtse90083@fpt.edu.vn", UserName = "phoht", DatePregnancy = DateTime.Now, ProfilePicture = "~/App/uploads/avatar/default.png", Status = 2 };
                UserManager.Create(user1, "12345678");
                UserManager.AddToRole(user1.Id, "Users");
            }

            // Create normal user
            if (UserManager.FindByName("khoapc") == null)
            {
                var user2 = new ApplicationUser { Email = "khoapcse90038@fpt.edu.vn", UserName = "khoapc", DatePregnancy = DateTime.Now, ProfilePicture = "~/App/uploads/avatar/default.png", Status = 2 };
                UserManager.Create(user2, "12345678");
                UserManager.AddToRole(user2.Id, "Users");
            }

            // Create normal user
            if (UserManager.FindByName("khoahn") == null)
            {
                var user3 = new ApplicationUser { Email = "khoahnse56@fpt.edu.vn", UserName = "khoahn", DatePregnancy = DateTime.Now, ProfilePicture = "~/App/uploads/avatar/default.png", Status = 2 };
                UserManager.Create(user3, "12345678");
                UserManager.AddToRole(user3.Id, "Users");
            }

            // Create normal user
            if (UserManager.FindByName("datnt") == null)
            {
                var user4 = new ApplicationUser { Email = "datntse90012@fpt.edu.vn", UserName = "datnt", DatePregnancy = DateTime.Now, ProfilePicture = "~/App/uploads/avatar/default.png", Status = 2 };
                UserManager.Create(user4, "12345678");
                UserManager.AddToRole(user4.Id, "Users");
            }

            // Create normal user
            if (UserManager.FindByName("trungnm") == null)
            {
                var user5 = new ApplicationUser { Email = "trungnmse90088@fpt.edu.vn", UserName = "trungnm", DatePregnancy = DateTime.Now, ProfilePicture = "~/App/uploads/avatar/default.png", Status = 2 };
                UserManager.Create(user5, "12345678");
                UserManager.AddToRole(user5.Id, "Users");
            }
        }
    }
}
