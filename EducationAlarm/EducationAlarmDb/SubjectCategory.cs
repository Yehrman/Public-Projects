using System.Collections.Generic;
namespace EducationAlarmDb
{
    public class SubjectCategory
    {
        public int SubjectCategoryId { get; set; }
        public string Category { get; set; }
        public   ICollection<Subject> Subjects { get; set; }     
    }
}