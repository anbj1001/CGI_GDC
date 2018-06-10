using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using DatabaseAdmin.DatabaseConnections;


namespace DatabaseAdmin.Model
{
    class EmployeeHelperClass
    {
        public string GetColumn(ColumnsInEmployee columnsInEmployee)
        {
            switch (columnsInEmployee)
            {
                case ColumnsInEmployee.firstname:
                    return "firstname";

                case ColumnsInEmployee.lastname:
                    return "lastname";

                case ColumnsInEmployee.address:
                    return "address";

                case ColumnsInEmployee.department:
                    return "department";

                case ColumnsInEmployee.email:
                    return "email";

                case ColumnsInEmployee.employee_id:
                    return "employee_id";

                case ColumnsInEmployee.phonenumber:
                    return "phonenumber";

                case ColumnsInEmployee.role:
                    return "role";

                case ColumnsInEmployee.team:
                    return "team";

                default:
                    break;
            }
            throw new Exception("Cannot find the column name");
        }
    }
}
