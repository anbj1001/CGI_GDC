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
using static DatabaseAdmin.Model.GetVisitorInfo;
using static DatabaseAdmin.Model.GetEmployeeInfo;
using static DatabaseAdmin.Model.Encryptor;
using static DatabaseAdmin.Model.GetBookedMeetingInfo;
using System.Xaml;



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
          this.frame.NavigationService.Navigate(new LoginPage());


        }
        //private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //}


        //private void btnStartPage_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}


