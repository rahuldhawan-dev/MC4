using System.Configuration;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Integration
{
    [TestClass]
    public class NotificationConfigurationTest : NotificationConfigurationTestBase
    {
        protected override string ConnectionString
            => ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString;


        [TestMethod]
        public override void TestEmbeddedResourcesMatchDatabaseConfiguration()
        {
            base.TestEmbeddedResourcesMatchDatabaseConfiguration();
        }
    }
}