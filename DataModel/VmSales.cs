
using AppDatabase;
using System;
using System.Collections.ObjectModel;

namespace POS
{
    public class VmSales:BaseViewModel
    {
        ISales db;
        IProduct pdb;

        #region public values 
        public ObservableCollection<salesView> sales { get; set; } = new ObservableCollection<salesView>();
        public decimal total_success_sales { get; set; } = 0;
        public decimal total_failed_sales { get; set; } = 0;
        public decimal total_void_sales { get; set; } = 0;
        public int count_of_success { get; set; } = 0;
        public int count_of_failed { get; set; } = 0;
        public int count_of_void { get; set; } = 0;
        public double rate_of_success { get; set; }
        void clear_values()
        {
            total_failed_sales = 0;
            total_success_sales = 0;
            total_void_sales = 0;
            count_of_void = 0;
            count_of_success = 0;
            count_of_failed = 0;
        }
        #endregion


        public VmSales()
        {
            db = new SaleApp();
            pdb = new ProductApp();
            //sales = getSales();
        }

        /// <summary>
        /// get all sales from the database and set them to the sale view model
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<salesView> getSales(VmSearchSaleFilter filter)
        {
            clear_values();
            ObservableCollection<salesView> sales = new ObservableCollection<salesView>();
            foreach (var item in db.sales(setFilter(filter)))
            {
                if (item.state == (int)SaleState.failed)
                {
                    total_failed_sales += item.TotalPrice;
                    count_of_failed += 1;

                }
                if (item.state == (int)SaleState.Void)
                {
                    total_void_sales += item.TotalPrice;
                    count_of_void += 1;
                }
                if (item.state == (int)SaleState.success)
                {
                    total_success_sales += item.TotalPrice;
                    count_of_success += 1;

                }
                sales.Add(salesTosaleView(item));
            }
             rate_of_success=count_of_success / (count_of_success + count_of_failed) * 100;
            return sales;
        }
        /// <summary>
        /// set the filter to meet the databse side filter <ref=helpers
        /// </summary>
        /// <param name="s">the filter fro the view side</param>
        /// <returns></returns>
        public searchSaleFilter setFilter(VmSearchSaleFilter s)
        {

            searchSaleFilter sf = new searchSaleFilter();
            sf.branchName = s.branchName;
            sf.productCode = s.productCode;
            sf.productName = s.productName;
            sf.refNumber = s.refNumber;
            sf.startDate = s.startDate;
            sf.endDate = s.endDate;
            sf.customerId = s.customerId;

            return sf;

        }
        salesView salesTosaleView(Sale item)
        {
            salesView s = new salesView();
            s.Branch = item.Branch;
            s.SaleId = item.SaleId;
            s.Ref = item.Ref;
            s.ProductId = item.ProductId;
            s.DateOfSale = item.DateOfSale;
            s.employeeUsername = item.employeeUsername;
            s.Price = item.Price;
            s.Quantity = item.Quantity;
            s.state = (SaleState)item.state;
            s.cashReceived = item.cashReceived;
            s.productCode = item.productCode;
            s.change = item.change;
            s.TotalPrice = item.TotalPrice;
            s.productName = item.productName;
            s.customer = item.customer;
            return s;
        }
    }
    public class salesView:BaseViewModel
    {
        public int SaleId { get; set; }
        public string Ref { get; set; }
        //public int? CustomerId { get; set; }
        public int ProductId { get; set; }
        //public int EmployeeId { get; set; }
        public DateTime DateOfSale { get; set; }
        // public string customer { get; set; }
        public string employeeUsername { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public SaleState state { get; set; }
        public decimal cashReceived { get; set; }
        public decimal change { get; set; }
        // public string comment { get; set; }
        public decimal TotalPrice { get; set; }
        public string Branch { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string customer { get; set; }
      
    }
}
