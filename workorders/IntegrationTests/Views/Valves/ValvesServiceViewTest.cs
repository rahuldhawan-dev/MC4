using System;
using System.Web.Mvc;
using IntegrationTests.Model;
using LINQTo271.Views.Valves;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;

namespace IntegrationTests.Views.Valves
{
    /// <summary>
    /// Summary description for ValvesServiceViewTest
    /// </summary>
    [TestClass]
    public class ValvesServiceViewTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void ValvesServiceViewTestInitialize()
        {
            _container = new Container();
            _simulator = new HttpSimulator();
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void ValvesServiceViewTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void GetValvesByStreet()
        {
            var count = 46;
            using (_simulator.SimulateRequest())
            {
                var street = StreetIntegrationTest.ReferenceStreet;
                var knownCategoryValues = String.Format("undefined:{0};Street:{1};",
                    10, street.StreetID);

                var results =
                    (new ValvesServiceView()).GetValvesByStreet(
                        knownCategoryValues, "Street");

                MyAssert.IsGreaterThanOrEqualTo(count, results.Length,
                    $"The reference street (Brighton Ave in Neptune, NJ) should have {count} valves.");
            }
        }
    }
}