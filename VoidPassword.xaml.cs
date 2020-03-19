using AppDatabase;
using Ninject;
using POS.Ioc;
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
using System.Windows.Shapes;

namespace POS
{
    /// <summary>
    /// Interaction logic for VoidPassword.xaml
    /// </summary>
    public partial class VoidPassword : Window
    {
        IEmployee db;
        public VoidPassword()
        {
            InitializeComponent();
             db = new EmployeeApp();
        }
          private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                textbox_username.Focus();    
            }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            text_warning.Text = "";
            if(db.authenticateAdmin(textbox_username.Text, passBox.Password))
            {
                //set the global password validated to true if password was correct
                IocContainer.Kenel.Get<AppViewModel>().void_auth_pass = true;
                Close();
            }
            else
            {
                text_warning.Text = "Please enter correct credentials";
            }
        }

      
    }
}
