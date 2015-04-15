using MomWorld.DataContexts;
using MomWorld.Entities;
using MomWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MomWorld.Controllers
{
    public class HomeController : Controller
    {

        private ArticleDb articleDb = new ArticleDb();
        private IdentityDb identityDb = new IdentityDb();
        private ImageDb imageDb = new ImageDb();


        public ActionResult Index()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            var users = identityDb.Users.ToList();
            Dictionary<string, int> listUsers = new Dictionary<string, int>();
            int artsCount = 0;
            int likesCount = 0;
            var top5Users = new List<ApplicationUser>();
            var listUserLikeArticle = new List<TopUsersModel>();
            foreach (var u in users)
            {
                var arts = articleDb.Articles.ToList().FindAll(a => a.UserId.Equals(u.Id));
                artsCount = arts.Count;
                foreach (var art in arts)
                {
                    likesCount += articleDb.ArticleLikes.ToList().FindAll(a => a.ArticleId.Equals(art.Id)).Count;
                }
                listUsers.Add(u.Id, artsCount * 10 + likesCount);
                listUserLikeArticle.Add(new TopUsersModel(u.Id, artsCount, likesCount));
                artsCount = 0;
                likesCount = 0;
            }
            var values = listUsers.Values.ToList().OrderByDescending(i=>i).ToList();

            foreach (var value in values)
            {
                var user = users.FirstOrDefault(u => u.Id.Equals(listUsers.FirstOrDefault(l => l.Value == value).Key));
                if (top5Users.Count <= 4)
                {
                    top5Users.Add(user);
                    users.Remove(user);
                    listUsers.Remove(user.Id);
                }
            }
            ViewBag.Top5Users = top5Users;
            ViewBag.ListUserLikeArticle = listUserLikeArticle;
            List<Article> articles = articleDb.Articles.OrderBy(art => art.PostedDate).Take(5).ToList();
            ViewData["Top5Articles"] = articles;

            if (User.Identity.IsAuthenticated)
            {
                var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                ViewBag.CurrentUser = user;
                ViewBag.ProfilePicture = user.ProfilePicture;
            }
            else
            {
                //Fix sau
                ViewBag.CurrentUser = new ApplicationUser();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult UploadImage()
        {
            if (HttpContext.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Request.Files["UploadedImage"];
                if (httpPostedFile != null)
                {
                    Image img = new Image();
                    string imageName = httpPostedFile.FileName;
                    string path = System.IO.Path.Combine(Server.MapPath("~/Images/uploads"), img.Id) + imageName.Substring(imageName.LastIndexOf('.'));

                    httpPostedFile.SaveAs(path);

                    string imgUrl = "~/Images/uploads/" + img.Id + imageName.Substring(imageName.LastIndexOf('.'));
                    img.Url = imgUrl;
                    imageDb.Entry(img).State = System.Data.Entity.EntityState.Added;
                    imageDb.SaveChanges();

                    return Json(imgUrl);
                }
            }
            return Json(null);
        }
    }
}