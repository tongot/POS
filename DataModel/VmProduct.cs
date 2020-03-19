using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace POS
{
    public class VmProduct : BaseViewModel
    {
        IProduct db;
        ICategory dbc;
        IStockTrack dbstk;
        public VmProduct()
        {
            db = new ProductApp();
            dbc = new CategoryApp();
            dbstk = new StockTrackApp();
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; } = 0;
        /// <summary>
        /// calculated from mark up price
        /// </summary>
        public decimal? Price { get; set; }
        public decimal? TotalValue { get; set; } = 0;
        public DateTime? DateOfLastUpdate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string Manufacturer { get; set; }
        public string ProductCode { get; set; }
        public string CapturedBy { get; set; }
        public int EmployeedId { get; set; }
        public int CategoryId { get; set; }
        public decimal? Markup { get; set; }
        public string BranchName { get; set; }
        public bool IsActiveEdit { get; set; }
        public bool IsTodelet { get; set; }
        public string categoryName { get; set; }

        public Category category { get; set; }


       


        public StringBuilder Erros { get; set; }
        string username = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username;
        public VmProductSearchFilter filter = new VmProductSearchFilter();
        public ObservableCollection<VmCategory> categories { get; set; }
        public ObservableCollection<VmBranch> branches { get; set; }

        /// <summary>
        /// validation of the product before edding or editting
        /// </summary>
        /// <param name="prod">the product to be add to the database</param>
        /// <returns></returns>
        public string ValidateProduct(VmProduct prod)
        {
            Erros = new StringBuilder();
            if (string.IsNullOrWhiteSpace(prod.ProductName))
            {
                Erros.Append("*Name is a required field\n");
            }
          
            if (string.IsNullOrWhiteSpace(prod.Manufacturer))
            {
                Erros.Append("*Manufacturer is a required field\n");
            }
            if (string.IsNullOrWhiteSpace(prod.categoryName))
            {
                Erros.Append("*Category Name is a required field\n");
            }
            if (string.IsNullOrWhiteSpace(prod.ProductCode))
            {
                Erros.Append("*Product Code is a required field\n");
            }
           
            return Erros.ToString();


        }


        /// <summary>
        /// set the product for view model to match database object product
        /// </summary>
        /// <param name="p">view model product</param>
        /// <returns></returns>
        public Product setProduct(VmProduct p)
        {
            Product product = new Product();
            product.ProductId = p.ProductId;
            product.ProductName = p.ProductName;
            product.Description = p.Description;
            product.Quantity = 0;
            product.TotalValue =0;
            product.DateOfLastUpdate = p.DateOfLastUpdate.Value;
            product.Manufacturer = p.Manufacturer;
            product.ProductCode = p.ProductCode;
            product.CapturedBy = p.CapturedBy;
            product.EmployeedId = p.EmployeedId;
            product.CategoryId = p.CategoryId;
            return product;

        }
        public void AddProduct(VmProduct p)
        {
            p.DateOfLastUpdate = DateTime.Now;
            var product = setProduct(p);
            db.addProduct(product, username);
        }
        /// <summary>
        /// get th eproducts to display to the screen
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<VmProduct> GetProducts(int Perpage, int currentPage, string searchString, VmProductSearchFilter f)
        {
            //filter.setZero(f);
            ObservableCollection<VmProduct> products = new ObservableCollection<VmProduct>();
            foreach (var p in db.GetAllProducts(currentPage, Perpage, filter.setFilter(f)))
            {
                VmProduct product = new VmProduct();
                product.ProductId = p.ProductId;
                product.ProductName = p.ProductName;
                product.Description = p.Description;
                product.Quantity = p.Quantity;
                product.TotalValue = p.TotalValue;
                product.DateOfLastUpdate = p.DateOfLastUpdate;
                product.Manufacturer = p.Manufacturer;
                product.ProductCode = p.ProductCode;
                product.CapturedBy = p.CapturedBy;
                product.EmployeedId = p.EmployeedId;
                product.CategoryId = p.CategoryId;
                product.IsActiveEdit = false;
                product.category = new Category();
                product.category = dbc.getCategoryById(product.CategoryId);
                product.categoryName = product.category != null ? product.category.Name : null;

                products.Add(product);
            }
            return products;
        }
        public void editProduct(VmProduct vmpr, OldProduct old)
        {
            db.updateProduct(setProduct(vmpr));
        }
        public int getTotalOfProducts(VmProductSearchFilter filter)
        {
            return db.totalProducts(this.filter.setFilter(filter));
        }
        public decimal totalValueOfGoods(VmProductSearchFilter filter)
        {
            return db.TotalValue(this.filter.setFilter(filter));
        }
        public int totalQntyOfGoods(VmProductSearchFilter filter)
        {
            return db.TotalQuantity(this.filter.setFilter(filter));
        }
        public void updateProductStockUp(int id,int qnt, decimal value)
        {
            db.updateQuantity(id, qnt, value, true);
        }
        public void deleteProduct(int id)
        {
            db.deleteProduct(id);
        }
        public void changePrice()
        {

        }

    }
  
}
