using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProjectDbConn;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CompuskillsMvcProject.Controllers
{
    public class HomeController : Controller
    {
        private BizAssistContext db = new BizAssistContext();
       public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity.GetUserId();

                var employee = db.Employees.Find(user);
                if (employee.Familiar == true)
                {
                    return RedirectToAction("GetStarted");
                }
            }
                return View();      
        }
        public ActionResult GetStarted()
        {
            var user = User.Identity.GetUserId();
            var Employee = db.Employees.Find(user);
            
            if (User.Identity.IsAuthenticated==false||Employee.EmailConfirmed==false)
            {
                return RedirectToAction("Login", "Account");
            }
        
            var partOfComp = db.CompanyEmployees.Any(x => x.EmployeeId == user);
            if (partOfComp == false)
            {
                return RedirectToAction("AddCompany", "Companies");
            }
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var Company = db.Companies.SingleOrDefault(x => x.CompanyId == company.CompanyId);
            var dateJoined = Company.DateJoined;
            if(User.IsInRole("Ceo") || User.IsInRole("Senior Managment"))
            { 
         
            
            if (db.ClientBills.Any(x =>x.CompanyId==Company.CompanyId &&x.DateBilled != DateTime.Today ))
            {
                var Billed = db.ClientBills.Any(x => x.CompanyId == company.CompanyId);

                if (Billed == false)
                {
                    switch (Company.ClientBillInterval)
                    {
                        case "daily":
                            if (DateTime.Today == dateJoined.AddDays(1))
                            {
                                return RedirectToAction("BillClients", "ClientBills", new { time = Company.ClientBillInterval });
                            }
                            break;
                        case "weekly":
                            if (DateTime.Today == dateJoined.AddDays(7))
                            {
                                return RedirectToAction("BillClients", "ClientBills", new { time = Company.ClientBillInterval });
                            }
                            break;
                        case "Every other week":
                            if (DateTime.Today == dateJoined.AddDays(14))
                            {
                                return RedirectToAction("BillClients", "ClientBills", new { time = Company.ClientBillInterval });
                            }
                            break;
                        case "monthly":
                            if (DateTime.Today == dateJoined.AddMonths(1))
                            {
                                return RedirectToAction("BillClients", "ClientBills", new { time = Company.ClientBillInterval });
                            }
                            break;
                    }

                }
                else
                {
                    var LastBill = db.ClientBills.Where(x => x.CompanyId == company.CompanyId).OrderByDescending(x => x.DateBilled).FirstOrDefault();
                    switch (Company.ClientBillInterval)
                    {
                        case "daily":
                            if (DateTime.Today == LastBill.DateBilled.AddDays(1))
                            {
                                return RedirectToAction("BillClients", "ClientBills", new { time = Company.ClientBillInterval });
                            }
                            break;
                        case "weekly":
                            if (DateTime.Today == LastBill.DateBilled.AddDays(7))
                            {
                                return RedirectToAction("BillClients", "ClientBills", new { time = Company.ClientBillInterval });
                            }
                            break;
                        case "Every other week":
                            if (DateTime.Today == LastBill.DateBilled.AddDays(14))
                            {
                                return RedirectToAction("BillClients", "ClientBills", new { time = Company.ClientBillInterval });
                            }
                            break;
                        case "monthly":
                            if (DateTime.Today == LastBill.DateBilled.AddMonths(1))
                            {
                                return RedirectToAction("BillClients", "ClientBills", new { time = Company.ClientBillInterval });
                            }
                            break;
                    }
                }

            }

                if (db.EmployeePayStubs.All(x => x.PayDay != DateTime.Today))
                {
                    var Payed = db.EmployeePayStubs.Any(x => x.CompanyId == company.CompanyId && x.PayDay != null);
                    if (Payed == false)
                    {
                        //   var dayAhead = dateJoined.AddDays(1);
                        switch (Company.EmployeePayInterval)
                        {
                            case "daily":
                                if (DateTime.Today == dateJoined.AddDays(1))
                                {
                                    return RedirectToAction("EmployeePay", "EmployeePayStubs", new { time = Company.EmployeePayInterval });
                                }
                                break;
                            case "weekly":
                                if (DateTime.Today == dateJoined.AddDays(7))
                                {
                                    return RedirectToAction("EmployeePay", "EmployeePayStubs", new { time = Company.EmployeePayInterval });
                                }
                                break;
                            case "Every other week":
                                if (DateTime.Today == dateJoined.AddDays(14))
                                {
                                    return RedirectToAction("EmployeePay", "EmployeePayStubs", new { time = Company.EmployeePayInterval });
                                }
                                break;
                            case "monthly":
                                if (DateTime.Today == dateJoined.AddMonths(1))
                                {
                                    return RedirectToAction("EmployeePay", "EmployeePayStubs", new { time = Company.EmployeePayInterval });
                                }
                                break;
                        }
                    }
                    else
                    {
                        var LastPayDay = db.EmployeePayStubs.Where(x => x.CompanyId == company.CompanyId).OrderByDescending(x => x.PayDay).FirstOrDefault();
                        switch (Company.EmployeePayInterval)
                        {
                            case "daily":
                                if (DateTime.Today == LastPayDay.PayDay.AddDays(1))
                                {
                                    return RedirectToAction("EmployeePay", "EmployeePayStubs", new { time = Company.EmployeePayInterval });
                                }
                                break;
                            case "weekly":
                                if (DateTime.Today == LastPayDay.PayDay.AddDays(7))
                                {
                                    return RedirectToAction("EmployeePay", "EmployeePayStubs", new { time = Company.EmployeePayInterval });
                                }
                                break;
                            case "Every other week":
                                if (DateTime.Today == LastPayDay.PayDay.AddDays(14))
                                {
                                    return RedirectToAction("EmployeePay", "EmployeePayStubs", new { time = Company.EmployeePayInterval });
                                }
                                break;
                            case "monthly":
                                if (DateTime.Today == LastPayDay.PayDay.AddMonths(1))
                                {
                                    return RedirectToAction("EmployeePay", "EmployeePayStubs", new { time = Company.EmployeePayInterval });
                                }
                                break;
                        }
                    }
                }

            }
            var currentUser = db.Employees.Find(user);
            if (currentUser.Familiar==false && User.IsInRole("Ceo")|| User.IsInRole("Senior Managment")||User.IsInRole("Human resources department"))
            {
                return View();
            }
            return RedirectToAction("Redirect");
        }
            //
            public ActionResult Redirect()
        {
            var user = User.Identity.GetUserId();
            var partOfCompany = db.CompanyEmployees.Any(x => x.EmployeeId == user);
            if (User.Identity.IsAuthenticated && partOfCompany==true)
            {
                if (User.IsInRole("Ceo"))
                {
                    return RedirectToAction("Dashboard");
                }
                else
                {
                return RedirectToAction("EmployeeDashboard", "WorkSchedules");
                }
            }
            else if(User.Identity.IsAuthenticated && partOfCompany==false)
            {
                return RedirectToAction("CompanySelect");
            }
         
            else
            {
                return RedirectToAction("Login","Account");
            }

        }
        public ActionResult HelpAbout()
        {
            return View();
        }
       
        [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
        public ActionResult Dashboard()
        {
                    return View();
        }
        [Authorize(Roles = "Ceo,Senior Managment,Finance department")]
        public ActionResult FinanceDashboard()
        {
            //Debug.WriteLine("Version: " + System.Environment.Version.ToString());
            return View();
        }

        public ActionResult CompanySelect()
        {
            return View();
        }
        public ActionResult SetToFamiliar()
        {
            var user = User.Identity.GetUserId();
            var employee = db.Employees.Find(user);
            employee.Familiar = true;
            db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Redirect");
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