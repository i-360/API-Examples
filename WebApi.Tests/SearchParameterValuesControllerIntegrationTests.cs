using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    [TestClass]
    public class SearchParameterValuesControllerIntegrationTests
    {
        private HttpClient _client;

        private static readonly List<KeyValuePair<HttpMethod, string>> Endpoints = new List
            <KeyValuePair<HttpMethod, string>>
        {
            new KeyValuePair<HttpMethod, string>(HttpMethod.Get, "SearchParameters/2/Values"),
            new KeyValuePair<HttpMethod, string>(HttpMethod.Get, "SearchParameters/5/Values?stateValue=47")
        };

        [TestInitialize]
        public void InitializeTests()
        {
            _client = new HttpClient { BaseAddress = new Uri(Settings.Default.TestUrl) };
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValueFactory.Create();
        }

        [TestCleanup]
        public void CleanupTests()
        {
            _client.Dispose();
            _client = null;
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GivenMethodAndUrl_SendAsync_ReturnsSuccess()
        {
            // Act
            foreach (var e in Endpoints)
            {
                VerifySuccess(e);
            }
        }

        private void VerifySuccess(KeyValuePair<HttpMethod, string> e)
        {
            // Act
            var response = _client.SendAsync(new HttpRequestMessage(e.Key, e.Value)).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
