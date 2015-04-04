using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Report
    {
        [Key]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string ArticleId { get; set; }

        public DateTime Date { get; set; }

        public string Content { get; set; }

        public Report()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
