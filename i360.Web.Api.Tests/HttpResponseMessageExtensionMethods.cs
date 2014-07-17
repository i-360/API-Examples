using System.Diagnostics;
using System.Net.Http;

namespace i360.Web.Api.Tests
{
    public static class HttpResponseMessageExtensionMethods
    {
        public static void PrintContent(this HttpResponseMessage response)
        {
            string content = response.Content.ReadAsStringAsync().Result;
            Debug.Print(content);
        }
    }
}
