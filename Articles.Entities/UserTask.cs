using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class UserTask
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        public string UserName { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public UserTask()
        {
            Id = Guid.NewGuid().ToString();
            IsCompleted = false;
            CreatedDate = DateTime.Now;
        }
    }
}
