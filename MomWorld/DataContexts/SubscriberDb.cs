using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MomWorld.DataContexts
{
    public class SubscriberDb : DbContext
    {
        public SubscriberDb()
            : base("MomWorldConnection")
        {
        }

        public DbSet<Subscriber> Subscribers { get; set; }

        public DbSet<UserTask> UserTasks { get; set; }

    }
}