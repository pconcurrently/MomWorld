using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomWorld.Entities
{
    public class UserRoutine
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Phase { get; set; }

        public int Count { get; set; }

      
        public UserRoutine()
        {
            Id = Guid.NewGuid().ToString();
            Count = 0;
        }
    }
}
