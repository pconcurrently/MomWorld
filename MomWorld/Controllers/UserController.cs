using MomWorld.DataContexts;
using MomWorld.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Cors;

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


        [HttpPost]
        [Route("api/User/UploadVideo")]
        public HttpResponseMessage UploadVideo()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {

                    var postedFile = httpRequest.Files[0];
                    var Username = httpRequest.Form.Get(0);
                    var VideoName = httpRequest.Form.Get(1);
                    var VideoID = httpRequest.Form.Get(2);

                    if (postedFile.ContentType == "image/png")
                    {
                        // Save Thumbnail to Responsitory
                        var filePath = HttpContext.Current.Server.MapPath("~/App/uploads/video/thumbnail/" + VideoID + ".png");
                        postedFile.SaveAs(filePath);
                    }
                    else
                    {
                        // Save Video to Responsitory
                        var filePath = HttpContext.Current.Server.MapPath("~/App/uploads/video/" + VideoID + ".mp4");
                        postedFile.SaveAs(filePath);
                    }
                    
                    

                    

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