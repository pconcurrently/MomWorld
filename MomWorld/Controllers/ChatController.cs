using MomWorld.DataContexts;
using MomWorld.Entities;
using MomWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MomWorld.Controllers
{
    public class ChatController : Controller
    {

        private IdentityDb identityDb = new IdentityDb();
        // GET: Chat
        public ActionResult Index(string username)
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            ViewData["ChatUsername"] = username;
            return View();
        }

        public ActionResult ChatWith(string id)
        {
            return View();
        }
    }
}