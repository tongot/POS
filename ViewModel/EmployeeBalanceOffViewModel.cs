using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS
{
    public class EmployeeBalanceOffViewModel : BaseViewModel
    {
        #region pulblic values 
        public int teller_id { get; set; }
        public string error { get; set; }
        public DateTime date_of_sale { get; set; }
        public decimal amount_to_settle { get; set; }
        decimal? _actual_amount;
        public decimal? actual_amount
        {
            get
            {
                return _actual_amount;
            }
            set
            {

                if (_actual_amount != value)
                {
                    _actual_amount = value;
                    settle_amount_comments();
                }
            }
        }


        string _settled;
        public string settled
        {
            get
            {
                settle_amount_values();
                return _settled;
            }
            set
            {
                if (value != _settled)
                {
                    _settled = value;
                }
            }
        }
        public string comment { get; set; }
        public string authoriser { get; set; }
        public int authoriser_id { get; set; }

        public EmployeeDaySales sales_values { get; set; }
        #endregion
        #region button commands
        public ICommand btn_balance { get; set; }
        public ICommand btn_update { get; set; }

        public ICommand btn_get_balance { get; set; }
        #endregion
        public bool saving { get; set; } = true;

        #region private properties 
        ISales dbs = new SaleApp();
        IEmployeeBalances dbe = new EmployeeBalancesApp();
        EmployeeDailyBalances bals = new EmployeeDailyBalances();
        int update_id = 0;
        #endregion

        #region constractor
        public EmployeeBalanceOffViewModel()
        {
            teller_id = IocContainer.Kenel.Get<AppViewModel>().teller_id;
            authoriser = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username;
            authoriser_id = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.EmployeeId.Value;
            date_of_sale = DateTime.Now;
            sales_values = dbs.employeeDayBalance(teller_id);
            amount_to_settle = sales_values.success_value;


            btn_balance = new RelayCommand(balance);
            btn_get_balance = new RelayCommand(balance_settle);

        }
        #endregion
        #region methods
        private void balance()
        {
            error = string.Empty;
            if(_actual_amount==null)
            {
                error = "Please enter a valid amount";
                return;
            }


            settle_amount_comments();
            bals.EmployeeId = teller_id;
            bals.date = date_of_sale;
            bals.closing_balance = amount_to_settle;
            bals.balance_difference = amount_to_settle - _actual_amount.Value;
            bals.balanced_of_by = authoriser_id;
            bals.balance_off_date = DateTime.Now;
            bals.comment = comment;
            bals.EmployeeDailyBalancesId = update_id;
            
            dbe.addNewBalance(bals);
            error = "Balance off successfully saved";

        }
        private void balance_settle()
        {
            error = string.Empty;
            var balances = dbe.getBalanceByDate(date_of_sale);
            if(balances==null)
            {
                error = "There are no balances on this date";
                return;
            }
            actual_amount = balances.closing_balance+balances.balance_difference;
            amount_to_settle = balances.closing_balance;
            update_id = balances.EmployeeDailyBalancesId;
            settled = string.Empty;
            comment = string.Empty;
        }
        private void update_balances()
        {

        }
        void settle_amount_comments()
        {
            if (_actual_amount != null)
            {
                if (_actual_amount > amount_to_settle)
                {
                    comment = $"Excess of {_actual_amount - amount_to_settle}";
                    settled = "YES";
                }
                else if (amount_to_settle > _actual_amount)
                {
                    comment = $"Short of {amount_to_settle - actual_amount }";
                    settled = "NO";
                }
                else
                {
                    comment = $"Balanced";
                    settled = "YES";
                    
                }
            }
        }
        void settle_amount_values()
        {
            if (_settled == "YES")
            {
                bals.settled = true;
                bals.balanced_off = AppDatabase.settled.Yes;
                bals.settlement_date = DateTime.Now;
                bals.sattled_by = authoriser_id;
            }
            else
            {
                bals.settled = false;
                bals.balanced_off = AppDatabase.settled.Yes;
                bals.settlement_date = null;
                bals.sattled_by = null;
            }
        }
        #endregion
    }
}
