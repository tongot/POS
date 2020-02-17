using AppDatabase;
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
        public VmProduct()
        {
            db = new ProductApp();
            dbc = new CategoryApp();
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        /// <summary>
        /// calculated from mark up price
        /// </summary>
        public double? Price { get; set; }
        public double? TotalValue { get; set; }
        public DateTime? DateOfLastUpdate { get; set; }
        public double? PurchasePrice { get; set; }
        public string Manufacturer { get; set; }
        public string ProductCode { get; set; }
        public string CapturedBy { get; set; }
        public int EmployeedId { get; set; }
        public int CategoryId { get; set; }
        public int BranchId { get; set; }
        public float? Markup { get; set; }
        public string BranchName { get; set; }
        public bool IsActiveEdit { get; set; }
        public bool IsTodelet { get; set; }
        public string categoryName { get; set; }
        public Category category { get; set; }
        public StringBuilder Erros { get; set; }
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
            double markup;
            int quantity;
            if (string.IsNullOrWhiteSpace(prod.ProductName))
            {
                Erros.Append("*Name is a required field\n");
            }
            if (prod.Markup == null)
            {
                Erros.Append("*Markup is a required field\n");
            }
            if (prod.Quantity == null)
            {
                Erros.Append("*Quantity is a required field\n");
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
            if (string.IsNullOrWhiteSpace(prod.BranchName))
            {
                Erros.Append("*Branch Name is a required field\n");
            }
            if (prod.PurchasePrice == null)
            {
                Erros.Append("*Purchase is a required field\n");
            }
            if (double.TryParse(prod.Markup.ToString(), out markup))
            {
                if (prod.Markup <= 0)
                {
                    Erros.Append("*markup must be greater than 0\n");
                }
            }
            else
            {
                Erros.Append("*Invalid markup\n");
            }
            if (int.TryParse(prod.Quantity.ToString(), out quantity))
            {
                if (prod.Quantity < 1)
                {
                    Erros.Append("*markup must be greater than 1\n");
                }
            }
            else
            {
                Erros.Append("*Invalid Quantity\n");
            }
            if (double.TryParse(prod.PurchasePrice.ToString(), out markup))
            {
                if (prod.PurchasePrice < 0.01)
                {
                    Erros.Append("*Purchase Price must be greater than 0.01\n");
                }
            }
            else
            {
                Erros.Append("*Invalid Purchase Price\n");
            }
            return Erros.ToString();


        }

        #region calculated fields
        public double? pricePerUnit
        {
            get
            {
                return ((Markup + 100) / 100) * PurchasePrice;
            }
            set
            {
                pricePerUnit = value;
            }
        }
        public double? Value
        {
            get
            {
                return pricePerUnit * Quantity;
            }
            set { Value = value; }
        }
        #endregion
        



        /// <summary>
        /// set the product to match database object
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Product setProduct(VmProduct p)
        {
            Product product = new Product();
            product.ProductId = p.ProductId;
            product.ProductName = p.ProductName;
            product.Description = p.Description;
            product.Quantity = p.Quantity.Value;
            product.Price = p.pricePerUnit.Value;
            product.TotalValue = p.Value.Value;
            product.DateOfLastUpdate = p.DateOfLastUpdate.Value;
            product.PurchasePrice = p.PurchasePrice.Value;
            product.Manufacturer = p.Manufacturer;
            product.ProductCode = p.ProductCode;
            product.CapturedBy = p.CapturedBy;
            product.EmployeedId = p.EmployeedId;
            product.CategoryId = p.CategoryId;
            product.BranchId = p.BranchId;
            product.BranchName = p.BranchName;
            product.markup = p.Markup.Value;
            return product;
        }
        /// <summary>
        /// add a new product to the database
        /// </summary>
        /// <param name="p"></param>
        public void AddProduct(VmProduct p)
        {
            db.addProduct(setProduct(p));
        }
        /// <summary>
        /// get th eproducts to display to the screen
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<VmProduct> GetProducts(int Perpage, int currentPage, string searchString, VmProductSearchFilter f)
        {
            //filter.setZero(f);
            ObservableCollection<VmProduct> products = new ObservableCollection<VmProduct>();
            foreach (var p in db.GetAllProducts(currentPage, Perpage,filter.setFilter(f)))
            {
                VmProduct product = new VmProduct();
                product.ProductId = p.ProductId;
                product.ProductName = p.ProductName;
                product.Description = p.Description;
                product.Quantity = p.Quantity;
                product.Price = p.Price;
                product.TotalValue = p.TotalValue;
                product.DateOfLastUpdate = p.DateOfLastUpdate;
                product.PurchasePrice = p.PurchasePrice;
                product.Manufacturer = p.Manufacturer;
                product.ProductCode = p.ProductCode;
                product.CapturedBy = p.CapturedBy;
                product.EmployeedId = p.EmployeedId;
                product.CategoryId = p.CategoryId;
                product.BranchId = p.BranchId;
                product.BranchName = p.BranchName;
                product.Markup = p.markup;
                product.IsActiveEdit = false;
                product.category = new Category();
                product.category = dbc.getCategoryById(product.CategoryId);
                product.categoryName = product.category != null ? product.category.Name : null;

                products.Add(product);
            }
            return products;
        }
        public void editProduct(VmProduct vmpr)
        {
            db.updateProduct(setProduct(vmpr));
        }
        public int getTotalOfProducts(VmProductSearchFilter filter)
        {
            return db.totalProducts(this.filter.setFilter(filter));
        }
        public double totalValueOfGoods(VmProductSearchFilter filter)
        {
            return db.TotalValue(this.filter.setFilter(filter));
        }
        public int totalQntyOfGoods(VmProductSearchFilter filter)
        {
            return db.TotalQuantity(this.filter.setFilter(filter));
        }
        public void deleteProduct(int id)
        {
            db.deleteProduct(id);
        }
    }
}
