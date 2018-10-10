using System.Collections.Generic;

namespace DatabaseAdmin.Model

{
    public class Employee 
    {
        
        public int? EmployeeID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public string Team { get; set; }

        public List<BookedMeeting> BookedMeetings;


        public Employee()
        {
            BookedMeetings = new List<BookedMeeting>();
        }
        public override string ToString()
        {
            return $"{EmployeeID} {Firstname} {Lastname}";
        }

    }
}
