using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MomWorld.Controllers
{
    public class AdvisoryController : Controller
    {
        // GET: Advisory
        public ActionResult Index()
        {
            return View();
        }

        // GET: Advisory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Advisory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Advisory/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Advisory/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Advisory/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Advisory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Advisory/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
