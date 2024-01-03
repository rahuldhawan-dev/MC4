using System.Collections.Generic;
using System.Configuration;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Configuration
{
    [TestClass]
    public class NotificationConfigurationTest : NotificationConfigurationTestBase
    {
        protected override string ConnectionString => ConfigurationManager.ConnectionStrings["MapCall"].ConnectionString;

        protected override Dictionary<string, string> EmbeddedExceptions => new Dictionary<string, string> { {"Employee", "MedicalCertificateExpiration"} };

        [TestMethod]
        public override void TestEmbeddedResourcesMatchDatabaseConfiguration()
        {
            base.TestEmbeddedResourcesMatchDatabaseConfiguration();
        }
    }
}
