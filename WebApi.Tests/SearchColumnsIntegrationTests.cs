using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    [TestClass]
    public class SearchColumnsIntegrationTests
    {
        private HttpClient _client;

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
            // Arrange
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "SearchColumns");

            // Act
            var response = _client.SendAsync(httpRequestMessage).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
