using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using DevExpress.Utils;
using DevExpress.Xpf.Gauges;
using DevExpress.Mvvm;

namespace Company.Schema.Controls
{

    public class BoolToVisibilityConverter : IValueConverter
    {
        #region IValueConvector implementation
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                bool isSegmentEnabled = (bool)value;
                if (isSegmentEnabled)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return null;
        }
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }

}
