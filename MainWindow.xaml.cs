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
using static DatabaseAdmin.DatabaseConnections.GetBookedMeetingInfo;
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
            //Loaded += MyWindow_Loaded;

            cBoxTry.ItemsSource = GetAllEmployeeMeeting(); // För att printa employee till comboboxen

        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //frame.NavigationService.Navigate(new LoginPage());
        }

        EmployeeHelperClass eHC = new EmployeeHelperClass();

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            //Visitor v;
            //v = new Visitor
            //{
            //    Firstname = "Kristian",
            //    Lastname = "Norqvist",
            //    VisitorID = 10020,
            //    BadgeReturned = true
            //};


            Employee emp;
            emp = new Employee()
            {//används även för UPDATE
                Firstname = "Klas",
                Lastname = "Klasson",
                Address = "Klassonvägen 33",
                PhoneNumber = "11223344",
                Email = "klas@klasson.se",
                Role = "Praktikant",
                Department = "Ekonomi",
                Team = "N/A"
                
            };
            int result = CreateEmployee(emp);
            //List<VisitorSearch> visitorsSearch = GetVisitorSearchInfo(vFirstname, vLastname, eFirstname, eLastname, eID, vCompany, mDepartment, checkedOut);
        }
            /*TEST FÖR SETBOOKEDMEETING . MÅSTE KOLLA UPP DATETIME AND TIMESTAMP
             
            Employee emp;
            emp = new Employee
            {//används även för UPDATE
                EmployeeID = 1020,
                Firstname = "Selma",
                Lastname = "Lindberg",

            };

            string vFirstname = "Annica";
            string vLastname = "Alienus";
            string vCompany = "MyProject";
            string vCity = "Mölndal";
            string mDepartment = "IT";
            string date ="2018-07-06";
            DateTime timeStart = Convert.ToDateTime("15:30");


            int result = CreateBookedMeeting(mDepartment, date, timeStart,emp);
            */
            //string eFirstname = null;
            //string eLastname = null;
            //int? eID = 1006;
            //DateTime? checkedOut = null;



        /// <summary>
        /// Test för att sätta datum 3 månader tillbaka. 
        /// skitmetod.... värdelös
        /// </summary>
        ///// <returns></returns>
        //private DateTime? test()
        //{
        //    //DateTime? meetingDate = dpickFrom.;
        //    //date = DateTime.Now;
        //    //date = date.AddMonths(-3);

        //    return date;
        //}


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
            DateTime? date = (DateTime)dpickFrom.SelectedDate;
            if (date.HasValue)
            {
                string formatted = date.Value.ToString("yyyy-MM-dd");
            }

        }
        private void dpickTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void btnStartPage_Click(object sender, RoutedEventArgs e)
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


