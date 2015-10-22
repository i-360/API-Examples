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
            const int touchPointParameter1 = 264;
            const int touchPointParameter2 = 266;
            const int touchPointParameter3 = 271;
            const int touchPointParameter4 = 273;
            const int touchPointParameter5 = 290;
            const int touchPointParameter6 = 291;
            const int touchPointParameter7 = 292;
            const int touchPointParameter8 = 293;

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

            // CategoryID#TouchpointID&DateRangeFrom#DateRangeTo, these values must be actual category and touch point IDs that exist in the database
            const string touchpointValue1 = "200#204&9/27/2013#10/22/2015";
            const string touchpointValue2 = "200#205&9/27/2013#10/22/2015";
            const string touchpointValue3 = "200#206&9/27/2013#10/22/2015";
            const string touchpointValue4 = "200#207&9/27/2013#10/22/2015";
            const string touchpointValue5 = "200#208&9/27/2013#10/22/2015";
            const string touchpointValue6 = "200#209&9/27/2013#10/22/2015";
            const string touchpointValue7 = "200#210&9/27/2013#10/22/2015";
            const string touchpointValue8 = "200#211&9/27/2013#10/22/2015";

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
                {touchPointParameter1, touchpointValue1},
                {touchPointParameter2, touchpointValue2},
                {touchPointParameter3, touchpointValue3},
                {touchPointParameter4, touchpointValue4},
                {touchPointParameter5, touchpointValue5},
                {touchPointParameter6, touchpointValue6},
                {touchPointParameter7, touchpointValue7},
                {touchPointParameter8, touchpointValue8}
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