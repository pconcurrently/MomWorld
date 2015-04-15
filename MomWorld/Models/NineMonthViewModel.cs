using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace MomWorld.Models
{
    public class NineMonthViewModel
    {
        public int Date { get; set; }

        public string Content { get; set; }

        public string[] Tags { get; set; }

        public string Description { get; set; }

        public string DescriptionImage { get; set; }
    }
}