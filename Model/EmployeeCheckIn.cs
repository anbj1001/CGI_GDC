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
        public int? EmployeeID { get; set; } // är väl employeeObjektets ID? Den speciella employeen måste väl ha koppling till incheckningen? Man skulle kanske kunna ha en lista med checkIns i 
        // konstruktorn i employee? Men då måste man ha en tabell i db oxå? 
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
    }
}
