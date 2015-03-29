using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    class Diary
    {
        public Diary()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public string Content { get; set; }
        public string time { get; set; }
    }
}
