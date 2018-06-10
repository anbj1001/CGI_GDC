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
using static DatabaseAdmin.DatabaseConnections.GetVisitorInfo;
using static DatabaseAdmin.DatabaseConnections.GetEmployeeInfo;
using static DatabaseAdmin.DatabaseConnections.Encryptor;
using System.Xaml;
using DatabaseAdmin.DatabaseConnections;

namespace DatabaseAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        Window1 window1 = new Window1();

        EmployeeHelperClass eHC = new EmployeeHelperClass();

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {

            string fName = "Ebba";
            string lName = "Johansson";
            try
            {
                //List<BookedMeeting> bookedMeetings = GetTimeAndDepartment(fName, lName);
                //List<Employee> employees = GetAllEmployeeMeeting();
                List<Visitor> visitorMeetings = GetAllVisitorMeeting();
                //List<EmployeeCheckIn> checkIns = GetAllEmployeeCheckIn();
                //List<Visitor> visitors = GetAllVisitor();
                //List<BookedMeeting> bookedMeetings = GetAllBookedMeeting();
                //List<Employee> employees = GetAllEmployee();
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);

            }


        }
        /*
         * TEST CODE TO UPDATE AN EMPLOYEER (INCL SWITCH-STATEMENT)
         * Workes for column firstname
        int ID = 1012;
        string column = eHC.GetColumn(ColumnsInEmployee.firstname);
        string changedValue = "Maja";
        Employee ee = GetOneEmployee(ID);
        int i = GetEmployeeInfo.UpdateEmployeeInfo(ee, column, changedValue);
        */

        /* TEST CODE TO ADD AN EMPLOYEE TO TABLE ADMIN
        *WORKES!
        Admin admin;
        admin = new Admin()
        {används även för UPDATE
            Firstname = "Wilma",
            Lastname = "Olsson",
            EmployeeID = 1011,
            Username = "Admin3",
            Password = Encryptor.MD5Hash("Admin3")
        };
        //int result = GetEmployeeInfo.MakeToAdmin(admin); 
        */


        /*TEST CODE FOR ADMINLOGIN
        * WORKES!
        {
            string username = "Admin3";
            string password = Encryptor.MD5Hash("Admin3");
        Admin a = GetEmployeeInfo.AdminLogIn(admin, username, password);
            if (a.AdminID != null)
            {

                this.Hide();
                window1.Show();
            }
            else
                MessageBox.Show("Du angav fel lösenord eller användarnamn");
        }
        */




        /* TEST CODE FOR GET LISTS FROM DB
         * WORKES!
                */
    }
}


