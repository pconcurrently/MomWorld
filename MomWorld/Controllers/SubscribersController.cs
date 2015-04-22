using MomWorld.DataContexts;
using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MomWorld.Controllers
{
    public class SubscribersController : Controller
    {
        private SubscriberDb db = new SubscriberDb();

        // GET: Subscribers
        public ActionResult Index()
        {
            return View();
        }

        // GET: Subscribers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public JsonResult Create(Subscriber model)
        {
            var subscriber1 = db.Subscribers.FirstOrDefault(s => s.Email.Equals(model.Email));
            var subscriber2 = db.Subscribers.FirstOrDefault(s => s.PhoneNumber.Equals(model.PhoneNumber));
            if (subscriber1 == null && subscriber2 == null)
            {

                if (ModelState.IsValid)
                {
                    db.Entry(model).State = EntityState.Added;
                    db.SaveChanges();

                    SMSServices.Send(model.PhoneNumber, "Mom's World: Cam on ban da dang ky dich vu tin nhan cua chung toi!");
                    if (model.Email != null)
                    {
                        System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
             new System.Net.Mail.MailAddress("momworld.notreply@gmail.com", "Mom's World"),
             new System.Net.Mail.MailAddress(model.Email));
                        m.Subject = "Mom's World Subscription";
                        m.Body = "Thank you for your subscription!";
                        m.IsBodyHtml = true;
                        MailServices.Send(m);
                    }
                    return Json("Successfully");
                }
            }
            else if (subscriber1 != null && subscriber2 != null)
            {
                return Json("Duplicated");
            }
            else if (subscriber1 != null)
            {
                subscriber1.PhoneNumber = model.PhoneNumber;
                subscriber1.DatePregnancy = model.DatePregnancy;
                db.Entry(subscriber1).State = EntityState.Modified;
                db.SaveChanges();
                SMSServices.Send(model.PhoneNumber, "Mom's World: Cam on ban da dang ky dich vu tin nhan cua chung toi!");
                return Json("PhoneUpdated");
            }
            else
            {
                subscriber2.Email = model.Email;
                subscriber2.DatePregnancy = model.DatePregnancy;
                db.Entry(subscriber2).State = EntityState.Modified;
                db.SaveChanges();

                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
            new System.Net.Mail.MailAddress("momworld.notreply@gmail.com", "Mom's World"),
            new System.Net.Mail.MailAddress(model.Email));
                m.Subject = "Mom's World Subscription";
                m.Body = "Thank you for your subscription!";
                m.IsBodyHtml = true;
                MailServices.Send(m);

                return Json("EmailUpdated");
            }
            return Json(null);
        }

        // GET: Subscribers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Subscribers/Edit/5
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

        // GET: Subscribers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Subscribers/Delete/5
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
