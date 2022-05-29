using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EducationAlarm.ViewModels
{
    public class AlarmSet
    {
       
        [Display(Name ="Please set the alarm")]
        [DataType(DataType.Time)]
        public TimeSpan AlarmTime { get; set; }
       // private List<string> operators { get; set; }
       // private void SetOperators()
       // {        
       //     operators.Add("Addition");
       //     operators.Add("Subtraction");
       //     operators.Add("Multiplaction");
       //     operators.Add("Division");
       // }
       //public List<string>GetOperators()
       // {
       //     if (operators.Count < 1)
       //     {
       //         SetOperators();
       //     }
       //     return operators;
       // }
    }
}