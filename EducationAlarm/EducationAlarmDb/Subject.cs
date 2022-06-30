using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationAlarmDb
{
   public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int SubjectCategoryId { get; set; }
        public virtual SubjectCategory SubjectCategory { get; set; }
        public ICollection <UserInformation> UserInformation { get; set; }
        public ICollection<ForeignWord> ForeignWords { get; set; }
    }
}
