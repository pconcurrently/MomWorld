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
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetResults(string id)
        {
            if (id == null || id == string.Empty)
            {
                return Json(string.Empty);
            }

            var arts = db.Articles.ToList().FindAll(a => a.Content.Contains(id)||a.Title.Contains(id));
            
            return Json(arts, JsonRequestBehavior.AllowGet);
        }
    }
}