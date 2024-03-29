﻿using Microsoft.AspNet.Identity.EntityFramework;
using MomWorld.Entities;
using MomWorld.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MomWorld.DataContexts
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        public IdentityDb()
            : base("MomWorldConnection", throwIfV1Schema: false)
        {
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }

        public DbSet<UserRoutine> UserRoutines { get; set; }
    }
}