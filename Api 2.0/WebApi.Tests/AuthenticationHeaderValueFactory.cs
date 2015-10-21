using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    internal class AuthenticationHeaderValueFactory
    {
        public static async Task<AuthenticationHeaderValue> CreateForApiTester()
        {
            return await Create(TestSettings.Default.ApiTesterUserName, TestSettings.Default.ApiTesterPassword, TestSettings.Default.LoginUrl);
        }

        public static async Task<AuthenticationHeaderValue> Create(string username, string password, string loginUrl)
        {
            var authResponse = await GetToken(username, password, loginUrl);
            var authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", authResponse);
            return authenticationHeaderValue;
        }

        private static async Task<string> GetToken(string username, string password, string loginUrl)
        {
            var client = new HttpClient();
            AddAcceptHeader(client);
            AddAuthorizationHeader(client);
            var content = CreateContent(username, password);
            var response = await client.PostAsync(loginUrl, content);
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadAsAsync<ApiAccessToken>();
            return token.AccessToken;
        }

        private static FormUrlEncodedContent CreateContent(string username, string password)
        {
            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("scope", "openid profile sampleApi")
            };
            var content = new FormUrlEncodedContent(keyValuePairs);
            return content;
        }
        
        private static void AddAuthorizationHeader(HttpClient client)
        {
            var authToken = CreateBasicAuthToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        private static void AddAcceptHeader(HttpClient client)
        {
            var mediaTypeWithQualityHeaderValue = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(mediaTypeWithQualityHeaderValue);
        }
        
        private static string CreateBasicAuthToken()
        {
            var plainTextBytes = Encoding.ASCII.GetBytes($"{"roclient"}:{"secret"}");
            var basicAuthToken = Convert.ToBase64String(plainTextBytes);
            return basicAuthToken;
        }
        
        [DataContract]
        private class ApiAccessToken
        {
            [DataMember(Name = "access_token")]
            public string AccessToken { get; set; }
        }
    }
}


