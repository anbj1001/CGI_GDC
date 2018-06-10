using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAdmin.Model
{
    class Visitor
    {
        public int? VisitorID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public int? VisitorBadge { get; set; }
        public DateTime? Check_in { get; set; }
        public DateTime? Check_out { get; set; }
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
