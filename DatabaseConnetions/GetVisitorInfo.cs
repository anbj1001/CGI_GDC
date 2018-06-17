using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAdmin.Model;
using DatabaseAdmin.Enums;
using static DatabaseAdmin.Model.HandleVisitor;


namespace DatabaseAdmin.DatabaseConnections
{
    static class GetVisitorInfo
    {
        static public List<Visitor> GetAllVisitor()
        {// Funkar

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
                            CheckInDate = (reader["check_in"] != DBNull.Value) ? (DateTime)reader.GetTimeStamp(1) : (DateTime?)null,
                            CheckOutDate = (reader["check_out"] != DBNull.Value) ? (DateTime)reader.GetTimeStamp(2) : (DateTime?)null,
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

        static public List<Visitor> GetVisitorMeeting(Visitor v, Employee e)
        {// Funkar men ej klar. Vilken info behövs?

            BookedMeeting bm;
            List<Visitor> visitorsMeetings = new List<Visitor>();

            var sql = new StringBuilder();
            sql.AppendLine("SELECT booked_meeting.date, booked_meeting.time_start, booked_meeting.meeting_department, employee.firstname, employee.lastname  " +
                " FROM(((visitor_meeting" +
                " INNER JOIN visitor ON visitor.visitor_id = visitor_meeting.visitor_id)" +
                " INNER JOIN booked_meeting ON visitor_meeting.booked_meeting_id = booked_meeting.booked_meeting_id)" +
                " INNER JOIN employee ON booked_meeting.visit_responsible = employee.employee_id) " +
                " WHERE 1 = 1");



            if (e.Firstname != null)
            {
                sql.AppendFormat(" AND employee.firstname = @eFirstname");
                sql.AppendLine();
            }
            if (e.Lastname != null)
            {
                sql.AppendFormat(" AND employee.lastname = @eLastname");
                sql.AppendLine();
            }
            if (v.Firstname != null)
            {
                sql.AppendFormat(" AND visitor.firstname = @vFirstname");
                sql.AppendLine();
            }
            if (v.Lastname != null)
            {
                sql.AppendFormat(" AND visitor.lastname = @vLastname");
                sql.AppendLine();
            }


            var stmt = sql.ToString();

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    if (e.Firstname != null)
                    {
                        cmd.Parameters.AddWithValue("@eFirstname", e.Firstname);
                    }
                    if (e.Lastname != null)
                    {
                        cmd.Parameters.AddWithValue("@eLastname", e.Lastname);
                    }
                    if (v.Firstname != null)
                    {
                        cmd.Parameters.AddWithValue("@vFirstname", v.Firstname);
                    }
                    if (v.Lastname != null)
                    {
                        cmd.Parameters.AddWithValue("@vLastname", v.Lastname);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {



                        while (reader.Read())
                        {
                            bm = new BookedMeeting
                            {

                                //BookedMeetingID = (reader["booked_meeting_id"] != DBNull.Value) ? reader.GetInt32(5) : (int?)null,
                                MeetingDate = (reader["date"] != DBNull.Value) ? reader.GetDateTime(1) : (DateTime?)null,
                                TimeStart = (reader["time_start"] != DBNull.Value) ? reader.GetTimeSpan(2):  (TimeSpan?) null,
                                MeetingDepartment = (reader["meeting_department"] != DBNull.Value) ? reader.GetString(0) : null,
                                //VisitResponsible = (reader["visit_responsible"] != DBNull.Value) ? reader.GetInt32(11) : (int?)null,


                            };
                            v = new Visitor(bm)
                            {
                                //VisitorID = (reader["visitor_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null,
                                Firstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(3) : null,
                                Lastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(4) : null,
                                Company = (reader["company"] != DBNull.Value) ? reader.GetString(5) : null,
                                City = (reader["city"] != DBNull.Value) ? reader.GetString(6) : null,
                                //VisitorBadge = (reader["visitor_badge"] != DBNull.Value) ? reader.GetInt32(5) : (int?)null,
                                CheckInDate = (reader["check_in"] != DBNull.Value) ? reader.GetTimeStamp(4).ToDateTime() : (DateTime?)null,
                                CheckOutDate = (reader["check_out"] != DBNull.Value) ? reader.GetTimeStamp(5).ToDateTime() : (DateTime?)null,

                            };
                            visitorsMeetings.Add(v);
                        }
                    }
                    return visitorsMeetings;
                }
            }
        }

        static public int CheckOutVisitor(Visitor v)
        {// Funkar
            int result;

            var sql = new StringBuilder();

            sql.AppendLine("UPDATE visitor  SET check_out  = current_timestamp WHERE visitor_id = @visitor_id;");

            // kolla metoden och kolla sendan om badge returned har värde lägg till i paramerter annars inte

            if (v.BadgeReturned == true)
            {
                sql.AppendFormat(" UPDATE visitor SET badge_returned = TRUE WHERE visitor_id = @visitor_id");
                sql.AppendLine();
            }

            var stmt = sql.ToString();

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {

                    if (v.VisitorID != null)
                    {
                        cmd.Parameters.AddWithValue("@visitor_id", v.VisitorID);
                    }
                    //if (v.BadgeReturned == true)
                    //{
                    //    cmd.Parameters.AddWithValue("@badge_returned", v.BadgeReturned);
                    //}
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        //För VisitorSearch
        static public List<VisitorSearch> GetVisitorSearchInfo(Employee e, Visitor v, BookedMeeting bm, DateTime? dateFrom, DateTime? dateTo, string timeFrom, string timeTo)
        {// 
            VisitorSearch vs;
            List<VisitorSearch> visitorSearch = new List<VisitorSearch>();

            var sql = new StringBuilder();
            sql.AppendLine("SELECT visitor.visitor_id, visitor.firstname, visitor.lastname, visitor.company, visitor.check_in_time, visitor.check_out_time, visitor.check_in_date, visitor.check_out_date," +
                " employee.employee_id, employee.firstname, employee.lastname,booked_meeting.meeting_department " +
                " FROM visitor" +
                " JOIN visitor_meeting ON visitor.visitor_id = visitor_meeting.visitor_id" +
                " JOIN booked_meeting ON visitor_meeting.booked_meeting_id = booked_meeting.booked_meeting_id" +
                " JOIN employee ON booked_meeting.visit_responsible = employee.employee_id" +
                " WHERE 1=1 ");



            if (timeFrom != null)
            {
                sql.AppendFormat(" AND visitor.check_in_time BETWEEN @timeFrom::timestamp");
                sql.AppendLine();
            }
            if (timeTo != null)
            {
                sql.AppendFormat(" AND @timeTo::timestamp");
                sql.AppendLine();
            }
            if (dateFrom != null)
            {// Funkar
                sql.AppendFormat(" AND visitor.check_in_date BETWEEN @dateFrom");
                sql.AppendLine();
            }
            if (dateTo != null)
            {// Funkar
                sql.AppendFormat(" AND @dateTo");
                sql.AppendLine();
            }
            if (v.Firstname != null)
            {// Funkar
                sql.AppendFormat(" AND visitor.firstname = @vFirstname");
                sql.AppendLine();

            }
            if (v.Lastname != null)
            {// Funkar
                sql.AppendFormat(" AND visitor.lastname = @vLastname");
                sql.AppendLine();
            }
            if (e.Firstname != null)
            {// Funkar
                sql.AppendFormat(" AND employee.firstname = @eFirstname");
                sql.AppendLine();

            }
            if (e.Lastname != null)
            {// Funkar
                sql.AppendFormat(" AND employee.lastname = @eLastname");
                sql.AppendLine();
            }
            if (e.EmployeeID != (int?)null)
            {// Funkar
                sql.AppendFormat(" AND employee.employee_id = @eID");
                sql.AppendLine();
            }
            if (v.Company != null)
            {// Funkar
                sql.AppendFormat(" AND visitor.company = @vCompany");
                sql.AppendLine();
            }
            if (bm.MeetingDepartment != null)
            {// Funkar
                sql.AppendFormat(" AND booked_meeting.meeting_department = @mDepartment");
                sql.AppendLine();
            }
            //if (v.CheckOutDate == null)
            //{// Funkar
            //    sql.AppendFormat(" AND visitor.check_out IS NULL");
            //    sql.AppendLine();
            //}
            var stmt = sql.ToString();


            /*   Var ska vi lägga allt det här???
             *  
                5.Besöksmottagare anställningsnummer
                 WHERE employee.employee_id::text LIKE 'X%'

               
                 9.Avdelning(dropdown lista ?)
                 WHERE booked_meeting.meeting_department = 'Ekonomi'
                 */


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {

                    // Lägger till parametern enbart om det finns ett värde i parametern, annars kraschar det
                    if (timeTo != null)
                    {
                        cmd.Parameters.AddWithValue("@timeTo", timeTo.ToString());

                    }
                    if (timeFrom != null)
                    {
                        cmd.Parameters.AddWithValue("@timeFrom", timeFrom.ToString());

                    }
                    if (dateTo != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
                    }
                    if (dateFrom != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@dateTo", dateTo);
                    }
                    if (v.Firstname != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@vFirstname", v.Firstname);
                    }
                    if (v.Lastname != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@vLastname", v.Lastname);
                    }
                    if (e.Firstname != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@eFirstname", e.Firstname);
                    }
                    if (e.Lastname != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@eLastname", e.Lastname);
                    }
                    if (e.EmployeeID != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@eID", e.EmployeeID);
                    }
                    if (v.Company != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@vCompany", v.Company);
                    }
                    if (bm.MeetingDepartment != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@mDepartment", bm.MeetingDepartment);
                    }
                    if (v.CheckOutDate != null)
                    {// Funkar
                        cmd.Parameters.AddWithValue("@checkedOut", v.CheckOutDate);
                    }



                    //cmd.Parameters.AddWithValue("@");

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


