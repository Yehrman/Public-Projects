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
    public class ChatsController : Controller
    {
        private BizAssistContext db = new BizAssistContext();

        // GET: Chats
 


        public ActionResult Index()
        {
            Delete();
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
    
            var chats = db.Chats.Include("Employee").Where(x => x.SenderId == user || x.RecieverId == user|| x.RecieverId==null && x.CompanyId == company.CompanyId);
            return View(chats);
        }
        public ActionResult Create()
        {
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user );
            ViewBag.RecieverId = new SelectList(db.CompanyEmployees.Include("Employee").Where(x => x.CompanyId == company.CompanyId &&x.EmployeeId!=user), "EmployeeId", "Employee.FullName");
            return PartialView ();
        }
        TimeZones zones = new TimeZones();

        // POST: Chats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecieverId,Message")] Chat chat)
        {
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
            if (ModelState.IsValid)
            {
              var time=  zones.ConvertToLocal(user, DateTime.UtcNow);
                chat.SenderName = company.Name;
                chat.SenderId = user;
                chat.CompanyId = company.CompanyId;
                chat.TimeChatSent = time;
                 db.Chats.Add(chat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
              ViewBag.RecieverId = new SelectList(db.CompanyEmployees.Include("Employee").Where(x => x.CompanyId == company.CompanyId), "EmployeeId", "Employee.FullName");
            return View(chat);
        }
        private void Delete()
        {
            var user = User.Identity.GetUserId();
            var company = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
            var daysAgo = DateTime.Today.AddDays(-3);
            var chats = db.Chats.Where(x => x.CompanyId == company.CompanyId &&x.TimeChatSent<daysAgo);
            foreach (var item in chats)
            {
                db.Chats.Remove(item);
            }
            db.SaveChanges();
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
