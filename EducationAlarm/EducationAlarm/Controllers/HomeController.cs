
using Microsoft.AspNet.Identity;
using System.Web.Mvc;


namespace EducationAlarm.Controllers
{
    public class HomeController : Controller
    {
      //  Alarm alarm = new Alarm();
        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "alarm");
            }
            return View();
           // var user = User.Identity.GetUserId();
           // if (user == null)
           // {
           //     return RedirectToAction("Login", "Account");
           // }
           //// alarm.ActivateAlarms(user);
           // return  RedirectToAction("index","Alarm"); 
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