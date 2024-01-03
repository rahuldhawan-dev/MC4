using System;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;

namespace IntegrationTests.Model
{
    [TestClass]
    public class ReportViewingIntegrationTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private IContainer _container;
        
        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void ReportViewingTestInitialize()
        {
            _container = new Container();
            _simulator = new HttpSimulator();
            _simulator = _simulator.SimulateRequest();
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void ReportViewingTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestGetReportViewingsByOpCenterAndDateRange()
        {
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            var employee = EmployeeRepository.SelectEmployeeByUserName("mcadmin");
            var dateViewed = DateTime.Now;
            var reportName = "FooReport";
            var opCode = "NJ7";

            var viewing = new ReportViewing {
                DateViewed = dateViewed,
                Employee = employee,
                ReportName = reportName
            };
            ReportViewingRepository.Insert(viewing);

            try
            {
                var retrieved =
                    ReportViewingRepository.
                        GetReportViewingsByOpCenterAndDateRange(
                            dateViewed.AddDays(-1), dateViewed.AddDays(1),
                            opCode, employee.EmployeeID).First();

                MyAssert.IsNotNullButInstanceOfType(retrieved, typeof(ReportViewing));
            }
            finally
            {
                ReportViewingRepository.Delete(viewing);
            }
        }
    }
}
