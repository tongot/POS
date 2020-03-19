
using System.Windows;
using System.Windows.Controls;

namespace POS
{
    /// <summary>
    /// Interaction logic for DiscountsPage.xaml
    /// </summary>
    public partial class DiscountsPage : Page
    {
        public DiscountsPage()
        {
            IDialogService dialogService = new DialogService(Application.Current.MainWindow);
            dialogService.Register<DialogViewModel, DialogWindow>();
            InitializeComponent();
            DataContext = new DiscountViewModel(dialogService);
        }
    }
}
