using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CompuskillsMvcProject.Models;
using Microsoft.AspNet.Identity;
using MvcProjectDbConn;

namespace CompuskillsMvcProject.Controllers
{
    [Authorize(Roles = "Ceo,Senior Managment,Finance Department")]
    public class EmployeePayStubsController : Controller
    {
        private BizAssistContext db = new BizAssistContext();

        // GET: EmployeePayStubs
        public ActionResult Index(DateTime? from = null,DateTime? to=null )
        {
            from = from ?? DateTime.Now.AddMonths(-1);
            to = to ?? DateTime.Now;
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var employeePayStubs = db.EmployeePayStubs.Include(e => e.Company).Include(e => e.Employee).Where(x => x.CompanyId == company.CompanyId && x.PayDay>=from && x.PayDay<=to   );
            return View(employeePayStubs);
        }
        //manual payEmployee needs work
        public ActionResult PayEmployees()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Search()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Search(DateTime? from=null,DateTime? to=null)
        {
            return RedirectToAction("Index", new {  from, to });
        }
        // GET: EmployeePayStubs/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.error = id == null;
            EmployeePayStub employeePayStub = db.EmployeePayStubs.Find(id);
            ViewBag.employee = employeePayStub == null;
            return View(employeePayStub);
        }
        Payment payment = new Payment();
        // GET: EmployeePayStubs/Create
        Dictionary<string, TimeSpan> TimeWorked(string time,int projectId=0)
        {
            Dictionary<string, TimeSpan> timeWorked = new Dictionary<string, TimeSpan>();
            var user = User.Identity.GetUserId();
            if (time == "On a per project basis")
            {
                timeWorked = payment.EmployeeProjectEntries(projectId, user);
            }
            else
            {
                timeWorked = payment.EntriesForEmployees(user, time);
            }
            return timeWorked;
        }
       
        public ActionResult EmployeePay(string time,int id=0)
        {
            var Total = TimeWorked(time,id);
            foreach (var item in Total)
            {
                var s = Convert.ToString(item.Value);
                var Hours = Convert.ToDecimal(TimeSpan.Parse(s).Hours);
                var Minutes = Convert.ToDecimal(TimeSpan.Parse(s).Minutes);
                var Seconds = Convert.ToDecimal(TimeSpan.Parse(s).Seconds);
                var findUser = User.Identity.GetUserId();
                var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == findUser);
                var rate = db.CompanyEmployees.SingleOrDefault(x => x.CompanyId == company.CompanyId && x.EmployeeId==item.Key);
                var payPerHour = rate.PayRate * Hours;
                var perMinute = rate.PayRate / 60;
                var payPerMinute = perMinute * Minutes;
                var perSecond = rate.PayRate / 3600;
                var payPerSecond = perSecond * Seconds;
                decimal TotalBill = payPerHour + payPerMinute + payPerSecond;
              
                if (TotalBill > 0)
                {
                    db.EmployeePayStubs.Add(new EmployeePayStub { CompanyId = company.CompanyId, EmployeeId=item.Key,PayCheck=TotalBill,PayDay=DateTime.Today });
                }
            }
            db.SaveChanges();
            var user = User.Identity.GetUserId();
            var employee = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
            var bill = db.ClientBills.Any(x => x.CompanyId == employee.CompanyId && x.DateBilled == DateTime.Today);
            if(bill==true)
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        //// GET: EmployeePayStubs/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    ViewBag.error = id == null;
        //    EmployeePayStub employeePayStub = db.EmployeePayStubs.Find(id);
        //    ViewBag.employee = employeePayStub == null;
        //    var user = User.Identity.GetUserId();
        //    var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
        //    ViewBag.EmployeeId = new SelectList(db.EmployeePayStubs.Include("Employee").Where(x=>x.CompanyId==company.CompanyId), "EmployeeId", "Employee.FullName");
        //    return View(employeePayStub);
        //}

        //// POST: EmployeePayStubs/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit( EmployeePayStub employeePayStub)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        employeePayStub.PayDay = DateTime.Today;
        //        db.Entry(employeePayStub).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    var user = User.Identity.GetUserId();
        //    var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
        //    ViewBag.EmployeeId = new SelectList(db.EmployeePayStubs.Include("Employee").Where(x => x.CompanyId == company.CompanyId), "EmployeeId", "Employee.FullName");
        //    return View(employeePayStub);
        //}

      
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
