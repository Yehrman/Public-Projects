using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MvcProjectDbConn;

namespace CompuskillsMvcProject.Controllers
{
    public class WorkSchedulesController : Controller
    {
        private BizAssistContext db = new BizAssistContext();

        // GET: WorkSchedules1
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult Index(DateTime? from=null,DateTime ? to=null)
        {
            from = from ?? DateTime.Today.AddDays(-1);
            to = to ?? DateTime.UtcNow;
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var workScheudules = db.WorkScheudules.Include("Client").Include("Company").Include("Employee").Include("Project").Where(x => x.CompanyId == company.CompanyId  && x.Date>=from && x.Date <=to).OrderByDescending(x => x.Date);
            return View(workScheudules);
        }
        public ActionResult EmployeeDashboard()
        {
            var CurrentUser = User.Identity.GetUserId();
            var Company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == CurrentUser);
            if (User.Identity.IsAuthenticated)
            {
                var ToDo = db.WorkScheudules.Include("Client").Include("Project").Where(x => x.EmployeeId == CurrentUser && x.Date == DateTime.Today && x.CompanyId == Company.CompanyId).OrderByDescending(x => x.Date);
                return View(ToDo);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpGet]
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult Search()
        {
            return PartialView();
        }
        [HttpPost]
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult Search(DateTime? from=null, DateTime ? to=null)
        {
            return RedirectToAction("Index", new {  from, to });
        }
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult TodaysJobs()
        {
            var CurrentUser = User.Identity.GetUserId();
            var Company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == CurrentUser);
            var ScheduledWork = db.WorkScheudules.Include("Client").Include("Company").Include("Employee").Include("Project").Where(x => x.CompanyId == Company.CompanyId && x.Date == DateTime.Today);
            return View(ScheduledWork);
        }
        // GET: WorkSchedules1/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.idError = id == null;
            WorkSchedule workSchedule = db.WorkScheudules.Find(id);
            ViewBag.schedule = workSchedule == null;
            return View(workSchedule);
        }

        // GET: WorkSchedules1/Create
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult Create()
        {
            //Hashem will help Hashem helped
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            ViewBag.EmployeeId = new SelectList(db.CompanyEmployees.Include("Employee").Where(x=>x.CompanyId==company.CompanyId ), "EmployeeId", "Employee.FullName");
            ViewBag.ProjectId = new SelectList(db.Projects.Where(x=>x.CompanyId==company.CompanyId), "ProjectId", "ProjectName");
            return View();
        }

        // POST: WorkSchedules1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult Create([Bind(Include = "EmployeeId,ProjectId,Date,Time,IsScheduledPunchOut,EndTime")] WorkSchedule workSchedule)
        {
                DateTime time = DateTime.Parse(workSchedule.Time);
                    var user = User.Identity.GetUserId();
                    var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            if (db.TimeSheetEntries.Any(x => x.EmployeeId == workSchedule.EmployeeId && x.StartTime < time && x.EndTime > time && x.EndTime != null))
            {
                ModelState.AddModelError("Mixup", "You can't schedule a job for a time that you are punched in");
            }
            else if (workSchedule.EndTime > 180)
            {
                ViewBag.ToHigh = true;
            }
            else if (workSchedule.IsScheduledPunchOut == true && workSchedule.EndTime == null)
            {
                ViewBag.nullEnd = true;
                    ViewBag.EmployeeId = new SelectList(db.CompanyEmployees.Include("Employee").Where(x => x.CompanyId == company.CompanyId), "Id", "Employee.FullName");
                    ViewBag.ProjectId = new SelectList(db.Projects.Where(x => x.CompanyId == company.CompanyId), "ProjectId", "ProjectName");
                    return View(workSchedule);
            }
            
            if (ModelState.IsValid)
            {
                if(workSchedule.IsScheduledPunchOut==false)
                {
                    workSchedule.EndTime = null;
                    workSchedule.EmployeeId = user;
                }

                 var project=  db.Projects.FirstOrDefault(x => x.ProjectId == workSchedule.ProjectId);             
                 workSchedule.CompanyId = project.CompanyId;
            
                    db.WorkScheudules.Add(workSchedule);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            
          //  var user = User.Identity.GetUserId();
            //var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            // ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", workSchedule.ClientId);
            ViewBag.EmployeeId = new SelectList(db.CompanyEmployees.Include("Employee").Where(x => x.CompanyId == company.CompanyId), "Id", "Employee.FullName");
            ViewBag.ProjectId = new SelectList(db.Projects.Where(x => x.CompanyId == company.CompanyId), "ProjectId", "ProjectName");
            return View(workSchedule);
        }
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult Edit(int? id)
        {
            ViewBag.idError = id == null;
            WorkSchedule workSchedule = db.WorkScheudules.Find(id);
            ViewBag.schedule = workSchedule == null;
           
            ViewBag.EmployeeId = new SelectList(db.CompanyEmployees.Include("Employee").Where(x => x.CompanyId == workSchedule.CompanyId), "EmployeeId", "Employee.FullName");
            return View(workSchedule);
        }
        //Need to work on edit
        // POST: WorkSchedules1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult Edit( WorkSchedule workSchedule)
        {//project cpmpany client id's
                  
            if (ModelState.IsValid)
            {                
                db.Entry(workSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.CompanyEmployees.Include("Employee").Where(x => x.CompanyId == workSchedule.CompanyId), "EmployeeId", "Employee.FullName");
            return View(workSchedule);
        }

        // GET: WorkSchedules1/Delete/5
      [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult Delete(int? id)
        {
        
            WorkSchedule workSchedule = db.WorkScheudules.Find(id);
            ViewBag.error = id == null;
            ViewBag.scheduleError =workSchedule == null;
            return View(workSchedule);
        }
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        // POST: WorkSchedules1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkSchedule workSchedule = db.WorkScheudules.Find(id);
            db.WorkScheudules.Remove(workSchedule);
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
