using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Category
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<Article> Articles { get; set; }

        public string Phase { get; set; }

        public Category()
        {
            Id = Guid.NewGuid().ToString();
        }

        public Category(string name, string description, string phase)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            Phase = phase;
        }

    }
}
