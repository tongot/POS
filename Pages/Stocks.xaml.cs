
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for SalesPage.xaml
    /// </summary>
    public partial class Stocks : Page
    {
        public Stocks()
        {
            InitializeComponent();
            DataContext = new stockViewModel();
        }
    }
}
