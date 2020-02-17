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
        public ICommand toProducts { get; set; }
        public ICommand toPos { get; set; }
        public ICommand toSales { get; set; }
        public ICommand toEmployees { get; set; }
        public ICommand toBranches { get; set; }
        public ICommand toCustomer { get; set; }
        public ICommand toCategories { get; set; }
        
        public ICommand toSettings { get; set; }


        public MenuViewModel()
        {
            toProducts = new RelayCommand(ViewProducts);
            toPos = new RelayCommand(ViewPos);
            toSales = new RelayCommand(ViewSales);
            toEmployees = new RelayCommand(ViewEmployees);
            toBranches = new RelayCommand(ViewBranches);
            toCustomer = new RelayCommand(ViewCustomers);
            toCategories = new RelayCommand(ViewCategories);
            toSettings = new RelayCommand(ViewSettings);
        }
        private void ViewProducts()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.product;
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
