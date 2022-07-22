using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationAlarm.Models;
using Microsoft.AspNet.Identity;
using EducationAlarm.ViewModels;
using EducationAlarmDb;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Media;

namespace EducationAlarm.Controllers
{
    //We need to add a db context here 
    public class AlarmController : Controller
    {
        private AlarmContext db = new AlarmContext();
        Alarm alarm = new Alarm();
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            var alarmTimes = db.UserTimes.Where(x => x.UserId == user);
            var amt = alarmTimes.Count();
            ViewBag.Amount = amt;
            TempData["amt"] = amt;
            return View(alarmTimes);
        }
     
    //    public async Task <ActionResult> LoadWords()
    //    {
    //        List<string> words = new List<string>();
    //        words.Add("Hello");
    //        words.Add("Goodbye");
    //        words.Add("You");
    //        words.Add("I");
    //        words.Add("car");
    //        words.Add("truck");
    //        words.Add("house");
    //        words.Add("furniture");
    //        var dbWords = db.ForeignWords;
    //        foreach (var item in dbWords)
    //        {
    //            words.Add(item.Word);
    //        }
    //        foreach (var item in words)
    //        {
    //        var client = new HttpClient();
    //        var request = new HttpRequestMessage
    //        {
    //            Method = HttpMethod.Get,
    //            RequestUri = new Uri("https://nlp-translation.p.rapidapi.com/v1/translate?text="+item+"&to=iw&from=en"),
    //            Headers =
    //{
    //    { "X-RapidAPI-Key", "key" },
    //    { "X-RapidAPI-Host", "p.rapidapi.com" },
    //},
    //        };
    //        using (var response = await client.SendAsync(request))
    //        {
    //            response.EnsureSuccessStatusCode();
    //            var body = await response.Content.ReadAsStringAsync();
    //           var Json = JsonConvert.DeserializeObject<TranslateApiResponse>(body);
    //           var newWord = db.ForeignWords.Add(new ForeignWord { Word=Json.original_text,Definintion=Json.translated_text[0].Values.ToString()});
    //        }

    //        }
    //        db.SaveChanges();
    //        return RedirectToAction("Index");
    //    }
        // GET: Alarm
        [HttpGet]
        public ActionResult AlarmSet()
        {
            return PartialView();
        }
        //[HttpGet]
        //public ActionResult SelectAlarmSound()
        //{
        //    ViewBag.SoundLocation = @"Home\educationalarm.com\wwwroot\Alarm Sounds";
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult SelectAlarmSound(Alarm alarm)
        //{

        //}
        [HttpPost]
        public ActionResult AlarmSet(AlarmSet alarmSet)
        {
            var user = User.Identity.GetUserId();
           int alarmAmt=(int)TempData["amt"];
            if (alarmAmt < 5)
            {
                db.UserTimes.Add(new UserTime { UserId = user, AlarmTime = alarmSet.AlarmTime, AlarmActivated = false });
                db.SaveChanges();
            }
            else
            {
                ViewBag.Error = true;// "You can only make 5 alarms";
                return View();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int?id)
        {
            var get = db.UserTimes.SingleOrDefault(x => x.id == id);
            return PartialView(get);
        }
        [HttpPost]
        public ActionResult Edit(UserTime userTime)
        {
            var alarm = db.UserTimes.SingleOrDefault(x => x.id == userTime.id);
            alarm.AlarmTime = userTime.AlarmTime;
            alarm.AlarmActivated = false;
            db.SaveChanges();
            return RedirectToAction("index");
        }
        UserAnswers ul = new UserAnswers();

        public ActionResult ActivateAlarm(int id)
        {
            //For some reason on the live server. It's not getting activated.Maybe
            //because of timezone problems
            var userid = User.Identity.GetUserId();
            alarm.ActivateAlarm(id,userid);
            return RedirectToAction("index");
        }
        MathProblems mp = new MathProblems();
        ForeignWordsImport fwi = new ForeignWordsImport();
        [HttpGet]
        public ActionResult Subject(int id)
        {
            List<int> mathAnswers = new List<int>();
            List<string> langWords = new List<string>();
            var user = User.Identity.GetUserId();
            var subject = db.UserInformation.Include("Subject").SingleOrDefault(x => x.IdentityUserId == user);
            string subj = subject.Subject.SubjectName;
            TempData["subj"] = subj;
            var category = db.Subjects.Include("SubjectCategory").FirstOrDefault(x => x.SubjectId == subject.SubjectId);
            int alarmid = db.UserTimes.SingleOrDefault(x => x.id == id).id;
            UserAnswers ua = new UserAnswers();
            ua.AlarmId = alarmid;
            if (category.SubjectCategory.Category == "Language")
            {
                var list1 = fwi.ShuffledList(user);
                ViewBag.LangList1 = list1;
                string word1 = list1[0];
                langWords.Add(word1);

                var list2 = fwi.ShuffledList(user);
                ViewBag.LangList2 = list2;
                string word2 = list2[0];
                langWords.Add(word2);

                var list3 = fwi.ShuffledList(user);
                ViewBag.LangList3 = list3;
                string word3 = list3[0];
                langWords.Add(word3);

                var list4 = fwi.ShuffledList(user);
                ViewBag.LangList4 = list4;
                string word4 = list4[0];
                langWords.Add(word4);
                TempData["langWords"] = langWords;
                return View("SubjectLanguage", ua);
            }
            else if (category.SubjectCategory.Category == "Math")
            {
                var list1 = mp.ShuffledList(user);
                ViewBag.MathList1 = list1;      
                int x1 = Convert.ToInt32(list1[0]);
                int y1 = Convert.ToInt32(list1[2]);
                var ans1 = vum.MathOp(subj, x1, y1);
                mathAnswers.Add(ans1);

                var list2 = mp.ShuffledList(user);
                ViewBag.MathList2 = list2;          
                int x2 = Convert.ToInt32(list2[0]);
                int y2 = Convert.ToInt32(list2[2]);
                var ans2 = vum.MathOp(subj, x2, y2);
                mathAnswers.Add(ans2);

                var list3 = mp.ShuffledList(user);
                ViewBag.MathList3 = list3;        
                int x3 = Convert.ToInt32(list3[0]);
                int y3 = Convert.ToInt32(list3[2]);
                var ans3 = vum.MathOp(subj, x3, y3);
                mathAnswers.Add(ans3);

                var list4 = mp.ShuffledList(user);
                ViewBag.MathList4 = list4;      
                int x4 = Convert.ToInt32(list4[0]);
                int y4 = Convert.ToInt32(list4[2]);
                var ans4 = vum.MathOp(subj, x4, y4);
                mathAnswers.Add(ans4);
      
                TempData["mathAns"] = mathAnswers;
                return View("SubjectMath", ua);
            }
            return RedirectToAction("index");
        }
    
        ViewUtilityMethods vum = new ViewUtilityMethods();
        ViewDbLookups vdbl = new ViewDbLookups();
        [HttpPost]
        public ActionResult Subject(UserAnswers ul)
        {
            bool answered = false;
             if (ul.LangAnswer1.Length >= 1 && ul.LangAnswer2.Length >= 1 && ul.LangAnswer3.Length >= 1 && ul.LangAnswer4.Length >= 1)
            {
                var langwords = (List<string>)TempData["langWords"];
                ViewBag.Symbol1 = vdbl.DefinitionMatched(langwords[0], ul.LangAnswer1);
                ViewBag.Symbol2 = vdbl.DefinitionMatched(langwords[1], ul.LangAnswer2);
                ViewBag.Symbol3 = vdbl.DefinitionMatched(langwords[2], ul.LangAnswer3);
                ViewBag.Symbol4 = vdbl.DefinitionMatched(langwords[3], ul.LangAnswer4);
                ViewBag.Subject = "language";
                answered = true;
                alarm.StopAlarm(answered, ul.AlarmId);
        
            }
           else if (ul.MathAnswer1.HasValue == true && ul.MathAnswer2.HasValue == true && ul.MathAnswer3.HasValue == true && ul.MathAnswer4.HasValue == true)
            {
                var answers = (List<int>)TempData["mathAns"];
                ViewBag.Symbol1 = vum.Symbol(answers[0], ul.MathAnswer1);
                ViewBag.Symbol2 = vum.Symbol(answers[1], ul.MathAnswer2);
                ViewBag.Symbol3 = vum.Symbol(answers[2], ul.MathAnswer3);
                ViewBag.Symbol4 = vum.Symbol(answers[3], ul.MathAnswer4);
                ViewBag.Subject = "math";
                answered = true;
                alarm.StopAlarm(answered, ul.AlarmId);
                ul.Subject= (string)TempData["subj"];              
            }

            else
            {
                ViewBag.Answered = answered;
            }

            return PartialView();
        }

 

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var get = db.UserTimes.SingleOrDefault(x => x.id == id);
            return View(get);
        }
        [HttpPost]
        public ActionResult Delete(UserTime time)
        {
            var get = db.UserTimes.SingleOrDefault(x => x.id == time.id);
            db.UserTimes.Remove(get);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }

}