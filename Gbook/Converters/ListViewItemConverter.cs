using System;
using System.Collections.Generic;
using System.Globalization;
using Gbook.ClassFiles;
using Xamarin.Forms;

namespace Gbook.Converters
{
    public class ListViewConverterItem : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<CategoryInfo> catList = (List<CategoryInfo>)value;
            int count = catList.Count;
            switch (count)
            {
                case 1:
                    return 0.7;
                case 2:
                    return 0.6;
                case 3:
                    return 0.5;
                default: return 0.4;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
