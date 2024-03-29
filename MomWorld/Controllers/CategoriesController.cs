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
using Microsoft.AspNet.Identity;

namespace MomWorld.Controllers
{
    public class CategoriesController : Controller
    {
        private CategoryDb db = new CategoryDb();
        private ArticleDb articleDb = new ArticleDb();
        private IdentityDb identityDb = new IdentityDb();
        private CommentDb commentDb = new CommentDb();

        // GET: Categories
        public ActionResult Index(string id)
        {
            if (id == null || (!id.Equals("MongCon") && !id.Equals("MangThai") && !id.Equals("TreSoSinh") && !id.Equals("NuoiDayTre")))
            {
                throw new Exception("Bad request");
            }

            List<Article> articles = articleDb.Articles.ToList();
            articles.RemoveAll(art => art.Status == (int)ArticleStatus.Pending || art.Status == (int)ArticleStatus.Bad);
            ViewData["Categories"] = db.Categories.ToList().FindAll(c => c.Phase.Equals(id));
            var categoryArticles = articles.FindAll(a => a.Phase.Equals(id));
            ViewData["Articles"] = categoryArticles;

            ViewBag.Phase = id;
            var currentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            ViewBag.CurrentUser = currentUser;
            ViewBag.TagsList = articleDb.Tags.ToList();

            //Get UserRoutine

            ViewBag.AllArticle = articleDb.Articles.ToList();
            if (User.Identity.IsAuthenticated)
            {
                string userId = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name)).Id;
                var isLike = articleDb.ArticleLikes.ToList().FirstOrDefault(al => al.ArticleId.Equals(id) && al.UserId.Equals(userId));

                var userRoutine = identityDb.UserRoutines.FirstOrDefault(ur => ur.UserId.Equals(currentUser.Id) && ur.Phase.Equals(id));
                if (userRoutine != null)
                {
                    userRoutine.Count += 1;
                    identityDb.Entry(userRoutine).State = EntityState.Modified;
                    identityDb.SaveChanges();
                }
                else
                {
                    var userRoutine2 = new UserRoutine();
                    userRoutine2.Phase = id;
                    userRoutine2.Count = 1;
                    userRoutine2.UserId = currentUser.Id;
                    identityDb.Entry(userRoutine2).State = EntityState.Added;
                    identityDb.SaveChanges();
                }
                //Suggest categories
                var userRoutines = identityDb.UserRoutines.ToList().FindAll(ur => ur.UserId.Equals(currentUser.Id));
                userRoutines = userRoutines.OrderByDescending(u => u.Count).ToList();
                var mostView = userRoutines.First() as UserRoutine;
                var categoriesList = db.Categories.ToList().FindAll(c => c.Phase.Equals(mostView.Phase));
                ViewBag.CategoriesList = categoriesList;
            }
            else
            {
                var categoriesList = db.Categories.ToList().FindAll(c => c.Phase.Equals("MangThai"));
                ViewBag.CategoriesList = categoriesList;
            }

            return View();
        }

        // GET: Categories/Details/5
        public ActionResult Details(string id)
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

            var articles = articleDb.Articles.ToList().FindAll(a => a.CategoryId.Equals(id));
            ViewBag.Articles = articles;
            ViewBag.TagsList = articleDb.Tags.ToList();
            ViewBag.Comments = commentDb.Comments.ToList();

            var currentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            ViewBag.CurrentUser = currentUser;

            //Get UserRoutine

            ViewBag.AllArticle = articleDb.Articles.ToList();
            if (User.Identity.IsAuthenticated)
            {
                string userId = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name)).Id;
                var isLike = articleDb.ArticleLikes.ToList().FirstOrDefault(al => al.ArticleId.Equals(id) && al.UserId.Equals(userId));

                var userRoutine = identityDb.UserRoutines.FirstOrDefault(ur => ur.UserId.Equals(currentUser.Id) && ur.Phase.Equals(category.Phase));
                if (userRoutine != null)
                {
                    userRoutine.Count += 1;
                    identityDb.Entry(userRoutine).State = EntityState.Modified;
                    identityDb.SaveChanges();
                }
                else
                {
                    var userRoutine2 = new UserRoutine();
                    userRoutine2.Phase = category.Phase;
                    userRoutine2.Count = 1;
                    userRoutine2.UserId = currentUser.Id;
                    identityDb.Entry(userRoutine2).State = EntityState.Added;
                    identityDb.SaveChanges();
                }
                //Suggest categories
                var userRoutines = identityDb.UserRoutines.ToList().FindAll(ur => ur.UserId.Equals(currentUser.Id));
                userRoutines = userRoutines.OrderByDescending(u => u.Count).ToList();
                var mostView = userRoutines.First() as UserRoutine;
                var categoriesList = db.Categories.ToList().FindAll(c => c.Phase.Equals(mostView.Phase));
                ViewBag.CategoriesList = categoriesList;
            }
            else
            {
                var categoriesList = db.Categories.ToList().FindAll(c => c.Phase.Equals("MangThai"));
                ViewBag.CategoriesList = categoriesList;
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

        // POST: Categories/Delete/5
        public JsonResult Delete(string id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
                cate.Phase = category.Phase;

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

        public JsonResult Get(string id)
        {
            return Json(db.Categories.FirstOrDefault(c => c.Id.Equals(id)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update([Bind(Include = "Id,Name,Description,Phase")]Category model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Successfully");
            }
            return Json(null);
        }
    }
}
