using MomWorld.DataContexts;
using MomWorld.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MomWorld.Controllers
{
    public class UserTasksController : Controller
    {
        private SubscriberDb db = new SubscriberDb();


        // POST: UserTasks/Create
        [HttpPost]
        public JsonResult Create(UserTask model)
        {
            try
            {
                model.UserName = User.Identity.Name;
                db.Entry(model).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                return Json("Successfull");
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        // POST: UserTasks/Delete/5
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                var task = db.UserTasks.FirstOrDefault(t => t.Id.Equals(id));
                if (task != null)
                {
                    db.UserTasks.Remove(task);
                    db.Entry(task).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    return Json("Successfull");
                }
                return Json(null);
            }
            catch (Exception)
            {
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult Complete(string id)
        {
            try
            {
                var task = db.UserTasks.FirstOrDefault(t => t.Id.Equals(id));
                if (task != null)
                {
                    task.IsCompleted = true;
                    db.Entry(task).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json("Successfull");
                }
                return Json(null);
            }
            catch (Exception)
            {
                return Json(null);
            }
        }
    }
}
