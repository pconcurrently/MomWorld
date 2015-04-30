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
        [Authorize]
        public ActionResult Index(string id)
        {
            if (id != null)
            {
                ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                ApplicationUser GetUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));
                ViewData["ViewUsername"] = GetUser.UserName;
                return View();
            }
            else
            {
                return RedirectToAction("Manage","Account");
            }
            
        }

        // Get Profile of Selected User
        [AllowAnonymous]
        public ActionResult GetProfile(string id)
        {
            if (id != null)
            {
                ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
                ApplicationUser GetUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));
                // If User is Not exists
                if (GetUser == null)
                {
                    return RedirectToAction("Index","Home");
                } else {
                    ViewData["ViewUsername"] = GetUser.UserName;
                    return View();
                }
                
            }
            else
            {
                return RedirectToAction("Manage", "Account"); ;
            }
        }
    }
}