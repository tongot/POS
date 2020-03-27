using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppDatabase;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace POS
{
    public class MasterChartViewModel : BaseViewModel
    {
        #region public variablles
        public Category category { get; set; }
        public List<Category> categories { get; set; }
        public Branch branch { get; set; }
        public List<Branch> branchies { get; set; }
        public string employee_username { get; set; }
        public string product_code { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string dates_error { get; set; } = "";
        public decimal sales_revenue { get; set; }
        public int sold_units { get; set; }
        public decimal tax { get; set; }
        public decimal discounts { get; set; }
        #endregion

        #region commands
        public ICommand btn_day { get; set; }
        public ICommand btn_month { get; set; }
        public ICommand btn_year { get; set; }
        public ICommand btn_reset { get; set; }
        #endregion
 
        #region database connect
        ICategory dbc = new CategoryApp();
        IBranch dbb = new BranchApp();
        ISales dbs = new SaleApp();
        #endregion

        #region chart variables
        /// <summary>
        /// line graph series for sales revenue
        /// </summary>
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        /// <summary>
        /// Row series values for number of products sold
        /// </summary>
        public SeriesCollection SeriesCollection_1 { get; set; }
        public string[] Labels_1 { get; set; }
        public Func<double, string> YFormatter_1 { get; set; }
        #endregion
        #region constructor
        public MasterChartViewModel()
        {
            categories = dbc.GetAllCategories(null).ToList();
            branchies = dbb.getAllBranches(null).ToList();

            btn_day = new RelayCommand(daySales);
            btn_month = new RelayCommand(monthSales);
            btn_year = new RelayCommand(yearSales);
            btn_reset = new RelayCommand(resetFilters);
        }
        #endregion
      
        
        private void daySales()
        {
            get_chart_data(periodOfSales.daily);

        }
        private void monthSales()
        {
            get_chart_data(periodOfSales.monthly);
        }
        private void yearSales()
        {
            get_chart_data(periodOfSales.yearly);
        }
        private void resetFilters()
        {
            branch = null;
            employee_username = null;
            product_code =null;
            start_date = null;
            end_date = null;
        }
        internal void getChartSeries(DateSalesValues day_sales)
        {
            if (day_sales.date != null)
            {

                ChartValues<decimal> v = new ChartValues<decimal>();
                string[] label = new string[31];

                for (int i = 0; i < day_sales.sales_revenue.Count(); i++)
                {
                    v.Add(day_sales.sales_revenue[i]);
                    label[i] = day_sales.date[i];
                }
                SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                     Title="Sales Revenue",
                     Values= v
                },
            };

                Labels = label;

                YFormatter = value => value.ToString("#,##0.00");
            }

            getChartSeries_1(day_sales);
        }
        internal void getChartSeries_1(DateSalesValues day_sales)
        {
            if (day_sales.product_name != null)
            {

                ChartValues<int> v = new ChartValues<int>();
                string[] label = new string[1000];

                for (int i = 0; i < day_sales.products_sold.Count(); i++)
                {
                    v.Add(day_sales.products_sold[i]);
                    label[i] = day_sales.product_name[i];
                }
                SeriesCollection_1 = new SeriesCollection
            {
                new RowSeries
                {
                     Title="Number products sold",
                     Values= v
                },
            };

                Labels_1 = label;

                YFormatter_1 = value => value.ToString();
            }
        }
        internal void get_chart_data(periodOfSales period)
        {

            if (start_date!=null & end_date!=null)
            {
             if(start_date>end_date)
                        {
                            dates_error = "Start date must be smaller than the end date";
                            return;
                        }
            }
            searchSaleFilter filter = new searchSaleFilter();
            filter.startDate = start_date;
            filter.endDate = end_date;
            filter.category = category;
            filter.productCode = product_code;
            filter.employee_username = employee_username;
            filter.branchName = branch != null ? branch.Name : null;

            var data = dbs.getMonthSales(filter, period);
            getChartSeries(data);
            sales_revenue = data.total_sales_revenue;
            sold_units = data.number_of_products;
            tax = data.total_tax;
            discounts = data.discount;
            
        }
    }
};