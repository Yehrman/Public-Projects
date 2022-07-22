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
using EducationAlarm.Models;
using EducationAlarmDb;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.Entity;

namespace EducationAlarm.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private AlarmSignInManager _signInManager;
        private AlarmUserManager _userManager;
        private AlarmContext db = new AlarmContext();
        public AccountController()
        {
        }

        public AccountController(AlarmUserManager userManager, AlarmSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public AlarmSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AlarmSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public AlarmUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AlarmUserManager>();
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Alarm");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

  
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            //Error getting thrown here
             RegisterViewModel model = new RegisterViewModel();  
            //error here
            ViewBag.SubjectId = new SelectList(db.Subjects.ToList(), "SubjectId", "SubjectName");
            ViewBag.TimeZone = new SelectList(model.TimeZones());
            return View();
        }
        void Update(UserInformation model)
        {
            try
            {
            // if(info.Subject=="Addition"||info.Subject== "multiplication" ||info.Subject== "division")
                db.UserInformation.Add(new UserInformation { IdentityUserId=model.IdentityUserId,FirstName=model.FirstName,LastName=model.LastName,TimeZone=model.TimeZone,SubjectId=model.SubjectId});
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {  
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new  IdentityUser{ UserName = model.Email, Email = model.Email, };
            
                var result =  UserManager.Create(user, model.Password);
                var data = new { id = user.Id };
                if (result.Succeeded)
                {
                    
                   // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    var subjectId = db.Subjects.SingleOrDefault(x => x.SubjectId == model.SubjectId);
                var info = new UserInformation { IdentityUserId=user.Id,FirstName = model.FirstName, LastName = model.LastName, SubjectId=subjectId.SubjectId ,TimeZone=model.TimeZone};
                    Update(info);
                  var newUser = db.UserInformation.Include("IdentityUser").SingleOrDefault(x => x.IdentityUserId == data.id);
                  
                    TempData["newUser"] =newUser;
                    Confirm(newUser,"confirm");
            return RedirectToAction("ConfirmEmail");
                 
                }
                AddErrors(result);
            }
            ViewBag.SubjectId = new SelectList(db.Subjects.ToList(), "SubjectId", "SubjectName");
            ViewBag.TimeZone = new SelectList(model.TimeZones());
            return View(model);
        }
        private static string Code { get; set; }
        private static void VerificationCode()
        {
            Random random = new Random();
            int code = random.Next(1000001, 9999999);
            Code = code.ToString();

        }
        private static string VerCode
        {
            get
            {
                return Code;
            }
        }
        private static DateTime TimeCodeSent;
        private void Confirm(UserInformation user,string description)
        {
           
          //  var thisUser = db.UserInformation.Include("IdentityUser").SingleOrDefault(x => x.IdentityUserId == id);
            VerificationCode();
            string code = VerCode;
            EmailSetup setup = new EmailSetup();
           // var newUser=(IdentityUser) TempData["newUser"]; 
            if (description == "confirm")
            {
                setup.Content = $"Dear {user.FirstName}  {user.LastName} Thank you for signing up with EducationAlarm. Please confirm your account by typing in this code " +
                      code + ". We look foward to a mutually rewarding relationship together.Sincerely  the EduationAlarm developer" + "  " + "P.S This code will expire in 15 minutes";
                setup.Subject = "Confirm email";
            }
            else if(description=="reset")
            {
                setup.Content = $"Dear {user.FirstName}  {user.LastName} We found your account. Please  type in this code  " +
             code + " in the Reset password field on the reset page . Sincerely  the EducationAlarm developer" + "    " + "P.S This code will expire in 15 minutes";
                setup.Subject = "Reset password";
            }
                 setup.Sender = "support@educationalarm.com";
           setup.SenderName = "Education Alarm";
            setup.Reciever = user.IdentityUser.Email;
            setup.RecieverName = user.FirstName + " " + user.LastName;
            setup.EmailPassword = "It'sfromHashem1";
            EmailMessage message = new EmailMessage();
            message.SendEmail(setup);
            TimeCodeSent = DateTime.UtcNow;
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public  ActionResult ConfirmEmail()
        {
            EmailConfirmation confirmation = new EmailConfirmation();
            UserInformation user = (UserInformation)TempData["newUser"];
            confirmation.Id = user.IdentityUserId;
            ViewBag.Message = "Please check your email for your confirmation code";
            return View(confirmation);
        }
        [HttpPost]
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public ActionResult ConfirmEmail(EmailConfirmation confirmation)
        {
          
            if (confirmation.Id == null || confirmation.Password == null)
            {
                return View("Error");
            }
            if (confirmation.Password == VerCode && DateTime.UtcNow < TimeCodeSent.AddMinutes(15))
            {
                var newUser = db.UserInformation.Include("IdentityUser").SingleOrDefault(x => x.IdentityUser.Id == confirmation.Id);
                newUser.IdentityUser.EmailConfirmed = true;
                db.Entry(newUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("code", "Please check your code and try again");
            return View();
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
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var loggedInUser = User.Identity.GetUserId();  
            var user = db.UserInformation.Include("IdentityUser").FirstOrDefault(x => x.IdentityUser.Email == model.Email);
            //var info = db.UserInformation.SingleOrDefault(x => x.IdentityUserId ==loggedInUser);
            if(user.IdentityUser.Id==loggedInUser&&user.IdentityUser.Email==model.Email)
            {
                Confirm(user, "reset");
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form give error message
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

       
        //We need to finish up the reset password
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = db.UserInformation.Include("IdentityUser").FirstOrDefault(x => x.IdentityUser.Email == model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            if (user.IdentityUser.Email == model.Email && Code == model.Code && DateTime.UtcNow < TimeCodeSent.AddMinutes(15))
            {

                UserManager.RemovePassword(user.IdentityUser.Id);
                UserManager.AddPassword(user.IdentityUser.Id, model.Password);
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            else
            {
                ModelState.AddModelError("error", "Please check your email and code and try again");
                return View();
            }
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
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
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
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
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
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
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
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
            return RedirectToAction("Index", "Home");
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