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
        string _Symbol { get; set; }
        public string DefinitionMatched(string foreignWord,string definition)
        {
           
            var word = db.ForeignWords.FirstOrDefault(x => x.Word == foreignWord);
            if (word.Definintion == definition)
            {
                _Symbol = "\u2713";
            }
            else
            {
                _Symbol = "x";
            }
            return _Symbol;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}