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
    }
}
