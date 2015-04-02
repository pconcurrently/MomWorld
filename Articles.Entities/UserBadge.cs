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
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Status { get; set; }

    }
}
