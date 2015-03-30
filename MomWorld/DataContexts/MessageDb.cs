using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MomWorld.DataContexts
{
    public class MessageDb : DbContext
    {
        public MessageDb()
            : base("MomWorldConnection")
        {
        }

        public DbSet<Message> Messages { get; set; }
    }
}