using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using EducationAlarm.Models;
using System.Threading.Tasks;

namespace EducationAlarm.Controllers
{
    public class HomeController : Controller
    {
        Alarm alarm = new Alarm();
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            alarm.ActivateAlarms(user);
            return  RedirectToAction("index","Alarm"); 
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}