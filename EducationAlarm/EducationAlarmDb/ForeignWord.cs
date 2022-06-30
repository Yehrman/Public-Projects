namespace EducationAlarmDb
{
    public class ForeignWord
    {
        public int ForeignWordId { get; set; }
       public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public string Word { get; set; }
        public string Definintion { get; set; }
    }
}