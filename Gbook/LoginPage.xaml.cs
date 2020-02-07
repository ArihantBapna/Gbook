using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gbook.Comms;
using Syncfusion.XForms.Backdrop;
using Xamarin.Forms;

namespace Gbook
{
    public partial class LoginPage : ContentPage
    {
        public static bool LoggedIn = false;

        public static Color g1, g2;
        public static double o1, o2;

        public LoginPage()
        {
            InitializeComponent();
            checkLoggedIn();
            SetColor();
        }

        private void SetColor()
        {
            g1 = Color.FromHex("00416A");
            g2 = Color.FromHex("E4E5E6");
            o1 = 0;
            o2 = 1;

            grad1.Color = LoginPage.g1;
            grad1.Offset = LoginPage.o1;

            grad2.Color = LoginPage.g2;
            grad2.Offset = LoginPage.o2;

            usnHolder.FocusedColor = g2;
            pwdHolder.FocusedColor = g2;
            rememberCB.CheckedColor = g1;
            acceptTOS.CheckedColor = g2;

            usnHolder.ContainerBackgroundColor = Color.FromRgba(0, 0, 0, 0.3);
            pwdHolder.ContainerBackgroundColor = Color.FromRgba(0, 0, 0, 0.3);

            LoginButton.BackgroundColor = g1;
        }

        private async void checkLoggedIn()
        {
            ClientInitializor.username = Xamarin.Essentials.Preferences.Get("username", "null");
            ClientInitializor.password = Xamarin.Essentials.Preferences.Get("password", "null");

            if(ClientInitializor.username != "null")
            {
                await Task.Run(() => DoLogin());

                if (DataFetcher.loggedIn)
                {
                    LoggedIn = true;

                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Navigation.PopToRootAsync();
                    }
                    Application.Current.MainPage = new NavigationPage(new GradesPage());
                    //await Navigation.PushModalAsync(new GradesPage());
                    ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = g1;
                }
                else
                {
                    usnHolder.Hint = "Incorrect Username or Password";
                    usnHolder.FocusedColor = Color.Red;
                    usnHolder.Focus();
                    pwd.Text = "";
                    usn.Text = "";
                    acceptTOS.IsChecked = false;
                    LoginButton.IsEnabled = true;
                    await LoginButton.FadeTo(1, 300);

                    LoginButton.IsVisible = true;

                    indicator.IsVisible = false;
                    indicator.IsEnabled = false;
                    indicator.IsRunning = false;
                }
            }
        }

        void TOSChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
        {
            if (e.IsChecked.HasValue && e.IsChecked.Value)
            {
                buttonHolder.ShouldIgnoreTouches = false;
                LoginButton.BackgroundColor = g1;
                LoginButton.IsEnabled = true;
            }
            else
            {
                buttonHolder.ShouldIgnoreTouches = true;
                LoginButton.BackgroundColor = Color.Gray;
                LoginButton.IsEnabled = false;
            }
        }

        async void OnLoginAttempt(object sender, EventArgs args)
        {
            LoginButton.IsEnabled = false;
            await LoginButton.FadeTo(0, 300);
            LoginButton.IsVisible = false;

            indicator.IsVisible = true;
            indicator.IsEnabled = true;
            indicator.IsRunning = true;

            ClientInitializor.username = usn.Text;
            ClientInitializor.password = pwd.Text;

            await Task.Run(() => DoLogin());

            if (!DataFetcher.loggedIn)
            {
                usnHolder.Hint = "Incorrect Username or Password";
                usnHolder.FocusedColor = Color.Red;
                usnHolder.Focus();
                pwd.Text = "";
                usn.Text = "";
                acceptTOS.IsChecked = false;
                LoginButton.IsEnabled = true;
                await LoginButton.FadeTo(1, 300);

                LoginButton.IsVisible = true;

                indicator.IsVisible = false;
                indicator.IsEnabled = false;
                indicator.IsRunning = false;
            }
            else
            {
                LoggedIn = true;

                if (rememberCB.IsChecked.Value)
                {
                    Xamarin.Essentials.Preferences.Set("username", ClientInitializor.username);
                    Xamarin.Essentials.Preferences.Set("password", ClientInitializor.password);
                }

                if (Device.RuntimePlatform == Device.iOS)
                {
                    await Navigation.PopToRootAsync();
                }
                Application.Current.MainPage = new NavigationPage(new GradesPage());
                //await Navigation.PushModalAsync(new GradesPage());
                ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = g1;

            }
        }

        private async Task DoLogin()
        {
            await DataFetcher.AttemptLogIn();
        }
    }
}
