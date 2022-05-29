using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using EducationAlarm.Models;
using EducationAlarmDb;

namespace EducationAlarm
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application.
    //UserManager is defined in ASP.NET Identity and is used by the application.
   

    // Configure the application sign-in manager which is used in this application.
    public class AlarmSignInManager : SignInManager<IdentityUser, string>
    {
        public AlarmSignInManager(AlarmUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(IdentityUser user)
        {
            return ((AlarmUserManager)UserManager).CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public static AlarmSignInManager Create(IdentityFactoryOptions<AlarmSignInManager> options, IOwinContext context)
        {
            return new AlarmSignInManager(context.GetUserManager<AlarmUserManager>(), context.Authentication);
        }
    }
}
