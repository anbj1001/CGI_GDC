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
            cBoxTry.ItemsSource = GetAllEmployeeMeeting(); // För att printa employee till comboboxen

        }
        Window1 window1 = new Window1();

        EmployeeHelperClass eHC = new EmployeeHelperClass();

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            string vFirstname = ""; 
            string vLastname = "";
            string eFirstname = "Clara";
            string eLastname = "Jonsson";
            
          

            List<VisitorSearch> visitorsSerach = GetVisitorSearchInfo(vFirstname);
        }
         
        /// <summary>
        /// Test för att sätta datum 3 månader tillbaka. 
        /// skitmetod.... värdelös
        /// </summary>
        /// <returns></returns>
        private DateTime? test()
        {
            DateTime date;
            date = DateTime.Now;
            date = date.AddMonths(-3);

            return date;
        }


        private void cBoxTry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /* Combobox som visar alla employees
            //List<Employee> employees = GetAllEmployeeMeeting();
            //cBoxTry.ItemsSource = employees;
            */
        }

        /*
         * TEST FÖR ATT ANVÄNDA DATEPICKER SOM DATUMVÄLJARE (ANVÄNDS EJ)*/
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;

            if (picker == null)
            {
                this.Title = "Inget datum valt";
            }
            else
            {
                this.Title = date.Value.ToShortDateString();
            }
        }
        private void dpickTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
      

            /*
             * TEST CODE FOR UPDATE EMPLOYEE. INKL ENUM DEKLARATION
            Employee emp = new Employee
            {
                EmployeeID = 1010,
                Firstname = "Freja",
                Lastname = "Larsson"
            };
            string input;

            input = eHC.GetColumn(DatabaseColumns.firstname);
            string firstname = "Sture-Berit";

            int result = UpdateEmployeeInfo(emp, input, firstname);
            */
        /*
         * TEST CODE FOR CHECKOUT VISITOR (AND SOM EMPLOYEE-PROP FROM  ADD ADMIN)
                    Visitor v;
                    v = new Visitor()
                    {//används även för UPDATE
                        VisitorID = 10008,
                        BadgeReturned = true
                        //Firstname = "Wilma",
                        //Lastname = "Olsson",
                        //EmployeeID = 1011,
                        //Username = "Admin3",
                        //Password = Encryptor.MD5Hash("Admin3")
                    };
                    //string fName = "Ebba";
                    //string lName = "Johansson";
                    int i = GetVisitorInfo.CheckOutVisitor(v);
                    */
        /*
* TEST CODE TO UPDATE AN EMPLOYEER (INCL SWITCH-STATEMENT)
* Workes for column firstname
int ID = 1012;
string column = eHC.GetColumn(DatabaseColumns.firstname);
string changedValue = "Maja";
Employee ee = GetOneEmployee(ID);
int i = GetEmployeeInfo.UpdateEmployeeInfo(ee, column, changedValue);
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
            try
            {
                //List<BookedMeeting> bookedMeetings = GetTimeAndDepartment(fName, lName);
                List<Employee> employees = GetAllEmployeeMeeting();
                //List<Visitor> visitorMeetings = GetAllVisitorMeeting();
                //List<EmployeeCheckIn> checkIns = GetAllEmployeeCheckIn();
                //List<Visitor> visitors = GetAllVisitor();
                //List<BookedMeeting> bookedMeetings = GetAllBookedMeeting();
                //List<Employee> employees = GetAllEmployee();
            }
            catch (PostgresException ex)
            {
                MessageBox.Show(ex.Message);

            }
                */
    }
}


