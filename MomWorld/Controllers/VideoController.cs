using MomWorld.DataContexts;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MomWorld.Controllers
{
    public class VideoController : Controller
    {
        private IdentityDb identityDb = new IdentityDb();
        // GET: Video
        public ActionResult Index()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            return View();
        }

        
    }
}