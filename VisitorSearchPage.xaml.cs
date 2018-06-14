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
using DatabaseAdmin.Model;
using DatabaseAdmin.DatabaseConnections;
using static DatabaseAdmin.DatabaseConnections.GetVisitorInfo;

namespace DatabaseAdmin
{
    /// <summary>
    /// Interaction logic for VisitorSearchPage.xaml
    /// </summary>
    public partial class VisitorSearchPage : Page
    {

        public VisitorSearchPage()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            int vID = 0;
            string vFirstname = "%";
            string vLastname = "%";
            string company = "%";
            DateTime vCheckIn;
            DateTime vCheckOut;
            int eID = 0;
            string eFirstname = "%";
            string eLastname = "%";
            string meetingDepartment = "%";



            //Try Catch?
            List<VisitorSearch> searchResults = new List<VisitorSearch>();

            searchResults = GetVisitorSearchInfo(vFirstname);
            CollectionViewSource itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            itemCollectionViewSource.Source = searchResults;
        }
    }
}
