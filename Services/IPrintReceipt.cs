

using System;
using System.Collections.Generic;

namespace POS
{
    public interface IPrintReceipt
    {
        event EventHandler<PrintReceiptEventArgs> printOutReciept;
    }
    public class PrintReceiptEventArgs:EventArgs
    {
        public PrintReceiptEventArgs(List<Cart> Cart, decimal totalPrice,decimal change, string Teller,string refnumber,decimal discount,decimal tax)
        {
            _Cart = Cart; _totalPrice = totalPrice;_change = change; _teller = Teller;_discount = discount;_tax = tax;_refnumber = refnumber;
        }

        public List<Cart> _Cart { get; private set; }
        public decimal _totalPrice { get; private set; }
        public decimal _change { get; private set; }
        public string _teller { get; private set; }
        public string _refnumber { get; private set; }
        public decimal _discount { get; private set; }
        public decimal _tax { get; private set; }



    }

}
