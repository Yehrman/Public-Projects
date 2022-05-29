using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;


namespace CompuskillsMvcProject.Models
{
    public class Utility
    {
        public decimal payRate { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        [DataType(DataType.Date)]
        public DateTime? From { get; set; }
        [DataType(DataType.Date)]
        public DateTime? To { get; set; }

  
    }
}