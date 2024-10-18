using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TimeAide.Web.Models;
using TimeAide.Common.Helpers;
using System.Web.Security;
using System.Text;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TimeAide.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private TimeAidePayrollContext db = new TimeAidePayrollContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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
        public ActionResult Login(string company, string returnUrl)

        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LogoPath = "/Content/Themes/assets/img/logo.png";
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
           // TimeAidePayrollContext db = new TimeAidePayrollContext();
            var result = SignInStatus.Success;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                var loginId = UserManager.FindByEmail(model.Email);
                if (loginId != null)
                {
                   var isActive= db.UserInformation.Where(w => w.LoginEmail == model.Email && w.LoginStatus == "Active" && w.DataEntryStatus == 1).Count();
                    if (isActive == 0)
                    {
                        ModelState.AddModelError("", "Inactive User.");
                        return View(model);
                    }
                }

            }
           
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    
                    // var UserContactInformation = db.UserContactInformation.FirstOrDefault(a => a.LoginEmail.ToLower() == model.Email.ToLower());

                    //  var UserInformation = db.UserInformation.FirstOrDefault(a => a.Id == UserContactInformation.UserInformationId);
                    var userRecord = db.UserInformation.Where(w => w.LoginEmail == model.Email).FirstOrDefault();
                    SessionHelper.SetSessionVariables(model.Email, userRecord.Id, userRecord.FirstName,userRecord.IsAdmin);
                    if (model.RememberMe)
                    {
                        // They do, so let's create an authentication cookie
                        var cookie = FormsAuthentication.GetAuthCookie(model.Email.ToLower(), model.RememberMe);

                        // Since they want to be remembered, set the expiration for 30 days
                        cookie.Expires = DateTime.Now.AddDays(30);
                        // Store the cookie in the Response
                        Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        // Otherwise set the cookie as normal
                        FormsAuthentication.SetAuthCookie(model.Email.ToLower(), model.RememberMe); // <- true/false
                    }

                    if (!string.IsNullOrEmpty(returnUrl))
                        return RedirectToLocal(returnUrl);
                    // return RedirectToAction("Index", "Home");
                    return RedirectToAction("Index", "PayrollManagement");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

         //
        // POST: /Account/Register
        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Register(RegisterViewModel model)
        {
            string status = "Success";
            string message = "";
           // if (ModelState.IsValid)
           // {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                UserManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    
                    message = "User is successfully created";
                }
                else
                {
                    status = "Error";
                    message = "Error While Creating user: ";
                    foreach (var error in result.Errors)
                    {
                        message += error+" |";
                    }
                   // AddErrors(result);
                }
               
           // }
            // If we got this far, something failed, redisplay form
            //return View(model);
            return Json(new { status = status, message = message });
        }      

        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        //
        //Save Details of New Password using Identity
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
          
            if (ModelState.IsValid)
            {


                UserManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                };

                var user = await UserManager.FindByNameAsync(SessionHelper.LoginEmail);
                var result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return PartialView("ChangePasswordConfirmation");
                }
                else
                {
                    AddErrors(result);
                }
            }
           
            // AddErrors(result);
             return PartialView(model);
           // return Json(new { status = status, message = message });
        }

        public ActionResult ChangePasswordByAdmin(int id)
        {
            var model = new SetPasswordViewModel() { UserInformationId = id };
            return PartialView(model);
        }
        [HttpPost]
        public async Task<JsonResult> ChangePasswordByAdmin(SetPasswordViewModel model)
        {

            string status = "Success";
            string message = "";

            var appUser = db.UserInformation.Find(model.UserInformationId);
            if (appUser!=null)
            {


                UserManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                };

                var user = await UserManager.FindByNameAsync(appUser.LoginEmail);
                var token = UserManager.GeneratePasswordResetToken(user.Id);
                //var token = UserManager.GeneratePasswordResetToken(userId);
                var result = UserManager.ResetPassword(user.Id, token, model.NewPassword);

                if (result.Succeeded)
                {
                    message = "Password is successfully changed!";
                }
                else
                {
                    status = "Error";
                    message = "Error while changing user password: ";
                    foreach (var item in result.Errors)
                    {
                        message += item + "|";
                    }
                }
            }
            else
            {
                status = "Error";
                message = "Data is not valid";
            }
            // AddErrors(result);
            // return View(model);
            return Json(new { status = status, message = message });

        }
        // POST: /Account/Register
        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Delete(string loginEmail)
        {
            string status = "Success";
            string message = "";
            // if (ModelState.IsValid)
            // {
            var user = await UserManager.FindByNameAsync(loginEmail);
            if (user != null)
            {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    message = "User is successfully Deleted";
                }
                else
                {
                    status = "Error";
                    message = "Error While Deleting user: ";
                    foreach (var error in result.Errors)
                    {
                        message += error + " |";
                    }
                    // AddErrors(result);
                }
            }
            // }
            // If we got this far, something failed, redisplay form
            //return View(model);
            return Json(new { status = status, message = message });
        }
        protected ActionResult GetErrors()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            Dictionary<String, String> errors = new Dictionary<string, string>();
            foreach (var eachState in ModelState)
            {
                if (eachState.Value != null && eachState.Value.Errors != null && eachState.Value.Errors.Count > 0)
                {
                    errors.Add(eachState.Key, eachState.Value.Errors[0].ErrorMessage);
                }
            }
            var entries = string.Join(",", errors.Select(x => "{" + string.Format("\"Key\":\"{0}\",\"Message\":\"{1}\"", x.Key, x.Value) + "}"));
            var jsonResult = Json(new { success = false, errors = "[" + string.Join(",", entries) + "]" }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.ContentType = "application/json";
            jsonResult.ContentEncoding = System.Text.Encoding.UTF8;   //charset=utf-8
            string json = JsonConvert.SerializeObject(jsonResult);
            return jsonResult;
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Index", "UserDashboard");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
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
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
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