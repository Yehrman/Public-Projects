using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CompuskillsMvcProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcProjectDbConn;

namespace CompuskillsMvcProject.Controllers
{
    public class CompaniesController : Controller
    {
        
        private BizAssistContext db = new BizAssistContext();
        
        const string EmployeeId = "35FB245B-FDE0-464D-9985-CFDD0F2AEB18";
        const string CeoId = "602514B5-413F-419D-97A1-2533130D8DC4";

                [HttpGet]
        public ActionResult AddCompany()
        {
            CompanyViewModel model = new CompanyViewModel();
            var intervals = model.Interval();
            ViewBag.ClientBillInterval = new SelectList(intervals);
            ViewBag.EmployeePayInterval = new SelectList(intervals);
            var zones = model.TimeZones();
            ViewBag.TimeZone = new SelectList(zones);
            return View();
            
        }
        [HttpPost]
        public ActionResult AddCompany(CompanyViewModel company)
        {           
            var user = User.Identity.GetUserId();
            Guid guid = Guid.NewGuid();
            var Employee = db.Employees.SingleOrDefault(x => x.Id == user);
            var password = Employee.PasswordHash;
            //  company.CompanyId = guid;
            if (ModelState.IsValid)
            {
                db.Companies.Add(new Company { CompanyId = guid, CompanyName = company.CompanyName, CompanyPhoneNumber = company.PhoneNumber, Email = company.Email,  ClientBillInterval = company.ClientBillInterval, EmployeePayInterval = company.EmployeePayInterval, TimeZone = company.TimeZone, DateJoined = DateTime.Today });
                var newCompany = db.Companies.SingleOrDefault(x => x.CompanyId == guid);
                var employee = db.Employees.SingleOrDefault(x => x.Id == user);
                db.CompanyEmployees.Add(new CompanyEmployee { EmployeeId = user, CompanyId = guid, RoleId = CeoId, Name = employee.FirstName + " " + employee.LastName });
                db.Permissions.Add(new IdentityUserRole { RoleId = CeoId, UserId = user });
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

       
      
                CompanyViewModel model = new CompanyViewModel();
                var intervals = model.Interval();
                ViewBag.ClientBillInterval = new SelectList(intervals);
                ViewBag.EmployeePayInterval = new SelectList(intervals);
                var zones = model.TimeZones();
                ViewBag.TimeZone = new SelectList(zones);
            
                return View();
            
        }
        public ActionResult AddEmployee()
        {
            var user = User.Identity.GetUserId();
            var compEmployee = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
            var company = db.Companies.FirstOrDefault(x => x.CompanyId == compEmployee.CompanyId);
            Employee emp = (Employee)TempData["Emp"];
           var newEmp=     db.CompanyEmployees.Add(new CompanyEmployee { CompanyId = company.CompanyId, EmployeeId = emp.Id, Name = emp.FirstName + " " + emp.LastName, RoleId = EmployeeId });
                db.Permissions.Add(new IdentityUserRole { UserId = emp.Id, RoleId = EmployeeId });
                db.SaveChanges();
            var newId = newEmp.id;
                return RedirectToAction("SetPayRate", "CompanyEmployees",new { id=newId});
            
        }

        [Authorize(Roles = "Ceo")]
        public ActionResult Details()
        {        
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var Company = db.Companies.Find(company.CompanyId);
            return View(Company);
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return PartialView();
        }

        [Authorize(Roles ="Ceo")]
         [HttpGet]
   public ActionResult Edit()
        {
            CompanyViewModel model = new CompanyViewModel();
            var intervals = model.Interval();
            ViewBag.ClientBillInterval = new SelectList(intervals);
            ViewBag.EmployeePayInterval = new SelectList(intervals);
            var zones = model.TimeZones();
            ViewBag.TimeZone = new SelectList(zones);
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var Company = db.Companies.Find(company.CompanyId);
            return View(Company);
        }
        [HttpPost]
        [Authorize(Roles = "Ceo")]
        public ActionResult Edit(Company company)
        {
            var Company = db.Companies.Find(company.CompanyId);
            Company.Address = company.Address;
            Company.ClientBillInterval = company.ClientBillInterval;
            Company.CompanyName = company.CompanyName;
            Company.CompanyPhoneNumber = company.CompanyPhoneNumber;
            Company.Email = company.Email;
            Company.EmployeePayInterval = company.EmployeePayInterval;
            Company.TimeZone = company.TimeZone;
            db.Entry(Company).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details");
        }
        [Authorize(Roles = "Ceo")]
        [HttpGet]
        public ActionResult Delete()
        {
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var Company = db.Companies.SingleOrDefault(x => x.CompanyId == company.CompanyId);
        
            return View(Company);
        }
        [HttpPost]
        [Authorize(Roles = "Ceo")]
        public ActionResult Delete(Company orginaztion)
        {
            try
            {
                var user = User.Identity.GetUserId();
                var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
                var Company = db.Companies.SingleOrDefault(x => x.CompanyId == company.CompanyId);
                if ( Company.Email == orginaztion.Email)
                {
                    var AmountOfEmployees = db.CompanyEmployees.Include("Employee").Where(x => x.CompanyId == Company.CompanyId).Count();
                    var Employees = db.CompanyEmployees.Include("Employee").Where(x => x.CompanyId == Company.CompanyId);
                    foreach (var item in Employees)
                    {
                        var Identity = db.Employees.Find(item.EmployeeId);
                        db.CompanyEmployees.Remove(item);
                        db.Employees.Remove(Identity);
                    }


                    db.Companies.Remove(Company);
                    db.DeletedCompanies.Add(new DeletedCompany { CompanyName = Company.CompanyName, AmountOfEmployees = AmountOfEmployees, DateJoined = Company.DateJoined, DateLeft = DateTime.Today });
                    db.SaveChanges();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("delete", "The delete didn't work.Check that you have the correct email and password and try again.");
                    return View();
                }
            }
            catch
            {
                return View(orginaztion);
            }
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
