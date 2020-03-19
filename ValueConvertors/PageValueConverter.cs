using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;


namespace POS
{
    /// <summary>
    /// convert the <see cref="ApplicationPage"/> to an actual page
    /// </summary>
    public class PageValueConverter : BaseValueConveter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((ApplicationPage)value)
            {
                case ApplicationPage.logInPage:
                    return new LoginPage();

                case ApplicationPage.menuPage:
                    return new Menu();

                case ApplicationPage.product:
                    return new Products();

                case ApplicationPage.category:
                    return new CategoryPage();

                case ApplicationPage.Branches:
                    return new Branches();

                case ApplicationPage.posPage:
                    return new Pos2Page();

                case ApplicationPage.salesPage:
                    return new SalesPage();

                case ApplicationPage.employeePage:
                    return new EmployeePage();

                case ApplicationPage.customerPage:
                    return new Customers();

                case ApplicationPage.roleManager:
                    return new roleManager();

                case ApplicationPage.connections:
                    return new DataBaseCon();

                case ApplicationPage.discountPage:
                    return new DiscountsPage();

                case ApplicationPage.taxPage:
                    return new TaxPage();

                case ApplicationPage.dashboard:
                    return new Dash_Board();

                case ApplicationPage.balanceOfEmployee:
                    return new EmployeeBalanceOff();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
