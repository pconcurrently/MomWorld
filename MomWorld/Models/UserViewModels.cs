using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MomWorld.Models
{
    public class UserViewModels
    {
    }

    public class UpdateProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DatePregnancy { get; set; } 
    }

    public class CreateStatusViewModel
    {
        public string CreatorName { get; set; }
        public string CreatorUsername { get; set; }
        public string CreatorAvatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
        public int like { get; set; }
    }


}