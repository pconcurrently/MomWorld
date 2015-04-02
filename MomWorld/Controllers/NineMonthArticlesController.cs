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
            return View();
        }

        // POST: NineMonthArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles="Admins")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Week,Content")] NineMonthArticle nineMonthArticle)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                db.NineMonthArticles.Add(nineMonthArticle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nineMonthArticle);
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
    }
}
