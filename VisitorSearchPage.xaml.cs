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
            List<VisitorSearch> searchResults = new List<VisitorSearch>();
            //  infoga anrop till sökmetoden här
            //  searchResults = metodnamnet
            CollectionViewSource itemCollectionViewSource;
            itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            itemCollectionViewSource.Source = searchResults;
        }
    }
}
