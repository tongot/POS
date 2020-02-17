using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace POS
{
    /// <summary>
    /// visiblity coverter for the views that converts to collapsed true or false 
    /// </summary>
    public class RolesViewConveter : BaseValueConveter<RolesViewConveter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           if(value!=null &parameter!=null)
            {
             if (hasRight((List<string>)value,(string)parameter))
                        {
                            return Visibility.Visible;
                        }
                        return Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        bool hasRight(List<string> roles,string right)
        {   
            
            var pm = IocContainer.Kenel.Get<AppViewModel>().RolesPermissions;
            foreach (var item in roles)
            {
                var userFunc = pm.roles.Where(x => x.Role == item).Select(x => x.permissions).ToList();
                var userRights = userFunc.Where(x => x.Contains(right));
                if(userRights.Count()>0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
