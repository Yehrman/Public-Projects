using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MvcProjectDbConn.Identity
{

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    //Dealing with the user in from the db point of view
    public class BizAssistUserManager : UserManager<Employee>
    {
        public BizAssistUserManager(IUserStore<Employee> store)
            : base(store)
        {
        }
        //All the rules for what is needed for authentication
        public static BizAssistUserManager Create(UserStore<Employee> userStore)
        {
            var manager = new BizAssistUserManager(userStore);
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Employee>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 10,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<Employee>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<Employee>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
           // manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            //var dataProtectionProvider = options.DataProtectionProvider;
          //  if (dataProtectionProvider != null)
            //{
              //  manager.UserTokenProvider =
                //    new DataProtectorTokenProvider<TtpUsersData>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}
            return manager;
        }
    }
}
