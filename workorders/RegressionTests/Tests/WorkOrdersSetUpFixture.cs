using System;
using MapCall.Common.Testing.Selenium;
using NUnit.Framework;
using Config = MMSINC.Testing.Utilities.RegressionTestConfiguration;

namespace RegressionTests.Tests
{
    [SetUpFixture]
    public class WorkOrdersSetUpFixture : SetUpFixtureBase
    {

        // NOTE: I'm not sure what this actually did. Removing it allows the 271 regressions to work locally again,
        //       and hopefully still works on TeamCity. 
        //protected override Uri GetFirstPageUri()
        //{
        //    return new Uri(Config.GetDevSiteUrl() + "/modules/WorkOrders/Views/WorkOrders/Input/WorkOrderInputResourceView.aspx");
        //}

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
