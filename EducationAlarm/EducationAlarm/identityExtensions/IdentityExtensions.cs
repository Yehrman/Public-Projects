using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using EducationAlarmDb;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EducationAlarm.IdentietyExtensions
{
    public static class IdentityExtensions
    {
        public static AlarmUserManager CreateUserManager(this IdentityFactoryOptions<AlarmUserManager> options, IOwinContext context)
        {
            var UserStore = new UserStore<IdentityUser>(context.Get<AlarmContext>());
            var Manager =AlarmUserManager.Create(UserStore);
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                Manager.UserTokenProvider =
                    new DataProtectorTokenProvider<IdentityUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return Manager;
        }
    }
}