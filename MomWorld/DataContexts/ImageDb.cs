using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MomWorld.DataContexts
{
    public class ImageDb : DbContext
    {
        public ImageDb()
            : base("MomWorldConnection")
        {
        }

        public DbSet<Image> Images { get; set; }
    }
}