using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace POS
{
    public class ProductViewModel : BaseViewModel
    {
        #region private propertiet
        //if update set to 1 update quantity to increase if -1 decrease quantity
        private static int update = 1;
        //intitial number of items per page
        int CurrentPageSet = 0;
        private readonly IDialogService dialogService;
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
        public int? stockChange { get; set; }
        public string stockChangeError { get; set; }
        public string ErrorForProduct { get; set; }
        
        public double valueOfgoods { get; set; }
        public int quantityOfGoods { get; set; }
        
        #region filters values
        public string searchString { get; set; }
        public VmProductSearchFilter filterValues { get; set; }= new VmProductSearchFilter();

        #endregion

        #region view behaviour
        public bool restockVisible { get; set; } = false;
        public bool sideBarVisible { get; set; } = false;
        public bool addBtnVisible { get; set; } = false;
        public bool editBtnVisible { get; set; } = false;
        public int itemsPerPage { get; set; } = 10;
        public bool filterVisible { get; set; } = false;
        public int Page { get; set; } = 1;
        public bool showDetails { get; set; } = false;
        public bool showEdit { get; set; } = false;
        public bool showAdd { get; set; } = false;
        #endregion    
        #endregion
        #region constractor  
        public ProductViewModel(IDialogService dialogService)
        {

            this.dialogService = dialogService;
            Initialise();
            #region relay command intialisation
            saveProduct = new RelayCommand(AddProduct);
            backHome = new RelayCommand(home);
            restockBtn = new RelayCommand(restock);
            destockBtn = new RelayCommand(destock);
            UpdateQuantity = new RelayCommand(updateQnty);
            setUpdateProductbtn = new RelayCommand(setupadateProduct);
            addProd = new RelayCommand(addProdView);
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


        #endregion
    }
    #endregion
    #region methods

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
        }
    /// <summary>
    /// method to post a new product
    /// </summary>
    private void AddProduct()
        {

            //find if branches are selected
            if (category.categoryId <= 0 | branch.BranchId <= 0)
            {
                ErrorForProduct = "Category or Branch are not selected";
                return;
            }
            //set selected
            Prod.CategoryId = category.categoryId;
            Prod.categoryName = category.Name;
            Prod.BranchId = branch.BranchId;
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
       

        /// <summary>
        /// show the edit text box for increasing or decreasing Quantity
        /// </summary>
        /// <param name="checkid">id of product to restock</param>
        private void restock(object checkid)
        {
            stockChangeError = "Restocking";
            update = 1;
            selectProductToEditQnty((int)checkid);
        }
        /// <summary>
        /// destock methode
        /// </summary>
        /// <param name="checkid">id of product to destock</param>
        private void destock(object checkid)
        {
            stockChangeError = "Destocking";
            update = -1;
            selectProductToEditQnty((int)checkid);
        }
        /// <summary>
        /// sets the product quantity value to the database
        /// </summary>
        private void updateQnty()
        {
            int stock;
            //check if product id set
            if (EditProduct != null)
            {
                //check if stckchange is a number and is greater than 1 otherwise throw error
                if (int.TryParse(stockChange.ToString(), out stock)&stockChange>0)
                {
                    stockChangeError = null;
                    //check if it is a destock or a restock
                    if (update == -1)
                    {
                        if (EditProduct.Quantity < stockChange)
                        {
                            stockChangeError = "Your destocking value is greater than the Quantity available";
                            return;
                        }
                        else
                        {
                            EditProduct.Quantity -= stockChange;
                            Prod.editProduct(EditProduct);
                        }
                    }
                    else
                    {
                        EditProduct.Quantity += stockChange;
                        Prod.editProduct(EditProduct);
                    }
                    //reset view to normal
                    stockChangeError = null;
                    stockChange = null;
                    EditProduct.IsActiveEdit = false;

                    RecalculateProductValue();
                }
                else
                {
                    stockChangeError = "stock value must be a positive number";
                }

            }


        }
        /// <summary>
        /// selects the product to restock or destock
        /// </summary>
        /// <param name="id">id of product to action</param>
        private void selectProductToEditQnty(int id)
        {
            var hasValue = listProduct.Where(x => x.IsActiveEdit).FirstOrDefault();
            if (hasValue != null)
            {
                hasValue.IsActiveEdit = false;
            }
            //get the product to effect change
            var Product = listProduct.Where(x => x.ProductId == id).FirstOrDefault();
            //set the visiblity to true
            Product.IsActiveEdit = true;
            EditProduct = Product;
        }
        /// <summary>
        /// recalculation of total value after an edit
        /// </summary>
        private void RecalculateProductValue()
        {
            EditProduct.TotalValue = EditProduct.Quantity * EditProduct.pricePerUnit;
        }
        /// <summary>
        /// setting for viewing details 
        /// </summary>
        /// <param name="id">id of the product to view</param>
        private void setupadateProduct(object id)
        {
            //manage view to give access to the edit panel
            Prod.IsActiveEdit = false;
            showAdd = false;
            showEdit = false;
            showDetails = true;
            //select the product to edit fro the listproduct
            Prod = listProduct.Where(x => x.ProductId == (int)id).FirstOrDefault();
            Prod.IsActiveEdit = true;
            //reload category and branch
        }

        private void setProductForUpdate()
        {
            //manage view to give access to the edit panel

            showAdd = false;
            showEdit = true;
            showDetails = false;
            //select the product to edit fro the listproduct
            Prod = listProduct.Where(x => x.ProductId == Prod.ProductId).FirstOrDefault();
            renewObj();
           

        }
        /// <summary>
        /// show the panel to add new product
        /// </summary>
        private void addProdView()
        {
            addBtnVisible = true;
            editBtnVisible = false;
            Prod.categories = category.GetAllCategories();
            Prod.branches = branch.GetBranchAllBranches();
            sideBarVisible = sideBarVisible ? false : true;
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
                if (branch.BranchId>0)
                {
                    Prod.BranchId = branch.BranchId;
                    Prod.BranchName = branch.Name;
                }
                Prod.editProduct(Prod);
                showAdd = false;
                showEdit = false;
                showDetails = true;
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
        /// <summary>
        /// page next control
        /// </summary>
        private void NextPage()
        {
            int availableItems = Page * CurrentPageSet;
            //gget the number of items in database
            if(availableItems<Prod.getTotalOfProducts(filterValues))
            {
               Page++; listProduct = Prod.GetProducts(CurrentPageSet, Page,searchString, filterValues);
            } 
        }
        /// <summary>
        /// page prev control
        /// </summary>
        private void PrevPage()
        {
            if (Page>1)
            {
                Page--;listProduct = Prod.GetProducts(CurrentPageSet, Page,searchString, filterValues);
            }
        }
        /// <summary>
        /// search control
        /// </summary>
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
        /// <summary>
        /// clear the filter to reset to no filter
        /// </summary>
        private void filterClear()
        {
            //filter clear
            setFiltersToZero();
            listProduct = Prod.GetProducts(itemsPerPage, Page, searchString, filterValues);
        }
        /// <summary>
        /// delete the product
        /// </summary>
        /// <param name="id"></param>
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
           
            var viewModel = new DialogViewModel("Are sure you want to delete this record");
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
        #endregion
    }
}
