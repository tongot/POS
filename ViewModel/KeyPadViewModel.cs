
using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace POS
{
    public class KeyPadViewModel : BaseViewModel, IRequestFocus, IPrintReceipt
    {
        bool fromHold = false;

        public event EventHandler<FocusRequestedEventArgs> FocusRequested;
        public event EventHandler<PrintReceiptEventArgs> printOutReciept;


        #region public properties
        public string textBoxString { get; set; } = "";

        private string _textBoxBarcode;
        /// <summary>
        /// handels property change for barcode search
        /// </summary>
        public string textBoxBarcode
        {
            get { return _textBoxBarcode; }
            set
            {
                if (_textBoxBarcode != value)
                {
                    //if (value.Length  >1)
                    //{
                    //vmPOS.ProductsSearched.Clear();
                    _textBoxBarcode = value;
                    if (vmPOS.AddProductToCart(textBoxBarcode))
                    {
                        OnFocusRequested(nameof(textBoxBarcode));
                    }
                    //SearchProductList = true;
                    ListOfProductToPay = true;
                    //}
                }
                if (_textBoxBarcode == string.Empty & vmPOS.saleError != "Insufficient amount")
                {
                    vmPOS.saleError = string.Empty;
                }
            }
        }
        public StringBuilder st { get; set; } = new StringBuilder();
        public StringBuilder SearchSt { get; set; } = new StringBuilder();
        public VmPOS vmPOS { get; set; } = new VmPOS();

        public string Error { get; set; } = "";
        public string username { get; set; }
        public string adminLogError { get; set; }



        #endregion
        #region visual control
        public bool NotesBtn { get; set; }
        public bool ChangeTxtBlock { get; set; }
        public bool KeyPadBtn { get; set; }
        public bool SearchProductList { get; set; }
        public bool ListOfProductToPay { get; set; }
        public bool IsSearching { get; set; }
        public bool stateBtn { get; set; }
        public bool btnClearHold { get; set; }
        public bool btnSearchVisibility { get; set; } = true;
        public bool hideProductView { get; set; } = false;
        public bool hideCustomerView { get; set; } = true;
        public bool showPayBtn { get; set; } = true;
        public bool passBoxShow { get; set; } = false;
        public bool showEnter { get; set; }
        public bool showBarCodeReader { get; set; } = true;
        public bool showSearchCustomer { get; set; } = true;
        public bool showAddPlusBtn { get; set; } = true;


        #endregion
        #region Commands for text button
        public ICommand btnOne { get; set; }
        public ICommand btnTwo { get; set; }
        public ICommand btnThree { get; set; }
        public ICommand btnFour { get; set; }
        public ICommand btnFive { get; set; }
        public ICommand btnSix { get; set; }
        public ICommand btnSeven { get; set; }
        public ICommand btnEight { get; set; }
        public ICommand btnNine { get; set; }
        public ICommand btnZero { get; set; }
        public ICommand btnDot { get; set; }
        public ICommand btnDZero { get; set; }


        #endregion
        #region function Buttons
        public ICommand BtnSearchProduct { get; set; }
        public ICommand BtnPay { get; set; }
        public ICommand btnEnter { get; set; }
        public ICommand btnClear { get; set; }
        public ICommand AddToChartBtn { get; set; }
        public ICommand saleBtn { get; set; }
        public ICommand voidBtn { get; set; }
        public ICommand holdBtn { get; set; }
        public ICommand viewHoldTransaction { get; set; }
        public ICommand ExitBtn { get; set; }
        public ICommand quantityMinusBtn { get; set; }
        public ICommand quantityPlusBtn { get; set; }
        public ICommand clearHoldbtn { get; set; }
        public ICommand btnSeachCustomer { get; set; }
        public ICommand btnSetCustomer { get; set; }
        public ICommand btnSetCustomerView { get; set; }
        public ICommand authoriseVoid { get; set; }
        public ICommand searchBtn { get; set; }






        #endregion
        #region constructor
        public KeyPadViewModel()
        {
            //set the view of pad to be fales 
            HidePadView();

            st.Append(textBoxString);
            btnOne = new RelayCommand(One);
            btnTwo = new RelayCommand(Two);
            btnThree = new RelayCommand(Three);
            btnFour = new RelayCommand(Four);
            btnFive = new RelayCommand(Five);
            btnSix = new RelayCommand(Six);
            btnSeven = new RelayCommand(Seven);
            btnEight = new RelayCommand(Eight);
            btnNine = new RelayCommand(Nine);
            btnZero = new RelayCommand(Zero);
            btnDZero = new RelayCommand(Dzero);
            btnDot = new RelayCommand(Dot);

            BtnSearchProduct = new RelayCommand(SearchPressed);
            BtnPay = new RelayCommand(PayPressed);
            btnEnter = new RelayCommand(Enter);
            btnClear = new RelayCommand(clearStrings);
            AddToChartBtn = new RelayCommand(AddToChart);
            quantityMinusBtn = new RelayCommand(quantityMinus);
            quantityPlusBtn = new RelayCommand(quantityPlus);
            saleBtn = new RelayCommand(sale);
            voidBtn = new RelayCommand(Void);
            holdBtn = new RelayCommand(Hold);
            viewHoldTransaction = new RelayCommand(viewHold);
            clearHoldbtn = new RelayCommand(clearHold);
            ExitBtn = new RelayCommand(Exit);
            btnSeachCustomer = new RelayCommand(searchCustomer);
            btnSetCustomer = new RelayCommand(setCustomer);
            btnSetCustomerView = new RelayCommand(setViewOfCustomerSearch);
            authoriseVoid = new RelayCommand(VoidEnter);
            searchBtn = new RelayCommand(search);
        }
        void setViewOfCustomerSearch()
        {
            if (hideCustomerView)
            {
                hideProductView = true;
                hideCustomerView = false;
                return;
            }
            hideCustomerView = true;
            hideProductView = false;
            return;
        }
        /// <summary>
        /// set the id of the customer selected
        /// </summary>
        /// <param name="obj"></param>
        void setCustomer(object obj)
        {
            if (obj != null)
            {
                vmPOS.customerId = (int)obj;
                vmPOS.customerNationaiId = vmPOS.customers.First(x => x.CustomerId == vmPOS.customerId).nationalId;
                setViewOfCustomerSearch();
            }
        }
        /// <summary>
        /// load customers searched in search result list
        /// </summary>
        void searchCustomer()
        {
            vmPOS.getCustomer();
        }
        void Exit()
        {
            if (vmPOS.Cart.Count > 0 | vmPOS.HoldList.Count > 0)
            {
                vmPOS.saleError = "Please clear your hold list and or sale list";
                return;
            }
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }
        void clearHold()
        {
            vmPOS.finalSaleUpdate(SaleState.failed);
            vmPOS.HoldList.Clear();
        }
        /// <summary>
        /// adding the product to the chart
        /// </summary>
        /// <param name="obj"></param>
        private void AddToChart(object obj)
        {
            HidePadView();
            //clear product in seach already

            int id = (int)obj;
            //select product from search list
            var selectedProduct = vmPOS.ProductsSearched.Where(x => x.ProductId == id).FirstOrDefault();
            //select product from chart
            var existingInChart = vmPOS.Cart.Where(x => x.ProductId == id).FirstOrDefault();
            //check if the product is already in chart
            if (existingInChart != null)
            {
                existingInChart.quantity += 1;
                existingInChart.price += selectedProduct.price;
                vmPOS.ProductsSearched.Clear();
            }
            else
            {
                vmPOS.Cart.Add(selectedProduct);
            }
            //increase the price
            vmPOS.totalPrice += selectedProduct.totalPrice;
            vmPOS.totalTax += selectedProduct.totalTax;
            vmPOS.totalDiscount += selectedProduct.totalDiscount;
            //clear searched chat
            vmPOS.ProductsSearched.Clear();
            clearStrings();
            OnFocusRequested(nameof(textBoxBarcode));
        }
        #endregion
        #region Button text
        private void One()
        {
            if (IsSearching)
            {
                SearchSt.Append("1"); generateString(); return;
            }
            st.Append("1");
            generateString();
        }
        private void Two()
        {
            if (IsSearching)
            {
                SearchSt.Append("2"); generateString(); return;
            }
            st.Append("2");
            generateString();
        }
        private void Three()
        {
            if (IsSearching)
            {
                SearchSt.Append("3"); generateString(); return;
            }
            st.Append("3");
            generateString();
        }
        private void Four()
        {
            if (IsSearching)
            {
                SearchSt.Append("4"); generateString(); return;
            }
            st.Append("4");
            generateString();
        }
        private void Five()
        {
            if (IsSearching)
            {
                SearchSt.Append("5"); generateString(); return;
            }
            st.Append("5");
            generateString();
        }
        private void Six()
        {
            if (IsSearching)
            {
                SearchSt.Append("6"); generateString(); return;
            }
            st.Append("6");
            generateString();
        }
        private void Seven()
        {
            if (IsSearching)
            {
                SearchSt.Append("7"); generateString(); return;
            }
            st.Append("7");
            generateString();
        }
        private void Eight()
        {
            if (IsSearching)
            {
                SearchSt.Append("8"); generateString(); return;
            }
            st.Append("8");
            generateString();
        }
        private void Nine()
        {
            if (IsSearching)
            {
                SearchSt.Append("9"); generateString(); return;
            }
            st.Append("9");
            generateString();
        }
        private void Zero()
        {
            if (IsSearching)
            {
                SearchSt.Append("0"); generateString(); return;
            }
            st.Append("0");
            generateString();
        }
        private void Dzero()
        {
            if (IsSearching)
            {
                SearchSt.Append("00"); generateString(); return;
            }
            st.Append("00");
            generateString();
        }
        private void Dot()
        {
            if (IsSearching)
            {
                SearchSt.Append("."); generateString(); return;
            }
            st.Append(".");
            generateString();
        }

        private void generateString()
        {
            clearTextBox();
            if (IsSearching)
            {
                textBoxBarcode = SearchSt.ToString(); return;
            }
            textBoxString = st.ToString();
        }
        #endregion

        #region methodes

        /// <summary>
        /// hides the view of the key pad
        /// </summary>
        private void HidePadView()
        {
            IsSearching = false; stateBtn = false; showPayBtn = true; passBoxShow = false;
            NotesBtn = false; ChangeTxtBlock = false; KeyPadBtn = false; SearchProductList = false; ListOfProductToPay = true;
        }
        /// <summary>
        /// search button is pressed to set true the view fo the keypad only and set issearch to true
        /// </summary>
        void SearchPressed()
        {
            if (canProceed(nameof(BtnSearchProduct)))
            {
                clearTextBox();
                NotesBtn = false; ChangeTxtBlock = false; KeyPadBtn = true; ListOfProductToPay = true;
                IsSearching = true;
                OnFocusRequested(nameof(textBoxBarcode));
            }

        }
        /// <summary>
        /// search button is pressed to set true the view fo the keypad and the note and change view and set issearch to false
        /// </summary>
        void PayPressed()
        {
            if (vmPOS.Cart.Count() > 0)
            {
                //check if stock is availabel
                if (vmPOS.stockAvailable(fromHold))
                {
                    clearTextBox();
                    //new
                    btnSearchVisibility = false;
                    showSearchCustomer = false;
                    showEnter = true;
                    showAddPlusBtn = false;
                    IsSearching = false; stateBtn = false; showPayBtn = false;
                    NotesBtn = true; ChangeTxtBlock = true; KeyPadBtn = true; 
                    SearchProductList = false; ListOfProductToPay = true;
                }

            }

        }
        private void search()
        {
            vmPOS.ProductsSearched.Clear();

            if (!string.IsNullOrWhiteSpace(textBoxBarcode))
            {
                vmPOS.AddProductToSearch(textBoxBarcode);
                OnFocusRequested(nameof(textBoxBarcode));
            }
        }
        /// <summary>
        /// add sale to the database with sale state failed
        /// </summary>
        void Enter()
        {
            if (canProceed(nameof(btnEnter)))
            {

                btnSearchVisibility = false;
                showSearchCustomer = false;
                showEnter = true;
                if (string.IsNullOrEmpty(vmPOS.saleError) | vmPOS.saleError == "Insufficient amount")
                {

                    //clear product in seach already
                    SearchProductList = false;
                    OnFocusRequested(nameof(textBoxString));
                    vmPOS.ProductsSearched.Clear();

                    decimal cashReceived = convertTocash();
                    if (cashReceived > 0)
                    {
                        if (vmPOS.totalPrice < cashReceived)
                        {
                            vmPOS.change = cashReceived - vmPOS.totalPrice;
                            vmPOS.setSale(cashReceived, vmPOS.change, fromHold);

                            if (vmPOS.executeVoid)
                            {
                                HidePadView();
                                stateBtn = true;
                                fromHold = false;
                                OnFocusRequested("sale");
                            }

                        }
                        else
                        {
                            vmPOS.saleError = "Insufficient amount";
                        }

                    }
                    clearStrings();
                }
            }
        }
        /// <summary>
        /// confirms the current sales as truly sold
        /// </summary>
        void sale()
        {
            if (canProceed(nameof(saleBtn)))
            {
                vmPOS.saleError = string.Empty;

                if (vmPOS.executeVoid)
                {
                   
                    btnSearchVisibility = true;
                    print(vmPOS.Cart.ToList(), vmPOS.totalPrice, vmPOS.change, vmPOS.user.username, vmPOS.refNumber);
                    vmPOS.finalSaleUpdate(SaleState.success);
                    HidePadView();
                    OnFocusRequested(nameof(textBoxBarcode));

                    showPayBtn = true;
                }
                else
                {
                    vmPOS.saleError = "check stock errors!";
                }
            }
        }
        /// <summary>
        /// void pressed show the admin passBox
        /// </summary>
        void Void()
        {
            if(canProceed(nameof(voidBtn)))
            {
                passBoxShow = true;
                btnSearchVisibility = true;
                OnFocusRequested("password");
            }

        }
        /// <summary>
        /// void auth
        /// </summary>
        /// <param name="pass">password for the admin</param>
        void VoidEnter(object pass)
        {
            adminLogError = "";
            if (string.IsNullOrWhiteSpace(username))
            {
                adminLogError = "please enter the username and passwod";
                return;
            }
            var passWord = pass as PasswordBox;
            if (validateAdmin(passWord))
            {
                if (vmPOS.executeVoid)
                {
                    vmPOS.finalSaleUpdate(SaleState.Void);
                    HidePadView();
                    btnSearchVisibility = true;
                    passBoxShow = false;
                    showPayBtn = true;
                    OnFocusRequested(nameof(textBoxBarcode));
                }
                else
                {
                    vmPOS.saleError = "check stock errors!";
                }
                username = string.Empty;

            }
            else
            {
                adminLogError = "Failed to authorise";
                OnFocusRequested("password");
                return;
            }


        }
        void Hold()
        {
            if (canProceed(nameof(holdBtn)))
            {
                if (vmPOS.executeVoid)
                {
                    if (vmPOS.HoldList.Count > 0)
                    {
                        vmPOS.saleError = "There is already an item in hold, make void";
                        btnClearHold = true;
                        return;
                    }
                    vmPOS.HoldList.Clear();


                    foreach (var item in vmPOS.Cart)
                    {
                        item.fromHold = true;
                        vmPOS.HoldList.Add(item);
                    }

                    vmPOS.finalSaleUpdate(SaleState.hold);
                    vmPOS.saleError = "Transaction sent to hold";
                    showPayBtn = true;
                    OnFocusRequested(nameof(textBoxBarcode));
                    HidePadView();
                }
                else
                {
                    vmPOS.saleError = "check stock errors!";
                }
            }
        }
        /// <summary>
        /// clears the bar code and the amount to pay textbox
        /// </summary>
        void clearTextBox()
        {
            textBoxString = string.Empty;
            textBoxBarcode = string.Empty;
        }
        void clearStrings()
        {
            clearTextBox();
            st.Clear();
            SearchSt.Clear();
            OnFocusRequested(nameof(textBoxBarcode));
        }
        /// <summary>
        /// convert the text string in text box to decimal
        /// </summary>
        /// <returns></returns>
        decimal convertTocash()
        {
            decimal cashPaid;
            if (!decimal.TryParse(textBoxString, out cashPaid))
            {
                vmPOS.saleError = "Entered amount";
                return 0;
            }
            return cashPaid;
        }
        void quantityPlus(object obj)
        {
            string code = (string)obj;
            vmPOS.AddProductToCart(code);
        }
        void quantityMinus(object obj)
        {
            int id = (int)obj;
            var product = vmPOS.Cart.Where(x => x.ProductId == id).FirstOrDefault();
            product.quantity -= 1;


            //recalculate totals 
            vmPOS.totalPrice = vmPOS.Cart.Sum(x => x.totalPrice);
            vmPOS.totalTax = vmPOS.Cart.Sum(x => x.totalTax);
            vmPOS.totalDiscount = vmPOS.Cart.Sum(x => x.totalDiscount);

            if (product.quantity == 0)
            {
                vmPOS.Cart.Remove(product);
                vmPOS.HoldList.Remove(product);
            }

        }
        void viewHold()
        {
            if (vmPOS.HoldList.Count > 0)
            {
                btnSearchVisibility = false;
                vmPOS.Cart.Clear();
                foreach (var item in vmPOS.HoldList)
                {
                    vmPOS.Cart.Add(item);
                }
                vmPOS.totalPrice = vmPOS.Cart.Sum(x => x.price);
                vmPOS.totalTax = vmPOS.Cart.Sum(x => x.tax);
                vmPOS.totalDiscount = vmPOS.Cart.Sum(x => x.discount);
                fromHold = true;
            }
            else
            {
                vmPOS.saleError = "No items in hold";
            }
        }
        bool validateAdmin(PasswordBox pass)
        {
            LogInView login = new LogInView();
            if (login.authenticateAdmin(username, pass.Password))
            {
                pass.Password = "";
                return true;
            }
            pass.Password = "";
            return false;
        }
        protected virtual void OnFocusRequested(string propertyName)
        {
            FocusRequested?.Invoke(this, new FocusRequestedEventArgs(propertyName));
        }
        public void print(List<POSProduct> cart, decimal totalPrice, decimal change, string tellerName, string refnumber)
        {
            printOutReciept?.Invoke(this, new PrintReceiptEventArgs(cart, totalPrice, change, tellerName, refnumber));
        }
        /// <summary>
        /// permissios for execution when button pressed
        /// the function chacks the name of button pressed 
        /// then return true if it can execute of return false
        /// </summary>
        /// <param name="name">name of the button</param>
        /// <returns></returns>
        int EnterPressedCount = 1;
        private bool canProceed(string name)
        {

            switch (name)
            {
                case nameof(btnEnter):
                    if(EnterPressedCount==1)
                    {
                        if(vmPOS.Cart.Count()>0)
                        {
                            EnterPressedCount += 1;
                            return true;
                        }
                    }
                    else 
                        {if(btnSearchVisibility == false & vmPOS.Cart.Count() > 0 & KeyPadBtn == false)
                        {
                            EnterPressedCount -= 1;
                            return true;
                        }

                    }
                       
                    break;
                case nameof(BtnSearchProduct):
                    if (btnSearchVisibility == true)
                        return true;
                    break;
                case nameof(holdBtn):
                    if (btnSearchVisibility == false& KeyPadBtn == false)
                        return true;
                    break;
                case nameof(voidBtn):
                    if (btnSearchVisibility == false & KeyPadBtn == false)
                        return true;
                    break;
                case nameof(saleBtn):
                    if (btnSearchVisibility == false & KeyPadBtn == false)
                        return true;
                    break;
                default:
                    return false;
            }

            return false;
        }
        #endregion
    }
}
