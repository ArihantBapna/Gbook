using System;
using System.Globalization;
using Gbook.ViewModel;
using Xamarin.Forms;

namespace Gbook.Converters
{
    public class GroupHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Data item = (Data)value;
            int c = (int)parameter;
            switch (c)
            {
                case 0:
                    return item.CourseName;
                case 1:
                    return item.OverallPercent;
                case 2:
                    return item.OverallColor;
                case 3:
                    return item.ClassType;
                default:
                    return item.CourseName;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
