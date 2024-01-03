using System;
using System.Web.Mvc;
using LINQTo271.Views.Streets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Interface;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;

namespace IntegrationTests.Views.Streets
{
    /// <summary>
    /// Summary description for StreetsServiceViewTest
    /// </summary>
    [TestClass]
    public class StreetsServiceViewTest
    {
        #region Constants

        public const int EXPECTED_STREETS = 511;

        #endregion

        #region Private Members

        private HttpSimulator _simulator;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void StreetsServiceViewTestInitialize()
        {
            _container = new Container();
            _simulator = new HttpSimulator();
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
;
        }

        [TestCleanup]
        public void StreetsServiceViewTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestGetStreetsByTown()
        {
            using (_simulator.SimulateRequest())
            {
                var town = TownTest.GetValidTown();
                var knownCategoryValues = String.Format("undefined:{0};",
                    town.TownID);

                var results =
                    (new StreetsServiceView()).GetStreetsByTown(
                        knownCategoryValues, "Town");

                Assert.IsTrue(results.Length >= EXPECTED_STREETS,
                    String.Format("The reference town (Neptune) should have {0} streets.", EXPECTED_STREETS));
            }
        }
    }
}