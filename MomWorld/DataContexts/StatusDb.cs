using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MomWorld.DataContexts
{
    public class StatusDb : DbContext
    {
        public StatusDb()
            : base("MomWorldConnection")
        {
        }
        DbSet<Status> Statuses { get; set; }
    }
}