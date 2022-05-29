using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProjectDbConn
{
    public class EmployeePayStub
    {
        public int id { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
       
        public decimal PayCheck { get; set; }
        public DateTime PayDay { get; set; }

    }
}

