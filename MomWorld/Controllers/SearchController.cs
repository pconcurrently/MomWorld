using MomWorld.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MomWorld.Controllers
{
    public class SearchController : Controller
    {
        private ArticleDb db = new ArticleDb();
        private IdentityDb identityDb = new IdentityDb();
        
        // GET: Search
        public ActionResult Index()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            return View();
        }

        public JsonResult GetResults(string id)
        {
            if (id == null || id == string.Empty)
            {
                return Json(string.Empty);
            }

            var arts = db.Articles.ToList().FindAll(a => a.Content.ToLower().Contains(id.ToLower())||a.Title.ToLower().Contains(id.ToLower()));
            
            return Json(arts, JsonRequestBehavior.AllowGet);
        }
    }
}