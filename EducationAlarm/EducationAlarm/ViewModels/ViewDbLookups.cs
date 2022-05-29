using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EducationAlarmDb;

namespace EducationAlarm.ViewModels
{
    public class ViewDbLookups
    {
        public string FindSubject(string id)
        {
            string subject = "";
            using (AlarmContext db = new AlarmContext())
            {
                var user = db.UserInformation.SingleOrDefault(x => x.IdentityUserId == id);
                subject = user.Subject;
            }
            return subject;
        }
    }
}