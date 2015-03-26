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

        DbSet<Subscriber> Subcribers { get; set; }
    }
}