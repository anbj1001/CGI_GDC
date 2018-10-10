using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Transactions;
using Npgsql;

namespace DatabaseAdmin.Model
{

    static class GetEmployeeInfo
    {

        /// <summary>
        /// Hämtar EmployeeID. Är med i en transactionScope för att skapa ett fullständigt drop-in besök
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int? GetEmployeeID(Employee e)
        {
            string stmt = "SELECT employee_id FROM employee WHERE firstname = @eFirstname AND lastname = @eLastname";

            using (TransactionScope scope = new TransactionScope())
            {

                using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
                {
                    using (var cmd = new NpgsqlCommand(stmt, conn))
                    {
                        cmd.Parameters.AddWithValue("@eFirstname", e.Firstname);
                        cmd.Parameters.AddWithValue("@eLastname", e.Lastname);

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                e = new Employee
                                {
                                    EmployeeID = (reader["employee_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null,
                                };
                            }
                        }
                        return e.EmployeeID;
                    }
                }
            }
        }

        /// <summary>
        /// Hämtar all information om alla anställda i db
        /// </summary>
        /// <returns></returns>
        static public List<Employee> GetAllEmployee()
        {
            List<Employee> employees = new List<Employee>();
            Employee e;

            string stmt = "SELECT employee_id, firstname, lastname, address, phonenumber, email, department, team FROM employee";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {

                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        e = new Employee
                        {
                            EmployeeID = (reader["employee_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null,
                            Firstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(1) : null,
                            Lastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(2) : null,
                            Address = (reader["address"] != DBNull.Value) ? reader.GetString(3) : null,
                            PhoneNumber = (reader["phonenumber"] != DBNull.Value) ? reader.GetString(4) : null,
                            Email = (reader["email"] != DBNull.Value) ? reader.GetString(5) : null,
                            Department = (reader["department"] != DBNull.Value) ? reader.GetString(6) : null,
                            Team = (reader["team"] != DBNull.Value) ? reader.GetString(7) : null,
                        };
                        employees.Add(e);
                    }
                }
                return employees;
            }
        }

        /// <summary>
        /// Hämtar tid och avdelning för alla möten med den angivna anställda 
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        static public VisitorSearchReturn GetTimeAndDepartment(Employee emp)
        {
            BookedMeeting bm = new BookedMeeting();
            //stringBuilder för att man ska kunna söka med bara för eller efternamn 
            var sql = new StringBuilder();
            sql.AppendLine("SELECT booked_meeting.meeting_department, booked_meeting.time_start, booked_meeting.booked_meeting_id, visit_responsible" +
                " FROM(booked_meeting INNER JOIN employee ON booked_meeting.visit_responsible = employee.employee_id)" +
                " WHERE 1 = 1");


            if (emp.Firstname != "")
            {
                sql.AppendFormat(" AND employee.firstname = @eFirstname");
                sql.AppendLine();
            }
            if (emp.Lastname != "")
            {
                sql.AppendFormat(" AND employee.lastname = @eLastname");
                sql.AppendLine();
            }
            VisitorSearchReturn vsr = new VisitorSearchReturn();
            VisitorSearch vs = new VisitorSearch();
            var stmt = sql.ToString();

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    if (emp.Firstname != "")
                    {
                        cmd.Parameters.AddWithValue("@eFirstname", emp.Firstname);
                    }
                    if (emp.Lastname != "")
                    {
                        cmd.Parameters.AddWithValue("@eLastname", emp.Lastname);
                    }

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vs = new VisitorSearch
                            {
                                BookedMeeting = new BookedMeeting
                                {
                                    MeetingDepartment = (reader["meeting_department"] != DBNull.Value) ? reader.GetString(0) : null,
                                    TimeStart = (reader["time_start"] != DBNull.Value) ? reader.GetTimeSpan(1) : (TimeSpan?)null,
                                    BookedMeetingID = (reader["booked_meeting_id"] != DBNull.Value) ? reader.GetInt32(2) : (int?)null,
                                },
                                Employee = new Employee
                                {
                                    EmployeeID = (reader["visit_responsible"] != DBNull.Value) ? reader.GetInt32(3) : (int?)null,
                                    Firstname = emp.Firstname,
                                    Lastname = emp.Lastname,

                                }
                            };
                            vsr.VisitorSearches.Add(vs);
                        }
                    }
                }
            }
            return vsr;
        }

        /// <summary>
        /// Uppdaterar alla värden för en anställd i varje query
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int UpdateEmployeeInfo(Employee e)
        {
            // Måste ju kunna göras snyggare....
            int result;

            string stmt = "UPDATE employee  SET firstname = @eFirstname, lastname = @eLastname, address = @eAddress, phonenumber = @ePhonenumber, email = @eEmail," +
                " department = @eDepartment, team = @eTeam WHERE employee_id = @employee_id";


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    cmd.Parameters.AddWithValue("@eFirstname", e.Firstname);
                    cmd.Parameters.AddWithValue("@eLastname", e.Lastname);
                    cmd.Parameters.AddWithValue("@eAddress", e.Address);
                    cmd.Parameters.AddWithValue("@ePhonenumber", e.PhoneNumber);
                    cmd.Parameters.AddWithValue("@eEMail", e.Email);
                    cmd.Parameters.AddWithValue("@eDepartment", e.Department);
                    cmd.Parameters.AddWithValue("@eTeam", e.Team);
                    cmd.Parameters.AddWithValue("@employee_id", e.EmployeeID);
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        /// <summary>
        /// För admin inloggning. Retunerar en int för att kunna bekräfta att querien är genomförd och admin är inloggad
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static public int? AdminLogIn(string username, string password)
        {
            int? result;
            string stmt = "SELECT admin_login_id FROM admin_login WHERE username = @username AND login_password = @password ";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        /// <summary>
        /// Används ej för tillfället då funktionen ej är implementerad i koden fn.
        /// Lägger till en anställd som admin. 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int MakeToAdmin(Admin a)
        {
            int result;
            string stmt = "INSERT INTO admin_login(fk_employee, login_password, username)" +
                        " VALUES((SELECT employee_id FROM employee WHERE firstname = @firstname AND lastname = @lastname AND employee_id = @employee_id)," +
                        "  @password, @username)";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    cmd.Parameters.AddWithValue("@employee_id", a.EmployeeID);
                    cmd.Parameters.AddWithValue("@firstname", a.Firstname);
                    cmd.Parameters.AddWithValue("@lastname", a.Lastname);
                    cmd.Parameters.AddWithValue("@password", a.Password);
                    cmd.Parameters.AddWithValue("@username", a.Username);
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        /// <summary>
        /// Raderar all information om en anställd. ( Med CASCADE i db )
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int DeleteOneEmployee(Employee e)
        {
            int result;
            string stmt = "DELETE FROM employee WHERE employee_id = @employee_id";


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    cmd.Parameters.AddWithValue("@employee_id", e.EmployeeID);
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        /// <summary>
        /// Skapar en ny anställd 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int CreateEmployee(Employee e)
        {
            int result;
            string stmt = "INSERT INTO employee(firstname, lastname, address, phonenumber, email, department, team)" +
                " VALUES( @firstname, @lastname, @address, @phonenumber, @email, @department, @team)";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    cmd.Parameters.AddWithValue("@firstname", e.Firstname);
                    cmd.Parameters.AddWithValue("@lastname", e.Lastname);
                    cmd.Parameters.AddWithValue("@address", e.Address);
                    cmd.Parameters.AddWithValue("@phonenumber", e.PhoneNumber);
                    cmd.Parameters.AddWithValue("@email", e.Email);
                    cmd.Parameters.AddWithValue("@department", e.Department);
                    cmd.Parameters.AddWithValue("@team", e.Team);
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

    }
}
