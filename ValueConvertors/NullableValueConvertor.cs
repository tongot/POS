
using System;
using System.Globalization;

namespace POS
{
    public class NullableValueConvertor : BaseValueConveter<NullableValueConvertor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return null;

            return value;
        }
    }
}
