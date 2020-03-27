using AppDatabase;
using Ninject;
using POS.Ioc;
using System.Windows.Input;

namespace POS
{
    /// <summary>
    /// menu view model
    /// </summary>
    public class MenuViewModel
    {
        IEmployee dbe;
        public ICommand toProducts { get; set; }
        public ICommand toPos { get; set; }
        public ICommand toSales { get; set; }
        public ICommand toEmployees { get; set; }
        public ICommand toBranches { get; set; }
        public ICommand toCustomer { get; set; }
        public ICommand toCategories { get; set; }
        public ICommand toTaxes { get; set; }
        public ICommand toDiscount { get; set; }
        public ICommand toSettings { get; set; }
        public ICommand toDashboards { get; set; }
        public ICommand toStocks { get; set; }
        public ICommand btnSignout { get; set; }
        public ICommand tochangePass { get; set; }

        public MenuViewModel()
        {
            dbe = new EmployeeApp();
            toProducts = new RelayCommand(ViewProducts);
            toPos = new RelayCommand(ViewPos);
            toSales = new RelayCommand(ViewSales);
            toEmployees = new RelayCommand(ViewEmployees);
            toBranches = new RelayCommand(ViewBranches);
            toCustomer = new RelayCommand(ViewCustomers);
            toCategories = new RelayCommand(ViewCategories);
            toSettings = new RelayCommand(ViewSettings);
            toTaxes = new RelayCommand(ViewTax);
            toDiscount = new RelayCommand(ViewDicount);
            toDashboards = new RelayCommand(ViewDashBoards);
            toStocks = new RelayCommand(ViewStocks);
            btnSignout = new RelayCommand(SignOut);
            tochangePass = new RelayCommand(ChangePass);

        }
        void SignOut()
        {
            dbe.logOutUser(User.username);

            IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username = string.Empty;
            IocContainer.Kenel.Get<AppViewModel>().CurrentUser.EmployeeId = null;
            IocContainer.Kenel.Get<AppViewModel>().NotLogged = true;
            IocContainer.Kenel.Get<AppViewModel>().Logged = false;

            IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_id = null;

            User.branch_id = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_id;
            User.username = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username;
            User.user_id = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.EmployeeId;
            User.branch_name = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_name;
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.logInPage;

        }
        private void ChangePass()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.changePassPage;
        }
        private void ViewStocks()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.stockPage;

        }
        private void ViewProducts()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.product;
        }
        private void ViewDashBoards()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.dashboard;
        }
        private void ViewTax()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.taxPage;
        }
        private void ViewDicount()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.discountPage;
        }
        private void ViewCategories()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.category;
        }
        private void ViewPos()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.posPage;
        }
        private void ViewSales()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.salesPage;
        }
        private void ViewEmployees()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.employeePage;
        }
        private void ViewBranches()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.Branches;
        }
        private void ViewCustomers()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.customerPage;
        }
        private void ViewSettings()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.roleManager;
        }
    }
}
