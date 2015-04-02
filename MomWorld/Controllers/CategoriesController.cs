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
    public class CategoriesController : Controller
    {
        private CategoryDb db = new CategoryDb();
        private ArticleDb articleDb = new ArticleDb();
        private IdentityDb identityDb = new IdentityDb();

        // GET: Categories
        public ActionResult Index(string id)
        {
            List<Article> articles = articleDb.Articles.ToList();
            articles.RemoveAll(art => art.Status.Equals(ArticleStatus.Pending) || art.Status.Equals(ArticleStatus.Bad));
            ViewData["Categories"] = db.Categories.ToList();
            ViewData["Articles"] = articles;
            if (id != null)
            {
                Article popupArticle = articles.FirstOrDefault(a => a.Id.Equals(id));
                if ((popupArticle.Status == (int)MomWorld.Entities.ArticleStatus.Pending) && popupArticle.UserId == (identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name)).Id))
                {
                    ViewData["IsPopup"] = "true";
                }
                else
                {
                    ViewData["IsPopup"] = "false";
                }
            }

            return View();
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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

        public JsonResult GetArticles()
        {
            var categoriesDb = db.Categories.ToList();
            return Json(categoriesDb, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateCategory(CategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                Category cate = new Category();
                cate.Name = category.Name;
                cate.Description = category.Description;

                try
                {
                    db.Categories.Add(cate);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(null);
                }
                return Json("Successfully");
            }
            return Json(null);

        }
    }
}
