using System;
using System.IO;
using System.Net;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace C_Sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            string _tenantID = "{Tenant ID}"; //Active Directory ID
            string _applicationID = "{Client Application ID}"; // Your AD Application ID a.k.s Client ID
            string _applicationSecret = "{Client Application Secret}"; // Your AD Application Secret a.k.s. Client Secret

            string _accessToken = CreateAccessToken(_tenantID, _applicationID, _applicationSecret);
            _accessToken = "Bearer " + _accessToken;

            string _result = HttpGet("{Your HttpTrigger Function URL with code}", _accessToken);

            Console.WriteLine(_result);
            Console.ReadLine();
        }
        public static string CreateAccessToken(string TenantID, string ApplicationID, string ApplicationSecret)
        {
            try
            {
                var context = new AuthenticationContext("https://login.microsoftonline.com/" + TenantID + "/oauth2/token");
                ClientCredential clientCredential = new ClientCredential(ApplicationID, ApplicationSecret);
                var tokenResponse = context.AcquireTokenAsync(ApplicationID, clientCredential).Result;
                var accessToken = tokenResponse.AccessToken;
                return accessToken;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string HttpGet(string url, string accessToken)
        {
            WebHeaderCollection headers = new WebHeaderCollection
            {
                { "Authorization", accessToken }
            };
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers = headers;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            return sr.ReadToEnd();
        }
    }
}