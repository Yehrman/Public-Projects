using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcProjectDbConn;
namespace CompuskillsMvcProject.Models
{
    public class Payment 
    {
        private BizAssistContext db = new BizAssistContext();
        private TimeSpan? Interval { get; set; }
        private TimeSpan NonNull { get; set; }
        private TimeSpan TotalTimeWorked { get; set; }
        private IQueryable<TimeSheetEntry> Entries { get; set; } 
        private DateTime BillPayInterval(string time)
        {
            DateTime date = DateTime.Today;
            switch (time)
            {
                case "daily":
                    date = date.AddDays(-1);
                    break;
                case "weekly":
                    date = date.AddDays(-7);
                    break;
                case "Every other week":
                    date = date.AddDays(-14);
                    break;
                case "Monthly":
                    date = date.AddMonths(-1);
                    break;
            }
            return date;
        }
        private List<ClientProject> Projects(string user)
        {
           var employee = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
           var projects= db.ClientProjects.Include("Project").Where(x => x.Project.CompanyId == employee.CompanyId);
           return projects.ToList();
        }
      
        public Dictionary<int,TimeSpan> EntriesForClients(string user,string time)
        {
            Dictionary<int, TimeSpan> timeWorked = new Dictionary<int, TimeSpan>();
            var interval = BillPayInterval(time);
            var employee = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var clientBilled = db.ClientBills.Any(x => x.CompanyId == employee.CompanyId);
            var projects = Projects(user);
            foreach (var project in projects)
            {

                if (clientBilled == false)
                {
                    Entries = db.TimeSheetEntries.Where(x => x.CompanyId == employee.CompanyId && x.ProjectId == project.ProjectId && x.StartTime != null && x.EndTime != null);
                }
                else
                {
                    Entries = db.TimeSheetEntries.Where(x => x.CompanyId == employee.CompanyId && x.ProjectId == project.ProjectId && x.StartTime >= interval && x.EndTime != null);
                }
                foreach (var item in Entries)
                {
                    Interval = item.EndTime - item.StartTime;
                    if (Interval.HasValue)
                    {
                        NonNull = Interval.Value;
                        Interval = NonNull;
                    }
                    TotalTimeWorked += NonNull;
                    NonNull = TimeSpan.Zero;
                    Interval = TimeSpan.Zero;
                }
                 timeWorked.Add(project.ProjectId, TotalTimeWorked);             
                TotalTimeWorked = TimeSpan.Zero;
            }
            return timeWorked;
           }
        private List<CompanyEmployee> CompanyEmployees(string user)
        {
            var employee = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var employees = db.CompanyEmployees.Where(x => x.CompanyId == employee.CompanyId).ToList();
            return employees;
        }
       public Dictionary<string,TimeSpan> EntriesForEmployees(string user ,string time)
        {
            Dictionary<string, TimeSpan> timeWorked = new Dictionary<string, TimeSpan>();
            var interval = BillPayInterval(time);
           var employee = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var employees = CompanyEmployees(user);
            var EmployeesPaid = db.EmployeePayStubs.Any(x => x.CompanyId == employee.CompanyId);
            foreach (var item in employees)
            {
            if(EmployeesPaid==false)
            {
                Entries = db.TimeSheetEntries.Where(x => x.CompanyId == employee.CompanyId && x.EmployeeId==item.EmployeeId &&x.EndTime!=null);
            }
                else
                {
                    Entries = db.TimeSheetEntries.Where(x => x.CompanyId == employee.CompanyId && x.EmployeeId == item.EmployeeId && x.StartTime >= interval & x.EndTime != null);
                }
                foreach (var entry in Entries)
                {
                    Interval = entry.EndTime - entry.StartTime;
                    if (Interval.HasValue)
                    {
                        NonNull = Interval.Value;
                        Interval = NonNull;
                    }
                    TotalTimeWorked += NonNull;
                    NonNull = TimeSpan.Zero;
                    Interval = TimeSpan.Zero;
                }

                timeWorked.Add(item.EmployeeId, TotalTimeWorked);
                TotalTimeWorked = TimeSpan.Zero;
            }
            return timeWorked;
        }
        public Dictionary<string,TimeSpan> EmployeeProjectEntries(int id,string user)
        {
            var company = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
            var employees = CompanyEmployees(user);
            Dictionary<string, TimeSpan> projectEntries = new Dictionary<string, TimeSpan>();
            foreach (var employee in employees)
            {
            var entries = db.TimeSheetEntries.Where(x => x.CompanyId == company.CompanyId && x.ProjectId == id && x.EndTime != null &&x.EmployeeId==employee.EmployeeId);
            foreach (var item in entries)
            {
                Interval = item.EndTime - item.StartTime;
                if (Interval.HasValue)
                {
                    NonNull = Interval.Value;
                    Interval = NonNull;
                }
                TotalTimeWorked += NonNull;
                    NonNull = TimeSpan.Zero;
                    Interval = TimeSpan.Zero;
                }
                projectEntries.Add(employee.EmployeeId, TotalTimeWorked);
                TotalTimeWorked = TimeSpan.Zero;
            }
            return projectEntries;
        }
        public Dictionary<int,TimeSpan>BillProjectEntries(int id,string user)
        {
            var company = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
            Dictionary<int, TimeSpan> projectEntries = new Dictionary<int, TimeSpan>();
            var entries = db.TimeSheetEntries.Where(x => x.CompanyId == company.CompanyId && x.ProjectId == id && x.EndTime != null);
            foreach (var item in entries)
            {
                Interval = item.EndTime - item.StartTime;
                if (Interval.HasValue)
                {
                    NonNull = Interval.Value;
                    Interval = NonNull;
                }
                TotalTimeWorked += NonNull;
            }
            projectEntries.Add(id, TotalTimeWorked);
            return projectEntries;
        }
    }
 }
   
   