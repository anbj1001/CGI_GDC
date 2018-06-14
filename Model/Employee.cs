using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<BookedMeeting> BookedMeetings { get; set; }

        public Employee(BookedMeeting bookedMeeting)
        {
            BookedMeetings = new List<BookedMeeting>
            {
                bookedMeeting
            };
        }
        public Employee()
        {

        }

        public override string ToString()
        {
            return $"{Firstname} {Lastname}"; 
        }

    }
}
