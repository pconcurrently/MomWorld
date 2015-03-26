using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Subscriber
    {
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        public string Email { get; set; }

        public DateTime? DatePregnancy { get; set; }

        public Subscriber()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
