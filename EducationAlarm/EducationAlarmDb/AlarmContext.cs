using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
namespace EducationAlarmDb
{
   public class AlarmContext :IdentityDbContext
    {
      public DbSet<UserTime> UserTimes { get; set; }
        public DbSet<ForeignLanguage> ForeignLanguages { get; set; }
        public DbSet <ForeignWord> ForeignWords { get; set; }
        public DbSet<UserInformation> UserInformation { get; set; }
        public static AlarmContext Create()
        {
            return new AlarmContext();
        }
        public AlarmContext()

           : base("AlarmContext")
        {
        }
    }
    }

