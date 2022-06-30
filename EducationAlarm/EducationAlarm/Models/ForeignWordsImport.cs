using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EducationAlarmDb;
namespace EducationAlarm.Models
{
    public class ForeignWordsImport :IDisposable
    {
        Random r = new Random();
        private AlarmContext db = new AlarmContext();
        ForeignWord GetForeignWord(int id)
        {
            var words = db.ForeignWords.Where(x => x.SubjectId == id).ToList();
            ForeignWord selectedWord = words.ElementAt(r.Next(0, words.Count()));
            return selectedWord;     
        }
        string Guess(int id)
        {
            var words = db.ForeignWords.Where(x => x.SubjectId == id).ToList();
            string guess = words.ElementAt(r.Next(0, words.Count())).Definintion;
            return guess;
        }
        List<string> TempList(int id)
        {
            List<string> temp = new List<string>();
            var word = GetForeignWord(id);
            temp.Add(word.Word);
            temp.Add(word.Definintion);
            for (int i = 0; i < 3; i++)
            {
                string guess = Guess(id);
                temp.Add(guess);
            }
            return temp;
        }
        public List<string> ShuffledList(string id)
        {
            var subject = db.UserInformation.SingleOrDefault(x => x.IdentityUserId == id).SubjectId;
           // var languageid = db.ForeignLanguages.Include("Subject").SingleOrDefault(x => x.).ForeignLanguageId;
            List<string> shuffled = new List<string>();
            var temp = TempList(subject);
            shuffled.Add(temp[0]);
            temp.Remove(temp[0]);
            while (temp.Count != 0)
            {
                int a = r.Next(0, temp.Count);
                shuffled.Add(temp[a]);
                temp.RemoveAt(a);
            }
            return shuffled;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}