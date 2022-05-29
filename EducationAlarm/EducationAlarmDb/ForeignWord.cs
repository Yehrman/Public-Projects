namespace EducationAlarmDb
{
    public class ForeignWord
    {
        public int ForeignWordId { get; set; }
        public int ForeignLanguageId { get; set; }
        public virtual ForeignLanguage ForeignLanguage { get; set; }
        public string Word { get; set; }
        public string Definintion { get; set; }
    }
}