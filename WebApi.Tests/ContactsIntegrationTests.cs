using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    [TestClass]
    public class ContactsIntegrationTests
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
        public void GivenContactId_Get_ReturnsContactData()
        {
            // Arrange
            const long contactId = 1;
            var requestUri = string.Format("Contacts/{0}", contactId);

            // Act
            var response = _client.GetAsync(requestUri).Result;
            
            // Assert
            response.EnsureSuccessStatusCode();
            response.PrintContent();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GivenSearchCriteria_Get_ReturnsContactData()
        {
            // Arrange
            const string firstName = "JOHN";
            const string lastName = "DOE";
            const string address1 = "100+MAIN+ST";
            const string city = "SMITHFIELD";
            const string state = "VA";
            var requestUri =
                string.Format("Contacts?firstname={0}&lastname={1}&address1={2}&city={3}&state={4}", firstName, lastName,
                    address1, city, state);

            // Act
            var response = _client.GetAsync(requestUri).Result;

            // Assert
            response.EnsureSuccessStatusCode();
            response.PrintContent();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GivenContacts_Put_ReturnsImportStatus()
        {
            // Arrange
            var contacts = new[]
            {
                new 
                {
                    Tags =
                        new []
                        {
                            new {Key="Events", Value="Richmond Rally 20130618"},
                            new {Key="Volunteers", Value="Poll driver"}
                        },
                    FirstName = "JOHN",
                    MiddleName = "QUINCY",
                    LastName = "PUBLIC"
                },
                new 
                {
                    Tags =
                        new []
                        {
                            new {Key="Events", Value="Richmond Rally 20130618"},
                            new {Key="Volunteers", Value="Poll driver"}
                        },
                    FirstName = "MARY",
                    MiddleName = "J",
                    LastName = "SMITH"
                }
            };
            var request = new {Contacts = contacts, ApiNotification = true};

            // Act
            var response = _client.PutAsJsonAsync("Contacts", request).Result;

            // Assert
            response.EnsureSuccessStatusCode();
            response.PrintContent();
        }
    }
}
