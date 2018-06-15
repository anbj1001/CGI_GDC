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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string vFirstname = null;
            string vLastname = null ;
            string eFirstname = null;
            string eLastname = null;
            int? eID = null;
            string vCompany = null;
            string mDepartment = "IT";



            List<VisitorSearch> searchResults = new List<VisitorSearch>();


            CollectionViewSource itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            searchResults = GetVisitorSearchInfo(vFirstname, vLastname , eFirstname, eLastname, eID, vCompany,mDepartment);
            itemCollectionViewSource.Source = searchResults;
        }


    }
}
