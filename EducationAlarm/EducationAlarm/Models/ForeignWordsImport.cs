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
        static List<string> unavailableForeignWords = new List<string>();
        ForeignWord GetForeignWord(List<ForeignWord> words)
        {
            ForeignWord selectedWord = words.ElementAt(r.Next(0, words.Count()-1));
            return selectedWord;     
        }
        string Guess(List<string> words,List<string> unavailableWords)
        {
            int amt = unavailableWords.Count()+1;
            var guesses = words.Except(unavailableWords).ToList();
            string guess = guesses.ElementAt(r.Next(0, guesses.Count-1));
                //words.Where(x=>x.Definintion.Any()!=unavailableWords.Any()).ElementAt(r.Next(0, words.Count()-amt));
            return guess;
        }
        List<string> TempList(int id)
        {
            var words = db.ForeignWords.Where(x => x.SubjectId == id).ToList();
            List<string> definitions = new List<string>();
            foreach (var item in words)
            {
                definitions.Add(item.Definintion);
            }
            List<string> temp = new List<string>();
            List<string> unavailableWords = new List<string>();
            var word = GetForeignWord(words);
            temp.Add(word.Word);
            temp.Add(word.Definintion);
            unavailableForeignWords.Add(word.Word);
            unavailableWords.Add(word.Definintion);
            for (int i = 0; i < 3; i++)
            {
            string guess = Guess(definitions,unavailableWords);
            temp.Add(guess);         
            unavailableWords.Add(guess);
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