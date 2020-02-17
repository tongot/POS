

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
        public PrintReceiptEventArgs(List<POSProduct> Cart, double totalPrice,double change, string Teller,string refnumber)
        {
            _Cart = Cart; _totalPrice = totalPrice;_change = change; _teller = Teller;
        }

        public List<POSProduct> _Cart { get; private set; }
        public double _totalPrice { get; private set; }
        public double _change { get; private set; }
        public string _teller { get; private set; }
        public string refnumber { get; private set; }

    }

}
