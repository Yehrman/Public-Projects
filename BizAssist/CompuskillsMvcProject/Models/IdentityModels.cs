using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcProjectDbConn;
namespace CompuskillsMvcProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class BizAssistData : IdentityUser
    {
        //You can add any property you want here e.g birthday
    
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<BizAssistData> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class BizAssistConn : IdentityDbContext<BizAssistData>
    {
        public BizAssistConn()
            : base("BizAssistContext", throwIfV1Schema: false)
        {
        }


    }
}