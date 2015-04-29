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
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            return View(db.NineMonthArticles.ToList());
           
        }

        // GET: NineMonthArticles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            //Suggest categories
            var userRoutines = identityDb.UserRoutines.ToList().FindAll(ur => ur.UserId.Equals(ViewBag.CurrentUser.Id));
            userRoutines = userRoutines.OrderByDescending(u => u.Count).ToList();
            var mostView = userRoutines.First() as UserRoutine;
            var categoriesList = articleDb.Categories.ToList().FindAll(c => c.Phase.Equals(mostView.Phase));
            ViewBag.CategoriesList = categoriesList;
            ViewBag.AllArticles = articleDb.Articles.ToList();
            ViewBag.CacThe = articleDb.Tags.ToList();

            
            NineMonthArticle nineMonthArticle = db.NineMonthArticles.Find(id);
            if (nineMonthArticle == null)
            {
                throw new HttpException(404, "Not Found");
            }
            if (nineMonthArticle.Tags == null)
            {
                return View(nineMonthArticle);
            }
            else
            {
                var articlesFromDb = articleDb.Articles.ToList();
                var articles = new List<Article>();
                string[] tagIdList = nineMonthArticle.Tags.Split(new string[] { ", " }, StringSplitOptions.None);
                bool removed = true;
                foreach (var item in articlesFromDb)
                {
                    foreach (var tag in tagIdList)
                    {
                        if (item.Tags != null && item.Tags.Contains(tag))
                        {
                            removed = false;
                        }
                    }
                    if (!removed)
                    {
                        articles.Add(item);
                        removed = true;
                    }
                }

                ViewBag.Articles = articles;

                Dictionary<string, string> tagsList = new Dictionary<string, string>();
                foreach (var item in tagIdList)
                {
                    tagsList.Add(item, articleDb.Tags.ToList().FirstOrDefault(t => t.Id.Equals(item)).Name);
                }
                ViewBag.TagsList = tagsList;
                ViewBag.TagIdList = tagIdList;

                return View(nineMonthArticle);
            }
        }

        // GET: NineMonthArticles/Create
        [Authorize(Roles = "Admins")]
        public ActionResult Create()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            ViewBag.TagsList = articleDb.Tags.ToList();
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
            article.Description = model.Description;
            article.DescriptionImage = model.DescriptionImage;

            if (model.Tags != null)
            {
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
            }

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
            ViewBag.CurrentUser= currentUser;
            return View();
        }

        public JsonResult GetContent(string id)
        {
            var article = db.NineMonthArticles.ToList().FirstOrDefault(n => n.Id.Equals(id)) as NineMonthArticle;
            return Json(article, JsonRequestBehavior.AllowGet);
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

            html = html.Substring(0, html.IndexOf("</p>"));
            var htmlSymbols = new string[] { "<br>", "<b>", "</b>", "<i>", "</i>", "<p>", "<p class=\"fr-tag\">", "</p>", "<hr>", "<strong>", "</strong>" };
            foreach (var item in htmlSymbols)
            {
                html = html.Replace(item, string.Empty);
            }
            return html;
        }

        public static string GetDescriptionImage(string html)
        {
            //step 1
            string imageLink = string.Empty;
            int start = html.IndexOf("<img");
            if (start == -1)
            {

                return string.Empty;
            }
            imageLink = html.Remove(0, start);
            int end = imageLink.IndexOf(">");
            imageLink = imageLink.Remove(end + 1);

            //step2
            start = imageLink.IndexOf("src=\"");
            imageLink = imageLink.Remove(0, start + 5);
            end = imageLink.IndexOf("\"");
            imageLink = imageLink.Remove(end);

            return imageLink;
        }
    }
}
