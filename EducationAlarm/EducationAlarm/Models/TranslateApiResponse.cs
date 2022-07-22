using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EducationAlarm.Models
{
    public class TranslateApiResponse
    {
        public int status { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string original_text { get; set; }
        public List<Dictionary<string,string>> translated_text { get; set; }
        public  int translated_characters { get; set; }
    }
}