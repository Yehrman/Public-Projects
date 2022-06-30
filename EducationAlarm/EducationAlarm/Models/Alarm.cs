using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Media;
using EducationAlarmDb;
using System.Globalization;
using System.Timers;
using System.Diagnostics;
namespace EducationAlarm.Models
{
    public class Alarm :IDisposable
    {
        private AlarmContext db = new AlarmContext();
        //Seting the alarm time and activating it when the time comes 
        internal DateTime AlarmRingTime { get; set; }
        private string SoundLocation { get; set; }
        Timer Timer = new Timer();
        //MathProblems mathSolutions = new MathProblems();
       
        
           SoundPlayer player = new SoundPlayer();
        private  void Set_Alarm()
        {    
            try
            {
                player.SoundLocation = @"C:\Windows\Media\Alarm01.wav";//SoundLocation;      
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
        private void TimeTillAlarm(DateTime time,int id)
        {
                //Only the last alarm  time works
                   TimeSpan timeSpan = time - DateTime.Now;
                    if (time > DateTime.Now)
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
        public DateTime ConcatDateStrings(string time)
        {
                    string DatePart = DateTime.Today.ToString("MM/dd/yy");
                    string DateandTime = DatePart +" "+ time;
                    DateTime Join = DateTime.Parse(DateandTime);
            return Join;
        }
        public void ActivateAlarm(int id)
        {
                var alarm = db.UserTimes.SingleOrDefault(x => x.id == id);
                 string t = alarm.AlarmTime.ToString();
                    var time = ConcatDateStrings(t);
                    if (time < DateTime.Now)
                    {
                        time.AddDays(1);
                    }
                    TimeTillAlarm(time,id);
        }
        public void Dispose()
        {
            Timer.Dispose();
            db.Dispose();
        }
    }
}
