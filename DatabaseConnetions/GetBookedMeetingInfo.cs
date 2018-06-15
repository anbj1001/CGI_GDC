using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using DatabaseAdmin.Model;

namespace DatabaseAdmin.DatabaseConnections
{
    static class GetBookedMeetingInfo
    {
        static public List<BookedMeeting> GetAllBookedMeeting()
        {

            string stmt = "select * from booked_meeting";
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                List<BookedMeeting> bookedMeetings = new List<BookedMeeting>();
                BookedMeeting bm;


                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bm = new BookedMeeting()
                        {
                            BookedMeetingID = reader.GetInt32(0),
                            VisitResponsible = (reader["visit_responsible"] != DBNull.Value) ? reader.GetInt32(1) : (int?)null,
                            MeetingDepartment = (reader["meeting_department"] != DBNull.Value) ? reader.GetString(2) : null,
                            MeetingDate = (DateTime)reader.GetDate(3),
                            TimeStart = reader.GetTimeSpan(4)
                        };
                        bookedMeetings.Add(bm);
                    }
                }
                return bookedMeetings;
            }
        }
        public static int CreateBookedMeeting(string mDepartment, DateTime mDate, DateTime timeStart, Employee e)
        {// Funkar
            int result;
            var sql = new StringBuilder();
            
            sql.AppendLine("INSERT INTO booked_meeting(meeting_department, date, time_start,visit_responsible)" +
           " VALUES((SELECT employee_id FROM employee WHERE employee_id = @employee_id ), @mDepartment,@mDate, @timeStart)");

            //if (e.Firstname != null)
            //{
            //    sql.AppendFormat(" AND firstname = @firstname ");
            //    sql.AppendLine();

            //}
            //if (e.Lastname != null)
            //{
            //    sql.AppendFormat(" AND lastname = @lastname ");
            //    sql.AppendLine();
            //}
            //if (e.EmployeeID != null)
            //{
            //    sql.AppendFormat(" OR employee_id = @employee_id )))");
            //    sql.AppendLine();
            //}


            var stmt = sql.ToString();

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    //if (e.Firstname != null)
                    //{
                    //    cmd.Parameters.AddWithValue("@firstname ", e.Firstname);
                    //}
                    //if (e.Lastname != null)
                    //{
                    //    cmd.Parameters.AddWithValue("@lastname ", e.Lastname);

                    //}
                    if (e.EmployeeID != null)
                    {
                        cmd.Parameters.AddWithValue("@visit_responsible", e.EmployeeID);
                    }

                    cmd.Parameters.AddWithValue("@mDepartment", mDepartment);
                    cmd.Parameters.AddWithValue("@mDate", mDate);
                    cmd.Parameters.AddWithValue("@timeStart", timeStart);

                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }
    }
}

