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

        private static readonly string ServerUploadFolder = "~/App/uploads/video/";

        // POST: api/User/UploadVideo
        //[HttpPost]
        //[DisableCors]
        //[Route("api/User/UploadVideo")]
        //public void uploadnow(HttpPostedFileBase file)
        //{
        //    var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
        //    await Request.Content.ReadAsMultipartAsync(streamProvider);

        //    return new FileResult
        //    {
        //        FileNames = streamProvider.FileData.Select(entry => entry.LocalFileName),
        //        Names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName),
        //        ContentTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType),
        //        Description = streamProvider.FormData["description"],
        //        CreatedTimestamp = DateTime.UtcNow,
        //        UpdatedTimestamp = DateTime.UtcNow,
        //        DownloadLink = "TODO, will implement when file is persisited"
        //    };
        //}




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
                    // var name = httpRequest.Form.Get(0);

                    // Save file to Responsitory
                    var filePath = HttpContext.Current.Server.MapPath("~/App/uploads/video/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    // Change User ProfilePiture in Database
                    //var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(name));
                    //user.ProfilePicture = "http://localhost:4444/App/uploads/video/" + name + ".3pg";
                    //identityDb.SaveChanges();

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