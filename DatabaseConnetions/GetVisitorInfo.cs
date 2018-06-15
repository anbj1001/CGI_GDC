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
                            Check_in = (reader["check_in"] != DBNull.Value) ? (DateTime)reader.GetTimeStamp(1) : (DateTime?)null,
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
        static public List<VisitorSearch> GetVisitorSearchInfo(string vFirstname, string vLastname, string eFirstname, string eLastname, int? eID, string vCompany, string mDepartment/*, DateTime? checkedOut*/)
        {// 
            VisitorSearch vs;
            List<VisitorSearch> visitorSearch = new List<VisitorSearch>();

            var sql = new StringBuilder();
            sql.AppendLine("SELECT visitor.visitor_id, visitor.firstname, visitor.lastname, visitor.company, visitor.check_in,visitor.check_out, employee.employee_id, " +
                " employee.firstname, employee.lastname,booked_meeting.meeting_department " +
                " FROM visitor" +
                " JOIN visitor_meeting ON visitor.visitor_id = visitor_meeting.visitor_id" +
                " JOIN booked_meeting ON visitor_meeting.booked_meeting_id = booked_meeting.booked_meeting_id" +
                " JOIN employee ON booked_meeting.visit_responsible = employee.employee_id" +
                " WHERE 1=1 ");

            if (vFirstname != null)
            {
                sql.AppendFormat(" AND visitor.firstname = @vFirstname");
                sql.AppendLine();

            }
            if (vLastname != null)
            {
                sql.AppendFormat(" AND visitor.lastname = @vLastname");
                sql.AppendLine();
            }
            if (eFirstname != null)
            {
                sql.AppendFormat(" AND employee.firstname = @eFirstname");
                sql.AppendLine();

            }
            if (eLastname != null)
            {
                sql.AppendFormat(" AND employee.lastname = @eLastname");
                sql.AppendLine();
            }
            if (eID != (int?)null)
            {
                sql.AppendFormat(" AND employee.employee_id = @eID");
                sql.AppendLine();
            }
            if (vCompany != null)
            {
                sql.AppendFormat(" AND visitor.company = @vCompany");
                sql.AppendLine();
            }
            if (mDepartment != null)
            {
                sql.AppendFormat(" AND booked_meeting.meeting_department = @mDepartment");
                sql.AppendLine();
            }
            //if (checkedOut == null )
            //{
            //    sql.AppendFormat(" AND visitor.check_out IS NULL");
            //    sql.AppendLine();
            //}
            var stmt = sql.ToString();


            /*   Var ska vi lägga allt det här???
             *   1.Datumintervall - Kalender
                 WHERE visitor.check_in BETWEEN 'XXXX-XX-XX%' AND 'XXXX-XX-XX%'

                 2.Tidsintervall in

                 3.Tidsintervall ut

                 4.Besöksmottagare namn
                 WHERE UPPER(employee.firstname) LIKE UPPER('X%') AND UPPER(employee.lastname) LIKE UPPER('X%')

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

                    //Lägger till parametern enbart om det finns ett värde i parametern, annars kraschar det
                    if (vFirstname != null)
                    {
                        cmd.Parameters.AddWithValue("@vFirstname", vFirstname);
                    }
                    if (vLastname != null)
                    {
                        cmd.Parameters.AddWithValue("@vLastname", vLastname);
                    }
                    if (eFirstname != null)
                    {
                        cmd.Parameters.AddWithValue("@eFirstname", eFirstname);
                    }
                    if (eLastname != null)
                    {
                        cmd.Parameters.AddWithValue("@eLastname", eLastname);
                    }
                    if (eID != null)
                    {
                        cmd.Parameters.AddWithValue("@eID", eID);
                    }
                    if (vCompany != null)
                    {
                        cmd.Parameters.AddWithValue("@vCompany", vCompany);

                    }
                    if (mDepartment != null)
                    {
                        cmd.Parameters.AddWithValue("@mDepartment", mDepartment);

                    }
                    //if (checkedOut != null)
                    //{
                    //    cmd.Parameters.AddWithValue("@checkedOut", checkedOut);

                    //}



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


