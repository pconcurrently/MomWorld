using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MomWorld.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Index(string username)
        {
            ViewData["ChatUsername"] = username;
            return View();
        }

        public ActionResult ChatWith(string id)
        {
            return View();
        }
    }
}