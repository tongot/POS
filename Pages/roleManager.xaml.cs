
using System.Windows;
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for roleManager.xaml
    /// </summary>
    public partial class roleManager : Page
    {
        public roleManager()
        {
            IDialogService dialogService = new DialogService(Application.Current.MainWindow);
            dialogService.Register<DialogViewModel, DialogWindow>();
            InitializeComponent();
            DataContext = new ViewModelRoleManager(dialogService);
        }
    }
}
