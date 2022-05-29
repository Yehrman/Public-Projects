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
    [Authorize(Roles = "Ceo,Senior Managment,Human resources department")]
    public class ProjectsController : Controller
    {
        private BizAssistContext db = new BizAssistContext();
       public ActionResult Index(string name="")
        {
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var projects = db.Projects.Where(x => x.CompanyId == company.CompanyId &&x.ProjectName.Contains(name) && x.IsDeleted==false || x.IsDeleted==null);
            return View(projects);
        }
        public ActionResult ClientProjectIndex(string name="")
        {
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var projects = db.ClientProjects.Include("Project").Include("Client").Where(x => x.Project.CompanyId == company.CompanyId && x.Project.ProjectName.Contains(name) &&x.Project.IsDeleted==false);
            return View(projects);
        }
        [HttpGet]
        public ActionResult Search()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Search(string name="")
        {
            return RedirectToAction("Index", new { name });
        }
        [HttpGet]
        public ActionResult ClientProjectSearch()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ClientProjectSearch(string name="")
        {
            return RedirectToAction("ClientProjectIndex", new { name });
        }
        public ActionResult CreateSelect()
        {
            return View();
        }
        public ActionResult Details(int? id)
        {
            ViewBag.error = id == null;
            Project project = db.Projects.Find(id);
            ViewBag.Project = project == null;
            return View(project);
        }
        public ActionResult ClientDetails(int? id)
        {
            ViewBag.error = id == null;
            ClientProject clientProject = db.ClientProjects.Include("Client").FirstOrDefault(x => x.ProjectId == id);
            ViewBag.Project = clientProject == null;
            return View(clientProject);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Project project)
        {
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            db.Projects.Add(new Project { CompanyId = company.CompanyId, ProjectName = project.ProjectName, IsCompleted = false, IsDeleted = false,IsForClient=false });
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult CreateProjectForClient(int ? id)
        {
            var user = User.Identity.GetUserId();
            if (id == null)
            {
                ViewBag.error = id == null;
                return View();
            }
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var client = db.Clients.SingleOrDefault(x => x.CompanyId == company.CompanyId && x.ClientId == id);
            CreateProjectForClientModel model = new CreateProjectForClientModel();
            var isClient = db.Clients.Any(x => x.ClientId == id);
            if (isClient==false)
            {
                ViewBag.project = true;
                return View();
            }
                model.ClientId = client.ClientId;
                return View(model);          
        }

        
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProjectForClient(CreateProjectForClientModel project)
        {
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            if (db.Clients.Any(x => x.ClientId == project.ClientId && x.BillByProject==true) && project.BillRate==null)
            {
                ModelState.AddModelError("bill", "You opted to bill on a per project basis.So you must fill out the bill rate.");
                return View(project);
            }
            if (ModelState.IsValid)
            {
                var newProj = db.Projects.Add(new Project { CompanyId = company.CompanyId, ProjectName = project.ProjectName, IsCompleted = false, IsDeleted = false,IsForClient=true });
                db.ClientProjects.Add(new ClientProject { ClientId = project.ClientId, ProjectId = newProj.ProjectId, BillRate = project.BillRate });
                db.SaveChanges();
                return RedirectToAction("ClientProjectIndex");
            }
            return View(project);
        }
        [HttpGet]
        public ActionResult Edit(int ? id)
        {
            ViewBag.error = id == null;
            Project project = db.Projects.Find(id);
            ViewBag.project = project == null;
            return View(project);
        }
        public ActionResult Edit(Project project)
        {
            var editedProject = db.Projects.Find(project.ProjectId);
          
                editedProject.ProjectName = project.ProjectName;
                editedProject.IsCompleted = project.IsCompleted;
            if (editedProject.IsCompleted == true)
            {
                editedProject.DateCompleted = DateTime.Now;
            }
                db.Entry(editedProject).State = EntityState.Modified;
                db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Projects/Edit/5
        public ActionResult ChangeProjectBillRate(int? id)
        {

            ViewBag.error = id == null;
            ClientProject project = db.ClientProjects.Find(id);
            ViewBag.project = project == null;
          
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProjectBillRate(ClientProject project)
        {
            var editedProj = db.ClientProjects.SingleOrDefault(x => x.id == project.id);
            if (ModelState.IsValid)
            {
                editedProj.BillRate = project.BillRate;          
                db.Entry(editedProj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ClientProjectIndex");
            }
      
            return View(project);
        }
        public ActionResult ConcludeProject(int id)
        {
            var project = db.Projects.Find(id);
            project.IsCompleted = true;
            project.DateCompleted = DateTime.Today;
            db.Entry(project).State = EntityState.Modified;
            db.SaveChanges();
            var user = User.Identity.GetUserId();
            var employee = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
            var company = db.Companies.SingleOrDefault(x => x.CompanyId == employee.CompanyId);
            if (company.EmployeePayInterval == "On a per project basis" && company.ClientBillInterval!="On a per project basis")
            {
                return RedirectToAction("EmployeePay", "EmployeePayStubs" ,new {time= "On a per project basis",id=id });
            }
            else if(company.ClientBillInterval == "On a per project basis")
            {
                return RedirectToAction("BillClients", "ClientBills", new { time = "On a per project basis",id=id });
            }
        
            return RedirectToAction("Index");
        }
      

        // GET: Projects/Delete/5
        //Working on project delete
        public ActionResult Delete(int? id)
        {
      
            Project project = db.Projects.Find(id);
            ViewBag.error = id == null;
            ViewBag.project = project == null;
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            Project project = db.Projects.SingleOrDefault(x => x.CompanyId == company.CompanyId && x.ProjectId == id);
            project.IsDeleted = true;
            db.Entry(project).State = EntityState.Modified;
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
