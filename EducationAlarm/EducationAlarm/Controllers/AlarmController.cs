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
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            var alarmTimes=db.UserTimes.Where(x=>x.UserId==user);
            return View(alarmTimes);
        }
        // GET: Alarm
        [HttpGet]
        public ActionResult AlarmSet()
        {
            return View();
        }
     
        Alarm alarm = new Alarm();
        [HttpPost]
        public ActionResult AlarmSet(AlarmSet alarmSet)
        {
            var user = User.Identity.GetUserId();
            //  var findUser = db.Users.SingleOrDefault(x => x.Id == user);
            db.UserTimes.Add(new UserTime {UserId=user, AlarmTime = alarmSet.AlarmTime });
            db.SaveChanges();
            //alarm.PutTogether();
            return RedirectToAction("Index");
        }
        [HttpGet]
       public ActionResult SubjectLanguage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubjectLanguage(UserLanguageAnswers ul)
        {
            if(ul.Answer1.Length>=1&&ul.Answer2.Length>=1&&ul.Answer3.Length>=1&&ul.Answer4.Length>=1)
            {
                bool answered = true;
                a.StopAlarm(answered);
            }
            return View();
        }

        
        public ActionResult SubjectMath()
        {
            return View();
        }
        Alarm a = new Alarm();
        [HttpPost]
        public ActionResult SubjectMath(UserMathAnswers um)
        {
            if(um.Answer.HasValue==true&&um.Answer2.HasValue==true&&um.Answer3.HasValue==true&&um.Answer4.HasValue==true)
            {
                bool answered = true;
                a.StopAlarm(answered);
            }
            else
            {
                ViewBag.Error = true;
            }
            return View();
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