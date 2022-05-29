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
       
        //Seting the alarm time and activating it when the time comes 
        internal DateTime AlarmRingTime { get; set; }
        private string SoundLocation { get; set; }
        Timer Timer = new Timer();
        //MathProblems mathSolutions = new MathProblems();
       
        
           SoundPlayer player = new SoundPlayer();
        private  void Set_Alarm()
        {
            ////Not sure if we need the if statement or not
            //DateTime now = DateTime.Now;
            //now = now.AddSeconds(-1);
            //if (time >= now && time < now.AddMinutes(1))
            //{
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
       
      public void StopAlarm(bool answered)
        {
            if(answered==true)
            {
                player.Stop();
            }
        }
        //Code works with 2 final alarms in list
        //Still needs work
        private void TimeTillAlarm(DateTime time)
        {
                //Only the last alarm  time works
                   TimeSpan timeSpan = time - DateTime.Now;
                    if (time > DateTime.Now)
                    {
                        double interval = timeSpan.TotalMilliseconds;
                    Timer.Interval = interval;
                Debug.WriteLine(timeSpan);
                    Timer.Start();
           
                    Timer.Elapsed += delegate { Set_Alarm(); };
                Timer.AutoReset = false;
                }                   
            }
        private DateTime ConcatDateStrings(string time)
        {
                    string DatePart = DateTime.Today.ToString("MM/dd/yy");
                    string DateandTime = DatePart +" "+ time;
                    DateTime Join = DateTime.Parse(DateandTime);
            return Join;
        }
        public void ActivateAlarms(string id)
        {
            using(AlarmContext db=new AlarmContext())
            {
                var alarms = db.UserTimes.Where(x => x.UserId == id);
                foreach (var item in alarms)
                {
                    string t = item.AlarmTime.ToString();
                    var time = ConcatDateStrings(t);

                    if (time < DateTime.Now)
                    {
                        time.AddDays(1);
                    }
                    TimeTillAlarm(time);
                }
            }
        }
        public void Dispose()
        {
            Timer.Dispose();
        }
    }
}
