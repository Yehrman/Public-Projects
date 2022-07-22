﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EducationAlarm.ViewModels
{
    public class UserAnswers
    {
       public int AlarmId { get; set; }
        public string LangAnswer1 { get; set; }
        public string LangAnswer2 { get; set; }
        public string LangAnswer3 { get; set; }
        public string LangAnswer4 { get; set; }
        public int? MathAnswer1 { get; set; }
        public int? MathAnswer2 { get; set; }
        public int? MathAnswer3 { get; set; }
        public int? MathAnswer4 { get; set; }
        public string Subject { get; set; }
        public UserAnswers()
        {
            LangAnswer1 = "";
            LangAnswer2 = "";
            LangAnswer3 = "";
            LangAnswer4 = "";
        }
    }
}