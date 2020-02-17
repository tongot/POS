
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for DataBaseCon.xaml
    /// </summary>
    public partial class DataBaseCon : Page
    {
        public DataBaseCon()
        {
            InitializeComponent();
            DataContext = new DataConnViewModel();
        }
    }
}
