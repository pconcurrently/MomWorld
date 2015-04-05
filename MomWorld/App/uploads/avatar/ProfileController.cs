using MomWorld.DataContexts;
using MomWorld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MomWorld.Controllers
{
    public class ProfileController : Controller
    {
        private IdentityDb identityDb = new IdentityDb();

        // GET: Profile
        public ActionResult Index(string id)
        {
            return View();
        }

        public ActionResult GetProfile(string id)
        {
            ApplicationUser GetUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));
            ViewData["ViewUsername"] = GetUser.UserName;

            return View();
        }
    }
}