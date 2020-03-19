
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
            Focus_FocusRequested(this, new FocusRequestedEventArgs("start"));
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
