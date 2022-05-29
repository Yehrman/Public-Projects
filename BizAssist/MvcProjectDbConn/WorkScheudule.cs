using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProjectDbConn
{
    public class WorkSchedule
    {
        public int id { get; set; }
        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; } 
      [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int? ClientId { get; set; }
          public virtual Client Client { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd }"), DataType(DataType.Date)]
        //[UIHint("PickDate")]
        public DateTime Date { get; set; }
        [DataType(DataType.Time)]
        [Required]
        public string Time { get; set; }
     
        public bool  IsScheduledPunchOut { get; set; }
         [Display(Name = " Minutes till punchOut (For a scheduled punchout)")]
        public double? EndTime { get; set; }


    }
} 