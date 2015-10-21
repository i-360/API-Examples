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
    public class SearchesControllerSmokeTests
    {
        private HttpClient _client;
        private const string ControllerName = "Searches";
        private static readonly string BaseUri = TestHelper.GetOrgIdAndControllerUrl(ControllerName);

        [TestInitialize]
        public void InitializeTests()
        {
            _client = new HttpClient { BaseAddress = new Uri(TestSettings.Default.TestUrl) };
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValueFactory.CreateForApiTester().Result;
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
                Columns = CreateColumnsList(),
                OrderBy = CreateOrderByList(),
                Criteria = CreateCriteria(),
                ExportType = 0,
                Randomized = true,
                RecordLimit = 100
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUri, request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenSearchCountRequest_Post_ReturnsSuccess()
        {
            // Arrange
            var request = new
            {
                Criteria = CreateCriteria(),
                CellPhones = true,
                Email = true,
                Households = true,
                MailingAddresses = true,
                Residences = true,
                Phones = true
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUri + "/Counts", request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GivenSearchResultsRequest_Post_ReturnsSuccess()
        {
            // Arrange
            var criteria = CreateCriteria();
            var request = new
            {
                Columns = CreateColumnsList(),
                OrderBy = CreateOrderByList(),
                Criteria = criteria,
                ExportType = 0,
                Randomized = true,
                RecordLimit = 100
            };

            // Act
            var response = await _client.PostAsJsonAsync(BaseUri + "/Results", request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        
        private static List<string> CreateOrderByList()
        {
            // See SearchColumns tests for these values
            return new List<string> { "firstname(c)", "lastname(c)", "gender(c)", "Phone(c)" };
        }

        private static List<string> CreateColumnsList()
        {
            // See SearchColumns tests for these values
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

        private static Dictionary<int, string> CreateCriteria()
        {
            // See Search Parameters tests for these values
            // Required parameters
            const int stateParameter = 2;
            const int searchTypeParameter = 3;
            const int ncoa = 13;
            const int registrationStatus = 28;

            // Optional parameters
            const int countyParameter = 5;
            const int radiusLatLongParameter = 27;
            const int hasEmailParameter = 377;
            const int ageRangeParameter = 15;
            const int touchPointParameter = 264;
            const int touchpointDateRangeParameter = 311;

            // See Search Parameter Values tests for these values
            const int virginiaStateValue = 47;
            const int myDataValue = 2;
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
            const string touchpointValue1 = "200#207"; // CategoryID#TouchpointID, these values must be actual category and touch point IDs that exist in the database
            const string touchpointDateRangeValue = "01/01/2015#10/21/2015";

            var criteria = new Dictionary<int, string>
            {
                {stateParameter, virginiaStateValue.ToString(CultureInfo.InvariantCulture)},
                {searchTypeParameter, myDataValue.ToString(CultureInfo.InvariantCulture)},
                {ncoa, "-1"},
                {registrationStatus, "-1"},
                {countyParameter, countyValues},
                {radiusLatLongParameter, radiusLatLongValue},
                {hasEmailParameter, "1"},
                {ageRangeParameter, ageRange},
                {touchPointParameter, touchpointValue1},
                {touchpointDateRangeParameter, touchpointDateRangeValue}
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