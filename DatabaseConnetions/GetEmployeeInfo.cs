using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAdmin.Model;
using DatabaseAdmin.Enums;
using Npgsql;

namespace DatabaseAdmin.DatabaseConnections
{
    static class GetEmployeeInfo
    {

        static public List<EmployeeCheckIn> GetAllEmployeeCheckIn()
        {// Funkar

            List<EmployeeCheckIn> checkIns = new List<EmployeeCheckIn>();
            EmployeeCheckIn eci;


            string stmt = "SELECT employee_id, check_in, check_out FROM employee_check_in";

            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {

                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        eci = new EmployeeCheckIn()
                        {

                            //CheckInID = reader.GetInt32(0),
                            EmployeeID = (reader["employee_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null,
                            CheckIn = (reader["check_in"] != DBNull.Value) ? reader.GetTimeStamp(1).ToDateTime() : (DateTime?)null,
                            CheckOut = (reader["check_out"] != DBNull.Value) ? reader.GetTimeStamp(2).ToDateTime() : (DateTime?)null,

                        };
                        checkIns.Add(eci);
                    }
                }
                return checkIns;
            }

        }

        static public List<Employee> GetAllEmployeeMeeting()
        {// Funkar!! 

            List<Employee> employees = new List<Employee>();
            BookedMeeting bm;
            Employee e;


            string stmt = "SELECT employee.employee_id, employee.firstname, employee.lastname, employee.department, employee.phonenumber, booked_meeting.booked_meeting_id," +
                "booked_meeting.meeting_department, booked_meeting.date, booked_meeting.time_start " +
                "FROM((employee_meeting INNER JOIN employee ON employee.employee_id = employee_meeting.employee_id) " +
                "FULL JOIN booked_meeting ON employee_meeting.booked_meeting_id = booked_meeting.booked_meeting_id) " +
                "GROUP BY firstname, lastname,department, phonenumber, meeting_department, employee.employee_id,booked_meeting.booked_meeting_id, date, time_start";


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

                            BookedMeetingID = (reader["booked_meeting_id"] != DBNull.Value) ? reader.GetInt32(5) : (int?)null,
                            MeetingDepartment = (reader["meeting_department"] != DBNull.Value) ? reader.GetString(6) : null,
                            MeetingDate = (reader["date"] != DBNull.Value) ? reader.GetDateTime(7) : (DateTime?)null,
                            TimeStart = (reader["time_start"] != DBNull.Value) ? reader.GetTimeSpan(8) : (TimeSpan?)null,

                        };
                        e = new Employee(bm)
                        {

                            EmployeeID = (reader["employee_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null,
                            Firstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(1) : null,
                            Lastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(2) : null,
                            Department = (reader["department"] != DBNull.Value) ? reader.GetString(3) : null,
                            PhoneNumber = (reader["phonenumber"] != DBNull.Value) ? reader.GetString(4) : null,

                        };
                        employees.Add(e);

                    }
                }
                return employees;
            }
        }

        static public List<BookedMeeting> GetTimeAndDepartment(string firstname, string lastname)
        {// Funkar
            BookedMeeting bm;
            List<BookedMeeting> bookedMeetings = new List<BookedMeeting>();


            string stmt = "SELECT booked_meeting.time_start, booked_meeting.meeting_department"
                + " FROM ((employee INNER JOIN employee_meeting ON employee.employee_id = employee_meeting.employee_id)"
                + " INNER JOIN booked_meeting ON employee_meeting.booked_meeting_id = booked_meeting.booked_meeting_id)"
                + " GROUP BY time_start, meeting_department, firstname, lastname "
                + " HAVING employee.firstname = @firstname AND employee.lastname = @lastname";


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    cmd.Parameters.AddWithValue("@firstname", firstname);
                    cmd.Parameters.AddWithValue("@lastname", lastname);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bm = new BookedMeeting
                            {

                                TimeStart = (reader["time_start"] != DBNull.Value) ? reader.GetTimeSpan(0) : (TimeSpan?)null,
                                MeetingDepartment = (reader["meeting_department"] != DBNull.Value) ? reader.GetString(1) : null,

                            };
                            bookedMeetings.Add(bm);
                        }
                    }
                    return bookedMeetings;
                }
            }
        }

        public static int UpdateEmployeeInfo(Employee e, string column, string changedValue)
        {// Funkar!! kan column sättas med att man på något sätt ger testboxen ett variabelnamn för ett kolumnnamn 
            int result;


            string stmt = $"UPDATE employee  SET { column}  = @changedValue WHERE employee_id = @employee_id OR firstname = @firstname AND lastname = @lastname";


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {

                    cmd.Parameters.AddWithValue("employee_id", e.EmployeeID);
                    cmd.Parameters.AddWithValue("firstname", e.Firstname);
                    cmd.Parameters.AddWithValue("@lastname", e.Lastname);
                    cmd.Parameters.AddWithValue("changedValue", changedValue);
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        static public Employee GetOneEmployeeFromID(int employeeID)
        {// Funkar

            string stmt = "SELECT employee_id, firstname, lastname,  address, phonenumber, email, role, department, team FROM employee WHERE employee_id = @employee_id";
            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                Employee e;
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    cmd.Parameters.AddWithValue("@employee_id", employeeID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            e = new Employee()
                            {
                                EmployeeID = (reader["employee_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null,
                                Firstname = (reader["firstname"] != DBNull.Value) ? reader.GetString(1) : null,
                                Lastname = (reader["lastname"] != DBNull.Value) ? reader.GetString(2) : null,
                                Address = (reader["address"] != DBNull.Value) ? reader.GetString(3) : null,
                                PhoneNumber = (reader["phonenumber"] != DBNull.Value) ? reader.GetString(4) : null,
                                Email = (reader["email"] != DBNull.Value) ? reader.GetString(5) : null,
                                Role = (reader["role"] != DBNull.Value) ? reader.GetString(6) : null,
                                Department = (reader["department"] != DBNull.Value) ? reader.GetString(7) : null,
                                Team = (reader["team"] != DBNull.Value) ? reader.GetString(8) : null,

                            };
                            //employees.Add(e);
                            return e;
                        }
                        throw new Exception("The querie return null");
                    }
                }
            }
        }

        static public Admin AdminLogIn(Admin a, string username, string password)
        {//Funkar

            string stmt = "SELECT admin_login_id FROM admin_login WHERE username = @username AND login_password = @password ";



            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {

                conn.Open();

                using (var cmd = new NpgsqlCommand(stmt, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            a.AdminID = (reader["admin_login_id"] != DBNull.Value) ? reader.GetInt32(0) : (int?)null;

                        }
                    }
                }
                return a;

            }
        }

        public static int MakeToAdmin(Admin a)
        {// Funkar
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

        public static int DeleteOneEmployee(Employee e)
        {//Funkar men ska det vara andra / fler parametrar än employee_id?
            int result;


            string stmt = $"DELETE FROM employee WHERE employee_id = @employee_id";


            using (var conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(stmt, conn))
                {

                    cmd.Parameters.AddWithValue("@employee_id", e.EmployeeID);
                    //cmd.Parameters.AddWithValue("@firstname", e.Firstname);
                    //cmd.Parameters.AddWithValue("@lastname", e.Lastname);
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        public static int CreateEmployee(Employee e)
        {// Funkar
            int result;
            string stmt = "INSERT INTO employee(firstname, lastname, address, phonenumber, email, role, department, team)" +
                " VALUES( @firstname, @lastname, @address, @phonenumber, @email, @role, @department, @team)";

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
                    cmd.Parameters.AddWithValue("@role", e.Role);
                    cmd.Parameters.AddWithValue("@department", e.Department);
                    cmd.Parameters.AddWithValue("@team", e.Team);
                    result = cmd.ExecuteNonQuery();

                }
            }
            return result;
        }

    }
}
