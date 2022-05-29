using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcProjectDbConn;
using Microsoft.AspNet.Identity;
using CompuskillsMvcProject.Models;

namespace CompuskillsMvcProject.Controllers
{
    [Authorize(Roles ="Ceo, Senior Managment,Finance Department")]
    public class ClientBillsController : Controller
    {
        private BizAssistContext db = new BizAssistContext();

        // GET: ClientBills
        public ActionResult Index( DateTime? from=null,DateTime? to=null, string name = "")
        {
            from = from ?? DateTime.Now.AddMonths(-1);
            to = to ?? DateTime.Now;
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            //The or code might be a problem here
            var clientBills = db.ClientBills.Include(c => c.Client).Include(c => c.Company).Where(x => x.CompanyId == company.CompanyId&&x.Client.FirstName.Contains(name)|| x.Client.LastName.Contains(name) && x.DateBilled >= from && x.DateBilled <= to).OrderByDescending(x => x.DateBilled);
                return View(clientBills);              
         }
        public ActionResult Bill()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Search()
        {
            return PartialView();
        }
  [HttpPost]
        public ActionResult Search(DateTime? from,DateTime? to,string name="")
        {
            return RedirectToAction("Index", new { from, to, name });
        }
        // GET: ClientBills/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.error = id == null;
     
            ClientBill clientBill = db.ClientBills.Find(id);
            ViewBag.ClientError = clientBill == null;
            return View(clientBill);
        }
        Payment payment = new Payment(); 
         Dictionary<int, TimeSpan> TimeWorked(string time,int projectId=0)
        {
            Dictionary<int, TimeSpan> timeWorked = new Dictionary<int, TimeSpan>();
            var user = User.Identity.GetUserId();
            if (time == "On a per project basis")
            {
                timeWorked = payment.BillProjectEntries(projectId, user);
            }
            else
            {
                timeWorked = payment.EntriesForClients(user, time);
            }
            return timeWorked;
        }

        public ActionResult BillClients(string time,int id=0)
        {            
            var Total = TimeWorked(time,id);
   
                var findUser = User.Identity.GetUserId();
                var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == findUser);
            foreach (var item in Total)
            {
                var s = Convert.ToString(item.Value);
                var Hours = Convert.ToDecimal(TimeSpan.Parse(s).Hours);
                var Minutes = Convert.ToDecimal(TimeSpan.Parse(s).Minutes);
                var Seconds = Convert.ToDecimal(TimeSpan.Parse(s).Seconds);
                var project = db.ClientProjects.Include("Client").FirstOrDefault(x => x.Client.CompanyId == company.CompanyId && x.ProjectId == item.Key);
                var client = db.Clients.FirstOrDefault(x=>x.ClientId==project.ClientId && x.CompanyId==company.CompanyId);
                decimal? payPerHour = 0m;
                if(client.BillByClient==true)
                {
                    payPerHour = client.BillRate;
                }
                if(client.BillByProject==true)
                {
                    payPerHour = project.BillRate;
                }
               var TotalForHours = payPerHour * Hours;
                var perMinute = payPerHour / 60;
                var TotalForMinutes = perMinute * Minutes;
                var perSecond = payPerHour / 3600;
                var payPerSecond = perSecond * Seconds;
                var Bill = TotalForHours + TotalForMinutes + payPerSecond;
                var TotalBill = Bill.GetValueOrDefault();
                if (TotalBill > 0)
                {
                    db.ClientBills.Add(new ClientBill { CompanyId = company.CompanyId, ClientId = client.ClientId, Bill = TotalBill, DateBilled = DateTime.Today,TimeWorked=item.Value,ProjectId=project.ProjectId });
                }
            }
            db.SaveChanges();
            var Company = db.Companies.Find(company.CompanyId);
            if (Company.ClientBillInterval == Company.EmployeePayInterval)
            {
                return RedirectToAction("EmployeePay", "EmployeePayStubs", new { time = time, id = id });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

  
        public ActionResult PaymentRecieved( int id)
        {
            var clientBill = db.ClientBills.Find(id);
            clientBill.DatePaymentRecieved = DateTime.Today;
                db.Entry(clientBill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                       
     
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
