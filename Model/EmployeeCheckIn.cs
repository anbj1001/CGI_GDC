using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAdmin.Model

{
    class EmployeeCheckIn
    {
        public int CheckInID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }

    }
}
