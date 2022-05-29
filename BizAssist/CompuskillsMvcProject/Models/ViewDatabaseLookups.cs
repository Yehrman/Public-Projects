using System;
using System.Collections.Generic;
using System.Linq;
using MvcProjectDbConn;
using Microsoft.AspNet.Identity;
using EncryptDecrypt;
namespace CompuskillsMvcProject.Models
{
    public class ViewDatabaseLookups
    {
        private BizAssistContext db = new BizAssistContext();
        private Company Companies(string user)
        {
            var employee = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var Company = db.Companies.Find(employee.CompanyId);
            return Company;
        }
        private bool EverBilled(string user)
        {
            bool DidBill = true;
            var company = Companies(user);
            bool Billed = db.ClientBills.Any(x => x.CompanyId == company.CompanyId);
            if (Billed == true)
            {
                DidBill = true;
            }
            else
            {
                DidBill = false;
            }
            return DidBill;
        }
        public DateTime DateBilled(string user)
        {
            var company = Companies(user);
            var Billed = EverBilled(user);
            var Interval = company.ClientBillInterval;
            DateTime date = new DateTime();
            if (Billed == true)
            {
                var DateBilled = db.ClientBills.Where(x => x.CompanyId == company.CompanyId).OrderByDescending(x => x.DateBilled).FirstOrDefault();
                date = DateBilled.DateBilled;
            }
            else
            {
                date = company.DateJoined;
            }
            return date;
        }
        private bool EverPayed(string user)
        {
            bool didPay = true;
            var company = Companies(user);
            var payed = db.EmployeePayStubs.Any(x => x.CompanyId == company.CompanyId);
            if (payed == true)
            {
                didPay = true;
            }
            else
            {
                didPay = false;
            }
            return didPay;
        }
        public DateTime DatePayed(string user)
        {
            var company = Companies(user);
            var payed = EverPayed(user);
            var interval = company.EmployeePayInterval;
            DateTime date = new DateTime();
            if (payed == true)
            {
                var datePayed = db.EmployeePayStubs.Where(x => x.CompanyId == company.CompanyId).OrderByDescending(x => x.PayDay).FirstOrDefault();
                date = datePayed.PayDay;
            }
            else
            {
                date = company.DateJoined;
            }
            return date;
        }
        public string BillPayMessage(string user, DateTime date)
        {
            string message = "";
            string typeOfPay = "";
            var interval = "";
            var company = Companies(user);
            if (date == DateBilled(user))
            {
                typeOfPay = "date to bill clients";
                interval = company.ClientBillInterval;
            }
            else if (date == DatePayed(user))
            {
                typeOfPay = "date to pay employees";
                interval = company.EmployeePayInterval;
            }

            switch (interval)
            {
                case "daily":
                    message = $"Your next {typeOfPay} is at {date.AddDays(1)}  ";
                    break;
                case "weekly":
                    message = $"Your next {typeOfPay} is at {date.AddDays(7)}  ";
                    break;
                case "Every other week":
                    message = $"Your next {typeOfPay} is at {date.AddDays(14)}  ";
                    break;
                case "monthly":
                    message = $"Your next {typeOfPay} is at {date.AddMonths(1)}  ";
                    break;
            }

            return message;
        }
        public bool IntervalEquality(string user)
        {
            bool intervalsAreEqual = false;
            var company = Companies(user);
            var billed = EverBilled(user);
            var DateBilled = db.ClientBills.Where(x => x.CompanyId == company.CompanyId).OrderByDescending(x => x.DateBilled).FirstOrDefault();
            if (company.ClientBillInterval == company.EmployeePayInterval && billed == true && DateBilled.DateBilled == DateTime.Today)
            {
                intervalsAreEqual = true;
            }
            else
            {
                intervalsAreEqual = false;
            }
            return intervalsAreEqual;
        }
        public List<string> Employees(string user)
        {
            var employee = db.CompanyEmployees.SingleOrDefault(x => x.EmployeeId == user);
            var employees = db.CompanyEmployees.Where(x => x.CompanyId == employee.CompanyId);


            List<string> role = new List<string>();

            foreach (var item in employees)
            {
                //needs work
                //Doesa'nt work to query identity tables.Opens a 2nd db connection.B'ezrat Hashem I'd like to solve this problem
                //  var userRole = db.Permissions.SingleOrDefault(x => x.UserId == item.EmployeeId);
                //we need to find all employees roles from this company

                switch (item.RoleId)
                {
                    case "D2EF7DA3-4C52-4B48-A141-D8E41CE8C7A9":
                        role.Add("Human resources department");
                        break;
                    case "602514B5-413F-419D-97A1-2533130D8DC4":
                        role.Add("Ceo");
                        break;
                    case "35FB245B-FDE0-464D-9985-CFDD0F2AEB18":
                        role.Add("Employee");
                        break;
                    case "DA5E99CF-E875-41DD-8300-9E0101D97D39":
                        role.Add("Finance Department");
                        break;
                    case "DA422AC8-7F2D-46AC-AE2C-C536FC98402F":
                        role.Add("Senior managment");
                        break;
                }
            }
            return role;
        }
        DataConnection connection = new DataConnection();
        public string ClientName(int id)
        {
            var client = db.Clients.Find(id);
            //Maybe we can have only one call to decrypt
           var first= connection.DecryptData(client.FirstName,client.DateAdded);
             var last=  connection.DecryptData(client.LastName,client.DateAdded);
            return first + " " + last;
        }
        public bool ClientBillType(int id)
        {
            var client = db.Clients.Find(id);
            bool type = true;
            if (client.BillByClient == true)
            {
                type = true;
            }
            if (client.BillByProject == true)
            {
                type = false;
            }
            return type;
        }
        public List<string> ProjectsThatAreBilledOnAPerProjectBasis(string user)
        {
            List<string> projectBillRate = new List<string>();
            var company = Companies(user);
            var proj = db.ClientProjects.Include("Client").Include("Project").Where(x => x.Client.CompanyId == company.CompanyId && x.BillRate != null);
            foreach (var item in proj)
            {
                projectBillRate.Add(item.Project.ProjectName);
            }
            return projectBillRate;
        }
        public bool PerProjects(string user)
        {
            var company = Companies(user);
            bool perProject = false;
            string interval = "On a per project basis";
            if (company.ClientBillInterval == interval || company.EmployeePayInterval == interval)
            {
                perProject = true;
            }
            return perProject;
        }
        public bool PunchOut(string user)
        {
            DateTime date = DateTime.UtcNow;
            TimeSpan span = new TimeSpan(10, 0, 0);
            DateTime time = date - span;
            bool punchedOut = false;
            if (db.TimeSheetEntries.Any(x => x.EmployeeId == user && x.StartTime >= time && x.EndTime == null))
            {
                punchedOut = true;
            }
            return punchedOut;
        }
        public bool IsCompanyEmployee(string user)
        {
            var CompanyEmp = db.CompanyEmployees.Any(x => x.EmployeeId == user);

            if (CompanyEmp == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool BillByProject(int id)
        {
            var billType = db.Clients.SingleOrDefault(x => x.ClientId == id);
            bool byProject = true;
            if (billType.BillByClient == true)
            {
                byProject = false;
            }
            return byProject;
        }
        public string CompanyName(string user)
        {
            var company = Companies(user);
            //    db.Dispose();
            return company.CompanyName;
        }
        public string EmployeeName(string user)
        {
            var employee = db.Employees.Find(user);
            return employee.FullName;
        }
        public static List<DateTime> ChatTimes = new List<DateTime>();
        
                  TimeZones timeZones = new TimeZones();
        public bool ChatSent(string user)
        {
            //Needs work
            bool sent = true;
           // var company = Companies(user);
            
            if (ChatTimes.Count() == 0)
            {
                var localTime =  timeZones.ConvertToLocal(user,DateTime.UtcNow);
                ChatTimes.Add(localTime);
            }
            var lastChat = ChatTimes.LastOrDefault();
           // We need to find a way to make this method work even if the chat was sent to all
           
         var newChat = db.Chats.Where(x => x.RecieverId == user && x.TimeChatSent > lastChat).Count();

            if (newChat == 0)
            {
                sent = false;
            }
            else
            {
                var localTime = timeZones.ConvertToLocal(user, DateTime.UtcNow);
                ChatTimes.Add(localTime);
            }
            return sent;
        }
        public DateTime? WillBePunchedOut(string user)
        {
            var time = db.TimeSheetEntries.SingleOrDefault(x => x.EmployeeId == user && x.EndTime != null && x.EndTime > DateTime.UtcNow);
            return time.EndTime;
        }
      
    }
}
