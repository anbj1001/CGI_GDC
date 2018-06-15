using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;
using Npgsql;
using DatabaseAdmin.Model;
using DatabaseAdmin.Enums;
using static DatabaseAdmin.DatabaseConnections.GetVisitorInfo;
using static DatabaseAdmin.DatabaseConnections.GetEmployeeInfo;
using static DatabaseAdmin.DatabaseConnections.Encryptor;
using System.Xaml;
using DatabaseAdmin.DatabaseConnections;
using FirstFloor.ModernUI;

namespace DatabaseAdmin.Model
{
     public class VisitorSearch
    {
         public int? VisitorID { get; set; }
         public string VisitorFirstname { get; set; }
         public string VisitorLastname { get; set; }
         public string Company { get; set; }
         public DateTime? VisitorCheck_in { get; set; }
         public DateTime? VisitorCheck_out { get; set; }
         public int? EmployeeID { get; set; }
         public string EmployeeFirstname { get; set; }
         public string EmployeeLastname { get; set; }
         public string MeetingDepartment { get; set; }



      
    }
}