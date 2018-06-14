using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAdmin.Model;
using DatabaseAdmin.Enums;


namespace DatabaseAdmin.DatabaseConnections
{
    static class GetVisitorInfo
    {
        static public List<Visitor> GetAllVisitor()
        {
            List<Visitor> visitors = new List<Visitor>();
            Visitor v;


            string stmt = "select visitor_id, check_in, check_out, visitor_badge,firstname,lastname, company, city from visitor";


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {

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
        {// Funkar men ej klar. Vilken info behövs?
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

                        };
                        visitorsMeetings.Add(v);
                    }
                }
                return visitorsMeetings;
            }
        }

        static public int CheckOutVisitor(Visitor v)
        {// Funkar
            int result;


            string stmt = $"UPDATE visitor  SET check_out  = current_timestamp WHERE visitor_id = @visitor_id; " +
                $"UPDATE visitor SET badge_returned = @badge_returned WHERE visitor_id = @visitor_id";


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {

                    cmd.Parameters.AddWithValue("@visitor_id", v.VisitorID);
                    cmd.Parameters.AddWithValue("@badge_returned", v.BadgeReturned);
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        //För VisitorSearch
        static public List<VisitorSearch> GetVisitorSearchInfo(string vFirstname/*, string vLastname, string eLastname*/)
        {// Uppfukkad!
            VisitorSearch vs;
            List<VisitorSearch> visitorSearch = new List<VisitorSearch>();

            string stmt = "SELECT visitor.visitor_id, visitor.firstname, visitor.lastname, visitor.company, visitor.check_in," +
                " visitor.check_out, employee.employee_id, employee.firstname, employee.lastname,booked_meeting.meeting_department" +
                " FROM visitor " +
                " JOIN visitor_meeting ON visitor.visitor_id = visitor_meeting.visitor_id" +
                " JOIN booked_meeting ON visitor_meeting.booked_meeting_id = booked_meeting.booked_meeting_id" +
                " JOIN employee ON booked_meeting.visit_responsible = employee.employee_id" +
                " WHERE UPPER(visitor.firstname) LIKE UPPER(@firstname) ";



            /*   Var ska vi lägga allt det här???
             *   1.Datumintervall - Kalender
                 WHERE visitor.check_in BETWEEN 'XXXX-XX-XX%' AND 'XXXX-XX-XX%'

                 2.Tidsintervall in

                 3.Tidsintervall ut

                 4.Besöksmottagare namn
                 WHERE UPPER(employee.firstname) LIKE UPPER('X%') AND UPPER(employee.lastname) LIKE UPPER('X%')

                 5.Besöksmottagare anställningsnummer
                 WHERE employee.employee_id::text LIKE 'X%'

                 6.Besökare namn
                 WHERE UPPER(visitor.firstname) LIKE UPPER('X%') AND UPPER(visitor.lastname) LIKE UPPER('X%') /// LIGGER I STMT
                 7.Besökare företagsnamn
                 WHERE UPPER(visitor.company) LIKE UPPER('X%')

                 8.Besökare som saknar uppgifter om utcheckning
                 WHERE visitor.check_out IS null

                 9.Avdelning(dropdown lista ?)
                 WHERE booked_meeting.meeting_department = 'Ekonomi'

                 */

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {

                    //Lägg till parametrar
                    cmd.Parameters.AddWithValue("@firstname", vFirstname);
                    //cmd.Parameters.AddWithValue("@vLastname", vLastname);


                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vs = new VisitorSearch
                            {

                                VisitorID = (reader["visitor_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null,
                                VisitorFirstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(1) : null,
                                VisitorLastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(2) : null,
                                Company = (reader["company"] != DBNull.Value) ? reader.GetString(3) : null,
                                VisitorCheck_in = (reader["check_in"] != DBNull.Value) ? reader.GetDateTime(4) : (DateTime?)null,
                                VisitorCheck_out = (reader["check_out"] != DBNull.Value) ? reader.GetDateTime(5) : (DateTime?)null,
                                EmployeeID = (reader["employee_id"] != DBNull.Value) ? reader.GetInt32(6) : (int?)null,
                                EmployeeFirstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(7) : null,
                                EmployeeLastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(8) : null,
                                MeetingDepartment = (reader["meeting_department"] != DBNull.Value) ? reader.GetString(9) : null,
                            };

                            visitorSearch.Add(vs);
                        }
                    }
                    return visitorSearch;
                }

            }
        }
    }
}


