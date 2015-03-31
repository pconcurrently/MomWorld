using MomWorld.DataContexts;
using MomWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
            Console.Write(value);
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

        // POST: api/User/UploadAvatar
        [HttpPost]
        public HttpResponseMessage UploadAvatar()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {                    
                    var postedFile = httpRequest.Files[file];
                    var name = httpRequest.Form.Get(0);

                    // Save file to Responsitory
                    var filePath = HttpContext.Current.Server.MapPath("~/App/uploads/avatar/" + name + ".png");
                    postedFile.SaveAs(filePath);

                    // Change User ProfilePiture in Database
                    var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(name));
                    user.ProfilePicture = "http://localhost:4444/App/uploads/avatar/" + name + ".png";
                    identityDb.SaveChanges();

                }
                result = Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return result;
            
        }
    }
}
