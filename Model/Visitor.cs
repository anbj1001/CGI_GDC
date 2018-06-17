using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAdmin.Model
{
    public class Visitor
    {
        public int? VisitorID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public int? VisitorBadge { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public bool BadgeReturned { get; set; }
        public List<BookedMeeting> BookedMeetings { get; set; }


        public Visitor(BookedMeeting bookedMeeting)
        {
            BookedMeetings = new List<BookedMeeting>
            {
                bookedMeeting
            };
        }
        public Visitor()
        {

        }
    }
}
