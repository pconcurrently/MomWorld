using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Message
    {
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        public string SenderUserName { get; set; }

        public string ReceiverUserName { get; set; }

        public string Content { get; set; }

        public bool IsRead { get; set; }

        public DateTime SentDate { get; set; }
    }
}
