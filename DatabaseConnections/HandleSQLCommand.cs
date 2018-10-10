using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAdmin.Model;

namespace DatabaseAdmin.DatabaseConnections
{
    public class HandleSQLCommand
    {
        public NpgsqlCommand ReturnCMDForVisitorSearch(VisitorSearch vs, int offset, int hitsPerPage, string stmt)
        {
            var cmd = new NpgsqlCommand
            {
                CommandText = stmt
            };

            if (vs.SearchParameters.DateInTo == null)
            {
                cmd.Parameters.AddWithValue("@dateOutTo", DateTime.Now);
            }
            if (vs.SearchParameters.DateInTo == null)
            {
                cmd.Parameters.AddWithValue("@dateInTo", DateTime.Now);
            }
            if (vs.SearchParameters.DateOutFrom != null)
            {
                cmd.Parameters.AddWithValue("@dateOutFrom", vs.SearchParameters.DateOutFrom);
            }
            if (vs.SearchParameters.DateOutTo != null)
            {
                cmd.Parameters.AddWithValue("@dateOutTo", vs.SearchParameters.DateOutTo);
            }
            if (vs.SearchParameters.DateInFrom != null)
            {
                cmd.Parameters.AddWithValue("@dateInFrom", vs.SearchParameters.DateInFrom);
            }
            if (vs.SearchParameters.DateInTo != null)
            {
                cmd.Parameters.AddWithValue("@dateInTo", vs.SearchParameters.DateInTo);
            }
            if (vs.Visitor.Firstname != "")
            {
                cmd.Parameters.AddWithValue("@vFirstname", vs.Visitor.Firstname);
            }
            if (vs.Visitor.Lastname != "")
            {
                cmd.Parameters.AddWithValue("@vLastname", vs.Visitor.Lastname);
            }
            if (vs.Employee.Lastname != "")
            {
                cmd.Parameters.AddWithValue("@eFirstname", vs.Employee.Lastname);
            }
            if (vs.Employee.Lastname != "")
            {
                cmd.Parameters.AddWithValue("@eLastname", vs.Employee.Lastname);
            }
            if (vs.Employee.EmployeeID != (int?)null)
            {
                cmd.Parameters.AddWithValue("@eID", vs.Employee.EmployeeID);
            }
            if (vs.BookedMeeting.MeetingDepartment != "")
            {
                cmd.Parameters.AddWithValue("@mDepartment", vs.BookedMeeting.MeetingDepartment);
            }
            if (stmt.Contains(" LIMIT @hitsPerPage OFFSET @offset"))
            {
                cmd.Parameters.AddWithValue("@offset", offset);
                cmd.Parameters.AddWithValue("@hitsPerPage", hitsPerPage);
            }
            return cmd;
        }

        private static string ReturnStringForVisitorSearch(VisitorSearch vs, string sqlStart, int offset, int hitsPerPage)
        {// 
            var sql = new StringBuilder();

            sql.AppendLine(
                " FROM visitor" +
                " JOIN visitor_meeting ON visitor.visitor_id = visitor_meeting.visitor_id" +
                " JOIN booked_meeting ON visitor_meeting.booked_meeting_id = booked_meeting.booked_meeting_id" +
                " JOIN employee ON booked_meeting.visit_responsible = employee.employee_id" +
                " WHERE 1=1 ");

            if (vs.SearchParameters.DateOutFrom != null)
            {
                sql.AppendLine(" AND visitor.check_out_old BETWEEN @dateOutFrom AND @dateOutTo ");
            }
            if (vs.SearchParameters.DateInFrom != null)
            {
                sql.AppendLine(" AND visitor.check_in_old BETWEEN @dateInFrom AND @dateInTo");
            }
            if (vs.Visitor.Firstname != "")
            {
                sql.AppendLine(" AND visitor.firstname = @vFirstname");
            }
            if (vs.Visitor.Lastname != "")
            {
                sql.AppendLine(" AND visitor.lastname = @vLastname");
            }
            if (vs.Employee.Firstname != "")
            {
                sql.AppendLine(" AND employee.firstname = @eFirstname");
            }
            if (vs.Employee.Lastname != "")
            {
                sql.AppendLine(" AND employee.lastname = @eLastname");
            }
            if (vs.Employee.EmployeeID != (int?)null)
            {
                sql.AppendLine(" AND employee.employee_id = @eID");
            }
            if (vs.BookedMeeting.MeetingDepartment != "")
            {
                sql.AppendLine(" AND booked_meeting.meeting_department = @mDepartment");
            }
            if (vs.Visitor.NotCheckedOut)
            {
                sql.AppendLine(" AND visitor.check_out_old IS NULL");
            }
            if (vs.Visitor.BadgeReturned)
            {
                sql.AppendLine(" AND visitor.badge_returned IS FALSE");
            }
            string newSql = sqlStart + sql.ToString();

            return newSql;
        }

    }
}
