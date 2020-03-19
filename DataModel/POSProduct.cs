

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
        decimal _price;
        public decimal price
        {
            get { return _price; }
            set { _price = Math.Round(value, 2); }
        }
        public int quantity { get; set; }
            
        public string ProductCode { get; set; }
        public string ProductState { get; set; }
        public string state { get; set; }
        public int saleId { get; set; }
        public decimal tax { get; set; }
        public decimal discount { get; set; }
        public bool fromHold { get; set; }

     public decimal totalPrice { get
            { return price * quantity; }
                }
        public decimal totalDiscount
        {
            get
            { return discount * quantity; }
        }
        public decimal totalTax
        {
            get
            { return tax * quantity; }
        }
    }

}
