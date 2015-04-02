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

namespace MomWorld.Controllers
{
    public class AnwsersController : Controller
    {
        private QuizDb db = new QuizDb();

        // GET: Anwsers
        public ActionResult Index()
        {
            var anwsers = db.Anwsers.Include(a => a.Option);
            return View(anwsers.ToList());
        }

        // GET: Anwsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer anwser = db.Anwsers.Find(id);
            if (anwser == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(anwser);
        }

        // GET: Anwsers/Create
        public ActionResult Create()
        {
            ViewBag.OptionId = new SelectList(db.Options, "Id", "QuestionId");
            return View();
        }

        // POST: Anwsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OptionId")] Answer anwser)
        {
            if (ModelState.IsValid)
            {
                db.Anwsers.Add(anwser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OptionId = new SelectList(db.Options, "Id", "QuestionId", anwser.OptionId);
            return View(anwser);
        }

        // GET: Anwsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer anwser = db.Anwsers.Find(id);
            if (anwser == null)
            {
                throw new HttpException(404, "Not Found");
            }
            ViewBag.OptionId = new SelectList(db.Options, "Id", "QuestionId", anwser.OptionId);
            return View(anwser);
        }

        // POST: Anwsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OptionId")] Answer anwser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(anwser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OptionId = new SelectList(db.Options, "Id", "QuestionId", anwser.OptionId);
            return View(anwser);
        }

        // GET: Anwsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer anwser = db.Anwsers.Find(id);
            if (anwser == null)
            {
                throw new HttpException(404, "Not Found");
            }
            return View(anwser);
        }

        // POST: Anwsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Answer anwser = db.Anwsers.Find(id);
            db.Anwsers.Remove(anwser);
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
