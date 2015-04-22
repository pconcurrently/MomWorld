using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using MomWorld.Models;
using MomWorld.Entities;
using System.Web.Helpers;
using MomWorld.DataContexts;
using System.Reflection;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp;
using FireSharp.Response;
using System.Data.Entity;
using System.IO.Ports;

namespace MomWorld.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ArticleDb articleDb = new ArticleDb();
        private IdentityDb identityDb = new IdentityDb();
        private SubscriberDb subscriberDb = new SubscriberDb();

        #region SMS
        SerialPort port = new SerialPort();
        SMS objSMS = new SMS();
        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        #endregion

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                var adminRole = identityDb.Roles.FirstOrDefault(ro => ro.Name.Equals("Admins")).Id;
                if (user != null)
                {
                    if (((user.Status == (int)IdentityStatus.Locked) || !user.EmailConfirmed) && !user.Roles.ToList()[0].RoleId.Equals(adminRole))
                    {
                        return RedirectToAction("LockedUser");
                    }
                    else
                    {
                        await SignInAsync(user, model.RememberMe);

                        // Sync User to Firebase
                        IFirebaseConfig config = new FirebaseConfig
                        {
                            AuthSecret = "MQN9HDJakBgjQy2mxTDig01jgcVaHXRRILop7hPe",
                            BasePath = "https://momworld.firebaseio.com/"
                        };
                        IFirebaseClient client = new FirebaseClient(config);

                        client.Update("User/" + user.UserName.ToLower(), user);


                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, Status = (int)IdentityStatus.Normal, ProfilePicture = "~/App/uploads/avatar/default.png" };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                UserManager.AddToRole(user.Id, "Users");
                if (result.Succeeded)
                {
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                        new System.Net.Mail.MailAddress("momworld.notreply@gmail.com", "Mom's World"),
                        new System.Net.Mail.MailAddress(user.Email));
                    m.Subject = "Mom's World Email confirmation";
                    m.Body = "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>";
                    m.IsBodyHtml = true;
                    MailServices.Send(m);
                    return View("DisplayEmail");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại hoặc chưa được xác nhận email");
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
     new System.Net.Mail.MailAddress("momworld.notreply@gmail.com", "Mom's World"),
     new System.Net.Mail.MailAddress(user.Email));
                m.Subject = "Mom's World Reset password";
                m.Body = "Please reset your password at this: <a href=\"" + callbackUrl + "\">link</a>";
                m.IsBodyHtml = true;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential("momworld.notreply@gmail.com", "Abcd1234@");
                smtp.EnableSsl = true;
                smtp.Send(m);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Không tồn tại tải khoản này");
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        public ActionResult GetDetail(string id)
        {
            return Json(UserManager.FindByName(id), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admins")]
        public ActionResult UsersManage()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            var users = UserManager.Users.ToList() as List<ApplicationUser>;

            Dictionary<string, string> userRoles = new Dictionary<string, string>();

            foreach (var user in users)
            {
                userRoles.Add(user.Id, identityDb.Roles.ToList().FirstOrDefault(r => r.Id.Equals(user.Roles.ToList()[0].RoleId)).Name);
                //var roles = UserManager.GetRoles(user.Id);
                //string role = roles.ToString();
            }
            var adminRole = identityDb.Roles.FirstOrDefault(ro => ro.Name.Equals("Admins")).Id;

            ViewData["Users"] = users;
            ViewData["Roles"] = userRoles;
            ViewData["AdminRole"] = adminRole;

            return View();
        }

        [Authorize(Roles = "Admins")]
        public ActionResult PostsManage()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            var articles = articleDb.Articles.ToList() as List<Article>;

            Dictionary<string, string> postedUsers = new Dictionary<string, string>();

            foreach (var article in articles)
            {
                postedUsers.Add(article.Id, UserManager.FindById(article.UserId).UserName);
            }

            Dictionary<string, string> modifiedUsers = new Dictionary<string, string>();
            foreach (var article in articles)
            {
                modifiedUsers.Add(article.Id, UserManager.FindById(article.LastModifiedUserId).UserName);
            }

            ViewData["Articles"] = articles;
            ViewData["PostedUsers"] = postedUsers;
            ViewData["LastModifiedUsers"] = modifiedUsers;
            return View();
        }

        public JsonResult GetUserInfo(string id)
        {
            var user = UserManager.Users.FirstOrDefault(u => u.Id.Equals(id));
            if (user != null)
            {
                EditUserViewModel editUser = new EditUserViewModel();
                editUser.FirstName = user.FirstName;
                editUser.LastName = user.LastName;
                editUser.Phone = user.Phone;
                editUser.UserName = user.UserName;
                editUser.Email = user.Email;
                editUser.Role = identityDb.Roles.ToList().FirstOrDefault(r => r.Id.Equals(user.Roles.ToList()[0].RoleId)).Name;

                return Json(editUser, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public JsonResult Update(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(model.UserName)) as ApplicationUser;
                IPasswordHasher myPasswordHasher = UserManager.PasswordHasher;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Phone = model.Phone;
                user.Email = model.Email;
                UserManager.RemoveFromRole(user.Id, identityDb.Roles.ToList().FirstOrDefault(r => r.Id.Equals(user.Roles.ToList()[0].RoleId)).Name);
                UserManager.AddToRole(user.Id, model.Role);
                if (model.Role.Equals("Admins"))
                {
                    user.Status = (int)IdentityStatus.Normal;
                }

                if (model.Password != null)
                    user.PasswordHash = myPasswordHasher.HashPassword(model.Password);
                try
                {
                    identityDb.Entry(user).State = EntityState.Modified;
                    identityDb.SaveChanges();
                    return Json("Successfully");
                }
                catch (Exception)
                {
                    return Json(null);
                }
            }
            return Json(null);

        }

        public JsonResult Create(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Phone = model.Phone, Status = (int)IdentityStatus.Normal, ProfilePicture = "~/App/uploads/avatar/default.png", EmailConfirmed = true };
                IdentityResult result = UserManager.Create(user, model.Password);
                UserManager.AddToRole(user.Id, model.Role);
                return Json("Successfully");
            }

            return Json(null);

        }

        public JsonResult Lock(string id)
        {
            var user = identityDb.Users.FirstOrDefault(a => a.Id.Equals(id));

            if (user != null && user.Status != (int)IdentityStatus.Locked)
            {
                user.Status = (int)IdentityStatus.Locked;
                identityDb.Entry(user).State = EntityState.Modified;
                identityDb.SaveChanges();
                return Json("Successfully");

            }
            return Json(null);

        }

        public JsonResult Unlock(string id)
        {
            var user = identityDb.Users.FirstOrDefault(a => a.Id.Equals(id));

            if (user != null && user.Status != (int)IdentityStatus.Normal)
            {
                user.Status = (int)IdentityStatus.Normal;
                identityDb.Entry(user).State = EntityState.Modified;
                identityDb.SaveChanges();
                return Json("Successfully");

            }
            return Json(null);

        }

        public JsonResult Delete(string id)
        {
            var user = identityDb.Users.FirstOrDefault(u => u.Id.Equals(id)) as ApplicationUser;
            identityDb.Users.Remove(user);
            identityDb.Entry(user).State = EntityState.Deleted;
            identityDb.SaveChanges();
            return Json("Successfully");
        }

        [AllowAnonymous]
        public ActionResult LockedUser()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult DisplayEmail()
        {
            return View();
        }

        [Authorize(Roles = "Admins")]
        public ActionResult SMSManager()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            return View();
        }

        public JsonResult SendSMS(SendSMSViewModel model)
        {
            return Json(SMSServices.Send(model));
        }


        public ActionResult Dashboard()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            return View();
        }


        //
        // GET: /Account/EventCalendar
        [AllowAnonymous]
        public ActionResult EventCalendar()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            return View();
        }


        public ActionResult Task()
        {
            ViewBag.CurrentUser = identityDb.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));
            ViewBag.UserTasks = subscriberDb.UserTasks.ToList().FindAll(uts => uts.UserName.Equals(User.Identity.Name)).OrderByDescending(ut => ut.CreatedDate).ToList();
            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [AllowAnonymous]
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}