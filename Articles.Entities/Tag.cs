using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Tag
    {
        [Key]
        [Required]
        [MaxLength(128)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Tag()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
