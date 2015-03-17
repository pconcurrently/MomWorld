using Microsoft.AspNet.Identity.EntityFramework;
using MomWorld.Entities;
using MomWorld.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.UI.WebControls;

namespace MomWorld.DataContexts
{
    public class ArticleDb : DbContext
    {
        public ArticleDb()
            : base("MomWorldConnection")
        {
        }

        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }

        public DbSet<Category> Categories { get; set; }
    }
    
}