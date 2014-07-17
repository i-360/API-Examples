using System;
using System.Net.Http;
using i360.Web.Api.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace i360.Web.Api.Tests
{
    [TestClass]
    public class ImportsControllerIntegrationTests
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
        public void GivenImportId_Get_ReturnsImportStatus()
        {
            // Arrange
            const long importId = 1;
            var requestUri = string.Format("Imports/{0}", importId);
            
            // Act
            var response = _client.GetAsync(requestUri).Result;

            // Assert
            response.EnsureSuccessStatusCode();
            response.PrintContent();
        }
    }
}
