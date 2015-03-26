using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class UserBadge
    {
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Badge { get; set; }

        [Required]
        public string Link { get; set; }

        public UserBadge()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
