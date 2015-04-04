using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MomWorld.Models
{
    public class ReportViewModel
    {
        public string ArticleId { get; set; }

        public string Content { get; set; }
    }

    public class ReportResultsViewModel
    {
        public string Content { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }

        public ReportResultsViewModel(string content, string username, string userid)
        {
            this.Content = content;
            this.UserName = username;
            this.UserId = userid;
        }
    }
}