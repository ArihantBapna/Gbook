using System;
using System.Globalization;
using Xamarin.Forms;

namespace Gbook.Converters
{
    public class AssignmentDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string date = (string) value;
            string[] split = date.Split(' ');
            string dT = split[0];
            DateTime oDate = DateTime.Parse(dT);
            string finalDate = oDate.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(oDate.Month) + " " +oDate.Year;
            return finalDate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
