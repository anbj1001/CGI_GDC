using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DatabaseAdmin.DatabaseConnections
{
    public class VisitorSearch
    {
        public int? VisitorID { get; set; }
        public string VisitorFirstname { get; set; }
        public string VisitorLastname { get; set; }
        public string Company { get; set; }
        public DateTime? VisitorCheck_in { get; set; }
        public DateTime? VisitorCheck_out { get; set; }
        public int? EmployeeID { get; set; }
        public string EmployeeFirstname { get; set; }
        public string EmployeeLastname { get; set; }
        public string MeetingDepartment { get; set; }
    }
}