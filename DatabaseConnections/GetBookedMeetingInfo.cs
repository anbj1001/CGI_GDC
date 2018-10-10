using System;
using System.Configuration;
using Npgsql;
using System.Text;
using System.Transactions;

namespace DatabaseAdmin.Model
{
    static class GetBookedMeetingInfo
    {
        /// <summary>
        /// Bokar ett möte NU för ett drop-in besök. Är i ett transactionsScope som skapar ett fullständigt drop-in besök
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static int BookMeetingNow(int? employeeID)
        {
            int result;
            string stmt = "INSERT INTO booked_meeting(visit_responsible, meeting_department, date, time_start)"
                          + " VALUES(@visit_responsible, (select employee.department FROM employee WHERE employee.employee_id = @visit_responsible),"
                          + " 'now()', 'now()') RETURNING booked_meeting_id";

            using (TransactionScope scope = new TransactionScope())
            {

                using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(stmt, conn))
                    {
                        cmd.Parameters.AddWithValue("@visit_responsible", employeeID);

                        result = (int)cmd.ExecuteScalar();
                    }
                }
                return result;
            }
        }
    }
}


