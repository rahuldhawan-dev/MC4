using System.Web.Mvc;
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
    /// Summary description for RestorationIntegrationTestTest
    /// </summary>
    [TestClass]
    public class RestorationIntegrationTest
    {
        #region Constants

        private const int RESTORATION_TYPE_ID = 1,
                          INITIAL_RESTORATION_METHOD_ID = 2,
                          FINAL_RESTORATION_METHOD_ID = 3;

        #endregion

        #region Private Members

        private HttpSimulator _simulator;
        private RestorationType _type;
        private WorkOrder _order;
        private Restoration _target;
        private IContainer _container;

        #endregion

        #region Private Static Methods

        private static void InsertRestoration(Restoration entity)
        {
            RestorationRepository.Insert(entity);
        }

        private static void DeleteRestoration(Restoration entity)
        {
            var order = entity.WorkOrder;
            RestorationRepository.Delete(entity);
            WorkOrderRepository.Delete(order);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationIntegrationTestInitialize()
        {
            _container = new Container();
            _simulator = new HttpSimulator().SimulateRequest();
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            _order = new TestWorkOrderBuilder();
            _type = RestorationTypeRepository.GetEntity(RESTORATION_TYPE_ID);
            _target = new TestRestorationBuilder()
                .WithWorkOrder(_order)
                .WithRestorationType(_type);
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void RestorationIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestDeleteRestoration()
        {
            InsertRestoration(_target);

            MyAssert.DoesNotThrow(() => DeleteRestoration(_target));
        }

        //[TestMethod]
        //public void TestSaveWithPartialRestorationMethod()
        //{
        //    var expected = RestorationMethodRepository.GetEntity(
        //        INITIAL_RESTORATION_METHOD_ID);
        //    _target.PartialRestorationMethod = expected;

        //    MyAssert.DoesNotThrow(() => InsertRestoration(_target));
        //    Assert.AreSame(expected, _target.PartialRestorationMethod);

        //    DeleteRestoration(_target);
        //}

        //[TestMethod]
        //public void TestSaveWithFinalRestorationMethod()
        //{
        //    var expected = RestorationMethodRepository.GetEntity(
        //        FINAL_RESTORATION_METHOD_ID);
        //    _target.FinalRestorationMethod = expected;

        //    MyAssert.DoesNotThrow(() => InsertRestoration(_target));
        //    Assert.AreSame(expected, _target.FinalRestorationMethod);

        //    DeleteRestoration(_target);
        //}

        [TestMethod]
        public void TestSaveWithApprovedBy()
        {
            Assert.Inconclusive("Test not yet written.");
        }

        [TestMethod]
        public void TestSaveWithRejectedBy()
        {
            Assert.Inconclusive("Test not yet written.");
        }
    }

    internal class TestRestorationBuilder : TestDataBuilder<Restoration>
    {
        #region Private Members

        private RestorationMethod _partialRestorationMethod,
                                  _finalRestorationMethod;
        private RestorationType _restorationType;
        private WorkOrder _workOrder;

        #endregion

        #region Exposed Methods

        public override Restoration Build()
        {
            var obj = new Restoration();
            if (_workOrder != null)
                obj.WorkOrder = _workOrder;
            if (_restorationType != null)
                obj.RestorationType = _restorationType;
            //if (_partialRestorationMethod != null)
            //    obj.PartialRestorationMethod = _partialRestorationMethod;
            //if (_finalRestorationMethod != null)
            //    obj.FinalRestorationMethod = _finalRestorationMethod;
            return obj;
        }

        public TestRestorationBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }

        public TestRestorationBuilder WithRestorationType(RestorationType restorationType)
        {
            _restorationType = restorationType;
            return this;
        }

        public TestRestorationBuilder WithPartialRestorationMethod(RestorationMethod partialRestorationMethod)
        {
            _partialRestorationMethod = partialRestorationMethod;
            return this;
        }

        public TestRestorationBuilder WithFinalRestorationMethod(RestorationMethod finalRestorationMethod)
        {
            _finalRestorationMethod = finalRestorationMethod;
            return this;
        }

        #endregion
    }
}