using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MvcProjectDbConn.Identity;
using MvcProjectDbConn;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CompuskillsMvcProject.IdentietyExtensions
{
    public static class IdentityExtensions
    {
        public static BizAssistUserManager CreateUserManager(this IdentityFactoryOptions<BizAssistUserManager> options, IOwinContext context)
        {
            var UserStore = new UserStore<Employee>(context.Get<BizAssistContext>());
            var Manager = BizAssistUserManager.Create(UserStore);
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                Manager.UserTokenProvider =
                    new DataProtectorTokenProvider<Employee>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return Manager;
        }
    }
}