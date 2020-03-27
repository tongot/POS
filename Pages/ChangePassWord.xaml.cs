using AppDatabase;
using Ninject;
using POS.Ioc;
using System.Windows;
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// this method violets the MVVM proceedure and will be changed with time
    /// </summary>
    public partial class ChangePassWord : Page
    {
        IEmployee db;
        public ChangePassWord()
        {
            InitializeComponent();
            db = new EmployeeApp();
        }

        private void Btn_enter_Click(object sender, RoutedEventArgs e)
        {
            text_error.Text = "";
            if (string.IsNullOrWhiteSpace(txtbox_oldpass.Password))
                {
                text_error.Text = "Please enter you old password";
                return;
            }
            if (!db.changePassword(User.username, txtbox_oldpass.Password, passBox.Password))
            {
                text_error.Text = "Password change failed, make sure\nyour old password is correct\nyou have entered a completly new password";
                return;
            }
            if (passBox.Password.Length<6)
            {
                text_error.Text = "Password too short";
                return;
            }
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }
    }
}
