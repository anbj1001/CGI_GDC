using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DatabaseAdmin.DatabaseConnections;
using DatabaseAdmin.Model;
using static DatabaseAdmin.DatabaseConnections.GetVisitorInfo;
using static DatabaseAdmin.DatabaseConnections.VisitorSearch;


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

        private void GetMeetingInfo()
        {
            // Besöksansvarig
            Employee e = new Employee
            {
                Firstname = "Arvid"/*txtStatEmployeeFirstname.Text*/,
                Lastname = "Ahmed"/*txtStatEmployeeLastname.Text*/,
                //Department = txtStatDepartment.Text

            };

            Visitor v = new Visitor
            {
                Firstname = "Per"/* txtStatVisitorFirstname.Text*/,
                Lastname = "Jensen" /*txtStatVisitorLastname.Text*/,
                Company = "Polismyndigheten"/*txtStatCompany.Text*/
            };
            List<Visitor> visitorMeeing = GetVisitorMeeting(v, e);
        }

        private void SendSearchInfo()
        {
            DateTime? dateFrom = dpickStatDateFrom.SelectedDate;
            string timeFrom = tpickFrom.Value.ToString();

            DateTime? dateTo = dpickStatDateTo.SelectedDate;
            string timeTo = tpickTo.Value.ToString();

            Employee emp = new Employee
            {//testEmployee
                Firstname = null,
                Lastname = null,
                EmployeeID = null,
            };

            Visitor v = new Visitor
            {// testVisitor
                Firstname = null,
                Lastname = null,
                Company = null,
                CheckOutDate = null,
                CheckInTime = null,

            };

            BookedMeeting bm = new BookedMeeting
            {// TestBookedMeeting
                MeetingDepartment = null
            };

            List<VisitorSearch> searchResults = new List<VisitorSearch>();

            CollectionViewSource itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            searchResults = GetVisitorSearchInfo(emp, v, bm, dateFrom, dateTo, timeFrom, timeTo);
            itemCollectionViewSource.Source = searchResults;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            GetMeetingInfo();


        }


    }
}
