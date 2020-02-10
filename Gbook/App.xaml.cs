using System;
using System.Threading.Tasks;
using Gbook.Comms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gbook
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTkwMDI1QDMxMzcyZTM0MmUzMFE5V2NsaHk2K0w5MXhNQVNMR0EvOWhvNzJ2dklXazAzMkpSYjdWZXVKUXM9");
            InitializeComponent();
        }

        protected async override void OnStart()
        {
            ClientInitializor.username = Xamarin.Essentials.Preferences.Get("username", "null");
            ClientInitializor.password = Xamarin.Essentials.Preferences.Get("password", "null");
            Console.WriteLine("------------------");
            Console.WriteLine(ClientInitializor.username);
            Console.WriteLine(ClientInitializor.password);
            if (ClientInitializor.username != "null")
            {
                await DataFetcher.AttemptLogIn();
                Console.WriteLine("Finished data load");
                Console.WriteLine(ClientInitializor.username);
                Console.WriteLine(ClientInitializor.password);
                if (DataFetcher.loggedIn)
                {
                    LoginPage.LoggedIn = true;
                    MainPage = new NavigationPage(new GradesPage());
                    //await Navigation.PushModalAsync(new GradesPage());
                    ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = LoginPage.g1;
                }
                else
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
