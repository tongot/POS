
using System.Windows;
using System.Windows.Controls;

namespace POS
{
    /// <summary>
    /// Interaction logic for DiscountsPage.xaml
    /// </summary>
    public partial class TaxPage : Page
    {
        public TaxPage()
        {
            IDialogService dialogService = new DialogService(Application.Current.MainWindow);
            dialogService.Register<DialogViewModel, DialogWindow>();
            InitializeComponent();
            DataContext = new TaxViewModel(dialogService);
        }
    }
}
