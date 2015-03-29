using MomWorld.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;

namespace MomWorld.Controllers
{
    public class UserController : ApiController
    {
        private IdentityDb identityDb = new IdentityDb();

        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/user1
        // Retrive User from Database using UserName 
        public object Get(string id)
        {
            // Get user by Uername
            var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));
            //user = new
            //{
            //    userName = id,
            //    firstName = "Trung",
            //    lastName = "Nguyen Minh",
            //    email = "trungnm0512@gmail.com",
            //    phone = "+(84)1655757445",
            //    description = "Tôi yêu màu tím và tôi là anh hùng màu hồng",
            //    avatar = "http://meobeoi.com/wp-content/uploads/2014/06/hoanhtrang.jpg",
            //    social = new
            //    {
            //        facebook = "facebook.com/conmeomaudendichandat",
            //        google = "plus.google.com/trungnm0512"
            //    },
            //    // Retrive Badge from BadgeUser Database using UserName 
            //    badges = new List<object> { 
            //        new { name = "Dinh Duong", image = "http://localhost:4444/Content/images/badge/dinhduong.png" },
            //        new { name = "Y te", image = "http://localhost:4444/Content/images/badge/yte.png" }
            //    }
            //};

            return user;
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/{{UserName}}
        public void Put(string id, object userPro)
        {
            
            var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));
            user.FirstName = "Trung";
            user.LastName = "Minh";
            
            
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
