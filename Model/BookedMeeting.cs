using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DatabaseAdmin.Model

{
    public class BookedMeeting 
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public int? BookedMeetingID { get; set; }
        public int? VisitResponsible { get; set; }  // employeeID  tror jag. Är väl en Employee? behövs väl inte? 
        public string MeetingDepartment { get; set; } 
        public DateTime? MeetingDate { get; set; } 
        public TimeSpan? TimeStart { get; set; } 
        public List<Visitor> Visitors { get; set; }

        public BookedMeeting()
        {
            // Om mötet inte har en besökare så finns inte mötet. 
            Visitors = new List<Visitor>();
        }


        public override string ToString()
        {
            return $"{BookedMeetingID} {MeetingDate} {TimeStart} {MeetingDepartment}";
        }
    }
}
