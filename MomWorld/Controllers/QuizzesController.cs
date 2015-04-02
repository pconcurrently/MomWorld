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

namespace MomWorld.Controllers
{
    [Authorize]
    public class QuizzesController : Controller
    {
        private QuizDb db = new QuizDb();
        private IdentityDb identityDb = new IdentityDb();

        // GET: Quizzes
        public ActionResult Index()
        {
            var quizzes = db.Quizzes.Include(q => q.QuizQuestion);
            return View(quizzes.ToList());
        }

        // GET: Quizzes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(quiz);
        }

        // GET: Quizzes/DoQuiz/5
        public ActionResult DoQuiz(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

            ViewData["QuizId"] = id;
            ViewData["UserName"] = CurrentUser.UserName;
            return View();

        }

        // GET: Quizzes/Create
        public ActionResult Create()
        {
            ViewBag.QuizQuestionId = new SelectList(db.QuizQuestions, "Id", "QuestionId");
            return View();
        }

        // POST: Quizzes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Intro,Description,CreatedDate,LastModifiedDate,QuizQuestionId")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Quizzes.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuizQuestionId = new SelectList(db.QuizQuestions, "Id", "QuestionId", quiz.QuizQuestionId);
            return View(quiz);
        }

        // GET: Quizzes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                throw new HttpException(404, "Not Found");
            }
            ViewBag.QuizQuestionId = new SelectList(db.QuizQuestions, "Id", "QuestionId", quiz.QuizQuestionId);
            return View(quiz);
        }

        // POST: Quizzes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Intro,Description,CreatedDate,LastModifiedDate,QuizQuestionId")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuizQuestionId = new SelectList(db.QuizQuestions, "Id", "QuestionId", quiz.QuizQuestionId);
            return View(quiz);
        }

        // GET: Quizzes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(quiz);
        }

        // POST: Quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            db.Quizzes.Remove(quiz);
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
    }
}
