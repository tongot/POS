

using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace POS
{
    public class VmPOS : BaseViewModel
    {
        IProduct db;
        ISales dbs;
        ICustomer dbc;


        public VmPOS()
        {
            db = new ProductApp();
            dbs = new SaleApp();
            dbc = new CustomerApp();

        }
        #region public properties
        public ObservableCollection<POSProduct> Cart { get; set; } = new ObservableCollection<POSProduct>();
        public ObservableCollection<Customer> customers { get; set; }
        public ObservableCollection<POSProduct> ProductsSearched { get; set; } = new ObservableCollection<POSProduct>();

        public ObservableCollection<POSProduct> HoldList { get; set; } = new ObservableCollection<POSProduct>();
        public string saleError { get; set; } = "";

        public bool executeVoid = false;
        public string searchCustomer { get; set; } = "";
        public int? customerId { get; set; }
        public string refNumber { get; set; } = "";
        public string customerNationaiId { get; set; }
        //username of  the current user logged in details
        public VmUserLogIn user = IocContainer.Kenel.Get<AppViewModel>().CurrentUser;
        decimal _totalPrice;
        public decimal totalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = Math.Round(value, 2); }
        }


        decimal _change;
        public decimal change
        {
            get { return _change; }
            set { _change = Math.Round(value, 2); }
        }
        decimal _totalTax;
        public decimal totalTax
        {
            get { return _totalTax; }
            set { _totalTax = Math.Round(value, 2); }
        }
        decimal _totalDiscount;
        public decimal totalDiscount
        {
            get { return _totalDiscount; }
            set { _totalDiscount = Math.Round(value, 2); }
        }

        List<int> salesIds = new List<int>();
        /// <summary>
        /// keep of the current iid sales
        /// </summary>
        public List<Sale> sales = new List<Sale>();

        #endregion

        #region methods

        public void AddProductToSearch(string code)
        {
            foreach (var item in db.CodeLikeProducts(code))
            {
                POSProduct posp = new POSProduct();
                //set product
                posp = setProductPos(item);
                //add product to list
                ProductsSearched.Add(posp);
            }
        }
        public bool AddProductToCart(string code)
        {
            saleError = string.Empty;
            //set product
            var posp = setProductPos(db.GetProductByCode(code));
            //add product to list
            if (posp.ProductState != "Product not found")
            {
                var InCart = Cart.Where(x => x.ProductId == posp.ProductId).FirstOrDefault();
                if (InCart != null)
                {
                    InCart.quantity += 1;
                }
                else
                {
                    Cart.Add(posp);
                }
                
                totalPrice=Cart.Sum(x=>x.totalPrice);
                totalDiscount = Cart.Sum(x => x.totalDiscount);
                totalTax = Cart.Sum(x => x.totalTax);
                return true;
            }
            else
            {
                saleError = "Product Not Found";
                return false;
            }

        }
        /// <summary>
        /// convert the product object from data context to POSproduct
        /// </summary>
        /// <param name="product">product toconvert</param>
        /// <returns></returns>
        POSProduct setProductPos(Product product)
        {
            POSProduct posp = new POSProduct();
            //check if product is not null
            if (product != null)
            {

                posp.ProductName = product.ProductName;
                posp.ProductCode = product.ProductCode;
                posp.ProductId = product.ProductId;
                posp.quantity = 1;
                posp.price = product.Price;
                posp.tax = product.taxToPay;
                posp.discount = product.discount;
            }
            else
            {
                posp.ProductState = "Product not found";
            }

            return posp;
        }
        /// <summary>
        /// initial save of sale
        /// </summary>
        /// <param name="cashRecieved">cash received from the customer</param>
        /// <param name="change">change to customer</param>
        ///  <param name="fromHold">if the transaction is from hold list or not</param>
        /// <returns></returns>
        public async Task setSale(decimal cashRecieved, decimal change, bool fromHold)
        {
            if (fromHold)
            {
                processHold(cashRecieved, change);
                return;
            }
            //check for stock
            if (stockAvailable(false))
            {
                executeVoid = true;
                string Ref = salesRef();
                refNumber = Ref;
                foreach (var item in Cart)
                {

                    //update quantity
                    db.updateQuantity(item.ProductId, item.quantity, 0);
                    Sale s = new Sale();

                    s.Branch = "";
                    s.customer = "customer test";
                    s.DateOfSale = DateTime.Now;
                    s.Ref = Ref;
                    s.employeeUsername = user.username;
                    s.EmployeeId = 2009;//user.EmployeeId.Value;
                    s.Price = item.price / item.quantity;
                    s.Quantity = item.quantity;
                    s.state = (int)SaleState.failed;
                    s.TotalPrice = item.price;
                    s.cashReceived = cashRecieved;
                    s.change = change;
                    s.ProductId = item.ProductId;
                    s.productCode = item.ProductCode;
                    s.productName = item.ProductName;
                    if (customerId != null)
                    {
                        s.CustomerId = customerId.Value;
                        s.customer = customerNationaiId;
                    }
                    await dbs.addSale(s);
                    item.saleId = s.SaleId;

                    sales.Add(s);

                    saleError = "Transaction Pending";
                    customerId = null;

                }
            }
            else
            {
                saleError = "stock fall, check for stock warning on list";
                executeVoid = false;
                return;
            }
        }
        /// <summary>
        /// save the product finally updating the status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task finalSaleUpdate(SaleState status)
        {

            foreach (var item in Cart)
            {
                Sale sale = dbs.getSaleById(item.saleId);
                resetToNewSale();
                //change state of sale to success
                sale.state = (int)status;
                await dbs.updateSaleState(sale);

                if (status == SaleState.Void | status == SaleState.failed)
                {
                    await db.updateQuantity(item.ProductId, sale.Quantity, 1);
                }
                //sales.Remove(item);
            }

            Cart.Clear();

        }
        /// <summary>
        /// check stock availability 
        /// </summary>
        /// <param name="fromhold">if it is from hold return true ... quantity already authorised</param>
        /// <returns></returns>
        public bool stockAvailable(bool fromhold)
        {
            if (fromhold)
                return true;
            foreach (var item in Cart)
            {
                int stockLeft = db.getQuantity(item.ProductId);
                if (stockLeft < item.quantity)
                {
                    item.state = "Stock fall, stock Left " + stockLeft;
                    return false;
                }
            }
            return true;

        }
        void resetToNewSale()
        {
            //Chart.Clear();
            saleError = string.Empty;
            totalPrice = 0;
            totalDiscount = 0;
            totalTax = 0;
            change = 0;
        }
        /// <summary>
        /// creates the ref number for a transaction
        /// </summary>
        /// <returns>the unique string</returns>
        string salesRef()
        {
            string Ref = "REF" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            return Ref;
        }
        /// <summary>
        /// commiuting hold items after retrival
        /// </summary>
        /// <param name="cash"></param>
        /// <param name="change"></param>
        void processHold(decimal cash, decimal change)
        {
            foreach (var item in Cart)
            {
                Sale sale = new Sale();
                sale = dbs.getSaleById(item.saleId);
                sale.Price = item.price / item.quantity;
                sale.Quantity = item.quantity;
                sale.TotalPrice = item.price;
                sale.cashReceived = cash;
                sale.change = change;

                dbs.updateSaleState(sale);

            }
            // clear the hold list after processing
            HoldList.Clear();
        }
        public void getCustomer()
        {
            customers = new ObservableCollection<Customer>(dbc.getCustomerByIdLike(searchCustomer));
        }

        #endregion
    }
}
