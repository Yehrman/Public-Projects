using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
namespace MvcProjectDbConn
{
    public class Employee: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool ? Familiar { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
            private set { }
        }
        
        public virtual ICollection<WorkSchedule> WorkSchedules { get; set; }
        public virtual ICollection<CompanyEmployee> CompanyEmployees { get; set; }
        public virtual ICollection<EmployeePayStub> EmployeePayStubs { get; set; }

        //   public virtual ICollection<Chat> ChatSender { get; set; }
        [ForeignKey("RecieverId")]
        public   ICollection<Chat> Chats { get; set; }
    }
}