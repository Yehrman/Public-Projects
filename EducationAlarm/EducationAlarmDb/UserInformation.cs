using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EducationAlarmDb
{
   public class UserInformation
    {
        public int id { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityUserId { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
    }
}
