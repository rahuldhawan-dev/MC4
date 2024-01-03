using System.IO;
using Contractors.Configuration;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using NHibernate;
using StructureMap;

namespace Contractors.Tests.Content
{
    [TestClass]
    public class BundleTest
    {
        #region Fields

        private MvcApplicationTester<MvcApplication> _app;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container(i => {
                i.For<ISession>().Mock();
                i.For<IAuthenticationService<ContractorUser>>().Mock();
                i.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
                ContractorsDependencies.RegisterRepositories(i);
            });
            _app = _container.GetInstance<MvcApplicationTester<MvcApplication>>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _app.Dispose();
        }

        #endregion

        private static string GetStaticFileAbsoluteRoot()
        {
            // NOTE: Something about running the tests causes Directory.GetCurrentDirectory to change.
            // If you run this test by itself, GetCurrentDirectory returns Contractor.Tests\bin\x64\debug
            // If you run all of the tests, GetCurrentDirectory returns one of the TestResults dirs instead.
            var currentDirectory = Directory.GetCurrentDirectory();

            if (currentDirectory.Contains("TeamCity") || currentDirectory.Contains("everything"))
            {
                return Path.GetFullPath(@"..\..\..\..\contractors\Contractors\");
            }

            if (currentDirectory.Contains("TestResults")) // The TeamCity path will also contain TestResults.
            {
                return
                    Path.GetFullPath(@"..\..\..\Contractors\"); // Needs to go up from TestResults\Deploy_342_blah\Out
            }

            return Path.GetFullPath(@"..\..\..\..\Contractors\"); // Needs to go up from Contractor.Tests\bin\x64\debug
        }

        [TestMethod]
        public void TestAllBundlesCompile()
        {
            var bundleTester = new BundleTester<MvcApplication>(_app) {
                StaticFileAbsoluteRoot = GetStaticFileAbsoluteRoot()
            };

            bundleTester.BundlesAreValidForApplication();
        }
    }
}
