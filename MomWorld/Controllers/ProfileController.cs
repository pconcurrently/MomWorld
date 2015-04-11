﻿using MomWorld.DataContexts;
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
                ApplicationUser GetUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));
                ViewData["ViewUsername"] = GetUser.UserName;
                return View();
            }
            else
            {
                return Redirect("http://localhost:4444/Account/Manage");
            }
            
        }

        [Authorize]
        public ActionResult GetProfile(string id)
        {
            if (id != null)
            {
                ApplicationUser GetUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(id));
                ViewData["ViewUsername"] = GetUser.UserName;
                return View();
            }
            else
            {
                return Redirect("http://localhost:4444/Account/Manage");
            }
        }
    }
}