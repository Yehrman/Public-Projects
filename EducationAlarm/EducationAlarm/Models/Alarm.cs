using System;
using System.Linq;
using System.Media;
using EducationAlarmDb;
using System.Timers;
using System.Diagnostics;
namespace EducationAlarm.Models
{
    public class Alarm :IDisposable
    {
        private AlarmContext db = new AlarmContext();
        //Seting the alarm time and activating it when the time comes 
        internal DateTime AlarmRingTime { get; set; }
     //   private string _SoundLocation { get; set; }
        Timer Timer = new Timer();
        //MathProblems mathSolutions = new MathProblems();
       //public string SetAlarmSound(string alarmLocation)
       // {
       //     _SoundLocation = alarmLocation;
       //     return _SoundLocation;
       // }
        
           SoundPlayer player = new SoundPlayer();
        private  void Set_Alarm()
        {    
            try
            {
                player.SoundLocation = @"E:\HostingSpaces\bizassis\educationalarm.com\wwwroot\alarmsounds\Alarm01.wav";
                player.PlayLooping();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }                
           }
       
      public void StopAlarm(bool answered,int id)
        {
            if(answered==true)
            {
                player.Stop();
                var alarm = db.UserTimes.SingleOrDefault(x => x.id == id);
                alarm.AlarmActivated = false;
                db.SaveChanges();
            }
        }
        private  UserTime AlarmTime(int id)
        {
            var alarm = db.UserTimes.SingleOrDefault(x => x.id == id);
            return alarm;
        }
        private void TimeTillAlarm(DateTime time,int id,string userId)
        {
            var localTime = GetLocalTime(userId);
            TimeSpan timeSpan = time - localTime;
                    if (time > localTime)
                    {
                        double interval = timeSpan.TotalMilliseconds;
                Timer.Interval = interval;
                Debug.WriteLine(timeSpan);
                Timer.Start();
                var alarm = AlarmTime(id);
                alarm.AlarmActivated = true;
                db.SaveChanges();
                Timer.Elapsed += delegate { Set_Alarm(); };
                Timer.AutoReset = false;
                }                   
            }
        private string GetTimeZone(string userId)
        {
            var clientInfo = db.UserInformation.SingleOrDefault(x => x.IdentityUserId == userId);
            var clientTimeZone = clientInfo.TimeZone;
            return clientTimeZone;
        }
        private DateTime GetLocalTime(string userId)
        {
            var clientTimeZone = GetTimeZone(userId);
            DateTime date = DateTime.Now;
            var zone = TimeZoneInfo.FindSystemTimeZoneById(clientTimeZone);
            DateTime localTime = TimeZoneInfo.ConvertTime(date, zone);
            return localTime;
        }
        private DateTime ConcatDateStrings(string time,string userId)
        {
            //This must be the users time zone
            var localTime = GetLocalTime(userId);
             string DatePart = localTime.ToString("MM/dd/yy");
             string DateandTime = DatePart +" "+ time;
              DateTime Join = DateTime.Parse(DateandTime);
            return Join;
        }
        public void ActivateAlarm(int id,string userId)
        {
                var alarm = db.UserTimes.SingleOrDefault(x => x.id == id);
                string t = alarm.AlarmTime.ToString();
                    var time = ConcatDateStrings(t,userId);
            var localTime = GetLocalTime(userId);
            if (time < localTime)
            {
                time.AddDays(1);
            }
            TimeTillAlarm(time,id,userId);
        }
        public void Dispose()
        {
            Timer.Dispose();
            db.Dispose();
        }
    }
}
