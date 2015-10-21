using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    [TestClass]
    public class SearchColumnsTests
    {
        private const string RequestUri = "SearchColumns";
        private HttpClient _client;

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
        public async Task GivenMethodAndUrl_SendAsync_ReturnsSuccess()
        {
            // Arrange
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, RequestUri);

            // Act
            var response = await _client.SendAsync(httpRequestMessage);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenUnsecureAddress_GetMethods_RedirectedToSecureAddress()
        {
            // Arrange
            var unsecuredUrl = Settings.Default.UnsecureUrl;
            _client.BaseAddress = new Uri(unsecuredUrl);

            // Act
            await TestHelper.VerifyRedirectedToSecureAddress(_client, HttpMethod.Get, RequestUri);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenNoAuthorization_EachEndpoint_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            await TestHelper.VerifyUnauthorizedErrorReturned(_client, HttpMethod.Get, RequestUri);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenAuthorizationWithoutRole_EachEndpoint_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValueFactory.CreateForApiTesterWithoutApiRole();

            // Act
            await TestHelper.VerifyUnauthorizedErrorReturned(_client, HttpMethod.Get, RequestUri);
        }
    }
}
