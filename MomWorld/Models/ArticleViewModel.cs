using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string Phase { get; set; }

        public string Description { get; set; }

        public string DescriptionImage { get; set; }
    }

    public class EditArticleViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime? PostedDate { get; set; }

        public int? ViewNumber { get; set; }

        public string Description { get; set; }


        public int Status { get; set; }

        public string[] Tags { get; set; }

        public string DescriptionImage { get; set; }

        public string Phase { get; set; }
    }
}