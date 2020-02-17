
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace POS
{
    public abstract class BaseValueConveter<T> : MarkupExtension, IValueConverter
        where T:class,new()
    {
        /// <summary>
        /// a single static instance of this value type
        /// </summary>
        private static T mConvertor = null;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConvertor ?? (mConvertor = new T());
        }
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

    }
}
