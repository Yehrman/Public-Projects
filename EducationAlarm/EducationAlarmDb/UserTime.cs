using System;
namespace EducationAlarmDb
{
    public class UserTime
    {
        public int id { get; set; }
       // [ForeignKey("User")]
        public string UserId { get; set; }
        public TimeSpan AlarmTime { get; set; }
     //   public string Subject { get; set; }
        public virtual UserInformation User { get; set; }
    }
}