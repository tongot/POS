
using POS.ViewModel;
using System.Windows;
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for Branches.xaml
    /// </summary>&
    public partial class Branches : Page
    {
        public Branches()
        {
            IDialogService dialogService = new DialogService(Application.Current.MainWindow);
            dialogService.Register<DialogViewModel, DialogWindow>();
            InitializeComponent();
            DataContext = new BranchViewModel(dialogService);
        }
    }
}
