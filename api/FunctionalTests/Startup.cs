using DeleporterCore.Client;
using MMSINC.Testing.SpecFlow.Infrastructure;
using MMSINC.Testing.SpecFlow.Library;
using NUnit.Framework;

namespace FunctionalTests
{
    [SetUpFixture]
    public class Startup
    {
        #region Exposed Methods

        [OneTimeSetUp]
        public void RunBeforeAllTests()
        {
            if (!WebServer.IsInitialized)
            {
                WebServer.Open();
            }

            Deleporter.Run(() =>
            {
                TestHelperProxy.EnableTestModeOnGlobal();
                TestHelperProxy.InitializeTestDatabase();
            });
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            Deleporter.Run(() =>
            {
                TestHelperProxy.DisableTestModeOnGlobal();
                TestHelperProxy.DestroyTestDatabase();
            });
            if (WebServer.IsInitialized)
            {
                WebServer.Close();
            }
        }

        #endregion
    }
}
