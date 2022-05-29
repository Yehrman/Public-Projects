using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CompuskillsMvcProject.Models;
using MvcProjectDbConn;
using MvcProjectDbConn.Identity;
using System.Data.Entity;
namespace CompuskillsMvcProject.Controllers
{
    
    public class AccountController : Controller
    {
        private TtpSignInManager _signInManager;
        private BizAssistUserManager _userManager;
        private BizAssistContext db = new BizAssistContext();
        public AccountController()
        {
        }

        public AccountController(BizAssistUserManager userManager, TtpSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public TtpSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<TtpSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public BizAssistUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<BizAssistUserManager>();
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
           var user = await UserManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    var employee = db.Employees.SingleOrDefault(x => x.Email == model.UserName);
                    Confirm(employee);
                    TempData["Emp"] = employee;
                    ViewBag.NotVerified = true;
                    return View();
                }
            }
           var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {                
                    case SignInStatus.Success:
                        return RedirectToAction("GetStarted", "Home");
                          
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                  //  case SignInStatus.RequiresVerification:
                    //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");                      
                        return View(model);
                }          
        }

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
            if(model.Password!=model.ConfirmPassword)
            {
                ModelState.AddModelError("password","Your confirmation does'nt match your password");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new Employee
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.Phonenumber,
                Familiar = false
            };


            var newUser = await UserManager.CreateAsync(user, model.Password);
            var data = new { id = user.Id };
            if (newUser == IdentityResult.Success)
                {             
                var newEmployee = db.Employees.Find(data.id);
                    TempData ["Emp"] = newEmployee;
                    Confirm(newEmployee);
               if (User.Identity.IsAuthenticated &&User.IsInRole("Ceo")||User.IsInRole("Human resources department")||User.IsInRole("Senior managment"))
                {
                    return RedirectToAction("AddEmployee", "Companies");
                }
                else
                {
                    ViewBag.Confirm = true;
                    //   return RedirectToAction("ConfirmEmail");
                    return View();
                }         
                }
                else
                {
                    ModelState.AddModelError("", "Registration failed please check your data and try again");
                    return View();
                }
       
        }
        private static string Code { get; set; }
        private static void VerificationCode()
        {
            Random random = new Random();
            int code=  random.Next(1000001, 9999999);
             Code = code.ToString();
           
        }
        private static string VerCode { get
            {
                return Code;
            } }
        //private static string EmailAddress { get; set; }
        //private static string GetEmailAddress { get
        //    {
        //        return EmailAddress;
        //    } }
        private static DateTime TimeCodeSent;
        private void Confirm(Employee employee)
        {
            VerificationCode();
            string code = VerCode;
            EmailSetup setup = new EmailSetup();
            // EmailAddress = employee.Email;
            setup.Content = $"Dear {employee.FirstName}  {employee.LastName} Thank you for signing up with bizassist. Please confirm your account by typing in this code " +
                  code + ". We look foward to a mutually rewarding relationship together.Sincerely  the bizAssist developer" + "  " + "P.S This code will expire in 15 minutes";
            //setup.Content = $"Dear {employee.FirstName}  {employee.LastName} Thank you for signing up with bizassist. Please confirm your account by clicking here  <a href='http://localhost:56588//Account/ConfirmEmailUsingLink/{code},{employee.Email}/'>clicking here </a> " +
            //   code+ ". We look foward to a mutually rewarding relationship together.Sincerely  the bizAssist developer"+"  "+ "P.S This code will expire in 15 minutes";
            setup.Sender = "support@bizassist.me";
            setup.SenderName = "BizAssist";
            setup.Reciever = employee.Email;
            setup.RecieverName = employee.FirstName + " " + employee.LastName;
            setup.Subject = "Confirm email";
            setup.EmailPassword = "t/szpjqrgy0mih[lvfoweFaEnb4cud";
            EmailMessage message = new EmailMessage();
            message.SendEmail(setup);
            TimeCodeSent = DateTime.UtcNow;
        }
        //public ActionResult ConfirmEmailUsingLink(string code,string email)
        //{
        //    //Try again
          
        //    ViewBag.Confirmed=true;
            
        //    if (code == VerCode && GetEmailAddress==email)
        //    {
        //    var newEmp = db.Employees.SingleOrDefault(x => x.Email == email);
        //        newEmp.EmailConfirmed = true;
        //        db.Entry(newEmp).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return View();

        //    }
        //    else
        //    {
        //        ViewBag.Confirmed = false;
        //        return View();
        //    }
        //}
     [HttpGet]
        public ActionResult ConfirmEmail()
        {
            EmailConfirmation confirmation = new EmailConfirmation();
            Employee emp = (Employee)TempData["Emp"];
            confirmation.Id = emp.Id; 
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
            if (confirmation.Password== VerCode&& DateTime.UtcNow<TimeCodeSent.AddMinutes(15))
            {
                //Should add viewbag notifying client that email confirmed
               // ViewBag.Confirmed = true;
                var newUser = db.Employees.Find(confirmation.Id);
                newUser.EmailConfirmed = true;
                db.Entry(newUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("code", "Please check your code and try again");
            return View(); 
        }
        private static string ResetCode { get; set; }
        private static void SetResetCode()
        {
            Random random = new Random();
            int code = random.Next(1000001, 9999999);
            ResetCode = code.ToString();

        }
        private static string GetResetCode
        {
            get
            {
                return ResetCode;
            }
        }
        private static DateTime TimeResetCodeSent { get; set; }
        private void Forgot(string email)
        {
            SetResetCode();
            string code = GetResetCode;
            var employee = db.Employees.SingleOrDefault(x => x.Email == email);
              EmailSetup setup = new EmailSetup();
            setup.Content = $"Dear {employee.FirstName}  {employee.LastName} We found your account. Please  type in this code  " +
                  code + " in the Reset password field on the reset page . Sincerely  the BizAssist developer" + "    " + "P.S This code will expire in 15 minutes";
            setup.Sender = "support@bizassist.me";
            setup.SenderName = "BizAssist";
            setup.Reciever = email;
            setup.RecieverName = employee.FirstName + " " + employee.LastName;
            setup.Subject = "Reset Password";
            setup.EmailPassword = "t/szpjqrgy0mih[lvfoweFaEnb4cud";
            EmailMessage message = new EmailMessage();
            message.SendEmail(setup);
           TimeResetCodeSent = DateTime.UtcNow;
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
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                Forgot(model.Email);
                return RedirectToAction("ForgotPasswordConfirmation");
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
        public ActionResult ResetPassword()
        {     
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
           
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Need to figure out why model.code is staying null
            var user = db.Employees.SingleOrDefault(x => x.Email == model.Email);
            if (user.Email==model.Email && GetResetCode==model.Code && DateTime.UtcNow<TimeResetCodeSent.AddMinutes(15))
            {

                UserManager.RemovePassword(user.Id);
           
                UserManager.AddPassword(user.Id, model.Password);
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


        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
          
             
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return RedirectToAction("Index", "Home");            
        }

        //
        //// GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}
     
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