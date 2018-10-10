using DatabaseAdmin.Model;
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
using static DatabaseAdmin.Model.Encryptor;
using static DatabaseAdmin.Model.GetEmployeeInfo;


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

        private void LoginInput()
        {
            int? result;
            string username = txtUsername.Text;
            string password = passwBoxLogin.Password;

            result = AdminLogIn(username, MD5Hash(password));
            if (result != (int?)null)
            {
                NavigationService.Navigate(new AdminStart());
            }
            else
            {
                MessageBox.Show("Du har angett fel användarnamn eller lösenord");
            }
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminStart());

            //LoginInput();

        }
    }
}
