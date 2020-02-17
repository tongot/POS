using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;


namespace POS
{
    /// <summary>
    /// convert the <see cref="ApplicationPage"/> to an actual page
    /// </summary>
    public class IocConvert : BaseValueConveter<IocConvert>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((string)value)
            {
                case nameof(AppViewModel):
                    return IocContainer.Kenel.Get<AppViewModel>();
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
