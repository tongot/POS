using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS
{
    public class SalePointViewmodel : BaseViewModel, IRequestFocus
    {


        #region Public properties 
        public ObservableCollection<Cart> CartItems { get; set; } = new ObservableCollection<Cart>();
        public ObservableCollection<Customer> customers { get; set; } = new ObservableCollection<Customer>();
        public event EventHandler<FocusRequestedEventArgs> FocusRequested;

        string _amount_received;
        public string textbox_pad {
            get {
                return _amount_received;
            }
            set {
                if(value!=_amount_received)
                {
                    try
                    {
                        amount_paid = Convert.ToDecimal(value);
                        if (CartItems.Count < 1)
                        {
                            error("There are no products in Cart to sale");
                            return;
                        }
                        if(amount_paid>_total_price)
                        {
                            change = amount_paid - _total_price;
                        }
                        else
                        {
                            change = 0;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    _amount_received = value;
                }
            }
        }
        public string text_error { get; set; } = "";


        decimal _total_tax;
        public decimal total_tax
        {
            get { return _total_tax; }
            set { _total_tax = Math.Round(value, 2); }
        }
        decimal _total_discount;
        public decimal total_discount
        {
            get { return _total_discount; }
            set { _total_discount = Math.Round(value, 2); }
        }
        decimal _total_price;
        public decimal total_price
        {
            get { return _total_price; }
            set {
                _total_price = Math.Round(value, 2);
                if (amount_paid > 0)
                {
                    change = amount_paid - _total_price;
                }
            }
        }

        public decimal change { get; set; }
        public decimal textbox_amount_received { get; set; }
        public string customer_id { get; set; }
        string _textbox_barcode = "";

        public string textbox_barcode
        {
            get
            {
                return _textbox_barcode;
            }
            set
            {
                if (_textbox_barcode != value)
                {
                    _textbox_barcode = value;
                    add_product_to_cart();
                }

            }
        }
        string _textbox_customer_id = "";

        public string textbox_customer_id
        {
            get
            {
                return _textbox_customer_id;
            }
            set
            {
                if (_textbox_customer_id != value)
                {
                    _textbox_customer_id = value;
                    if(_textbox_customer_id.Length>5)
                    {
                        get_customer();
                    }
                }

            }
        }
        #endregion

        #region private properties 
        StringBuilder text_price_input = new StringBuilder();
        IProduct dbp;
        ISales dbs;
        ICustomer dbc;
        string Ref;
        decimal amount_paid=0;
        int transaction_state = 0;

        static class customer
        {
            public static int customer_id = 0;
            public static string customer_national_id = "";
        }
        
        #endregion

        #region true false values
        public bool show_error { get; set; } = false;
        public bool show_start_payment { get; set; } = true;
        public bool show_payment_panel { get; set; } = true;
        public bool show_adjust_btn { get; set; } = true;
        public bool show_customer_panel { get; set; } = false;
        #endregion

        #region Commands for text button
        public ICommand btnOne { get; set; }
        public ICommand btnTwo { get; set; }
        public ICommand btnThree { get; set; }
        public ICommand btnFour { get; set; }
        public ICommand btnFive { get; set; }
        public ICommand btnSix { get; set; }
        public ICommand btnSeven { get; set; }
        public ICommand btnEight { get; set; }
        public ICommand btnNine { get; set; }
        public ICommand btnZero { get; set; }
        public ICommand btnDot { get; set; }
        public ICommand btnDZero { get; set; }
        public ICommand btnTen { get; set; }
        public ICommand btnTwenty { get; set; }
        public ICommand btnFifty { get; set; }
        public ICommand btnHundred { get; set; }
        public ICommand btnTwoHundred { get; set; }
        #endregion

        #region commnads
        public ICommand btn_increase_quantity { get; set; }
        public ICommand btn_decrease_quantity { get; set; }
        public ICommand btn_clear { get; set; }
        public ICommand btn_enter { get; set; }
        public ICommand btn_void { get; set; }
        public ICommand btn_sale { get; set; }

        public ICommand btn_show_customer_panel { get; set; }
        public ICommand btn_set_customer { get; set; }
        public ICommand btn_cancel_customer { get; set; }

        #endregion
        #region contructors
        public SalePointViewmodel()
        {
            dbp = new ProductApp();
            dbs = new SaleApp();
            dbc = new CustomerApp();

            btnOne = new RelayCommand(One);
            btnTwo = new RelayCommand(Two);
            btnThree = new RelayCommand(Three);
            btnFour = new RelayCommand(Four);
            btnFive = new RelayCommand(Five);
            btnSix = new RelayCommand(Six);
            btnSeven = new RelayCommand(Seven);
            btnEight = new RelayCommand(Eight);
            btnNine = new RelayCommand(Nine);
            btnZero = new RelayCommand(Zero);
            btnDot = new RelayCommand(Dot);
            btnTen = new RelayCommand(ten);
            btnTwenty = new RelayCommand(twenty);
            btnFifty = new RelayCommand(fifty);
            btnHundred = new RelayCommand(hundred);
            btnTwoHundred = new RelayCommand(two_hundred);

            btn_decrease_quantity = new RelayCommand(reduce_item_quantity);
            btn_increase_quantity = new RelayCommand(increase_item_quantity);
            btn_clear = new RelayCommand(clear_textbox_pad);
            btn_enter = new RelayCommand(enter_sale);
            btn_void = new RelayCommand(void_pressed);
            btn_show_customer_panel = new RelayCommand(show_customer);
            btn_cancel_customer = new RelayCommand(exit_customer);
            btn_set_customer = new RelayCommand(set_customer);
            btn_sale = new RelayCommand(sale_pressed);

        }
        #endregion

        #region text button methods
        private void One()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("1");
            textbox_pad = text_price_input.ToString();
        }
        private void Two()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("2");
            textbox_pad = text_price_input.ToString();
        }
        private void Three()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("3");
            textbox_pad = text_price_input.ToString();
        }
        private void Four()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("4");
            textbox_pad = text_price_input.ToString();
        }
        private void Five()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("5");
            textbox_pad = text_price_input.ToString();
        }
        private void Six()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("6");
            textbox_pad = text_price_input.ToString();
        }
        private void Seven()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("7");
            textbox_pad = text_price_input.ToString();
        }
        private void Eight()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("8");
            textbox_pad = text_price_input.ToString();
        }
        private void Nine()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("9");
            textbox_pad = text_price_input.ToString();
        }
        private void Zero()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            text_price_input.Append("0");
            textbox_pad = text_price_input.ToString();
        }

        private void Dot()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }

            text_price_input.Append(".");
            textbox_pad = text_price_input.ToString();
        }

        private void ten()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            note_key_handler(10);

        }
        private void twenty()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            note_key_handler(20);
        }
        private void fifty()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            note_key_handler(50);
        }
        private void hundred()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            note_key_handler(100);
        }
        private void two_hundred()
        {
            if (CartItems.Count < 1)
            {
                error("No products charged");
                return;
            }
            note_key_handler(200);

        }

        #endregion

        #region functional buttons
        private void set_customer(object id)
        {
            if(transaction_state==0)
            {
                 var cs = customers.Where(x => x.CustomerId == (int)id).FirstOrDefault();
                customer.customer_id = cs.CustomerId;
                customer.customer_national_id = cs.nationalId;
                error($"Customer {cs.FullName} has been set");
                exit_customer();
            }
            else
            {
                error("Can not set customer");
            }
           
        }
        private void exit_customer()
        {
            show_customer_panel = false;
            show_payment_panel = true;
            OnFocusRequested(nameof(textbox_barcode));
        }
        private void show_customer()
        {
            show_customer_panel = true;
            show_payment_panel = false;
            OnFocusRequested(nameof(textbox_customer_id));
        }
        private void void_pressed()
        {
            if (transaction_state != 1)
            {
                error("No transaction found");
                return;
            }
            error("");
            if(!string.IsNullOrEmpty(Ref))
            {
                OnFocusRequested("AuthVoid");
                if (IocContainer.Kenel.Get<AppViewModel>().void_auth_pass)
                {
                    var sales = dbs.getSaleByRef(Ref);
                    foreach (var item in sales)
                    {
                        Sale sale = item;
                        item.state = (int)SaleState.Void;
                        //reverse quantity
                        dbp.updateSaleGoods(item.ProductId, item.stock_id,true );
                        dbs.updateSaleState(sale);
                    }
                    start_new_sale();
                }
                else
                {
                    OnFocusRequested("AuthVoid");
                }
            }
            else
            {
                error("No Sale to void");
            }
            
        }
        private void sale_pressed()
        {
            if(transaction_state!=1)
            {
                error("No transaction found");
                return;
            }
            error("");
            if (!string.IsNullOrEmpty(Ref))
            {
                    var sales = dbs.getSaleByRef(Ref);
                    foreach (var item in sales)
                    {
                        Sale sale = item;
                        item.state = (int)SaleState.success;
                        //reverse quantity
                        dbs.updateSaleState(sale);
                    }
                    start_new_sale();
            }
            else
            {
                error("No Products to sale");
            }
        }
        private void clear_textbox_pad()
        {
            text_price_input.Clear();
            textbox_pad = "";
            amount_paid = 0;
            change = 0;
        }
        private void add_product_to_cart()
        {
            error("");
            Product product = dbp.GetProductByCode(_textbox_barcode, User.branch_id);
            if (product != null)
            {
                //check if product is in stock
                if (product.running_stock != null)
                {
                    //update the quantity of products by decreasing
                    dbp.updateSaleGoods(product.ProductId,product.running_stock.stock_id,false);
                     dbp.productOnsaleUpdate(product.ProductId, User.username, true,1);

                    //update totals
                    total_tax += product.taxToPay;
                    total_discount += product.discount;
                    total_price += product.running_stock.price;

                    //get product already in cart 
                    var in_cart = CartItems.Where(x => x.product_id == product.ProductId).FirstOrDefault();

                    //adjust price in cart
                    if (in_cart != null)
                    {
                        in_cart.discount_on_product += product.discount;
                        in_cart.tax_on_product += product.taxToPay;
                        in_cart.quantity += 1;
                    }
                    //add new product to cart
                    else
                    {
                        Cart cart = new Cart();
                        cart.product_name = product.ProductName;
                        cart.product_id = product.ProductId;
                        cart.product_code = product.ProductCode;
                        cart.discount_on_product = product.discount;
                        cart.tax_on_product = product.taxToPay;
                        cart.price = product.running_stock.price;
                        cart.actual_discount = product.discount;
                        cart.actual_tax = product.taxToPay;
                        cart.actual_price = product.running_stock.price;
                        cart.quantity = 1;
                        cart.stock_id = product.running_stock.stock_id;

                        CartItems.Add(cart);
                    }
                    OnFocusRequested(nameof(textbox_barcode));

                }
                else
                {
                    error("Product out of stock");
                }
            }
            else
            {
                error("Product not found");
            }
        }

        private void enter_sale()
        {
            if(transaction_state==0)
            {
                error("");
                    if (CartItems.Count < 1)
                    {
                       error("There are no items to sale");
                        OnFocusRequested(nameof(textbox_barcode));
                        return;
                    }
                    if(amount_paid<_total_price)
                    {
                        error("Insufficient funds");
                        OnFocusRequested(nameof(textbox_pad));
                        return;
                    }
                    record_sale();
                    show_adjust_btn = false;
                    error("transaction in pending");
                    transaction_state = 1;
                    customer.customer_id = 0;
            }
            else
            {
                error("please complete the current sale by pressing SALE, HOLD or VOID");
            }
           

        }

        private void reduce_item_quantity(object id)
        {
            int product_id = (int)id;
            var item = CartItems.Where(x => x.product_id == product_id).FirstOrDefault();
            dbp.updateSaleGoods(product_id,item.stock_id,true);
            dbp.productOnsaleUpdate(product_id, User.username, false,1);

            item.quantity -= 1;
            decrease_totals(item);
        }
        private void increase_item_quantity(object id)
        {

            int product_id = (int)id;

            Product product = dbp.getProductById(product_id);
            if (product.Quantity < 1)
            {
                error("no more products in stock");
                return;
            }
            var item = CartItems.Where(x => x.product_id == product_id).FirstOrDefault();
            dbp.updateSaleGoods(product_id, item.stock_id, false);
            dbp.productOnsaleUpdate(product_id, User.username, true,1);

            item.quantity += 1;
            increase_totals(item);
        }
        #endregion
        #region helpers
        void note_key_handler(int key_value)
        {
            text_price_input.Clear();
            try
            {
                if (!string.IsNullOrEmpty(textbox_pad))
                {
                    decimal current_value = Convert.ToDecimal(textbox_pad);
                    current_value += key_value;
                    text_price_input.Append(current_value);
                }
                else
                {
                    text_price_input.Append(key_value);
                }
            }
            catch (Exception)
            {
                error("Not a valid money value");
            }
            textbox_pad = text_price_input.ToString();
        }
        void start_new_sale()
        {
            clear_textbox_pad();
            transaction_state = 0;
            total_discount = 0;
            total_price = 0;
            total_tax = 0;
            CartItems.Clear();
            show_adjust_btn = true;
            IocContainer.Kenel.Get<AppViewModel>().void_auth_pass = false;
            change = 0;
            amount_paid = 0;
            exit_customer();
        }
        void error(string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                show_error = false;
                text_error = error;
                return;
            }
            show_error = true;
            text_error = error;
            return;
        }
        void increase_totals(Cart cart_item)
        {
            total_tax += cart_item.actual_tax;
            total_discount += cart_item.actual_discount;
            total_price += cart_item.actual_price;
        }
        void decrease_totals(Cart cart_item)
        {
            total_tax -= cart_item.actual_tax;
            total_discount -= cart_item.actual_discount;
            total_price -= cart_item.actual_price;
            if (cart_item.quantity == 0)
            {
                CartItems.Remove(cart_item);
            }
        }
        async void record_sale()
        {
            Sale sale = new Sale();
            Ref = salesRef();
            foreach (var item in CartItems)
            {
                sale.Branch = User.branch_name;
                sale.customer = "Anonymous Customer";
                sale.DateOfSale = DateTime.Now;
                sale.Ref = Ref;
                sale.employeeUsername = User.username;
                sale.EmployeeId = User.user_id;//user.EmployeeId.Value;
                sale.Price = item.price;
                sale.Quantity = item.quantity;
                sale.state = (int)SaleState.failed;
                sale.TotalPrice = total_price;
                sale.cashReceived = amount_paid;
                sale.change = change;
                sale.ProductId = item.product_id;
                sale.productCode = item.product_code;
                sale.productName = item.product_name;
                sale.stock_id = item.stock_id;
                sale.runningDate = IocContainer.Kenel.Get<AppViewModel>().running_date;
                if (customer.customer_id != 0)
                {
                    sale.CustomerId = customer.customer_id;
                    sale.customer = customer.customer_national_id;
                }
                
                await dbs.addSale(sale);
                await dbp.productOnsaleUpdate(item.product_id, User.username, false, item.quantity);

            }

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

        void get_customer()
        {
            foreach (var item in dbc.getCustomerByIdLike(_textbox_customer_id))
            {
                customers.Add(item);
            }
        }
        #endregion

        #region event handlers
        protected virtual void OnFocusRequested(string propertyName)
        {
            FocusRequested?.Invoke(this, new FocusRequestedEventArgs(propertyName));
        }
        #endregion
    }
    public class Cart : BaseViewModel
    {
        public int product_id { get; set; }
        public string product_code { get; set; }
        public Guid stock_id { get; set; }

        decimal _price;
        public decimal price
        {
            get { return _price; }
            set { _price = Math.Round(value, 2); }
        }

        decimal _tax_on_product;
        public decimal tax_on_product
        {
            get { return _tax_on_product; }
            set
            {
                _tax_on_product = Math.Round(value, 2);
            }
        }
        decimal _discount_on_product;
        public decimal discount_on_product
        {
            get { return _discount_on_product; }
            set
            {
                _discount_on_product = Math.Round(value, 2);
            }
        }
        public int quantity { get; set; }
        public string product_name { get; set; }
        decimal _actual_price;
        public decimal actual_price
        {
            get { return _actual_price; }
            set
            {
                _actual_price = Math.Round(value, 2);
            }
        }
        decimal _actual_tax;
        public decimal actual_tax
        {
            get { return _actual_tax; }
            set
            {
                _actual_tax = Math.Round(value, 2);
            }
        }
        decimal _actual_discount;
        public decimal actual_discount
        {
            get { return _actual_discount; }
            set
            {
                _actual_discount = Math.Round(value, 2);
            }
        }

    }

}
