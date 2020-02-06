using System;
using System.Globalization;
using Gbook.Methods;
using Xamarin.Forms;
using Color = System.Drawing.Color;

namespace Gbook.Converters
{
    public class ColorGradientConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            Color col = ColorGet.ColorFromPercent((int)Math.Round(val, 0));
            return col;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
