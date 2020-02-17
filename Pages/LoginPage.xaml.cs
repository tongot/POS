
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            DataContext = new LogInViewModel();
        }
   
    }
}
