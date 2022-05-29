using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProjectDbConn
{
    public class TimeSheetEntry
    {
        //Going foward only EndTime and ClientId should be null and possibly StartTime if it's easier to shtim with endTime
        public int TimeSheetEntryId { get; set; }
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }


        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public DateTime? StartTime { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }   
    }
}