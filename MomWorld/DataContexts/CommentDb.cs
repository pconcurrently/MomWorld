﻿using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MomWorld.DataContexts
{
    public class CommentDb : DbContext
    {
        public CommentDb()
            : base("MomWorldConnection")
        {
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Article> Articles { get; set; }
    }
}