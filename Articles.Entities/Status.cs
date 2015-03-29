using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Status
    {
        [Key]
        public string Id { get; set; }

        public string CreatorName { get; set; }

        public string CreatorAvatar { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Content { get; set; }

        public int like { get; set; }

        public Status()
        {
            Id = Guid.NewGuid().ToString();
        }


    }

    public class StatusComment
    {
        [Key]
        public string Id { get; set; }

        public string CreatorName { get; set; }

        public string CreatorAvatar { get; set; }

        public virtual Status Status { get; set; }

        public string StatusId { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Content { get; set; }
        
        public StatusComment()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
