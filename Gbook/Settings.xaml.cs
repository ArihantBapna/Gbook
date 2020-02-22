using System;
using System.Collections.Generic;
using Gbook.ClassFiles;
using Gbook.Converters;
using Gbook.ViewModel;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;

namespace Gbook
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            this.Title = "Settings";
            grad1.Color = LoginPage.g1;
            grad1.Offset = LoginPage.o1;
            grad2.Color = LoginPage.g2;
            grad2.Offset = LoginPage.o2;

            SetPicker();

            NavListRepo r = new NavListRepo();
            navList.ItemsSource = r.Navy;
            StudentName.Text = Globals.Dataset[0].StudentName;
            navList.SelectionChanged += navListTapped;

        }

        private void SetPicker()
        {
            colorPick1.BindingContext = new ColorInfo();
            colorPick1.SelectionChanged += chipgroup1_SelectionChanged;
            colorPick1.SetBinding(SfChipGroup.SelectedItemProperty, new Binding("SelectedItem", BindingMode.TwoWay));
            Binding b = new Binding("Colors");
            b.Converter = new ColorToChipConverter();
            colorPick1.SetBinding(SfChipGroup.ItemsSourceProperty, b);

            colorPick2.BindingContext = new ColorInfo();
            colorPick2.SelectionChanged += chipgroup1_SelectionChanged;
            colorPick2.SetBinding(SfChipGroup.SelectedItemProperty, new Binding("SelectedItem", BindingMode.TwoWay));
            Binding b2 = new Binding("Colors");
            b2.Converter = new ColorToChipConverter2();
            colorPick2.SetBinding(SfChipGroup.ItemsSourceProperty, b2);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            double heightSide = Application.Current.MainPage.Height;
            navDraw.HeightRequest = heightSide;

            if (Xamarin.Essentials.Preferences.Get("chartType", 0) == 1)
            {
                ChartSwitch.IsOn = true;
            }
        }

        private void navListTapped(object sender, ItemSelectionChangedEventArgs e)
        {
            NavList r = (NavList)navList.SelectedItem;
            String m = r.NavTitle;
            if (m == "Grades")
            {
                NavigationPage p = new NavigationPage(new GradesPage()) { BarBackgroundColor = LoginPage.g1, BarTextColor = Color.White };
                Application.Current.MainPage = p;
            }
            else if (m == "Settings")
            {
                Application.Current.MainPage = new NavigationPage(new Settings()) { BarBackgroundColor = LoginPage.g1, BarTextColor = Color.White };
            }
        }

        void hamburgerButton_Clicked(object sender, EventArgs e)
        {
            navDraw.ToggleDrawer();
        }

        private void chipgroup1_SelectionChanged(object sender, Syncfusion.Buttons.XForms.SfChip.SelectionChangedEventArgs e)
        {
            Console.WriteLine(e.AddedItem);
        }


        private void ChartSwitch_Changed(object sender, SwitchStateChangedEventArgs e)
        {
            SfSwitch tempSwitch = (SfSwitch)sender;
            bool state = (bool)tempSwitch.IsOn;

            if (state)
            {
                Xamarin.Essentials.Preferences.Set("chartType", 1);
            }
            else
            {
                Xamarin.Essentials.Preferences.Set("chartType", 0);
            }
        }
    }
}
