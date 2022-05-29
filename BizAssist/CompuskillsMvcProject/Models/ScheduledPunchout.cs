using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace CompuskillsMvcProject.Models
{
    public class ScheduledPunchout
    {
        [Required]
        public int JobId { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public DateTime PunchOut { get; set; }
    }
}