using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MomWorld.DataContexts;
using MomWorld.Entities;
using MomWorld.Models;

namespace MomWorld.Controllers
{
    [Authorize]
    public class NineMonthArticlesController : Controller
    {
        private NineMonthArticleDb db = new NineMonthArticleDb();
        private ArticleDb articleDb = new ArticleDb();
        private IdentityDb identityDb = new IdentityDb();

        // GET: NineMonthArticles
        public ActionResult Index()
        {
            return View(db.NineMonthArticles.ToList());
        }

        // GET: NineMonthArticles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NineMonthArticle nineMonthArticle = db.NineMonthArticles.Find(id);
            if (nineMonthArticle == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(nineMonthArticle);
        }

        // GET: NineMonthArticles/Create
        [Authorize(Roles = "Admins")]
        public ActionResult Create()
        {
            ViewData["Tags"] = GetTags(null);
            return View();
        }

        // POST: NineMonthArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(NineMonthViewModel model)
        {
            NineMonthArticle article = new NineMonthArticle();
            article.Date = model.Date;
            article.Content = model.Content;
            article.Description = ParseHtml(model.Content);
            if (model.Tags.Length < 1)
            {
                article.Tags = string.Empty;
            }
            else if (model.Tags.Length > 1)
            {
                article.Tags = model.Tags[0];
                for (var index = 1; index <= model.Tags.Length - 1; index++)
                {
                    article.Tags += ", " + model.Tags[index];
                }
            }
            else
                article.Tags = model.Tags[0];

            try
            {
                db.NineMonthArticles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Timeline", "NineMonthArticles");
            }
            catch (Exception)
            {
                return View(model);
            }

        }

        // GET: NineMonthArticles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NineMonthArticle nineMonthArticle = db.NineMonthArticles.Find(id);
            if (nineMonthArticle == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(nineMonthArticle);
        }

        // POST: NineMonthArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Week,Content")] NineMonthArticle nineMonthArticle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nineMonthArticle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nineMonthArticle);
        }

        // GET: NineMonthArticles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NineMonthArticle nineMonthArticle = db.NineMonthArticles.Find(id);
            if (nineMonthArticle == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(nineMonthArticle);
        }

        // POST: NineMonthArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NineMonthArticle nineMonthArticle = db.NineMonthArticles.Find(id);
            db.NineMonthArticles.Remove(nineMonthArticle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Timeline()
        {
            ViewData["NineMonthArticles"] = db.NineMonthArticles.OrderBy(o => o.Date).ToList();
            ApplicationUser currentUser = identityDb.Users.FirstOrDefault(x => x.UserName.Equals(User.Identity.Name));
            ViewData["CurrentUser"] = currentUser;
            return View();
        }

        public JsonResult SetDate(DateTime date)
        {
            try
            {
                var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                user.DatePregnancy = date;
                identityDb.Entry(user).State = EntityState.Modified;
                identityDb.SaveChanges();
                return Json("Successfully");
            }
            catch (Exception)
            {
                return Json(null);
            }
        }
        private MultiSelectList GetTags(string[] selectedValues)
        {

            List<Tag> Tags = articleDb.Tags.ToList();

            return new MultiSelectList(Tags, "Id", "Name", selectedValues);

        }

        public static string ParseHtml(string html)
        {

            html = html.Substring(0, html.IndexOf("</p>") - 1);
            var htmlSymbols = new string[] { "<br>", "<b>", "</b>", "<i>", "</i>", "<p>", "<p class=\"fr-tag\">", "</p>", "<hr>" };
            foreach (var item in htmlSymbols)
            {
                html = html.Replace(item, string.Empty);
            }
            return html;
        }
    }
}
