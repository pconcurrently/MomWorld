﻿using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public object Get(string id)
        {
            // Get user by Uername
            var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));


            return user;
        }

        // TODO: Update user profile
        // PUT: api/User/{{UserName}}
        public void Put(string id, UpdateProfileViewModel userPro)
        {
            var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));
            user.FirstName = userPro.FirstName;
            user.LastName = userPro.LastName;
            user.PhoneNumber = userPro.PhoneNumber;
            user.DatePregnancy = userPro.DatePregnancy;
            //user.Facebook = userPro.Facebook;
            //user.Google = userPro.Google;

            // Add Social to Firebase
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "MQN9HDJakBgjQy2mxTDig01jgcVaHXRRILop7hPe",
                BasePath = "https://momworld.firebaseio.com/"
            };
            IFirebaseClient client = new FirebaseClient(config);

            client.Update("User/" + user.UserName, user);

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
                    user.ProfilePicture = "~/App/uploads/avatar/" + name + ".png";
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
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("video");
            container.SetPermissions(
            new BlobContainerPermissions
            {
                PublicAccess =
                    BlobContainerPublicAccessType.Blob
            });

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

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

                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(VideoID + ".mp4");

                        // Save to Blog
                        blockBlob.UploadFromStream(postedFile.InputStream);

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