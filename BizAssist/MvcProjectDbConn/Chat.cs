using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProjectDbConn
{
  public  class Chat
    {
        public int id { get; set; }
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string RecieverId { get; set; }
        public  Employee Employee { get; set; }
      
        [Display(Name ="chat message")]
        [Required]
        public string Message { get; set; }
        [Display(Name ="Time chat sent")]
        public DateTime TimeChatSent { get; set; }
     
    }
}
