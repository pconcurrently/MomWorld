using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MomWorld.Models
{
    public class ArticleViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string CategoryId { get; set; }

        public string[] Tags { get; set; }
    }
}