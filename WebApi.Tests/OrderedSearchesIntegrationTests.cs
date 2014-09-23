using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    [TestClass]
    public class OrderedSearchesIntegrationTests
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
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "OrderedSearches/1");

            // Act
            var response = _client.SendAsync(httpRequestMessage).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GivenSearchRequest_Put_ReturnsSuccess()
        {
            // Arrange
            var criteria = CreateCriteria();
            var request = new 
            {
                ApiNotification = true,
                EmailNotification = true,
                Columns = new List<string>
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
                },
                OrderBy = new List<string> { "firstname(c)", "lastname(c)", "gender(c)", "Phone(c)" },
                Criteria = criteria,
                ExportType = 0,
                Randomized = true,
                RecordLimit = 100
            };

            // Act
            var response = _client.PostAsJsonAsync("OrderedSearches", request).Result;

            // Assert
            response.EnsureSuccessStatusCode();
        }

        private static Dictionary<int, string> CreateCriteria()
        {
            const int stateParameter = 2;
            const int virginiaStateValue = 47;
            const int searchTypeParameter = 3;
            const int i360DataValue = 1;
            const int countyParameter = 5;
            var countyValues = CreateCountyValues();
            const int radiusLatLongParameter = 27;
            const string radiusLatLongValue =
                "poly\n" +
                "38.87424355282139#-77.30616654214167\n" +
                "38.85419431555039#-77.33878220376276\n" +
                "38.82210377736585#-77.32024277505182\n" +
                "38.829057955280234#-77.27561081704401\n" +
                "38.8590066481109#-77.26634110268854\n" +
                "38.87424355282139#-77.30616654214167\n";
            const int hasEmailParameter = 377;
            const int ageRangeParameter = 15;
            const string ageRange = "18#30";

            var criteria = new Dictionary<int, string>
            {
                {stateParameter, virginiaStateValue.ToString(CultureInfo.InvariantCulture)},
                {searchTypeParameter, i360DataValue.ToString(CultureInfo.InvariantCulture)},
                {countyParameter, countyValues},
                {radiusLatLongParameter, radiusLatLongValue},
                {hasEmailParameter, "1"},
                {ageRangeParameter, ageRange}
            };
            return criteria;
        }

        private static string CreateCountyValues()
        {
            const int fairfaxCountyValue = 146;
            const int arlingtonCountyValue = 169;
            const int kingGeorgeCountyValue = 175;
            var counties = new[] { fairfaxCountyValue, arlingtonCountyValue, kingGeorgeCountyValue };
            var countyValues = string.Join(", ", counties);
            return countyValues;
        }
    }
}
