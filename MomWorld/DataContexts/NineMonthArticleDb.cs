using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MomWorld.DataContexts
{
    public class NineMonthArticleDb : DbContext
    {
        public NineMonthArticleDb()
            : base("MomWorldConnection")
        {
        }

        public DbSet<NineMonthArticle> NineMonthArticles { get; set; }
    }
}