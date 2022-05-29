using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcProjectDbConn;
using NodaTime;

namespace CompuskillsMvcProject.Models
{
    public class TimeZones
    {
        private BizAssistContext db = new BizAssistContext();
     
        //private static readonly DateTimeZone ServerZone = DateTimeZoneProviders.Bcl["PST"];
       public  DateTime ConvertToLocal(string user,DateTime date)
        {
            var employee = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
            var company = db.Companies.Find(employee.CompanyId);
            var zone = company.TimeZone;       
            var getClientZone = DateTimeZoneProviders.Bcl[zone];
            //why can't I take values out of a database and convert to local time using this code?
            return Instant.FromDateTimeUtc(date).InZone(getClientZone).ToDateTimeUnspecified();
        }
       

        public string PunchinOut(string user,DateTime date,string inOut)
        {
            return $"You punched {inOut} at" + ConvertToLocal(user, date);
        }
        public string ScheduledPunchout(string user,DateTime date)
        {
            return "You will be punched out at " + ConvertToLocal(user, date);
        }
   public DateTime ConvertEntries(DateTime? nullDate,string user)
        {
            var employee = db.CompanyEmployees.FirstOrDefault(x => x.EmployeeId == user);
            var company = db.Companies.Find(employee.CompanyId);
            var zone = company.TimeZone;
         DateTime LocalTime=  nullDate ?? DateTime.Now;
            
            switch(zone)
                {
                case "Israel Standard Time":
                   LocalTime= LocalTime.AddHours(2);
                    break;
                case "Eastern Standard Time":
                  LocalTime=  LocalTime.AddHours(-5);
                    break;
                case "Central standard time":
                  LocalTime=  LocalTime.AddHours(-6);
                    break;
                case "Mountain standard time":
                  LocalTime=  LocalTime.AddHours(-7);
                    break;
                case "Pacific standard time":
                  LocalTime=  LocalTime.AddHours(-8);
                    break;
            }
            return LocalTime;
        }
       
        public void Dispose()
        {
            db.Dispose();
        }
    }
}