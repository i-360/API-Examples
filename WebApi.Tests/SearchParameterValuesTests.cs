using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    [TestClass]
    public class SearchParameterValuesTests
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
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValueFactory.CreateForApiTester();
        }

        [TestCleanup]
        public void CleanupTests()
        {
            _client.Dispose();
            _client = null;
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public void GivenMethodAndUrl_SendAsync_ReturnsSuccess()
        {
            Task.WaitAll(Endpoints.Select(e => TestHelper.VerifySuccess(_client, e.Key, e.Value)).ToArray());
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public void GivenUnsecureAddress_GetMethods_RedirectedToSecureAddress()
        {
            // Arrange
            var unsecuredUrl = Settings.Default.UnsecureUrl;
            _client.BaseAddress = new Uri(unsecuredUrl);

            // Act
            Task.WaitAll(
                Endpoints.Select(e => TestHelper.VerifyRedirectedToSecureAddress(_client, e.Key, e.Value))
                    .ToArray());
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public void GivenNoAuthorization_EachEndpoint_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            Task.WaitAll(Endpoints.Select(e => TestHelper.VerifyUnauthorizedErrorReturned(_client, e.Key, e.Value)).ToArray());
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public void GivenAuthorizationWithoutRole_EachEndpoint_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValueFactory.CreateForApiTesterWithoutApiRole();

            // Act
            Task.WaitAll(Endpoints.Select(e => TestHelper.VerifyUnauthorizedErrorReturned(_client, e.Key, e.Value)).ToArray());
        }
    }
}
