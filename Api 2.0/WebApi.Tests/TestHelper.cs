using WebApi.Tests.Properties;

namespace WebApi.Tests
{
    /// <summary>
    /// Helper methods for common testing needs.
    /// </summary>
    public class TestHelper
    {
        public static string GetOrgIdAndControllerUrl(string controllerName)
        {
            return GetOrgIdAndControllerUrl(TestSettings.Default.OrgId, controllerName);
        }

        public static string GetOrgIdAndControllerUrl(int orgId, string controllerName)
        {
            return "Org/" + orgId + "/" + controllerName;
        }
    }
}