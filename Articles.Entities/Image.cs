using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class Image
    {
        public Image()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public string Url { get; set; }
    }
}
