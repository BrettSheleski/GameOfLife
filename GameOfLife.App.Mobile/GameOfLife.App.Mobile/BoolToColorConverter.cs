using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace GameOfLife.App.Mobile
{
    public class BoolToColorConverter : IValueConverter
    {
        public Color TrueColor { get; set; } = Color.Black;
        public Color FalseColor { get; set; } = Color.Transparent;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? TrueColor : FalseColor;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color c)
            {
                return c == TrueColor;
            }
            return false;
        }
    }
}
