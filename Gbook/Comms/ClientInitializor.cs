using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gbook.Comms
{
    public class ClientInitializor
    {
        public static HttpClient Client = new HttpClient();
        public static bool LoggedIn = false;

        private static CookieContainer cookieContainer;
        public static string username = "";
        public static string password = "";
        public static string schoolId = "";
        public static string CurrentTerm = "";

        public static async Task<bool> ReturnTheClient()
        {
            var baseAddress = new Uri("https://portal.mcpsmd.org/public/");
            cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            Client = new HttpClient(handler) { BaseAddress = baseAddress };
            var homePageResult = Client.GetAsync("/");
            homePageResult.Result.EnsureSuccessStatusCode();

            var content = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("pstoken", "5033312431ufrTNXNWEqd0OAOvqbIwfLclM97XUP"),
            new KeyValuePair<string, string>("contextData", "008517F219AE6E7C2A7C78026E8677CC2E8671FE032A37A6286A6F87237F5AA4"),
            new KeyValuePair<string, string>("dbpw", "77935b96b41be49bf7bfd05349bc19f0"),
            new KeyValuePair<string, string>("translator_username", ""),
            new KeyValuePair<string, string>("translator_password", ""),
            new KeyValuePair<string, string>("translator_ldappassword", ""),
            new KeyValuePair<string, string>("returnUrl", ""),
            new KeyValuePair<string, string>("serviceName", "PS+Parent+Portal"),
            new KeyValuePair<string, string>("serviceTicket", ""),
            new KeyValuePair<string, string>("pcasServerUrl", "%2F"),
            new KeyValuePair<string, string>("credentialType", "User+Id+and+Password+Credential"),
            new KeyValuePair<string, string>("ldappassword", password),
            new KeyValuePair<string, string>("account", username),
            new KeyValuePair<string, string>("pw", "8d96f4fcf1e0d4e2ce67419b28cb0997"),
            new KeyValuePair<string, string>("translatorpw", "")
                });
            var loginResult = Client.PostAsync("/guardian/home.html", content).Result;
            loginResult.EnsureSuccessStatusCode();
            await ReturnRAsync(loginResult);
            if (LoggedIn)
            {
                schoolId = cookieContainer.GetCookies(new Uri("https://portal.mcpsmd.org/public/guardian/home.html"))[3].Value;
                return true;
            }
            return false;
        }

        private static async Task ReturnRAsync(HttpResponseMessage loginResult)
        {
            var r = await loginResult.Content.ReadAsStringAsync();
            if (r.Length > 10000) LoggedIn = true;
            else LoggedIn = false;
        }

    }
}

