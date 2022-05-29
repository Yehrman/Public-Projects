using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProjectDbConn
{
   public class Company
    {
        public Guid CompanyId { get; set; }
       [Required]
        public string CompanyName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string  CompanyPhoneNumber { get; set; }
        public string Address { get; set; } 
    
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string EmployeePayInterval { get; set; }
        public string ClientBillInterval { get; set; }
        public DateTime DateJoined { get; set; }
        public string TimeZone { get; set; }
        public bool ? IsDeleted { get; set; }
        public ICollection<CompanyEmployee> CompanyEmployees { get; set; }
        public ICollection<Client> Clients { get; set; }
        public ICollection<EmployeePayStub> EmployeePayStubs { get; set; }
        public ICollection<ClientBill> ClientBills { get; set; }
   
    }
}
