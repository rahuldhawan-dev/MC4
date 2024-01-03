using System.Reflection;
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
    /// Summary description for MaterialsUsedTestTest
    /// </summary>
    [TestClass]
    public class MaterialsUsedTest
    {
        #region Private Members

        private HttpSimulator _simulator;
        private MaterialsUsed _target;
        private IContainer _container;

        #endregion

        #region Exposed Static Methods

        public static void DeleteMaterialsUsed(MaterialsUsed entity)
        {
            var order = entity.WorkOrder;
            MaterialsUsedRepository.Delete(entity);
            WorkOrderIntegrationTest.DeleteWorkOrder(order);
        }

        #endregion

        #region Private Methods

        protected void DeleteObject(MaterialsUsed entity)
        {
            DeleteMaterialsUsed(entity);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void MaterialsUsedTestInitialize()
        {
            _container = new Container();
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            _simulator = new HttpSimulator();
            _simulator.SimulateRequest();

            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));

            _target = new TestMaterialsUsedBuilder();
        }

        [TestCleanup]
        public void MaterialsUsedTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestCannotChangeWorkOrderAfterSave()
        {
            MaterialsUsedRepository.Insert(_target);
            var order = new WorkOrder();

            MyAssert.Throws(() => _target.WorkOrder = order,
                typeof(DomainLogicException),
                "Attempting to change the WorkOrder of a given MaterialsUsed object after it has been set should throw an exception");

            DeleteObject(_target);
        }
    }

    internal class TestMaterialsUsedBuilder : TestDataBuilder<MaterialsUsed>
    {
        #region Constants

        private const short QUANTITY = 1;

        #endregion

        #region Private Members

        private short? _quantity = QUANTITY;
        private WorkOrder _workOrder = WorkOrderIntegrationTest.GetValidWorkOrder();

        #endregion

        #region Private Methods

        private void SetQuantity(MaterialsUsed materialsUsed)
        {
            var fi = materialsUsed.GetType().GetField("_quantity",
                BindingFlags.Instance | BindingFlags.NonPublic);
            fi.SetValue(materialsUsed, _quantity);
        }

        #endregion

        #region Exposed Methods

        public override MaterialsUsed Build()
        {
            var mu = new MaterialsUsed();
            if (_quantity != null)
                SetQuantity(mu);
            if (_workOrder != null)
                mu.WorkOrder = _workOrder;
            return mu;
        }

        public TestMaterialsUsedBuilder WithQuantity(short? quantity)
        {
            _quantity = quantity;
            return this;
        }

        public TestMaterialsUsedBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }

        #endregion
    }
}