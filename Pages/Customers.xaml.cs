
using POS.ViewModel;
using System.Windows;
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for Branches.xaml
    /// </summary>&
    public partial class Customers : Page
    {
        public Customers()
        {

            IDialogService dialogService = new DialogService(Application.Current.MainWindow);
            dialogService.Register<DialogViewModel, DialogWindow>();
            InitializeComponent();
            DataContext = new CustomerViewModel(dialogService);
        }
    }
}
