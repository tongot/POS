using System;
using System.Globalization;

using System.Windows;

namespace POS
{
    /// <summary>
    /// visiblity coverter for the views that converts to collapsed true or false 
    /// </summary>
    public class visibilityConverter : BaseValueConveter<visibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
