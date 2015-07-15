using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    [TestClass]
    public class OrderedSearchesTests
    {
        private const int ValidSearchId = 123;
        private const string ControllerName = "OrderedSearches";
        private static readonly string RequestUri = string.Format("{0}/{1}", ControllerName,
            ValidSearchId);
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
        public async Task GivenMethodAndUrl_Get_ReturnsSuccess()
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
        public async Task GivenSearchRequest_Post_ReturnsSuccess()
        {
            // Arrange
            var criteria = CreateCriteria();
            var request = new
            {
                ApiNotification = true,
                Columns = new List<string> // See SearchColumns tests for these values
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
                OrderBy = new List<string> { "firstname(c)", "lastname(c)", "gender(c)", "Phone(c)" }, // See SearchColumns tests for these values
                Criteria = criteria,
                ExportType = 0,
                Randomized = true,
                RecordLimit = 100
            };

            // Act
            var response = await _client.PostAsJsonAsync(ControllerName, request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenUnsecureAddress_Post_ReturnForbidden()
        {
            // Arrange
            var unsecuredUrl = Settings.Default.UnsecureUrl;
            _client.BaseAddress = new Uri(unsecuredUrl);

            // Act
            await TestHelper.VerifyForbiddenErrorReturned(_client, HttpMethod.Post, ControllerName);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenUnsecureAddress_Get_RedirectedToSecureAddress()
        {
            // Arrange
            var unsecuredUrl = Settings.Default.UnsecureUrl;
            _client.BaseAddress = new Uri(unsecuredUrl);

            // Act
            await TestHelper.VerifyRedirectedToSecureAddress(_client, HttpMethod.Get, RequestUri);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenNoAuthorization_Get_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            await TestHelper.VerifyUnauthorizedErrorReturned(_client, HttpMethod.Get, RequestUri);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenNoAuthorization_Post_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            await TestHelper.VerifyUnauthorizedErrorReturned(_client, HttpMethod.Post, ControllerName);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenAuthorizationWithoutRole_Get_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValueFactory.CreateForApiTesterWithoutApiRole();

            // Act
            await TestHelper.VerifyUnauthorizedErrorReturned(_client, HttpMethod.Get, RequestUri);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenAuthorizationWithoutRole_Post_ReturnsUnathorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValueFactory.CreateForApiTesterWithoutApiRole();

            // Act
            await TestHelper.VerifyUnauthorizedErrorReturned(_client, HttpMethod.Post, RequestUri);
        }

        private static Dictionary<int, string> CreateCriteria()
        {
            // See Search Parameters tests for these values
            const int stateParameter = 2;
            const int searchTypeParameter = 3;
            const int ncoa = 13;
            const int registrationStatus = 28;
            const int countyParameter = 5;
            const int radiusLatLongParameter = 27;
            const int hasEmailParameter = 377;
            const int ageRangeParameter = 15;

            // See Search Parameter Values tests for these values
            const int virginiaStateValue = 47;
            const int i360DataValue = 1;
            var countyValues = CreateCountyValues();
            const string radiusLatLongValue =
                "poly\n" +
                "38.87424355282139#-77.30616654214167\n" +
                "38.85419431555039#-77.33878220376276\n" +
                "38.82210377736585#-77.32024277505182\n" +
                "38.829057955280234#-77.27561081704401\n" +
                "38.8590066481109#-77.26634110268854\n" +
                "38.87424355282139#-77.30616654214167\n";
            const string ageRange = "18#30";

            var criteria = new Dictionary<int, string>
            {
                {stateParameter, virginiaStateValue.ToString(CultureInfo.InvariantCulture)},
                {searchTypeParameter, i360DataValue.ToString(CultureInfo.InvariantCulture)},
                {countyParameter, countyValues},
                {radiusLatLongParameter, radiusLatLongValue},
                {hasEmailParameter, "1"},
                {ageRangeParameter, ageRange},
                {ncoa, "-1"},
                {registrationStatus, "-1"}
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
