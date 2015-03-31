﻿using System;
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            if (article.Status == (int)ArticleStatus.Approved || article.Status == (int)ArticleStatus.CreatedByAdmins
                || article.Status == (int)ArticleStatus.Normal)
            {
                article.ViewNumber += 1;
                db.SaveChanges();
                ApplicationUser postedUser = identityDb.Users.FirstOrDefault(x => x.Id.Equals(article.UserId));
                Category category = categoryDb.Categories.FirstOrDefault(c => c.Id.Equals(article.CategoryId));
                var comments = commentDb.Comments.ToList().FindAll(cmt => cmt.ArticleId.Equals(article.Id));
                comments.OrderBy(cmt => cmt.Date);

                ViewData["PostedUser"] = postedUser;
                ViewData["Article"] = article;
                ViewData["Category"] = category;
                ViewData["Comments"] = comments;
                return View();
            }
            else
            {
                //TO DO
                //Bài báo xấu
                return Redirect("Bài báo xấu");
            }
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,UserId,CategoryId,Title,Content,PostedDate,LastModifiedDate,ViewNumber,LastSeenUserId")] Article article)
        {
            var store = new UserStore<ApplicationUser>(new IdentityDb());
            var userManager = new UserManager<ApplicationUser>(store);
            var user = userManager.FindByNameAsync(User.Identity.Name).Result;

            article.UserId = user.Id;
            article.LastSeenUserId = user.Id;
            article.PostedDate = DateTime.Now;
            article.LastModifiedDate = DateTime.Now;
            article.ViewNumber = 0;
            article.Description = ParseHtml(article.Content);

            if (!user.Roles.FirstOrDefault().ToString().Equals("Admins"))
            {
                article.Status = (int)ArticleStatus.Pending;
            }
            else
            {
                article.Status = (int)ArticleStatus.CreatedByAdmins;
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Details", "Articles",routeValues: new { id = article.Id});
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            return View(article);
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
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,UserId,CategoryId,Title,Content,PostedDate,LastModifiedDate,ViewNumber,LastSeenUserId")] Article article)
        {
            article.LastModifiedDate = DateTime.Now;
            ApplicationUser currentUser = identityDb.Users.FirstOrDefault(x => x.UserName.Equals(User.Identity.Name));
            article.LastSeenUserId = currentUser.Id;
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", routeValues: new {id=article.Id });
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
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

        public static string ParseHtml(string html){

            html = html.Substring(0, html.IndexOf("</p>") - 1);
        //var htmlSymbols = new string[] {"<p>", "</p>", "<br>", "<b>","</b>"};
        //    foreach(var item in htmlSymbols)
        //    {
        //        html = html.Replace(item,string.Empty);
        //    }
            return html;
        }
        
    }
}
