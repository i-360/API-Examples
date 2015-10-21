using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    [TestClass]
    public class ImportsTests
    {
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
