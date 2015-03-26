using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MomWorld.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/user1
        // Retrive User from Database using UserName 
        public object Get(string id)
        {
            var user = new
            {
                userName = id,
                firstName = "Trung",
                lastName = "Nguyen Minh",
                email = "trungnm0512@gmail.com",
                phone = "+(84)1655757445",
                description = "Tôi yêu màu tím và tôi là anh hùng màu hồng",
                avatar = "http://meobeoi.com/wp-content/uploads/2014/06/hoanhtrang.jpg",
                social = new
                {
                    facebook = "facebook.com/conmeomaudendichandat",
                    google = "plus.google.com/trungnm0512"
                },
                // Retrive Badge from BadgeUser Database using UserName 
                badges = new List<object> { 
                    new { name = "Dinh Duong", image = "http://localhost:4444/Content/images/badge/dinhduong.png" },
                    new { name = "Y te", image = "http://localhost:4444/Content/images/badge/yte.png" }
                }
            };

            return user;
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/5
        public void Put(string id, [FromBody]string value)
        {
            /* Update User profile */
            Console.WriteLine("I am here !");
            Console.WriteLine(value);
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
