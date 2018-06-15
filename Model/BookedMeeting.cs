using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAdmin.Model

{
  public class BookedMeeting
    {
        public int? BookedMeetingID { get; set; }
        public int? VisitResponsible { get; set; }
        public string MeetingDepartment { get; set; }
        public DateTime? MeetingDate { get; set; }
        public TimeSpan? TimeStart { get; set; }
       
       

    }
}
