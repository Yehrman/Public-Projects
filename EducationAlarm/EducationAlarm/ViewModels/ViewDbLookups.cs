using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EducationAlarmDb;

namespace EducationAlarm.ViewModels
{
    public class ViewDbLookups:IDisposable
    {
        private AlarmContext db = new AlarmContext();
        public string FindSubject(string id)
        {        
               var user = db.UserInformation.Include("Subject").SingleOrDefault(x => x.IdentityUserId == id);
            string subject = user.Subject.SubjectName;       
               return subject;
        }
        public bool? AlarmActivated(int id)
        {
            var alarm = db.UserTimes.SingleOrDefault(x => x.id == id);
            return alarm.AlarmActivated;
        }
        public List <Subject> GetCategory(int id)
        {
            var subjects = db.Subjects.Where(x => x.SubjectCategoryId == id).ToList();
            return subjects;
        }
  
        public void Dispose()
        {
            db.Dispose();
        }
    }
}