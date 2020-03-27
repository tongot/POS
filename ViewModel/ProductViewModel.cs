using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace POS
{
    public class ProductViewModel : BaseViewModel
    {
        #region private propertiet

        //intitial number of items per page
        int CurrentPageSet = 0;
        private readonly IDialogService dialogService;
        IStock dbs = new StockApp();

        #endregion
        #region commands
        public ICommand saveProduct { set; get; }
        public ICommand backHome { set; get; }
        public ICommand restockBtn { get; set; }
        public ICommand destockBtn { get; set; }
        public ICommand deleteBtn { get; set; }
        public ICommand cancelDeleteBtn { get; set; }
        public ICommand setDeleteBtn { get; set; }
        public ICommand UpdateQuantity { get; set; }
        public ICommand setUpdateProductbtn { get; set; }
        public ICommand addProd { get; set; }
        public ICommand updateProductBtn { get; set; }
        public ICommand setPerPageBtn { get; set; }
        public ICommand nextPageBtn { get; set; }
        public ICommand prevPageBtn { get; set; }
        public ICommand searchBtn { get; set; }
        public ICommand filterClearBtn { get; set; }
        public ICommand filterVisibleBtn { get; set; }
        public ICommand btnEdit { get; set; }
        public ICommand btnAdd { get; set; }


        #endregion
        #region public properties
        public VmProduct Prod { get; set; } = new VmProduct();
        public VmProduct EditProduct { get; set; } = new VmProduct();
        public VmCategory category { get; set; } = new VmCategory();
        public VmBranch branch { get; set; } = new VmBranch();
        public ObservableCollection<VmProduct> listProduct { get; set; } = new ObservableCollection<VmProduct>();
        public string ErrorForProduct { get; set; }

        public decimal valueOfgoods { get; set; }
        public int quantityOfGoods { get; set; }

        #region filters values
        public string searchString { get; set; }
        public VmProductSearchFilter filterValues { get; set; } = new VmProductSearchFilter();

        #endregion

        #region view behaviour
        public bool sideBarVisible { get; set; } = false;
        public bool addBtnVisible { get; set; } = false;
        public bool editBtnVisible { get; set; } = false;
        public int itemsPerPage { get; set; } = 10;
        public bool filterVisible { get; set; } = false;
        public int Page { get; set; } = 1;
        public bool showDetails { get; set; } = false;
        public bool showEdit { get; set; } = false;
        public bool showAdd { get; set; } = false;
        public bool showStock { get; set; } = false;
        public bool stock_details { get; set; } = true;
        public bool stock_addNew { get; set; } = false;

        #endregion    
        #endregion
        #region stock managment
        public ICommand btn_showstock { get; set; }
        public ICommand btn_showstockclose { get; set; }
        public ICommand btn_AddNewStock { get; set; }
        public ICommand btn_CloseNewStock { get; set; }
        public ICommand btn_SaveNewStock { get; set; }
        public ICommand btn_get_stockdetail { get; set; }
        public ICommand btn_change_price { get; set; }
        public ICommand btn_cancel_change_price { get; set; }
        public ICommand btn_save_newprice { get; set; }
        public ICommand btn_update_all { get; set; }

        public bool show_update_price { get; set; }
        public ObservableCollection<stock> stocks { get; set; }
        public stock stock_item { get; set; } = new stock();
        public stock stocks_detail { get; set; }
        public string errors { get; set; } = "";

        public decimal price { get; set; }
        public decimal purchase_price { get; set; }
        public decimal is_selected { get; set; }
        public decimal stock_id { get; set; }

        int _quantity;
        public int quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    total_value = price * _quantity;
                }
            }
        }
        public decimal total_value { get; set; }
        decimal _mark_up;
        public decimal mark_up
        {
            get
            {
                return _mark_up;
            }
            set
            {
                if (_mark_up != value)
                {
                    _mark_up = value;
                    price = purchase_price + (_mark_up / 100 * purchase_price);
                }
            }
        }
        private void addNewStock()
        {
            errors = ValidateNewStock(this);
            if (!string.IsNullOrEmpty(errors))
            {
                return;
            }

            Stock new_stock = new Stock();

            new_stock.date_of_stock = DateTime.Now;
            new_stock.ProductId = Prod.ProductId;
            new_stock.BranchId = branch.BranchId;
            new_stock.price = price;
            new_stock.markup = mark_up;
            new_stock.is_running_stock = false;
            new_stock.purchase_price = purchase_price;
            new_stock.quantity = quantity;
            new_stock.total_value = total_value;
            new_stock.current_running_stock = quantity;
            new_stock.batch_number = stockBatchNo();
            new_stock.last_update_date = DateTime.Now;

            dbs.addNewStock(new_stock);
            Prod.updateProductStockUp(new_stock.ProductId, new_stock.quantity, new_stock.total_value);

            price = 0;
            mark_up = 0;
            quantity = 0;
            total_value = 0;
            purchase_price = 0;
            branch.BranchId = 0;

            showStockbtn();

        }
        string stockBatchNo()
        {
            string batch = "B" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            return batch;
        }
        public string ValidateNewStock(ProductViewModel prod)
        {
            var Errors = new StringBuilder();
            decimal markup, purchasePrice;
            int quantity_;

            if (branch.BranchId == 0)
            {
                Errors.Append("*Branch Name is a required field\n");
            }
            if (decimal.TryParse(purchase_price.ToString(), out purchasePrice))
            {
                if (purchase_price <= 0.01m)
                {
                    Errors.Append("*purchase price must be greater than 0\n");
                }
            }
            else
            {
                Errors.Append("*Invalid purchase price\n");
            }
            if (int.TryParse(quantity.ToString(), out quantity_))
            {
                if (quantity < 1)
                {
                    Errors.Append("*quantity must be greater than 1\n");
                }
            }
            else
            {
                Errors.Append("*Invalid Quantity\n");
            }
            if (decimal.TryParse(mark_up.ToString(), out markup))
            {
                if (mark_up == 0)
                {
                    Errors.Append("*mark up must not be 0");
                }
            }
            else
            {
                Errors.Append("*Invalid Purchase Price\n");
            }
            return Errors.ToString();


        }
        private void getBatchdetail(object obj)
        {
            stocks_detail.is_selected = false;
            stocks_detail = stocks.Where(x => x.stock_id == (Guid)obj).FirstOrDefault();
            stocks_detail.is_selected = true;

            stock_details = true;
            stock_addNew = false;
        }
        private void changePrice()
        {
            show_update_price = true;
        }
        private void cancelChangePrice()
        {
            show_update_price = false;
        }
        private void saveNewPrice()
        {
            if(stocks_detail.quantity>0)
            {
                    dbs.updatePrice(setNewPrice(),false);
                    showStockbtn();
            }
        
            
        }
        private void saveAllNewPrice()
        {
            if (stocks_detail.quantity > 0)
            {
                dbs.updatePrice(setNewPrice(), true);
                showStockbtn();
            }
        }
        PriceChangeData setNewPrice()
        {
            PriceChangeData price = new PriceChangeData();
            price.branch_id = stocks_detail.BranchId;
            price.product_id = stocks_detail.ProductId;
            price.new_markup = stocks_detail.markup;
            price.new_price = stocks_detail.purchase_price + (stocks_detail.markup / 100 * stocks_detail.purchase_price);
            price.edited_by = User.username;
            price.stock_id = stocks_detail.stock_id;
            price.batch_no = stocks_detail.batch_number;
            return price;
        }
        private void showStockbtn()
        {
            showAdd = false;
            showEdit = false;
            showDetails = false;
            showStock = true;
            stock_addNew = false;
            stock_details = true;
            show_update_price = false;
            //get the stocks for the current product
            if(stocks!=null)
            {
                stocks.Clear();
            }
            stocks = new ObservableCollection<stock>(stock_item.setStock(Prod.ProductId));
            stocks_detail = new stock();
            if (stocks.Count > 0)
            {
                stocks_detail = stocks.FirstOrDefault();
                stocks_detail.is_selected = true;
            }

        }
        private void closeStock()
        {
            showAdd = false;
            showEdit = false;
            showDetails = true;
            showStock = false;
        }
        #endregion

        #region constractor  
        public ProductViewModel(IDialogService dialogService)
        {

            this.dialogService = dialogService;
            Initialise();
            #region relay command intialisation
            saveProduct = new RelayCommand(AddProduct);
            backHome = new RelayCommand(home);
            setUpdateProductbtn = new RelayCommand(setupadateProduct);
            updateProductBtn = new RelayCommand(postUpdate);
            setPerPageBtn = new RelayCommand(setPerPage);
            nextPageBtn = new RelayCommand(NextPage);
            prevPageBtn = new RelayCommand(PrevPage);
            searchBtn = new RelayCommand(Search);
            filterClearBtn = new RelayCommand(filterClear);
            filterVisibleBtn = new RelayCommand(showFilter);
            setDeleteBtn = new RelayCommand(setDelete);
            cancelDeleteBtn = new RelayCommand(cancelDelete);
            deleteBtn = new RelayCommand(delete);
            btnEdit = new RelayCommand(setProductForUpdate);
            btnAdd = new RelayCommand(addNew);
            btn_showstock = new RelayCommand(showStockbtn);
            btn_showstockclose = new RelayCommand(closeStock);
            btn_CloseNewStock = new RelayCommand(closeNewStock);
            btn_AddNewStock = new RelayCommand(openNewStock);
            btn_SaveNewStock = new RelayCommand(addNewStock);
            btn_get_stockdetail = new RelayCommand(getBatchdetail);
            btn_cancel_change_price = new RelayCommand(cancelChangePrice);
            btn_change_price = new RelayCommand(changePrice);
            btn_save_newprice = new RelayCommand(saveNewPrice);
            btn_update_all = new RelayCommand(saveAllNewPrice);
            #endregion
        }
         #endregion
    #region methods
        private void closeNewStock()
        {
            stock_details = true;
            stock_addNew = false;

        }
        private void openNewStock()
        {
            stock_details = false;
            stock_addNew = true;
            renewObj();
        }
        void Initialise()
        {
            showDetails = false;
            //set the initial number of items per page
            CurrentPageSet = itemsPerPage;
            //get the value of good selected
            valueOfgoods = Prod.totalValueOfGoods(filterValues);
            //get the  quantity of goods selected
            quantityOfGoods = Prod.totalQntyOfGoods(filterValues);
            //get products accoding to the filter set
            listProduct = Prod.GetProducts(itemsPerPage, Page, searchString, filterValues);
            //set view model category
          
            if(listProduct.Count>0)
            {
                    Prod= listProduct.FirstOrDefault();
                    Prod.IsActiveEdit = true;
                showDetails = true;
            }
            

        }
        void renewObj()
        {
            Prod.categories = new ObservableCollection<VmCategory>();
            //set view model collection of Braches availabe
            Prod.branches = new ObservableCollection<VmBranch>();

            //get all branches
            branch = new VmBranch();
            Prod.branches = branch.GetBranchAllBranches();
            //get all categories
            category = new VmCategory();
            Prod.categories = category.GetAllCategories();
        }
        /// <summary>
        /// open add window
        /// </summary>
        private void addNew()
        {
            Prod.IsActiveEdit = false;
            Prod = new VmProduct();
            //set view model category
            renewObj();
            showAdd = true;
            showEdit = false;
            showDetails = false;
            showStock = false;
        }
    /// <summary>
    /// method to post a new product
    /// </summary>
    private void AddProduct()
        {

            //find if branches are selected
            if (category.categoryId <= 0 )
            {
                ErrorForProduct = "Please select Category";
                return;
            }
            //set selected
            Prod.CategoryId = category.categoryId;
            Prod.categoryName = category.Name;
            Prod.BranchName = branch.Name;
            //set date ofEdit
            Prod.DateOfLastUpdate = DateTime.Now;

            //to come from system log in
            Prod.CapturedBy = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username;
            ErrorForProduct = Prod.ValidateProduct(Prod);
            if (string.IsNullOrEmpty(ErrorForProduct))
            {
                Prod.AddProduct(Prod);
                showAdd = false;
                showEdit = false;
                showDetails = true;
                showStock = false;
                Initialise();
                return;
            }

        }
        /// <summary>
        /// button back to home function
        /// </summary>
        private void home()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }  

        private void setupadateProduct(object id)
        {
            //manage view to give access to the edit panel
            Prod.IsActiveEdit = false;
            showAdd = false;
            showEdit = false;
            showStock = false;
            showDetails = true;
            //select the product to edit fro the listproduct
            Prod = listProduct.Where(x => x.ProductId == (int)id).FirstOrDefault();

            //holds the old values of the product before edit

            Prod.IsActiveEdit = true;
            //reload category and branch
        }
        OldProduct oldProduct = new OldProduct();
        private void setProductForUpdate()
        {
            //manage view to give access to the edit panel

            showAdd = false;
            showEdit = true;
            showDetails = false;
            showStock = false;
            //select the product to edit fro the listproduct
            Prod = listProduct.Where(x => x.ProductId == Prod.ProductId).FirstOrDefault();
           
            //set old product before edit
            oldProduct.markup =Prod.Markup.Value;
            oldProduct.dateLastUpdated = Prod.DateOfLastUpdate.Value;
            oldProduct.Quantity = Prod.Quantity.Value;
            oldProduct.ProductId = Prod.ProductId;

            renewObj();
           

        }

        /// <summary>
        /// make the filter panel visible or visible false
        /// </summary>
        private void showFilter()
        {
            filterVisible = filterVisible?false:true;
        }
        /// <summary>
        /// post edited product to the database
        /// </summary>
        private void postUpdate()
        {
            if (Prod.categories.Count < 0 | Prod.branches.Count < 0)
            {
                ErrorForProduct = "Category or Branch are not selected";
                return;
            }
            ErrorForProduct = Prod.ValidateProduct(Prod);
            if (string.IsNullOrEmpty(ErrorForProduct))
            {
                if(category.categoryId>0)
                {
                    Prod.CategoryId = category.categoryId;
                    Prod.categoryName = category.Name;

                }
                Prod.editProduct(Prod,oldProduct);
                showAdd = false;
                showEdit = false;
                showDetails = true;
                showStock = false;
                return;
            }
        }
        /// <summary>
        /// set the number of items that must be displayed
        /// </summary>
        private void setPerPage()
        {
            Page = 1;
            CurrentPageSet = itemsPerPage;
            listProduct = Prod.GetProducts(itemsPerPage, Page,searchString,filterValues);
        }
 
        private void NextPage()
        {
            int availableItems = Page * CurrentPageSet;
            //gget the number of items in database
            if(availableItems<Prod.getTotalOfProducts(filterValues))
            {
               Page++; listProduct = Prod.GetProducts(CurrentPageSet, Page,searchString, filterValues);
            } 
        }

        private void PrevPage()
        {
            if (Page>1)
            {
                Page--;listProduct = Prod.GetProducts(CurrentPageSet, Page,searchString, filterValues);
            }
        }

        private void Search()
        {
            //check if filters are valid
            if (filterValues.validateFilterValue(filterValues))
            {
                Page = 1;
                quantityOfGoods = Prod.totalQntyOfGoods(filterValues);
                valueOfgoods = Prod.totalValueOfGoods(filterValues);
                listProduct = Prod.GetProducts(CurrentPageSet, Page, searchString,filterValues);
            }  
        }

        private void filterClear()
        {
            //filter clear
            setFiltersToZero();
            listProduct = Prod.GetProducts(itemsPerPage, Page, searchString, filterValues);
        }

        private void setDelete(object id)
        {
            var hasValue = listProduct.Where(x => x.IsActiveEdit).FirstOrDefault();
            if (hasValue != null)
            {
                hasValue.IsTodelet = false;
            }
            //get the product to effect change
            var Product = listProduct.Where(x => x.ProductId == (int)id).FirstOrDefault();
            //set the visiblity to true
            Product.IsTodelet = true;
            EditProduct = Product;
        }
        private void delete()
        {
           
            var viewModel = new DialogViewModel("Are sure you want to delete this record\n Please note:\n-all stocks and sales of this product will also be deleted");
            bool? result = dialogService.ShowDialog(viewModel);
            if (result == true)
            {
                Prod.deleteProduct(Prod.ProductId);
                Initialise();
            }
            else
            {
                return;
            }
        }
        private void cancelDelete()
        {
            EditProduct.IsTodelet = false;
        }
        /// <summary>
        /// clear filters
        /// </summary>
       private void setFiltersToZero()
       {
            filterValues.searchString = "";
            filterValues.minPrice = 0;
             filterValues.maxPrice = 0;
              filterValues.minQuantity = 0;
             filterValues.maxQuantity = 0;
            filterValues.minTotalValue = 0;
            filterValues.maxTotalValue = 0;
        }

       
        public class stock: BaseViewModel
        {
            IStock dbs = new StockApp();
            public Guid stock_id { get; set; }
            public DateTime? date_of_stock { get; set; }
            public int quantity { get; set; }
            public decimal purchase_price { get; set; }
            public decimal price { get; set; }
            public decimal total_value { get; set; }
            public decimal markup { get; set; }
            public int ProductId { get; set; }
            public int BranchId { get; set; }
            public int current_running_stock { get; set; }
            public decimal current_revenue { get; set; }
            public bool is_running_stock { get; set; }
            public string batch_number { get; set; }
            public virtual Branch Branch { get; set; }
            public bool is_selected { get; set; }

            public List<stock> setStock(int prodduct_id)
            {
                List<stock> stocks = new List<stock>();
                foreach (var item in dbs.getStocksForProduct(prodduct_id))
                {

                    stock stock = new stock();
                    stock.stock_id = item.stock_id;
                    stock.date_of_stock = item.date_of_stock;
                    stock.quantity = item.quantity;
                    stock.purchase_price = item.purchase_price;
                    stock.price = item.price;
                    stock.total_value = item.total_value;
                    stock.markup = item.markup;
                    stock.ProductId = item.ProductId;
                    stock.BranchId = item.BranchId;
                    stock.current_revenue = item.current_revenue;
                    stock.current_running_stock = item.current_running_stock;
                    stock.is_running_stock = item.is_running_stock;
                    stock.batch_number = item.batch_number;
                    stock.Branch = item.Branch;
                    stock.is_selected = false;

                    stocks.Add(stock);
                }
                return stocks;
            }
        }
        #endregion
    }
}
