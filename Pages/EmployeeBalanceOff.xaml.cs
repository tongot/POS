
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for EmployeeBalanceOff.xaml
    /// </summary>
    public partial class EmployeeBalanceOff : Page
    {
        public EmployeeBalanceOff()
        {
            InitializeComponent();
            DataContext = new EmployeeBalanceOffViewModel();
        }
    }
}
