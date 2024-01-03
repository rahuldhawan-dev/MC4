using System;
using System.Web.Mvc;
using IntegrationTests.Model;
using LINQTo271.Views.Hydrants;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;

namespace IntegrationTests.Views.Hydrants
{
    /// <summary>
    /// Summary description for HydrantsServiceViewTest
    /// </summary>
    [TestClass]
    public class HydrantsServiceViewTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void HydrantsServiceViewTestInitialize()
        {
            _container = new Container();
            var mockUser = new MockUser();
            _container.Inject<IDataContext>(new WorkOrdersDataContext());

            _simulator = new HttpSimulator();
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void HydrantsServiceViewTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void GetHydrantsByStreet()
        {
            using (_simulator.SimulateRequest())
            {
                var street = StreetIntegrationTest.ReferenceStreet;
                var knownCategoryValues = $"undefined:{10};Street:{street.StreetID};";

                var results = (new HydrantsServiceView()).GetHydrantsByStreet(knownCategoryValues, "Street");

                Assert.AreEqual(3, results.Length, "The reference street (Riverside Dr in Neptune, NJ) should have 3 active hydrants.");
            }
        }
    }
}