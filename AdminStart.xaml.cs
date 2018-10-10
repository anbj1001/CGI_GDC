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


namespace DatabaseAdmin
{
    /// <summary>
    /// Interaction logic for AdminStart.xaml
    /// </summary>
    public partial class AdminStart : Page
    {
        public AdminStart()
        {
            InitializeComponent();
        }

        // Statistiktabben börjar
        /// <summary>
        /// Skickar med de parametrar som användaren vill använda för att söka efter till metoden för querin till databasen.
        /// Retunerar informationen som matchar sökpreferensen
        /// </summary>
        private void SendSearchInfo()
        {
            try
            {
              
                CollectionViewSource itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));

                VisitorSearch vs = InputVariables();
                VisitorSearchReturn vsr = CompleteStatSearch(vs, Offset, HitsPerPage);//returnerar ETT objekt med en count och lista med sökresultat
                int countTotalHits = vsr.CountTotalHits;
                ReturnTotalPages(countTotalHits);
                lblTotalPages.Content = TotalPages;

                lblTotalVisitors.Content = countTotalHits;
                itemCollectionViewSource.Source = vsr.VisitorSearches;//printar listan i objektet. Visitor, Employee och BookedMeeting

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }

        }

        private VisitorSearch InputVariables()
        {
            VisitorSearch vs = new VisitorSearch
            {
                Employee = new Employee
                {
                    EmployeeID = txtStatEmployeeID.Text != "" ? Convert.ToInt32(txtStatEmployeeID.Text) : (int?)null,
                    Firstname = txtStatEmployeeFirstname.Text ?? null,
                    Lastname = txtStatEmployeeLastname.Text ?? null,
                },
                Visitor = new Visitor
                {
                    Firstname = txtStatVisitorFirstname.Text ?? null,
                    Lastname = txtStatVisitorLastname.Text ?? null,
                    Company = txtStatVisitorCompany.Text ?? null,
                    BadgeReturned = (bool)chkBadgeReturned.IsChecked,
                    NotCheckedOut = (bool)chkCheckedOut.IsChecked
                },
                BookedMeeting = new BookedMeeting
                {
                    MeetingDepartment = txtStatEmployeeDepartment.Text ?? null
                },
                SearchParameters = new ParametersVisitorSearch
                {
                    DateInFrom = dtPickInFrom.Value,
                    DateInTo = dtPickInTo.Value,
                    DateOutFrom = dtPickOutFrom.Value,
                    DateOutTo = dtPickOutTo.Value
                }
            };
            return vs;
        }

        private void btnStatSearch_Click(object sender, RoutedEventArgs e)
        {
            SendSearchInfo();
            HandleBtnPageing();
        }

        private void btnStatCLearAll_Click(object sender, RoutedEventArgs e)
        {
            txtStatEmployeeFirstname.Clear();
            txtStatEmployeeLastname.Clear();
            txtStatEmployeeDepartment.Clear();
            txtStatEmployeeID.Clear();
            txtStatVisitorCompany.Clear();
            txtStatVisitorFirstname.Clear();
            txtStatVisitorLastname.Clear();
        }

        //Statistiktabben slutar


        // Personaltabben börjar

        private void GetEmployees()
        {
            CollectionViewSource EmployeeCollectionViewSource = (CollectionViewSource)(FindResource("EmployeeCollectionViewSource"));
            List<Employee> emp = GetAllEmployee();
            EmployeeCollectionViewSource.Source = emp;
        }


        /// <summary>
        /// Uppdaterar information om en emoployee om den är vald i griden, annars skapas en ny
        /// lägger till nya värden att uppdatera. måste hämta infon från GetAllEmployee genom knapp "sök" hamnar de i griden. Väljer därifrån
        /// </summary>
        private void UpdateEmployee()
        {
            Employee selectedEmployee = new Employee();
            selectedEmployee = (Employee)grdEmployee.SelectedItem;

            int result;

            if (selectedEmployee == null)
            {
                Employee e = new Employee
                {
                    Firstname = txtUpdateEmployeeFirstname.Text ?? null,
                    Lastname = txtUpdateEmployeeLastname.Text ?? null,
                    Address = txtUpdateEmployeeAddress.Text ?? null,
                    PhoneNumber = txtUpdateEmployeePhonenumber.Text ?? null,
                    Email = txtUpdateEmployeeEmail.Text ?? null,
                    Department = txtUpdateEmployeeDepartment.Text ?? null,
                    Team = txtUpdateEmployeeTeam.Text ?? null,
                };

                result = CreateEmployee(e);

                if (result > 0)
                {
                    MessageBox.Show("Informationen är sparad");
                }
            }

            else if (selectedEmployee != null)
            {
                Employee e = new Employee
                {
                    EmployeeID = Convert.ToInt32(lblEmployeeId.Content),
                    Firstname = txtUpdateEmployeeFirstname.Text ?? null,
                    Lastname = txtUpdateEmployeeLastname.Text ?? null,
                    Address = txtUpdateEmployeeAddress.Text ?? null,
                    PhoneNumber = txtUpdateEmployeePhonenumber.Text ?? null,
                    Email = txtUpdateEmployeeEmail.Text ?? null,
                    Department = txtUpdateEmployeeDepartment.Text ?? null,
                    Team = txtUpdateEmployeeTeam.Text ?? null,
                };

                result = UpdateEmployeeInfo(e);

                if (result > 0)
                {
                    MessageBox.Show("Informationen är sparad");
                }
            }
        }

        private void btnUserSearch_Click(object sender, RoutedEventArgs e)
        {
            GetEmployees();
        }

        /// <summary>
        /// Väljer objekt Employee för att hantera infon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Employee selectedEmployee = new Employee();
            selectedEmployee = (Employee)grdEmployee.SelectedItem;

            lblEmployeeId.Content = selectedEmployee.EmployeeID ?? null;
            txtUpdateEmployeeFirstname.Text = selectedEmployee.Firstname ?? null;
            txtUpdateEmployeeLastname.Text = selectedEmployee.Lastname ?? null;
            txtUpdateEmployeeAddress.Text = selectedEmployee.Address ?? null;
            txtUpdateEmployeePhonenumber.Text = selectedEmployee.PhoneNumber ?? null;
            txtUpdateEmployeeEmail.Text = selectedEmployee.Email ?? null;
            txtUpdateEmployeeDepartment.Text = selectedEmployee.Department ?? null;
            txtUpdateEmployeeTeam.Text = selectedEmployee.Team ?? null;
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
            Employee selectedEmployee = new Employee();
            selectedEmployee = (Employee)grdEmployee.SelectedItem;
            if (selectedEmployee == null)
            {
                MessageBox.Show("Välj en användare");
            }
            else
            {
                int result = DeleteOneEmployee(selectedEmployee);
                if (result > 0)
                {
                    MessageBox.Show("Användaren är raderad");
                }
                GetEmployees();
            }
        }


        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            txtUpdateEmployeeFirstname.Clear();
            txtUpdateEmployeeLastname.Clear();
            txtUpdateEmployeeAddress.Clear();
            txtUpdateEmployeePhonenumber.Clear();
            txtUpdateEmployeeEmail.Clear();
            txtUpdateEmployeeDepartment.Clear();
        }

        // Personaltabben slutar

        //Besökstabben börjar

        private void GetMeeting()
        {
            CollectionViewSource BookedMeetingsCollectionViewSource = (CollectionViewSource)(FindResource("BookedMeetingsCollectionViewSource"));

            Employee emp = new Employee
            {
                Firstname = /*txtVisitEFirstname.Text ?? null*/"Alice",
                Lastname = /*txtVisitELastname.Text ?? null*/ "Hasselbom",
                Department = "Marknad"
            };
            //vsr är för att få ut alla värderna i griden...
            VisitorSearchReturn vsr = GetTimeAndDepartment(emp);

            BookedMeetingsCollectionViewSource.Source = vsr.VisitorSearches;
        }

        private void CheckInVisitor()
        {//
            int? result = 0;

            VisitorSearch selectedItem = (VisitorSearch)grdBookedMeetings.SelectedItem;
            if (selectedItem != null)
            {
                Employee emp = new Employee
                {
                    Firstname =/* "Alice"*/ txtVisitEFirstname.Text ?? null,
                    Lastname = /*"Hasselbom"*/txtVisitELastname.Text ?? null,
                    Department = /*"FörsökEtt"*/txtVisitEDepartment.Text ?? null
                };
                Visitor v = new Visitor
                {
                    Firstname = "Kristian"/*txtVisitVFirstname.Text ?? null*/,
                    Lastname = "Norqvist"/*txtVisitVLastname.Text ?? null*/,
                    Company = "Företaget"/*txtVisitVCompany.Text ?? null*/
                };


                //  Vad har jag tänkt? Vart finns kopplingen till employee? VisitResponsible finns ju i bookedMeeting men det är en vanlig int och har ingen koppling till DENNA employeen
                result = AddVisitorToMeeting(GetVisitorInfo.CheckInVisitor(v), selectedItem.BookedMeeting.BookedMeetingID);
            }

            if (result > 0)
            {
                MessageBox.Show("Fan vad du är BRA!!!");
            }
        }
        private void btnCheckInDropInVisit_Click(object sender, RoutedEventArgs e)
        {
            int? result;

            Employee emp = new Employee()
            {
                Firstname = "Alice" /*txtVisitEFirstname.Text ?? null*/,
                Lastname = "Hasselbom"/*txtVisitELastname.Text ?? null*/,
                Department = "Marknad"/*txtVisitEDepartment.Text ?? null*/
            };

            Visitor v = new Visitor()
            {
                Firstname = "Kristian"/*txtVisitVFirstname.Text ?? null*/,
                Lastname = "Norqvist"/*txtVisitVLastname.Text ?? null*/,
                Company = "Företaget"/*txtVisitVCompany.Text ?? null*/
            };

            result = AddVisitorToMeeting(GetVisitorInfo.CheckInVisitor(v), BookMeetingNow(GetEmployeeID(emp)));

            //result = ReturnCompleteDropInCheckIn(emp, v);
            //result = SetMeetingAndCheckInVisitor(emp, v);
            if (result > 0)
            {
                MessageBox.Show($"Ett möte med {emp.Firstname} {emp.Lastname} är bokat och besökaren är incheckad");
            }
        }


        private void btnVisitCheckIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckInVisitor();
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message);
            }
        }

        private void btnVisitSearch_Click_1(object sender, RoutedEventArgs e)
        {
            //    if (txtVisitEFirstname.Text == "" || txtVisitELastname.Text == "" || txtVisitEDepartment.Text == "")
            //    {
            //        string msg = "Du måste fylla i besöksmottagarens namn och avdelning";
            //        MessageBox.Show(msg);
            //    }

            //    else
            //    {
            GetMeeting();
            //}
        }

        private void btnVisitClearAll_Click(object sender, RoutedEventArgs e)
        {
            txtVisitVFirstname.Clear();
            txtVisitVLastname.Clear();
            txtVisitVCompany.Clear();

            txtVisitEFirstname.Clear();
            txtVisitELastname.Clear();
            txtVisitEDepartment.Clear();
        }

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
                        NavigationService.Navigate(new LoginPage());
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
                btnPrevious.IsEnabled = false;
                btnNext.IsEnabled = false;
            }
            else
            {
                if (CurrentPageNumber >= (TotalPages - TotalPages))
                {
                    btnNext.IsEnabled = true;

                    if (CurrentPageNumber <= (TotalPages - TotalPages))
                    {
                        btnPrevious.IsEnabled = false;
                    }
                    else
                        btnPrevious.IsEnabled = true;
                }
                // TotalPages behöver minskas med 1 då TotalPages startar från 1 men CurrentPageNumber från 0.
                if (CurrentPageNumber == (TotalPages - 1))
                {
                    btnNext.IsEnabled = false;
                }
            }

            lblCurrentPage.Content = printCurrentPageNumber;
        }

        private void btnNext_Click_1(object sender, RoutedEventArgs e)
        {
            CalcNextPage();
            SendSearchInfo();
            HandleBtnPageing();
        }

        private void cboResultPerPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int hitsPerPage = Convert.ToInt32((string)cboxResultPerPage.SelectedValue);

            SetHitsPerPage(hitsPerPage);
            SendSearchInfo();

            HandleBtnPageing();

        }

        //  LoggaUttabben börjar

        //Pagingfunktionen slutar


        private void grdBookedMeetings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }



        private void btnVisitSearch_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
