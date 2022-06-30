using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationAlarm.Models;
using Microsoft.AspNet.Identity;
using EducationAlarm.ViewModels;
using EducationAlarmDb;

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
            return View(alarmTimes);
        }
        // GET: Alarm
        [HttpGet]
        public ActionResult AlarmSet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AlarmSet(AlarmSet alarmSet)
        {
            var user = User.Identity.GetUserId();
            //  var findUser = db.Users.SingleOrDefault(x => x.Id == user);
            db.UserTimes.Add(new UserTime { UserId = user, AlarmTime = alarmSet.AlarmTime, AlarmActivated = false });
            db.SaveChanges();
            //alarm.PutTogether();
            return RedirectToAction("Index");
        }
        UserAnswers ul = new UserAnswers();

        public ActionResult ActivateAlarm(int id)
        {
            alarm.ActivateAlarm(id);
            return RedirectToAction("index");
        }
        MathProblems mp = new MathProblems();
        [HttpGet]
        public ActionResult Subject(int id)
        {
            List<int> mathAnswers = new List<int>();
            List<string> MathEquations = new List<string>();
            var user = User.Identity.GetUserId();
            var subject = db.UserInformation.Include("Subject").SingleOrDefault(x => x.IdentityUserId == user);
            string subj = subject.Subject.SubjectName;
            TempData["subj"] = subj;
            var category = db.Subjects.Include("SubjectCategory").FirstOrDefault(x => x.SubjectId == subject.SubjectId);
            // var category = subject.Subject.SubjectCategoryId;
            int alarmid = db.UserTimes.SingleOrDefault(x => x.id == id).id;
            UserAnswers ua = new UserAnswers();
            ua.AlarmId = alarmid;
            if (category.SubjectCategory.Category == "Language")
            {
                return View("SubjectLanguage", ua);
                //Need to do the same what we did for math here
            }
            else if (category.SubjectCategory.Category == "Math")
            {
                var list1 = mp.ShuffledList(user);
                ViewBag.MathList1 = list1;
             //   string equation1 = list1[0].ToString() + " " + list1[1].ToString() + " " + list1[2].ToString();
               // MathEquations.Add(equation1);
                int x1 = Convert.ToInt32(list1[0]);
                int y1 = Convert.ToInt32(list1[2]);
                var ans1 = vum.MathOp(subj, x1, y1);
                mathAnswers.Add(ans1);
                var list2 = mp.ShuffledList(user);
                ViewBag.MathList2 = list2;
                //string equation2 = list2[0].ToString() + " " + list2[1].ToString() + " " + list2[2].ToString();
                //MathEquations.Add(equation2);
                int x2 = Convert.ToInt32(list2[0]);
                int y2 = Convert.ToInt32(list2[2]);
                var ans2 = vum.MathOp(subj, x2, y2);
                mathAnswers.Add(ans2);
                var list3 = mp.ShuffledList(user);
                ViewBag.MathList3 = list3;
                //string equation3 = list3[0].ToString() + " " + list3[1].ToString() + " " + list3[2].ToString();
                //MathEquations.Add(equation3);
                int x3 = Convert.ToInt32(list3[0]);
                int y3 = Convert.ToInt32(list3[2]);
                var ans3 = vum.MathOp(subj, x3, y3);
                mathAnswers.Add(ans3);
                var list4 = mp.ShuffledList(user);
                ViewBag.MathList4 = list4;
               // string equation4 = list4[0].ToString() + " " + list4[1].ToString() + " " + list4[2].ToString();
               // MathEquations.Add(equation4);
                int x4 = Convert.ToInt32(list4[0]);
                int y4 = Convert.ToInt32(list4[2]);
                var ans4 = vum.MathOp(subj, x4, y4);
                mathAnswers.Add(ans4);
               // TempData["mathProbs"] = MathEquations;
                TempData["mathAns"] = mathAnswers;
                return View("SubjectMath", ua);
            }


            return RedirectToAction("index");
        }
    
        ViewUtilityMethods vum = new ViewUtilityMethods();
        [HttpPost]
        public ActionResult Subject(UserAnswers ul)
        {
            bool answered = false;
            if (ul.MathAnswer1.HasValue == true && ul.MathAnswer2.HasValue == true && ul.MathAnswer3.HasValue == true && ul.MathAnswer4.HasValue == true)
            {

                //List<int?> UserAnswers = new List<int?>();
                //UserAnswers.Add(ul.MathAnswer1);
                //UserAnswers.Add(ul.MathAnswer2);
                //UserAnswers.Add(ul.MathAnswer3);
                //UserAnswers.Add(ul.MathAnswer4);
                var answers = (List<int>)TempData["mathAns"];
                ViewBag.Symbol1 = vum.Symbol(answers[0], ul.MathAnswer1);
                ViewBag.Symbol2 = vum.Symbol(answers[1], ul.MathAnswer2);
                ViewBag.Symbol3 = vum.Symbol(answers[2], ul.MathAnswer3);
                ViewBag.Symbol4 = vum.Symbol(answers[3], ul.MathAnswer4);
                answered = true;
                alarm.StopAlarm(answered, ul.AlarmId);
                ul.Subject= (string)TempData["subj"];
               
            }

            else if (ul.LangAnswer1.Length >= 1 && ul.LangAnswer2.Length >= 1 && ul.LangAnswer3.Length >= 1 && ul.LangAnswer4.Length >= 1)
            {
                
                 answered = true;
                alarm.StopAlarm(answered, ul.AlarmId);
        
            }

            return PartialView();
          
            //PartialView needs to be added and we still need to figure out how to get the
            //right answers
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