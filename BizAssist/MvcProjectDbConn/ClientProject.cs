using System;
using System.ComponentModel.DataAnnotations;

namespace MvcProjectDbConn
{
    public class ClientProject
    {
        public int id { get; set; }
  
        public int ClientId { get; set; }
        
        public virtual Client Client { get;set; }
        public int ProjectId { get; set; }
      
        public virtual Project Project { get; set; }
        [DataType(DataType.Currency)]

        public decimal? BillRate { get; set; }
      
       
    }
}