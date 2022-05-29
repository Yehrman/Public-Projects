using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MvcProjectDbConn
{
    public class Client
    {
        public int ClientId { get; set; }
        
        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Display(Name ="First Name")]
       public string FirstName { get; set; }
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool BillByClient { get; set; }
        public bool BillByProject { get; set; }
       
     
        [DataType(DataType.Currency)]
        public decimal? BillRate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DateAdded { get; set; }

        public ICollection<ClientBill> ClientBills { get; set; }
        public ICollection<ClientProject> ClientProjects { get; set; }
    }
}