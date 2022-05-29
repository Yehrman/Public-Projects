using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MvcProjectDbConn
{
    public class Project
    {
        public int ProjectId { get; set; }
        [Required]
        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Display(Name ="project name")]
        [Required]
        public string ProjectName { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsForClient { get; set; }
        public DateTime? DateCompleted { get; set; }
        public bool ? IsDeleted { get; set; }
        public ICollection<ClientBill> ClientBills { get; set; }
        public ICollection<ClientProject> ClientProjects { get; set; }
        public ICollection<WorkSchedule> WorkSchedules { get; set; }
        public ICollection<TimeSheetEntry> TimeSheetEntries { get; set; }
    }
}