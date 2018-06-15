using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAdmin.Model
{
    public class SetBookedMeeting
    {
        public BookedMeeting CreateBookedMeeting(Employee e, string vFirstname, string vLastname, string vCompany, string vCity, string mDepartment, DateTime? date/*, TimeSpan timeStart*/)
        {
            Visitor v;
            BookedMeeting bM;


            if (mDepartment == null)
            {
                bM = new BookedMeeting
                {
                    VisitResponsible = e.EmployeeID,
                    MeetingDepartment = e.Department,
                    MeetingDate = date
                    //TimeStart = timeStart
                };
                return bM;
            }
            else
            {
                bM = new BookedMeeting
                {
                    VisitResponsible = e.EmployeeID,
                    MeetingDepartment = mDepartment,
                    MeetingDate = date
                };
            }
            return bM;
        }

    }
}
