using System;
using System.Globalization;
using Gbook.Methods;
using Xamarin.Forms;


namespace Gbook.Converters
{
    public class LegendItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double percent = (double)value;

            if (targetType.Name == "String")
            {
                return percent.ToString();
            }
            return ColorGet.ColorFromPercent((int)Math.Round(percent, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
