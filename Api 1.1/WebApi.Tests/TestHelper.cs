using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

namespace WebApi.Tests
{
    /// <summary>
    /// Helper methods for common testing needs.
    /// </summary>
    public class TestHelper
    {
        public static async Task VerifyRedirectedToSecureAddress(HttpClient client, HttpMethod method, string requestUri)
        {
            var response = await client.SendAsync(new HttpRequestMessage(method, requestUri));

            response.RequestMessage.RequestUri.Scheme.Should().Be("https");
        }

        public static async Task VerifyUnauthorizedErrorReturned(HttpClient client, HttpMethod method, string requestUri)
        {
            var response = await client.SendAsync(new HttpRequestMessage(method, requestUri));

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, "{0} command at {1} should be unauthorized.", method.ToString(), requestUri);
            response.Headers.WwwAuthenticate.Should().ContainSingle(x => x.Scheme == "Basic");
        }

        public static async Task VerifyForbiddenErrorReturned(HttpClient client, HttpMethod method, string requestUri)
        {
            var response = await client.SendAsync(new HttpRequestMessage(method, requestUri));

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden, "{0} command at {1} should be forbidden.", method.ToString(), requestUri);
        }

        public static async Task VerifySuccess(HttpClient client, HttpMethod method, string requestUri)
        {
            var response = await client.SendAsync(new HttpRequestMessage(method, requestUri));

            response.EnsureSuccessStatusCode();
        }
    }
}
