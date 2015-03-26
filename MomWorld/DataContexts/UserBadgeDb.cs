using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MomWorld.DataContexts
{
    public class UserBadgeDb : DbContext
    {
        public UserBadgeDb()
            : base("MomWorldConnection")
        {
        }

        DbSet<UserBadge> UserBadges { get; set; }
    }
}