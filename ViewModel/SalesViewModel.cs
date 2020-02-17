

using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace POS
{
    public class SalesViewModel:BaseViewModel
    {
        public VmSales vmsales { get; set; }

        public VmSearchSaleFilter filter { get; set; } = new VmSearchSaleFilter();
        #region button commands 
        public ICommand btnBackHome { get; set; }
        public ICommand searchBtn { get; set; }
        public ICommand btnClear { get; set; }
        #endregion
        public SalesViewModel()
        {
            vmsales = new VmSales();
            btnBackHome = new RelayCommand(BackHome);
            searchBtn = new RelayCommand(Search);
            btnClear = new RelayCommand(clear);
        }

        #region methods
        private void BackHome()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }
        private void Search()
        {
            vmsales.sales=vmsales.getSales(filter);
        }

        private void clear()
        {
            filter.clear();
        }
        #endregion
    }
}
