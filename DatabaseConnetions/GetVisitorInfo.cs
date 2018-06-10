using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAdmin.Model;

namespace DatabaseAdmin.DatabaseConnections
{
    static class GetVisitorInfo
    {
        static public List<Visitor> GetAllVisitor()
        {
            string stmt = "select visitor_id, check_in, check_out, visitor_badge,firstname,lastname, company, city from visitor";
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                List<Visitor> visitors = new List<Visitor>();
                Visitor v;


                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        v = new Visitor()
                        {
                            VisitorID = reader.GetInt32(0),

                            Check_in = (DateTime)reader.GetTimeStamp(1),
                            Check_out = (reader["check_out"] != DBNull.Value) ? (DateTime)reader.GetTimeStamp(2) : (DateTime?)null,
                            VisitorBadge = (reader["visitor_badge"] != DBNull.Value) ? reader.GetInt32(3) : (int?)null,
                            Firstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(4) : null,
                            Lastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(5) : null,
                            Company = (reader["company"] != DBNull.Value) ? reader.GetString(6) : null,
                            City = (reader["city"] != DBNull.Value) ? reader.GetString(7) : null,

                        };
                        visitors.Add(v);
                    }
                }
                return visitors;
            }
        }
        
        static public List<Visitor> GetAllVisitorMeeting()
        {// Funkar men ej klar
            Visitor v;
            BookedMeeting bm;
            List<Visitor> visitorsMeetings = new List<Visitor>();


            string stmt = "SELECT  visitor.firstname, visitor.lastname, visitor.company, visitor.city, visitor.check_in," +
                " visitor.check_out, booked_meeting.meeting_department, booked_meeting.date, booked_meeting.time_start " +
                "FROM(((visitor_meeting INNER JOIN visitor ON visitor.visitor_id = visitor_meeting.visitor_id) " +
                "INNER JOIN booked_meeting ON visitor_meeting.booked_meeting_id = booked_meeting.booked_meeting_id) " +
                "INNER JOIN employee ON booked_meeting.visit_responsible = employee.employee_id) ";


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bm = new BookedMeeting
                        {

                            //BookedMeetingID = (reader["booked_meeting_id"] != DBNull.Value) ? reader.GetInt32(5) : (int?)null,
                            MeetingDepartment = (reader["meeting_department"] != DBNull.Value) ? reader.GetString(6) : null,
                            MeetingDate = (reader["date"] != DBNull.Value) ? reader.GetDateTime(7) : (DateTime?)null,
                            TimeStart = (reader["time_start"] != DBNull.Value) ? reader.GetTimeSpan(8) : (TimeSpan?)null,
                            //VisitResponsible = (reader["visit_responsible"] != DBNull.Value) ? reader.GetInt32(11) : (int?)null,


                        };
                        v = new Visitor(bm)
                        {
                            //VisitorID = (reader["visitor_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null,
                            Firstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(0) : null,
                            Lastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(1) : null,
                            Company = (reader["company"] != DBNull.Value) ? reader.GetString(2) : null,
                            City = (reader["city"] != DBNull.Value) ? reader.GetString(3) : null,
                            //VisitorBadge = (reader["visitor_badge"] != DBNull.Value) ? reader.GetInt32(5) : (int?)null,
                            Check_in = (reader["check_in"] != DBNull.Value) ? reader.GetTimeStamp(4).ToDateTime() : (DateTime?)null,
                            Check_out = (reader["check_out"] != DBNull.Value) ? reader.GetTimeStamp(5).ToDateTime() : (DateTime?)null,



                            //BookedMeetingID = (reader["booked_meeting_id"] != DBNull.Value) ? reader.GetInt32(1) : (int?)null,

                        };
                        visitorsMeetings.Add(v);
                    }
                }
                return visitorsMeetings;
            }
        }

       
    }
}


