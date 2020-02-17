
using System.Windows;
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        public EmployeePage()
        {
            IDialogService dialogService = new DialogService(Application.Current.MainWindow);
            dialogService.Register<DialogViewModel, DialogWindow>();
            InitializeComponent();
            DataContext = new EmployeeViewModel(dialogService);
        }
    }
}
