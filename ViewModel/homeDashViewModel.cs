using AppDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;

namespace POS
{
    public class homeDashViewModel:BaseViewModel
    {

        public Func<ChartPoint, string> PointLabel { get; set; }

        #region public values 
        public string   sales_revenue  { get; set; }
        public string product_count { get; set; }
        public string sold_units { get; set; }
        public string void_sales { get; set; }
        public string failed_sales { get; set; }

        //for product
        public string product_value { get; set; }
        public string number_of_Product { get; set; }
        public string high_stock_product_name { get; set; }
        public int high_stock_product_qnt { get; set; }
        public string low_stock_product_name { get; set; }
        public int low_stock_product_qnt { get; set; }
        public int quantity_of_products { get; set; }
        #endregion

        public ChartValues<decimal> failed { get; set; }
        public ChartValues<decimal> success { get; set; }
        public ChartValues<decimal> Void { get; set; }


        #region private data
        ISales dbs = new SaleApp();
        IProduct dbp = new ProductApp();
        #endregion

        #region contructor
        public homeDashViewModel()
        {
           
            PointLabel = chartPoint =>
               string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            GetSalesValues();
            // for products
            GetProductValues();
           
        }
        #endregion

        #region methods
        void GetSalesValues()
        {
            var sales_values = dbs.getAllSalesRevenue();

            //for sales 
            sales_revenue = string.Format("{0:#,##0.00}", sales_values.sales_revenue);
            failed_sales = string.Format("{0:#,##0.00}", sales_values.failed_sales);
            void_sales = string.Format("{0:#,##0.00}", sales_values.void_sales);
            sold_units = string.Format("{0}", sales_values.sold_units);

            failed = new ChartValues<decimal>() { sales_values.failed_sales };
            success = new ChartValues<decimal> { sales_values.sales_revenue };
            Void = new ChartValues<decimal> { sales_values.void_sales };
        }
        void GetProductValues()
        {
            var product_values = dbp.GetProductValues();

            product_value = string.Format("{0:#,##0.00}", product_values.product_value);
            number_of_Product = string.Format("{0:#,##0.00}", product_values.number_of_Product);
            high_stock_product_name = product_values.high_stock_product!=null? product_values.high_stock_product.ProductName:"";
 

            high_stock_product_qnt =product_values.high_stock_product != null ? product_values.high_stock_product.Quantity : 0; 

            low_stock_product_name = product_values.low_stock_product!=null? product_values.low_stock_product.ProductName:"";
            low_stock_product_qnt = product_values.low_stock_product!=null? product_values.low_stock_product.Quantity:0;
            quantity_of_products = product_values.quantity_of_products;
        }
        #endregion
    }
}