using System;
using System.Collections.Generic;
using Gbook.ClassFiles;
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

            grad1.Color = LoginPage.g1;
            grad1.Offset = LoginPage.o1;
            grad2.Color = LoginPage.g2;
            grad2.Offset = LoginPage.o2;

            NavListRepo r = new NavListRepo();
            navList.ItemsSource = r.Navy;
            StudentName.Text = Globals.Dataset[0].StudentName;
            navList.SelectionChanged += navListTapped;

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
