using Npgsql;
using System;
using System.Configuration;
using System.Text;
using System.Transactions;
using static DatabaseAdmin.Model.GetEmployeeInfo;
using static DatabaseAdmin.Model.GetBookedMeetingInfo;
using System.Collections.Generic;

namespace DatabaseAdmin.Model
{
    static class GetVisitorInfo
    {
        /// <summary>
        ///Checkar in en besökare på ett möte.  Är i ett transactionsScope som skapar ett fullständigt drop-in besök
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public static int CheckInVisitor(Visitor visitor)
        {
            int result;
            string stmt = "INSERT INTO visitor(check_in_old, firstname, lastname, company) VALUES(@check_in_old, @firstname, @lastname, @company) RETURNING visitor_id";

            using (TransactionScope scope = new TransactionScope())
            {
                using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(stmt, conn))
                    {
                        cmd.Parameters.AddWithValue("check_in_old", DateTime.Now);
                        cmd.Parameters.AddWithValue("firstname", visitor.Firstname);
                        cmd.Parameters.AddWithValue("lastname", visitor.Lastname);
                        cmd.Parameters.AddWithValue("company", visitor.Company);
                        result = (int)cmd.ExecuteScalar();
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// Lägger till en besökare till ett möte. Är i ett transactionsScope som skapar ett fullständigt drop-in besök
        /// </summary>
        /// <param name="visitorID"></param>
        /// <param name="bookedMeetingID"></param>
        /// <returns></returns>
        public static int AddVisitorToMeeting(int? visitorID, int? bookedMeetingID)
        {
            int result;
            string stmt = "INSERT INTO visitor_meeting(visitor_id, booked_meeting_id) VALUES(@visitor_id, @booked_meeting_id)";

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand(stmt, conn))
                        {
                            cmd.Parameters.AddWithValue("visitor_id", visitorID);
                            cmd.Parameters.AddWithValue("booked_meeting_id", bookedMeetingID);
                            result = cmd.ExecuteNonQuery();
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception)
            {
                throw new Exception("Något gick fel och besökaren är fortfarande inte incheckad!");
            }
            return result;
        }

        /// <summary>
        /// Bygger ihop de olika strängarna för att skapa en fullständig query. Returnerar en lista med Employee, Visitor och BookedMeeting
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="offset"></param>
        /// <param name="hitsPerPage"></param>
        /// <returns></returns>
        static private List<VisitorSearch> GetVisitorSearchInfo(VisitorSearch vs, int offset, int hitsPerPage)
        {
            VisitorSearchReturn vsr = new VisitorSearchReturn();



            // start på söksträngen 
            string sqlStart = " SELECT visitor.visitor_id, visitor.firstname, visitor.lastname, visitor.company, visitor.check_in_old, visitor.check_out_old, " +
                " employee.employee_id, employee.firstname, employee.lastname,booked_meeting.meeting_department ";

            // Slutet på söksträngen. LIMIT OFFSET ska alltid ligga sist i queryn
            string sqlEnd = " GROUP BY visitor.visitor_id, employee.employee_id, booked_meeting.booked_meeting_id" +
                " ORDER BY visitor.check_in_old ASC" +
                " LIMIT @hitsPerPage OFFSET @offset";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    // start på strängen skickas med och addas i metoden
                    string stmt = ReturnStringForVisitorSearch(vs,sqlStart, offset, hitsPerPage);

                    // joinar de olika delarna av söksträngen 
                    stmt = stmt + sqlEnd;

                    //skapar new Command och skickar tillbaka sträng och aktuella parametrar
                    NpgsqlCommand cmd = ReturnCMDForVisitorSearch(vs, offset, hitsPerPage, stmt);

                    conn.Open();
                    cmd.Connection = conn;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vs = new VisitorSearch
                            {
                                Visitor = new Visitor
                                {
                                    VisitorID = (reader["visitor_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null,
                                    Firstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(1) : null,
                                    Lastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(2) : null,
                                    Company = (reader["company"] != DBNull.Value) ? reader.GetString(3) : null,
                                    CheckIn = (reader["check_in_old"] != DBNull.Value) ? reader.GetDateTime(4) : (DateTime?)null,
                                    CheckOut = (reader["check_out_old"] != DBNull.Value) ? reader.GetDateTime(5) : (DateTime?)null,

                                },
                                Employee = new Employee
                                {
                                    EmployeeID = (reader["employee_id"] != DBNull.Value) ? reader.GetInt32(6) : (int?)null,
                                    Firstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(7) : null,
                                    Lastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(8) : null,

                                },
                                BookedMeeting = new BookedMeeting
                                {
                                    MeetingDepartment = (reader["meeting_department"] != DBNull.Value) ? reader.GetString(9) : null,
                                }
                            };
                            vsr.VisitorSearches.Add(vs);
                        }
                    }
                    scope.Complete();
                }
            }
            return vsr.VisitorSearches;
        }

        /// <summary>
        /// Räknar fullständigt antal träffar för söket och påverkas inte av pagingens LIMIT och OFFSET. 
        /// Ingår i ett transactionsScope med VisitorSearch()
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="offset"></param>
        /// <param name="hitsPerPage"></param>
        /// <returns></returns>
        static private int CountDBHits(VisitorSearch vs, int offset, int hitsPerPage)
        {
            VisitorSearchReturn vsr = new VisitorSearchReturn();

            // Start på countQueryn
            string sqlStart = "SELECT COUNT(*) ";

            // Komplett söksträng retuneras
            string stmt = ReturnStringForVisitorSearch(vs,sqlStart, offset, hitsPerPage);

          


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    NpgsqlCommand cmd = ReturnCMDForVisitorSearch(vs, offset, hitsPerPage, stmt);

                    conn.Open();
                    cmd.Connection = conn;

                    vsr.CountTotalHits = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return vsr.CountTotalHits;
            }
        }

        /// <summary>
        /// Publik metod att kalla för att få ett komplett sökresultat då de båda metoderna enbart retunerar vad som är logiskt för dem själva 
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="offset"></param>
        /// <param name="hitsPerPage"></param>
        /// <returns></returns>
        public static VisitorSearchReturn CompleteStatSearch(VisitorSearch vs, int offset, int hitsPerPage)
        {
            VisitorSearchReturn vsr = new VisitorSearchReturn
            {
                CountTotalHits = CountDBHits(vs, offset, hitsPerPage),
                VisitorSearches = GetVisitorSearchInfo(vs, offset, hitsPerPage)
            };
            return vsr;
        }

        /// <summary>
        /// Skapar NPSQL-command från stmt (fullständig query) och lägger till cmd.parametrarna
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="offset"></param>
        /// <param name="hitsPerPage"></param>
        /// <param name="stmt"></param>
        /// <returns></returns>
        private static NpgsqlCommand ReturnCMDForVisitorSearch(VisitorSearch vs, int offset, int hitsPerPage, string stmt)
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

        private static string ReturnStringForVisitorSearch(VisitorSearch vs, string sqlStart, int offset, int hitsPerPage)// Testar om man kan lägga till sqlStart i denna metoden
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




        #region MyRegion


        ///// <summary>
        ///// Bygger den delen av queryn som är generell för båda metoderna för count och visitorSearch. 
        ///// Lägger till de addOns som ska med. 
        ///// </summary>
        ///// <param name="vs"></param>
        ///// <param name="offset"></param>
        ///// <param name="hitsPerPage"></param>
        ///// <returns></returns>
        //private static string ReturnStringForVisitorSearch(VisitorSearch vs, string sqlStart, int offset, int hitsPerPage)// Testar om man kan lägga till sqlStart i denna metoden
        //{// Funkar
        //    var sql = new StringBuilder();

        //    sql.AppendLine(
        //        " FROM visitor" +
        //        " JOIN visitor_meeting ON visitor.visitor_id = visitor_meeting.visitor_id" +
        //        " JOIN booked_meeting ON visitor_meeting.booked_meeting_id = booked_meeting.booked_meeting_id" +
        //        " JOIN employee ON booked_meeting.visit_responsible = employee.employee_id" +
        //        " WHERE 1=1 ");

        //    if (vs.SearchParameters.DateOutFrom != null)
        //    {
        //        sql.AppendLine(" AND visitor.check_out_old BETWEEN @dateOutFrom AND @dateOutTo ");
        //    }
        //    if (vs.SearchParameters.DateInFrom != null)
        //    {
        //        sql.AppendLine(" AND visitor.check_in_old BETWEEN @dateInFrom AND @dateInTo");
        //    }
        //    if (vs.Visitor.Firstname != "")
        //    {
        //        sql.AppendLine(" AND visitor.firstname = @vFirstname");
        //    }
        //    if (vs.Visitor.Lastname != "")
        //    {
        //        sql.AppendLine(" AND visitor.lastname = @vLastname");
        //    }
        //    if (vs.Employee.Firstname != "")
        //    {
        //        sql.AppendLine(" AND employee.firstname = @eFirstname");
        //    }
        //    if (vs.Employee.Lastname != "")
        //    {
        //        sql.AppendLine(" AND employee.lastname = @eLastname");
        //    }
        //    if (vs.Employee.EmployeeID != (int?)null)
        //    {
        //        sql.AppendLine(" AND employee.employee_id = @eID");
        //    }
        //    if (vs.BookedMeeting.MeetingDepartment != "")
        //    {
        //        sql.AppendLine(" AND booked_meeting.meeting_department = @mDepartment");
        //    }
        //    if (vs.Visitor.NotCheckedOut)
        //    {
        //        sql.AppendLine(" AND visitor.check_out_old IS NULL");
        //    }
        //    if (vs.Visitor.BadgeReturned)
        //    {
        //        sql.AppendLine(" AND visitor.badge_returned IS FALSE");
        //    }
        //    string newSql = sqlStart + sql.ToString();

        //    return newSql;
        //}
        #endregion
    }
}


