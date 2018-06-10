using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAdmin.Model;

namespace DatabaseAdmin.Model
{
    class Admin : Employee
    {
        public int? AdminID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
