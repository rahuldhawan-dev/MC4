using MapCall.Common.Testing.Selenium;
using NUnit.Framework;

namespace RegressionTests.Tests
{
    [SetUpFixture]
    public class MapCallSetUpFixture : SetUpFixtureBase
    {
        [OneTimeSetUp]
        public override void NamespaceSetUp()
        {
            base.NamespaceSetUp();
        }

        [OneTimeTearDown]
        public override void NamespaceTeardown()
        {
            base.NamespaceTeardown();
        }
    }
}
