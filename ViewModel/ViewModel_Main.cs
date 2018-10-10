using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using DatabaseAdmin.Model;
using static DatabaseAdmin.Model.GetVisitorInfo;
using static DatabaseAdmin.Model.GetEmployeeInfo;
using static DatabaseAdmin.Model.GetBookedMeetingInfo;
using static DatabaseAdmin.Model.CalculatePaging;



namespace DatabaseAdmin.ViewModel
{
    public class ViewModel_Main
    {

        public Employee Employee // kan ju inte konvertera Employee till string. Kan man använda var? 
        { get; set; }
        public Visitor Visitor { get; set; }
        public BookedMeeting BookedMeeting { get; set; }
        public VisitorSearch VisitorSearch { get; set; }
        public VisitorSearchReturn VisitorSearchReturn { get; set; }
        public ParametersVisitorSearch ParameterVisitorSearch { get; set; }


        // Statistiktabben börjar
        /// <summary>
        /// Skickar med de parametrar som användaren vill använda för att söka efter till metoden för querin till databasen.
        /// Retunerar informationen som matchar sökpreferensen
        /// </summary>
        private void SendSearchInfo()
        {
            try
            {

                //CollectionViewSource itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));


                VisitorSearch = InputVariables();
                VisitorSearchReturn = GetVisitorSearchInfo(VisitorSearch, Offset, HitsPerPage);//returnerar ETT objekt
                int countTotalHits = VisitorSearchReturn.CountTotalHits;
                ReturnTotalPages(countTotalHits);
                lblTotalPages = TotalPages.ToString();

                lblTotalVisitors = countTotalHits.ToString();
                //itemCollectionViewSource.Source = VisitorSearchReturn.VisitorSearches;//printar listan i objektet. Visitor, Employee och BookedMeeting

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }

        }
        private string lblTotalVisitors;
        private string lblTotalPages;
        private int txtStatEmployeeID;
        private string txtStatEmployeeFirstname;
        private string txtStatEmployeeLastname;
        private string txtStatVisitorFirstname;
        private string txtStatVisitorLastname;
        private string txtStatVisitorCompany;
        private string txtStatEmployeeDepartment;
        private bool chkBadgeReturned;
        private bool chkCheckedOut;
        private DateTime dtPickInFrom;
        private DateTime dtPickInTo;
        private DateTime dtPickOutFrom;
        private DateTime dtPickOutTo;
        private VisitorSearch InputVariables()
        {
            VisitorSearch = new VisitorSearch
            {

                Employee = new Employee
                {
                    EmployeeID = txtStatEmployeeID,
                    Firstname = txtStatEmployeeFirstname,
                    Lastname = txtStatEmployeeLastname,
                },

                Visitor = new Visitor
                {
                    Firstname = txtStatVisitorFirstname,
                    Lastname = txtStatVisitorLastname,
                    Company = txtStatVisitorCompany,
                    BadgeReturned = (bool)chkBadgeReturned,
                    NotCheckedOut = (bool)chkCheckedOut
                },
                BookedMeeting = new BookedMeeting
                {
                    MeetingDepartment = txtStatEmployeeDepartment
                },
                SearchParameters = new ParametersVisitorSearch
                {
                    DateInFrom = dtPickInFrom,
                    DateInTo = dtPickInTo,
                    DateOutFrom = dtPickOutFrom,
                    DateOutTo = dtPickOutTo
                }

            };


            return VisitorSearch;
        }



        private void btnStatSearch_Click(object sender, RoutedEventArgs e)
        {
            SendSearchInfo();
            HandleBtnPageing();
        }

        //Hur löser jag clear från view?
        //private void btnStatCLearAll_Click(object sender, RoutedEventArgs e)
        //{
        //    txtStatEmployeeFirstname.Clear();
        //    txtStatEmployeeLastname.Clear();
        //    txtStatEmployeeDepartment.Clear();
        //    txtStatEmployeeID.Clear();
        //    txtStatVisitorCompany.Clear();
        //    txtStatVisitorFirstname.Clear();
        //    txtStatVisitorLastname.Clear();
        //}


        //Statistiktabben slutar


        // Personaltabben börjar

        private void GetEmployees()
        {
            //CollectionViewSource EmployeeCollectionViewSource = (CollectionViewSource)(FindResource("EmployeeCollectionViewSource"));
            List<Employee> emp = GetAllEmployee();
            //EmployeeCollectionViewSource.Source = emp;
        }

        private string txtUpdateEmployeeFirstname;
        private string txtUpdateEmployeeLastname;
        private string txtUpdateEmployeeAddress;
        private string txtUpdateEmployeePhonenumber;
        private string txtUpdateEmployeeEmail;
        private string txtUpdateEmployeeDepartment;
        private string txtUpdateEmployeeTeam;

        /// <summary>
        /// Uppdaterar information om en emoployee om den är vald i griden, annars skapas en ny
        /// lägger till nya värden att uppdatera. måste hämta infon från GetAllEmployee genom knapp "sök" hamnar de i griden. Väljer därifrån
        /// </summary>
        private void UpdateEmployee()
        {
            Employee selectedEmployee = new Employee();
            //selectedEmployee = (Employee)grdEmployee.SelectedItem;

            int result;

            if (selectedEmployee == null)
            {

                Employee = new Employee
                {
                    Firstname = txtUpdateEmployeeFirstname,
                    Lastname = txtUpdateEmployeeLastname,
                    Address = txtUpdateEmployeeAddress,
                    PhoneNumber = txtUpdateEmployeePhonenumber,
                    Email = txtUpdateEmployeeEmail,
                    Department = txtUpdateEmployeeDepartment,
                    Team = txtUpdateEmployeeTeam,
                };

                result = CreateEmployee(Employee);

                if (result > 0)
                {
                    MessageBox.Show("Informationen är sparad");
                }
            }

            else if (selectedEmployee != null)
            {
                Employee = new Employee
                {
                    EmployeeID = Convert.ToInt32(lblEmployeeId),
                    Firstname = txtUpdateEmployeeFirstname,
                    Lastname = txtUpdateEmployeeLastname,
                    Address = txtUpdateEmployeeAddress,
                    PhoneNumber = txtUpdateEmployeePhonenumber,
                    Email = txtUpdateEmployeeEmail,
                    Department = txtUpdateEmployeeDepartment,
                    Team = txtUpdateEmployeeTeam,

                };
                result = UpdateEmployeeInfo(Employee);

                if (result > 0)
                {
                    MessageBox.Show("Informationen är sparad");
                }

            }
        }
        private string lblEmployeeId;
        private void btnUserSearch_Click(object sender, RoutedEventArgs e)
        {
            GetEmployees();
        }
        /// <summary>
        /// OUTPUT
        /// Väljer objekt Employee för att hantera infon 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Employee = (Employee)grdEmployee.SelectedItem;

            lblEmployeeId = Employee.EmployeeID.ToString();
            txtUpdateEmployeeFirstname = Employee.Firstname;
            txtUpdateEmployeeLastname = Employee.Lastname;
            txtUpdateEmployeeAddress = Employee.Address;
            txtUpdateEmployeePhonenumber = Employee.PhoneNumber;
            txtUpdateEmployeeEmail = Employee.Email;
            txtUpdateEmployeeDepartment = Employee.Department;
            txtUpdateEmployeeTeam = Employee.Team;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            UpdateEmployee();
        }


        /// <summary>
        /// Raderar en användare
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Employee = new Employee();
            //Employee = (Employee)grdEmployee.SelectedItem;
            if (Employee == null)
            {
                MessageBox.Show("Välj en användare");
            }
            else
            {
                int result = DeleteOneEmployee(Employee);
                if (result > 0)
                {
                    MessageBox.Show("Användaren är raderad");
                }
                GetEmployees();
            }
        }

        // Hur göra med Clear();?
        //private void btnClearAll_Click(object sender, RoutedEventArgs e)
        //{
        //    txtUpdateEmployeeFirstname.Clear();
        //    txtUpdateEmployeeLastname.Clear();
        //    txtUpdateEmployeeAddress.Clear();
        //    txtUpdateEmployeePhonenumber.Clear();
        //    txtUpdateEmployeeEmail.Clear();
        //    txtUpdateEmployeeDepartment.Clear();
        //}

        // Personaltabben slutar

        //Besökstabben börjar
        private string txtVisitEFirstname;
        private string txtVisitELastname;
        private string txtVisitEDepartment;
        private string txtVisitVFirstname;
        private string txtVisitVLastname;
        private string txtVisitVCompany;

        private void GetMeeting()
        {
            //CollectionViewSource BookedMeetingsCollectionViewSource = (CollectionViewSource)(FindResource("BookedMeetingsCollectionViewSource"));

            Employee = new Employee
            {
                Firstname = /*txtVisitEFirstname*/ "Alice",
                Lastname = /*txtVisitELastname*/ "Hasselbom",
                Department = "Marknad"// det ska ju vara meetingDepartment för bokat möte men nu är det employeeDepartment
            };
            //vsr är en fullösning men för att få ut alla värderna i griden...
            VisitorSearchReturn = GetTimeAndDepartment(this.Employee);

            //BookedMeetingsCollectionViewSource.Source = VisitorSearchReturn.VisitorSearches;

        }

        //private void btnCheckInDropInVisit_Click(object sender, RoutedEventArgs e)
        public void CheckInDropInVisit()
        {
            
            int? result;


            Employee = new Employee()
            {
                Firstname = "Alice" /*txtVisitEFirstname*/,
                Lastname = "Hasselbom"/*txtVisitELastname*/,
                Department = "Marknad"/*txtVisitEDepartment*/
            };

            Visitor = new Visitor()
            {
                Firstname =/* "Kristian"*/txtVisitVFirstname,
                Lastname = /*"Norqvist"*/txtVisitVLastname,
                Company = /*"Företaget"*/txtVisitVCompany
            };

            result = SetMeetingAndCheckInVisitor(this.Employee, this.Visitor);
            if (result > 0)
            {
                MessageBox.Show($"Ett möte med {Employee.Firstname} {Employee.Lastname} är bokat och besökaren är incheckad");
            }


        }


        private void btnVisitCheckIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                int? result;
                //VisitorSearch = (VisitorSearch)grdBookedMeetings.SelectedItem;

                Employee = VisitorSearch.Employee;
                BookedMeeting = VisitorSearch.BookedMeeting;

                if (VisitorSearch != null)
                {

                    Visitor = new Visitor
                    {
                        Firstname = txtVisitVFirstname,
                        Lastname = txtVisitVLastname,
                        Company = txtVisitVCompany,
                        CheckIn = DateTime.Now,
                    };

                    result = SetMeetingAndCheckInVisitor(this.Employee, this.Visitor, this.BookedMeeting);

                    if (result > 0)
                    {
                        MessageBox.Show("Besökaren är incheckad");
                    }
                }
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }

        }

        //private void btnVisitSearch_Click(object sender, RoutedEventArgs e)
        public void BtnVisitSsearch()
        {
            //if (txtVisitEFirstname == "" || txtVisitELastname == "" || txtVisitEDepartment == "")
            //{
            //    string msg = "Du måste fylla i besöksmottagarens namn och avdelning";
            //    MessageBox.Show(msg);
            //}

            //else
            //{
                GetMeeting();
                //}
            }

            // HUR GÖRA MED Clear();?
            //private void btnVisitClearAll_Click(object sender, RoutedEventArgs e)
            //{
            //    txtVisitVFirstname.Clear();
            //    txtVisitVLastname.Clear();
            //    txtVisitVCompany.Clear();

            //    txtVisitEFirstname.Clear();
            //    txtVisitELastname.Clear();
            //    txtVisitEDepartment.Clear();
            //}

            //  Besökstabben slutar
            /// <summary>
            ///  LoggaUttabben börjar
            /// </summary>
            private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                if (sender is TabItem tabItem)

                {
                    string msgLogOut = "Vill du logga ut?";
                    string msgBoxHeader = "Utloggning";

                    MessageBoxResult result = MessageBox.Show(msgLogOut, msgBoxHeader, MessageBoxButton.OKCancel);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            //NavigationService.Navigate(new LoginPage());
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }


                }
            }

            //  LoggaUttabben slutar



            //  Pagingfunktionen börjar


            private void btnPrevious_Click_1(object sender, RoutedEventArgs e)
            {
                CalcPrevPage();
                SendSearchInfo();
                HandleBtnPageing();

            }

            /// <summary>
            /// Samlad metod för btn next och previous. Printar korrigerat sidnummer till lbl
            /// </summary>
            private void HandleBtnPageing()
            {
                // För att printa korrekt antal sidor
                int printCurrentPageNumber = CurrentPageNumber + 1;

                if (TotalPages == 1)
                {
                    //btnPrevious.IsEnabled = false;
                    //btnNext.IsEnabled = false;
                }
                else
                {
                    if (CurrentPageNumber >= (TotalPages - TotalPages))
                    {
                        //btnNext.IsEnabled = true;

                        if (CurrentPageNumber <= (TotalPages - TotalPages))
                        {
                            //btnPrevious.IsEnabled = false;
                        }
                        //else
                            //btnPrevious.IsEnabled = true;
                    }
                    // TotalPages behöver minskas med 1 då TotalPages startar från 1 men CurrentPageNumber från 0.
                    if (CurrentPageNumber == (TotalPages - 1))
                    {
                        //btnNext.IsEnabled = false;
                    }
                }

                lblCurrentPage = printCurrentPageNumber;
            }
        private int lblCurrentPage;

        private void btnNext_Click_1(object sender, RoutedEventArgs e)
        {
            CalcNextPage();
            SendSearchInfo();
            HandleBtnPageing();
        }

        private void cboResultPerPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //int hitsPerPage = Convert.ToInt32((string)cboxResultPerPage.SelectedValue);

            SetHitsPerPage(hitsPerPage);
            SendSearchInfo();

            HandleBtnPageing();

        }

        //  LoggaUttabben börjar

        //Pagingfunktionen slutar


        private void grdBookedMeetings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}



