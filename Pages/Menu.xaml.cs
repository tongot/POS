
using System.Windows.Controls;

namespace POS
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
        }
    }
}
