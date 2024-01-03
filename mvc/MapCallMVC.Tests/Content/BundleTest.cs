using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using MMSINC.Testing;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Content
{
    [TestClass]
    public class BundleTest
    {
        #region Fields

        private MvcApplicationTester<MvcApplication> _app;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            var container = MapCallMvcApplicationTester.InitializeDummyObjectFactory();
            _app = container.With(true).GetInstance<MvcApplicationTester<MvcApplication>>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _app.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestAllBundlesCompile()
        {
            var bundleTester = new BundleTester<MvcApplication>(_app);
            bundleTester.StaticFileAbsoluteRoot = @"C:\Solutions\mapcall-monorepo\mvc\MapCallMVC\";
            bundleTester.BundlesAreValidForApplication();
        }
    }
}
