

using System;

namespace POS
{
    /// <summary>
    /// model for the product to be sold or searched
    /// </summary>
    public class POSProduct:BaseViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double price { get; set; }
        public int quantity { get; set; } 
        public string ProductCode { get; set; }
        public string ProductState { get; set; }
        public string state { get; set; }
        public int saleId { get; set; }
        public bool fromHold { get; set; }


    }
    /// <summary>
    /// class for mirraring the sales database
    /// </summary>
    //public class SaleProduct:BaseViewModel
    //{
    //    public int SaleId { get; set; }
    //    public string Doctor { get; set; }
    //    public int? CustomerId { get; set; }
    //    public int ProductId { get; set; }
    //    public int EmployeeId { get; set; }
    //    public DateTime DateOfSale { get; set; }
    //    public string customer { get; set; }
    //    public string employeeUsername { get; set; }
    //    public double Price { get; set; }
    //    public int Quantity { get; set; }
    //    public string Branch { get; set; }

    //}
}
