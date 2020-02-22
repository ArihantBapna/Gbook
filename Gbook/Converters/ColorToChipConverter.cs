using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace Gbook.Converters
{
    public class ColorToChipConverter : IValueConverter
    {

        SfChip selectedChip = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<SfChip> colorChips = new ObservableCollection<SfChip>();
            foreach (var item in value as ObservableCollection<Color>)
            {

                var colorChip = new SfChip() { BackgroundColor = (Color)item, ShowSelectionIndicator = true, SelectionIndicatorColor = Color.Transparent, CornerRadius = 20, WidthRequest = 40, HeightRequest = 40, Margin = 10, BorderWidth = 1 };
                colorChip.BorderColor = Color.FromRgb(-(colorChip.BackgroundColor.R - 1), -(colorChip.BackgroundColor.G - 1), -(colorChip.BackgroundColor.B - 1));
                var mean = (colorChip.BackgroundColor.R + colorChip.BackgroundColor.G + colorChip.BackgroundColor.B) / 3;
                colorChip.BorderColor = mean < 0.5 ? Color.White : Color.Black;

                colorChip.Clicked += ColorChip_Clicked;
                colorChips.Add(colorChip);

            }
            return colorChips;
        }
        private void ColorChip_Clicked(object sender, EventArgs e)
        {
            if (selectedChip != null)
            {
                selectedChip.ShowSelectionIndicator = false;
                selectedChip.BorderWidth = 1;
            }

            selectedChip = (sender as SfChip);

            Color col = selectedChip.BackgroundColor;
            Xamarin.Essentials.Preferences.Set("Color1", col.ToHex());
            LoginPage.g1 = col;
            Application.Current.MainPage = new NavigationPage(new Settings()) { BarBackgroundColor = LoginPage.g1, BarTextColor = Color.White};

            (selectedChip.Parent as FlexLayout).BackgroundColor = selectedChip.BackgroundColor;

            selectedChip.ShowSelectionIndicator = true;
            selectedChip.SelectionIndicatorColor = selectedChip.BorderColor;
            selectedChip.BorderWidth = 3;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorToChipConverter2 : IValueConverter
    {

        SfChip selectedChip = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<SfChip> colorChips = new ObservableCollection<SfChip>();
            foreach (var item in value as ObservableCollection<Color>)
            {

                var colorChip = new SfChip() { BackgroundColor = (Color)item, ShowSelectionIndicator = true, SelectionIndicatorColor = Color.Transparent, CornerRadius = 20, WidthRequest = 40, HeightRequest = 40, Margin = 10, BorderWidth = 1 };
                colorChip.BorderColor = Color.FromRgb(-(colorChip.BackgroundColor.R - 1), -(colorChip.BackgroundColor.G - 1), -(colorChip.BackgroundColor.B - 1));
                var mean = (colorChip.BackgroundColor.R + colorChip.BackgroundColor.G + colorChip.BackgroundColor.B) / 3;
                colorChip.BorderColor = mean < 0.5 ? Color.White : Color.Black;
                colorChip.Clicked += ColorChip_Clicked;
                colorChips.Add(colorChip);

            }
            return colorChips;
        }
        private void ColorChip_Clicked(object sender, EventArgs e)
        {
            if (selectedChip != null)
            {
                selectedChip.ShowSelectionIndicator = false;
                selectedChip.BorderWidth = 1;
            }

            selectedChip = (sender as SfChip);

            Color col = selectedChip.BackgroundColor;
            Xamarin.Essentials.Preferences.Set("Color2", col.ToHex());
            LoginPage.g2 = col;
            Application.Current.MainPage = new NavigationPage(new Settings()) { BarBackgroundColor = LoginPage.g1, BarTextColor = Color.White };

            (selectedChip.Parent as FlexLayout).BackgroundColor = selectedChip.BackgroundColor;

            selectedChip.ShowSelectionIndicator = true;
            selectedChip.SelectionIndicatorColor = selectedChip.BorderColor;
            selectedChip.BorderWidth = 3;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
