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
        private CommentDb commentDb = new CommentDb();


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
            List<Article> articles = articleDb.Articles.OrderByDescending(art => art.ViewNumber).Take(5).ToList();
            ViewData["Top5Articles"] = articles;

            List<Article> MongCon = articleDb.Articles.ToList().FindAll(a => a.Phase.Equals("MongCon")).OrderByDescending(art => art.PostedDate).Take(5).ToList();
            List<Article> MangThai = articleDb.Articles.ToList().FindAll(a => a.Phase.Equals("MangThai")).OrderByDescending(art => art.PostedDate).Take(5).ToList();
            List<Article> TreSoSinh = articleDb.Articles.ToList().FindAll(a => a.Phase.Equals("TreSoSinh")).OrderByDescending(art => art.PostedDate).Take(5).ToList();
            List<Article> NuoiDayTre = articleDb.Articles.ToList().FindAll(a => a.Phase.Equals("NuoiDayTre")).OrderByDescending(art => art.PostedDate).Take(5).ToList();

            ViewData["MongCon"] = MongCon;
            ViewData["MangThai"] = MangThai;
            ViewData["TreSoSinh"] = TreSoSinh;
            ViewData["NuoiDayTre"] = NuoiDayTre;
            ViewData["PostedUsers"] = identityDb.Users.ToList();

            Dictionary<string, int> likesNumber = new Dictionary<string, int>();
            Dictionary<string, int?> viewsNumber = new Dictionary<string, int?>();
            Dictionary<string, int> commentsNumber = new Dictionary<string, int>();
            List<Article> all = new List<Article>();
            all.AddRange(MongCon);
            all.AddRange(MangThai);
            all.AddRange(TreSoSinh);
            all.AddRange(NuoiDayTre);

            foreach(var a in all)
            {
                likesNumber.Add(a.Id, articleDb.ArticleLikes.ToList().FindAll(art => art.ArticleId.Equals(a.Id)).Count);
            }

            foreach (var a in all)
            {
                commentsNumber.Add(a.Id, commentDb.Comments.ToList().FindAll(c => c.ArticleId.Equals(a.Id)).Count);
            }

            foreach (var a in all)
            {
                viewsNumber.Add(a.Id, a.ViewNumber);
            }

            ViewData["LikesNumber"] = likesNumber;
            ViewData["ViewsNumber"] = viewsNumber;
            ViewData["CommentsNumber"] = commentsNumber;

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

        public ActionResult UnderConstruction()
        {
            return View();
        }

        public ActionResult RulesViolation()
        {
            return View();
        }
    }
}