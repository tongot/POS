
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Controls;


namespace POS
{
    /// <summary>
    /// Interaction logic for Pos2Page.xaml
    /// </summary>
    public partial class Pos2Page : Page
    {
        public Pos2Page()
        {
            InitializeComponent();
            DataContext = new SalePointViewmodel();
        }
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            IRequestFocus focus = (IRequestFocus)DataContext;
            focus.FocusRequested += Focus_FocusRequested;
            IPrintReceipt receipt = (IPrintReceipt)DataContext;
            receipt.printOutReciept += Receipt_printOutReciept;
            Focus_FocusRequested(this, new FocusRequestedEventArgs("start"));
        }
        List<Cart> Cart = new List<Cart>();
        decimal totalPrice, change, discount = 0, tax = 0;
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
            refnumber = e._refnumber;
            tax = e._tax;
            discount = e._discount;

            doc.PrintPage += new PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing

            //on a till you will not want to ask the user where to print but this is fine for the test envoironment.

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                doc.Print();

            }
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
            graphic.DrawString("Teller: " + teller, new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
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
                item.product_name = item.product_name + " X" + item.quantity;

                float roundup = item.product_name.Length / 24f;
                double numberOfLine = Math.Ceiling(roundup);
                if (numberOfLine > 1)
                {
                    graphic.DrawString(item.product_name.Substring(0, 24), new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
                }
                else
                {
                    graphic.DrawString(item.product_name, new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
                }

                graphic.DrawString(string.Format("{0:c}", (item.price * item.quantity).ToString()), new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), 270, pointY);
                pointY += (int)(fontheight * 0.7);
                if (numberOfLine > 1)
                {
                    int sub = 24;
                    int subEnd;
                    int LengthLeft = item.product_name.Length - 24;
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
                        string post = item.product_name.Substring(sub, subEnd);
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
            graphic.DrawString("Change", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            graphic.DrawString(change.ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 270, pointY);

            pointY += (int)fontheight;
            graphic.DrawString("Tax", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            graphic.DrawString(tax.ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 270, pointY);

            pointY += (int)fontheight;
            graphic.DrawString("Discount", new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);
            graphic.DrawString(discount.ToString(), new Font("Courier New", 10, FontStyle.Bold), new SolidBrush(System.Drawing.Color.Black), 270, pointY);

            pointY += (int)fontheight;
            graphic.DrawString("...............Thank you.............", new Font("Courier New", 10), new SolidBrush(System.Drawing.Color.Black), pointX, pointY);

        }

        private void Focus_FocusRequested(object sender, FocusRequestedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "textbox_barcode":
                    start_screen_control.textbox_barcode.Focus();
                    start_screen_control.textbox_barcode.SelectAll();
                    break;
                case "textbox_pad":
                    pay_screen_control.textbox_pad.Focus();
                    pay_screen_control.textbox_pad.SelectAll();
                    break;
                case "start":
                    start_screen_control.textbox_barcode.Focus();
                    break;
                case "textbox_customer_id":
                    customer_screen_control.textbox_customer_id.Focus();
                    customer_screen_control.textbox_customer_id.SelectAll();
                    break;
                case "AuthVoid":
                    VoidPassword voidPassword = new VoidPassword();
                    voidPassword.ShowDialog();
                    break;

            }
        }


    }
}
