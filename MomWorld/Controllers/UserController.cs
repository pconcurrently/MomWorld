using MomWorld.DataContexts;
using MomWorld.Models;
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


            return user;
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {

        }

        // PUT: api/User/{{UserName}}
        public void Put(string id, UpdateProfileViewModel userPro)
        {
            var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));
            user.FirstName = userPro.FirstName;
            user.LastName = userPro.LastName;
            user.PhoneNumber = userPro.PhoneNumber;
            

            identityDb.SaveChanges();
            
            
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
