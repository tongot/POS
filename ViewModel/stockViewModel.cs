

using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace POS
{
    public class stockViewModel:BaseViewModel
    {
        private IStock dbs;
        private IBranch dbb;
        public int? product_id { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string batchnumber { get; set; }
        public string branch_name { get; set; }
        public int quantity_available { get; set; } = 0;
        public decimal revenue_generated { get; set; } = 0;
        public Branch Branch { get; set; } = new Branch();

        public ObservableCollection<Stock> stocks { get; set; }
        public ObservableCollection<Branch> branches { get; set; }


        #region commands 
        public ICommand btnSearch { get; set; }
        public ICommand btnClear { get; set; }
        public ICommand btnBackHome { get; set; }
        #endregion

        #region constructor
        public stockViewModel()
        {
            dbs = new StockApp();
            dbb = new BranchApp();
            dbb.getAllBranches(null);
            btnSearch = new RelayCommand(Search);
            btnBackHome = new RelayCommand(backHome);
        }
        #endregion

        #region methods
        private void Search()
        {
            SalesFilter f = new SalesFilter();
            f.batchnumber = batchnumber;
            f.product_id = product_id;
            f.start_date = start_date;
            f.end_date = end_date;
            f.branch_name = Branch.Name;

            stocks = new ObservableCollection<Stock>( dbs.getFilteredStock(f));
            if(stocks.Count()>0)
            {
                revenue_generated = stocks.Sum(x => x.current_revenue);
                quantity_available = stocks.Sum(x => x.current_running_stock);
            }
        }
        private void backHome()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }
        #endregion
    }
}
