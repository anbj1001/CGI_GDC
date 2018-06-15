using DatabaseAdmin.DatabaseConnections;
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


namespace DatabaseAdmin
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            //string username = "Admin3";
            //string password = Encryptor.MD5Hash("Admin3");
            //Admin a = GetEmployeeInfo.AdminLogIn(admin, username, password);
            //if (a.AdminID != null)
            //{

            //    this.Hide();
            //    window1.Show();
            //}
            //else
            //    MessageBox.Show("Du angav fel lösenord eller användarnamn");
            NavigationService.Navigate(new AdminStart());
            
            

            

        }
    }
}
