using System.Diagnostics;
using System.Net.Http;

namespace WebApi.Tests
{
    public static class HttpResponseMessageExtensionMethods
    {
        public static void PrintContent(this HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().Result;
            Debug.Print(content);
        }
    }
}
