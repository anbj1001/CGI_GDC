using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using DatabaseAdmin.DatabaseConnections;
using DatabaseAdmin.Enums;


namespace DatabaseAdmin.Model
{
     public class EmployeeHelperClass
    {
        public string GetColumn(DatabaseColumns columnsInEmployee)
        {
            switch (columnsInEmployee)
            {
                case DatabaseColumns.firstname:
                    return "firstname";

                case DatabaseColumns.lastname:
                    return "lastname";

                case DatabaseColumns.address:
                    return "address";

                case DatabaseColumns.department:
                    return "department";

                case DatabaseColumns.email:
                    return "email";

                case DatabaseColumns.employee_id:
                    return "employee_id";

                case DatabaseColumns.phonenumber:
                    return "phonenumber";

                case DatabaseColumns.role:
                    return "role";

                case DatabaseColumns.team:
                    return "team";

                default:
                    break;
            }
            throw new Exception("Cannot find the column name");
        }
    }
}
