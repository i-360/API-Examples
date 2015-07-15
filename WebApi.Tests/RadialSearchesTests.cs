using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    [TestClass]
    public class RadialSearchesTests
    {
        private HttpClient _client;
        private const string RequestUri = "RadialSearches";

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
        public async Task GivenSearchRequest_Post_ReturnsSuccess()
        {
            // Arrange
            var request = new
            {
                ApiNotification = true,
                Columns = CreateColumns(),
                OrderBy = CreateOrderBy(),
                ExportType = 0,
                Randomized = true,
                RecordLimit = 100,
                Location = "2000 Clarendon Blvd, Arlington, VA",
                Radius = 2
            };

            // Act
            var response = await PostAsJsonAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public void GivenSearchInvalidRequest_Post_ReturnsBadRequest()
        {
            var requestMissingRadius = new
            {
                ApiNotification = true,
                Columns = CreateColumns(),
                OrderBy = CreateOrderBy(),
                ExportType = 0,
                Randomized = true,
                RecordLimit = 100,
                Location = "2000 Clarendon Blvd, Arlington, VA"
            };

            var requestMissingLocation = new
            {
                ApiNotification = true,
                Columns = CreateColumns(), // See Search Columns tests for these values
                OrderBy = CreateOrderBy(), // See Search Columns tests for these values
                ExportType = 0,
                Randomized = true,
                RecordLimit = 100,
                Radius = 2
            };

            var taskList = new List<Task>
            {
                VerifyBadRequestReturned(requestMissingRadius),
                VerifyBadRequestReturned(requestMissingLocation),
                VerifyBadRequestReturned(null)
            };

            Task.WaitAll(taskList.ToArray());
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenUnsecureAddress_NonGetMethods_ReturnForbidden()
        {
            // Arrange
            var unsecuredUrl = Settings.Default.UnsecureUrl;
            _client.BaseAddress = new Uri(unsecuredUrl);

            // Act
            await TestHelper.VerifyForbiddenErrorReturned(_client, HttpMethod.Post, RequestUri);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenNoAuthorization_EachEndpoint_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            await TestHelper.VerifyUnauthorizedErrorReturned(_client, HttpMethod.Post, RequestUri);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenAuthorizationWithoutRole_EachEndpoint_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization =
                AuthenticationHeaderValueFactory.CreateForApiTesterWithoutApiRole();

            // Act
            await TestHelper.VerifyUnauthorizedErrorReturned(_client, HttpMethod.Post, RequestUri);
        }

        private async Task VerifyBadRequestReturned(object request)
        {
            var response = await _client.PostAsJsonAsync(RequestUri, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private static List<string> CreateOrderBy()
        {
            return new List<string> { "firstname(c)", "lastname(c)", "gender(c)", "Phone(c)" };
        }

        private static List<string> CreateColumns()
        {
            return new List<string>
            {
                "firstname(c)",
                "lastname(c)",
                "gender(c)",
                "Phone(c)",
                "C_AddrLine1",
                "C_AddrLine2",
                "C_City",
                "C_State",
                "C_Zip5"
            };
        }

        private async Task<HttpResponseMessage> PostAsJsonAsync(object request, int retries = 10)
        {
            var response = await _client.PostAsJsonAsync(RequestUri, request);

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable && retries > 0)
            {
                await Task.Delay(1000);
                response = await PostAsJsonAsync(request, --retries);
            }
            return response;
        }
    }
}
