using System.Web.Mvc;
using MMSINC.Exceptions;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using Subtext.TestLibrary;
using WorkOrders.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for WorkCategoryIntegrationTestTest
    /// </summary>
    [TestClass]
    public class WorkCategoryIntegrationTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private WorkCategory _target;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkCategoryIntegrationTestInitialize()
        {
            _container = new Container();
            _target = new TestWorkCategoryBuilder();
            _simulator = new HttpSimulator().SimulateRequest();
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void WorkCategoryIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestCreateNewWorkCategory()
        {
            MyAssert.DoesNotThrow(() => WorkCategoryRepository.Insert(_target));

            WorkCategoryRepository.Delete(_target);
        }

        [TestMethod]
        public void TestCannotSaveWorkCategoryWithoutDescription()
        {
            _target = new TestWorkCategoryBuilder().WithDescription(null);

            MyAssert.Throws<DomainLogicException>(
                () => WorkCategoryRepository.Insert(_target));
        }
    }

    internal class TestWorkCategoryBuilder : TestDataBuilder<WorkCategory>
    {
        #region Private Members

        private string _description = "Test Description";

        #endregion

        #region Exposed Methods

        public override WorkCategory Build()
        {
            var obj = new WorkCategory();
            if (_description != null)
                obj.Description = _description;
            return obj;
        }

        public TestWorkCategoryBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        #endregion
    }
}
