using System;
using System.ComponentModel.DataAnnotations;

namespace MvcProjectDbConn
{
    public class ClientBill
    {
        public int id { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int ? ProjectId { get; set; }
      public virtual Project Project { get; set; }
        [DataType(DataType.Currency)]
        public decimal Bill { get; set; }
        public TimeSpan? TimeWorked { get; set; }
        public DateTime DateBilled { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd }"), DataType(DataType.Date)]
        public DateTime? DatePaymentRecieved { get; set; }
    }
}