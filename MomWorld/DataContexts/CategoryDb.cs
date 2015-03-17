using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MomWorld.Entities;
using System.Data.Entity;

namespace MomWorld.DataContexts
{
    public class CategoryDb : DbContext
    {
        public CategoryDb()
            : base("MomWorldConnection")
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}