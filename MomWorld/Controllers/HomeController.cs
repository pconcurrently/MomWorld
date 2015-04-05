using MomWorld.DataContexts;
using MomWorld.Entities;
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
        

        public ActionResult Index()
        {            
            List<Article> articles = articleDb.Articles.OrderBy(art=>art.PostedDate).Take(5).ToList();
            ViewData["Top5Articles"] = articles;

            if (User.Identity.IsAuthenticated)
            {
                var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                ViewData["CurrentUser"] = user;
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
        public void uploadnow(HttpPostedFileWrapper upload)
        {
            if (upload != null)
            {
                string ImageName = upload.FileName;
                string path = System.IO.Path.Combine(Server.MapPath("~/Images/uploads"), ImageName);
                upload.SaveAs(path);
            }
        }
    }
}