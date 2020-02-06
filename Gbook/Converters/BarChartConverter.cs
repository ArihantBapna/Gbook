using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Color = System.Drawing.Color;
using Syncfusion.SfChart.XForms;
using Gbook.ClassFiles;
using Gbook.Methods;

namespace Gbook.Converters
{
    public class BarChartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<CategoryInfo> cats = new List<CategoryInfo>();
            double d = 0;
            int par = (int)parameter;
            switch (par)
            {
                case 1:
                    {
                        cats = (List<CategoryInfo>)value;
                        switch (cats.Count)
                        {
                            case 1:
                                return 0.25;
                            case 2:
                                return 0.7;
                            case 3:
                                return 0.6;
                            case 4:
                                return 0.55;
                            default:
                                return 0.4;
                        }
                    }
                case 2:
                    {
                        cats = (List<CategoryInfo>)value;
                        switch (cats.Count)
                        {
                            case 1:
                                return 60;
                            case 2:
                                return 45;
                            case 3:
                                return 27;
                            default:
                                return 17;
                        }
                    }
                case 3:
                    d = (double)value;
                    return ColorGet.ColorFromPercent((int)Math.Round(d, 0));
                case 4:
                    {
                        cats = (List<CategoryInfo>)value;
                        ChartColorModel cm = new ChartColorModel();
                        cm.Palette = ChartColorPalette.Custom;
                        ChartColorCollection cmc = new ChartColorCollection();
                        foreach (CategoryInfo ci in cats)
                        {
                            cmc.Add(ColorGet.ColorFromPercent((int)Math.Round(ci.Percent, 0)));
                        }
                        cm.CustomBrushes = cmc;
                        return cm;
                    }
                default:
                    return 10;
                case 5:
                    {
                        cats = (List<CategoryInfo>)value;
                        switch (cats.Count)
                        {
                            case 1:
                                return 40;
                            case 2:
                                return 30;
                            case 3:
                                return 20;
                            default:
                                return 10;
                        }
                    }
                case 6:
                    {
                        d = (double)value;
                        if (d < 20)
                        {
                            return new Thickness(0, -20);
                        }
                        return -1;
                    }

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
