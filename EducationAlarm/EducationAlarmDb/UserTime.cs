using System;
using System.ComponentModel.DataAnnotations;

namespace EducationAlarmDb
{
    public class UserTime
    {
        public int id { get; set; }
       // [ForeignKey("User")]
        public string UserId { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan AlarmTime { get; set; }
     //   public string Subject { get; set; }
     public bool AlarmActivated { get; set; }
        public virtual UserInformation User { get; set; }
    }
}