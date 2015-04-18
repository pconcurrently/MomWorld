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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace MomWorld.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private ArticleDb db = new ArticleDb();
        private IdentityDb identityDb = new IdentityDb();
        private CategoryDb categoryDb = new CategoryDb();
        private CommentDb commentDb = new CommentDb();

        // GET: Articles
        public ActionResult Index()
        {
            var comments = commentDb.Comments.ToList();
            var articles = db.Articles.ToList();

            ViewData["Comments"] = comments;
            return View(articles);
        }

        // GET: Articles/Details/5
        [AllowAnonymous]
        public ActionResult Details(string id)
        {
            int likesNumber = 0;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                throw new HttpException(404, "Not Found");
            }
            if ((article.Status == (int)ArticleStatus.Approved || article.Status == (int)ArticleStatus.CreatedByAdmins
                || article.Status == (int)ArticleStatus.Normal || article.Status == (int)ArticleStatus.Reported) || User.IsInRole("Admins"))
            {
                article.ViewNumber += 1;
                db.SaveChanges();
                string userId = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name)).Id;

                ApplicationUser postedUser = identityDb.Users.FirstOrDefault(x => x.Id.Equals(article.UserId));
                Category category = categoryDb.Categories.FirstOrDefault(c => c.Id.Equals(article.CategoryId));
                var comments = commentDb.Comments.ToList().FindAll(cmt => cmt.ArticleId.Equals(article.Id));
                comments.OrderBy(cmt => cmt.Date);
                var articleLikes = db.ArticleLikes.ToList().FindAll(al => al.ArticleId.Equals(article.Id));

                var isLike = db.ArticleLikes.ToList().FirstOrDefault(al => al.ArticleId.Equals(id) && al.UserId.Equals(userId));
                if (article.Tags != null)
                {
                    var tags = db.Tags.ToList().FindAll(t => article.Tags.Contains(t.Id));
                    ViewData["Tags"] = tags;
                }

                ViewData["PostedUser"] = postedUser;

                ViewData["UserArticles"] = db.Articles.ToList().FindAll(a => a.UserId.Equals(postedUser.Id)).Count;
                foreach (var item in db.Articles.ToList().FindAll(a => a.UserId.Equals(postedUser.Id)))
                {
                    likesNumber += db.ArticleLikes.ToList().FindAll(a => a.ArticleId.Equals(item.Id)).Count;
                }
                Dictionary<string, string> profilePictures = new Dictionary<string, string>();
                foreach(var item in comments)
                {
                    profilePictures.Add(item.UserName, identityDb.Users.FirstOrDefault(u => u.UserName.Equals(item.UserName)).ProfilePicture);
                }

                ViewData["UserLikes"] = likesNumber;
                ViewData["Article"] = article;
                ViewData["Category"] = category;
                ViewData["Comments"] = comments;
                ViewData["ArticleLikes"] = articleLikes;
                ViewData["IsLike"] = isLike;
                ViewData["TagsList"] = db.Tags.ToList();
                ViewData["ProfilePictures"] = profilePictures;

                ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

                return View();
            }
            else
            {
                //TO DO
                //Bài báo xấu
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: Articles/Create
        public ActionResult Create(string id)
        {
            if (id == null || (!id.Equals("MongCon") && !id.Equals("MangThai") && !id.Equals("TreSoSinh") && !id.Equals("NuoiDayTre")))
            {
                throw new Exception("Bad request");
            }
            else
            {
                ViewBag.Phase = id;
                ViewBag.TagsList = db.Tags.ToList();
                ViewData["Tags"] = GetTags(null);
                ViewBag.CategoryId = new SelectList(db.Categories.ToList().FindAll(c=>c.Phase.Equals(id)), "Id", "Name");
                ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                return View();

            }
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ArticleViewModel model, string id)
        {
            Article article = new Article();
            var store = new UserStore<ApplicationUser>(new IdentityDb());
            var userManager = new UserManager<ApplicationUser>(store);
            var user = userManager.FindByNameAsync(User.Identity.Name).Result;

            article.UserId = user.Id;
            article.LastSeenUserId = user.Id;
            article.PostedDate = DateTime.Now;
            article.LastModifiedDate = DateTime.Now;
            article.LastModifiedUserId = user.Id;
            article.ViewNumber = 0;
            article.Title = model.Title;
            article.Content = model.Content;
            article.Description = model.Description;
            article.DescriptionImage = model.DescriptionImage;
            article.CategoryId = model.CategoryId;
            article.Phase = id;

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
            else
            {
                article.Tags = string.Empty;
            }
            if (!User.IsInRole("Admins"))
            {
                article.Status = (int)ArticleStatus.Pending;
            }
            else
            {
                article.Status = (int)ArticleStatus.CreatedByAdmins;
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            try
            {
                article.Description = model.Description;
                db.Entry(article).State = EntityState.Added;
                db.SaveChanges();
                if (article.Status == (int)ArticleStatus.CreatedByAdmins)
                {
                    return RedirectToAction("Details", "Articles", routeValues: new { id = article.Id });
                }
                return RedirectToAction("Index", "Categories", routeValues: new { id = article.Phase });
            }
            catch (Exception)
            {
                ViewData["Tags"] = GetTags(null);
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
                return View(article);
            }
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                throw new HttpException(404, "Not Found");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            if (article.Tags != null)
            {
                ViewBag.Tags2 = GetTags(article.Tags.Split(new string[] { ", " }, StringSplitOptions.None));
                ViewBag.SelectedValues = article.Tags.Split(new string[] { ", " }, StringSplitOptions.None);
            }
            else
            {
                ViewBag.Tags2 = GetTags(null);
            }

            
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,UserId,CategoryId,Title,Content,PostedDate,LastModifiedDate,ViewNumber,LastSeenUserId,Status,Description,Tags2")] Article article)
        {
            //var article = db.Articles.FirstOrDefault(a => a.Id.Equals(model.Id));
            //article.CategoryId = model.CategoryId;
            //article.Title = model.Title;
            //article.Content = model.Content;

            if (article.Tags2 != null)
            {

                if (article.Tags2.Length < 1)
                {
                    article.Tags = string.Empty;
                }
                else if (article.Tags2.Length > 1)
                {
                    article.Tags = article.Tags2[0];
                    for (var index = 1; index <= article.Tags2.Length - 1; index++)
                    {
                        article.Tags += ", " + article.Tags2[index];
                    }
                }
                else
                    article.Tags = article.Tags2[0];
            }
            else
            {
                article.Tags = string.Empty;
            }
            article.LastModifiedDate = DateTime.Now;
            ApplicationUser currentUser = identityDb.Users.FirstOrDefault(x => x.UserName.Equals(User.Identity.Name));
            article.LastSeenUserId = currentUser.Id;
            article.LastModifiedUserId = currentUser.Id;
            article.DescriptionImage = GetDescriptionImage(article.Content);
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", routeValues: new { id = article.Id });
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        public JsonResult Delete(string articleId)
        {
            Article article = db.Articles.Find(articleId);
            db.Articles.Remove(article);
            db.Entry(article).State = EntityState.Deleted;
            db.SaveChanges();
            return Json("Successfully");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetArticles(string id)
        {
            var articlesDb = db.Articles.ToList();
            List<Article> returnArticles = articlesDb;
            foreach (var art in articlesDb.ToList())
            {
                if (!art.CategoryId.Equals(id))
                {
                    returnArticles.Remove(art);
                }
            }
            return Json(returnArticles, JsonRequestBehavior.AllowGet);
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

        public JsonResult Report(ReportViewModel model)
        {
            var report = new Report();
            report.UserId = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name)).Id;
            report.ArticleId = model.ArticleId;
            report.Content = model.Content;
            report.Date = DateTime.Now;
            try
            {
                db.Reports.Add(report);
                var article = db.Articles.FirstOrDefault(art => art.Id.Equals(report.ArticleId));
                if (article.Status != (int)ArticleStatus.Reported)
                {
                    article.Status = (int)ArticleStatus.Reported;
                    db.Entry(article).State = EntityState.Modified;
                }
                db.Entry(report).State = EntityState.Added;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(null);

            }
            return Json("Successfully");
        }

        public JsonResult Like(string articleId)
        {
            string userId = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name)).Id;
            ArticleLike check = db.ArticleLikes.ToList().FirstOrDefault(al => al.ArticleId.Equals(articleId) && al.UserId.Equals(userId));
            if (check == null)
            {
                try
                {
                    ArticleLike artlike = new ArticleLike();
                    artlike.UserId = userId;
                    artlike.ArticleId = articleId;
                    artlike.Date = DateTime.Now;
                    db.ArticleLikes.Add(artlike);
                    db.Entry(artlike).State = EntityState.Added;
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(null);
                }
                return Json("Successfully");
            }
            else
            {
                return Json(null);
            }
        }

        public JsonResult Approve(string articleId)
        {
            var art = db.Articles.FirstOrDefault(a => a.Id.Equals(articleId));

            if (art != null)
            {
                if (art.Status != (int)ArticleStatus.Approved)
                {
                    art.Status = (int)ArticleStatus.Approved;
                    db.Entry(art).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("Successfully");
                }
            }
            return Json(null);
        }

        public JsonResult Lock(string articleId)
        {
            var art = db.Articles.FirstOrDefault(a => a.Id.Equals(articleId));

            if (art != null)
            {
                art.Status = (int)ArticleStatus.Bad;
                db.Entry(art).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Successfully");

            }
            return Json(null);

        }

        private MultiSelectList GetTags(string[] selectedValues)
        {

            List<Tag> Tags = db.Tags.ToList();

            return new MultiSelectList(Tags, "Id", "Name", selectedValues);

        }

        //Get
        public JsonResult GetReports(string id)
        {
            if (id == null || id == string.Empty)
            {
                return Json(string.Empty);
            }
            List<ReportResultsViewModel> results = new List<ReportResultsViewModel>();
            var reports = db.Reports.ToList().FindAll(r => r.ArticleId.Equals(id));
            foreach (var report in reports)
            {
                results.Add(new ReportResultsViewModel(report.Content, identityDb.Users.FirstOrDefault(u => u.Id.Equals(report.UserId)).UserName, report.UserId));
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }

}
