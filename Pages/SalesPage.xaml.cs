
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for SalesPage.xaml
    /// </summary>
    public partial class SalesPage : Page
    {
        public SalesPage()
        {
            InitializeComponent();
            DataContext = new SalesViewModel();
        }
    }
}
