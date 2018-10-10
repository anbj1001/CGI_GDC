namespace DatabaseAdmin.Model
{
    public class VisitorSearch
    {
        public Visitor Visitor { get; set; }
        public Employee Employee { get; set; }
        public BookedMeeting BookedMeeting { get; set; }
        public ParametersVisitorSearch SearchParameters { get; set; }
    }
}