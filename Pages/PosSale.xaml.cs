using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Controls;
namespace POS
{
    /// <summary>
    /// Interaction logic for PosSale.xaml
    /// </summary>
    public partial class PosSale : Page
    {
        public PosSale()
        {
            InitializeComponent();
            //DataContext = new KeyPadViewModel();
            Focus();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            
            //IRequestFocus focus = (IRequestFocus)DataContext;
            IPrintReceipt receipt = (IPrintReceipt)DataContext;
           // focus.FocusRequested += Focus_FocusRequested;
            receipt.printOutReciept += Receipt_printOutReciept;
        }
        List<POSProduct> Cart = new List<POSProduct>();
        decimal totalPrice,change;
        string teller;
        string refnumber;

        private void Receipt_printOutReciept(object sender, PrintReceiptEventArgs e)
        {
            System.Windows.Controls.PrintDialog dlg = new System.Windows.Controls.PrintDialog();
            dlg.PageRangeSelection = PageRangeSelection.AllPages;
            dlg.UserPageRangeEnabled = true;
            PrintDocument doc = new PrintDocument();

            Cart = e._Cart;
            totalPrice = e._totalPrice;
            change = e._change;
            teller = e._teller;
            refnumber = e.refnumber;

            doc.PrintPage += new PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing

            //on a till you will not want to ask the user where to print but this is fine for the test envoironment.

            //Nullable<bool> result = dlg.ShowDialog();

            //if (result == true)
            //{
                doc.Print();

            //}
        }
        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            Graphics graphic = e.Graphics;
            Font font = new Font("Courier New", 12);
            float fontheight = font.GetHeight();

            int pointX = 10;
            int pointY = 10;
            //int offset = 32;

            graphic.DrawString("Groove Tek Shop", new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            pointY += (int)fontheight;
            graphic.DrawString("Teller: "+teller, new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            pointY += (int)fontheight;
            graphic.DrawString("REF: " + refnumber, new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            pointY += (int)fontheight;
            string tableHeader = "Name".PadRight(30) + "Price";
            graphic.DrawString(tableHeader, new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            pointY += (int)fontheight;
            graphic.DrawString("--------------------------------------", new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            pointY += (int)fontheight;

            foreach (var item in Cart)
            {
                item.ProductName = item.ProductName + " X" + item.quantity;

                float roundup = item.ProductName.Length / 24f;
                double numberOfLine = Math.Ceiling(roundup);
                if (numberOfLine > 1)
                {
                    graphic.DrawString(item.ProductName.Substring(0, 24), new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
                }
                else
                {
                    graphic.DrawString(item.ProductName, new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
                }

                graphic.DrawString(string.Format("{0:c}",(item.price * item.quantity).ToString()), new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), 270, pointY);
                pointY += (int)(fontheight * 0.7);
                if (numberOfLine > 1)
                {
                    int sub = 24;
                    int subEnd;
                    int LengthLeft = item.ProductName.Length - 24;
                    for (int i = 1; i < numberOfLine; i++)
                    {
                        if (LengthLeft > 24)
                        {
                            subEnd = 24;
                        }
                        else
                        {
                            subEnd = LengthLeft;
                        }
                        string post = item.ProductName.Substring(sub, subEnd);
                        graphic.DrawString(post, new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
                        pointY += (int)(fontheight * 0.7);
                        LengthLeft -= 24;
                        sub += 24;
                    }
                }
            }
            pointY += (int)fontheight;
            graphic.DrawString("Total", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            graphic.DrawString(totalPrice.ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 270, pointY);

            pointY += (int)fontheight;
            graphic.DrawString("Change", new Font("Courier New", 10,FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            graphic.DrawString(change.ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 270, pointY);

            pointY += (int)fontheight;
            graphic.DrawString("...............Thank you.............", new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);

        }

        //private void Focus_FocusRequested(object sender, FocusRequestedEventArgs e)
        //{
        //    switch(e.PropertyName)
        //    {
        //        case "textBoxString":
        //            KeyPad.keyPadButtons.TextBoxPad.Focus();
        //            KeyPad.keyPadButtons.TextBoxPad.SelectAll();
        //            break;
        //        case "textBoxBarcode":
        //            BarCodeSearch.Focus();
        //            BarCodeSearch.SelectAll();
        //            break;
        //        case "sale":
        //            KeyPad.btnSale.Focus();
        //            break;
        //        case "password":
        //            username.Focus();
        //            username.SelectAll();
        //            break;

        //  }
       // }
    }
}
