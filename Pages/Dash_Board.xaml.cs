
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for Dash_Board.xaml
    /// </summary>
    public partial class Dash_Board : Page
    {
        public Dash_Board()
        {
            InitializeComponent();
            DataContext = new Dash_boardViewModel();
        }
    }
}
