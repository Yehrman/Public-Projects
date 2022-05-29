using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompuskillsMvcProject.Models
{
    public class CreateProjectForClientModel
    {
    
        [Display(Name = "project name")]
        [Required]
        public string ProjectName { get; set; }
    //    public Guid CompanyId { get; set; }
        public int  ClientId { get; set; }
        [Display(Name = "Project bill rate")]
     
        public decimal ?  BillRate { get; set; }

         
    }
}