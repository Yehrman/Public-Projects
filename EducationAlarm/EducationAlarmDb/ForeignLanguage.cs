using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationAlarmDb
{
   public class ForeignLanguage
    {
       public int ForeignLanguageId { get; set; }
        public string Language { get; set; }
        public ICollection<ForeignWord> ForeignWords { get; set; }
    }
}
