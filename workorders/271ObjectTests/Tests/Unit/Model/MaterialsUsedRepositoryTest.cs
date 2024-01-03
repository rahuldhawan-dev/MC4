using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    [TestClass]
    public class MaterialsUsedRepositoryTest : EventFiringTestClass
    {
        private TestMaterialsUsedRepository _target;
        private IWorkOrdersWorkOrderRepository _workOrderRepository;
        private ITable<MaterialsUsed> _dataTable;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            EventFiringTestClassInitialize();
            _container = new Container();

            _mocks
               .DynamicMock(out _workOrderRepository)
               .DynamicMock(out _dataTable);

            _container.Inject(_workOrderRepository);

            _target = new TestMaterialsUsedRepositoryBuilder()
               .WithDataTable(_dataTable);

            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            EventFiringTestClassCleanup();
        }

        [TestMethod]
        public void TestInsertMaterialUsedThrowsExceptionWhenWorkOrderMaterialsAlreadyApproved()
        {
            var workOrderId = 666;

            using (_mocks.Record())
            {
                SetupResult.For(_workOrderRepository.Get(workOrderId)).Return(
                    new WorkOrder {
                        MaterialsApprovedOn = DateTime.Now,
                        MaterialsApprovedBy = new Employee()
                    });
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws<DomainLogicException>(() =>
                    MaterialsUsedRepository.InsertMaterialUsed(workOrderId, null, null, 1, null));
            }
        }
    }

    internal class TestMaterialsUsedRepositoryBuilder : TestDataBuilder<TestMaterialsUsedRepository>
    {
        private ITable<MaterialsUsed> _dataTable;

        public override TestMaterialsUsedRepository Build()
        {
            var obj= new TestMaterialsUsedRepository();
            if (_dataTable != null)
                obj.SetDataTable(_dataTable);
            return obj;
        }

        public TestMaterialsUsedRepositoryBuilder WithDataTable(ITable<MaterialsUsed> dataTable)
        {
            _dataTable = dataTable;
            return this;
        }
    }

    internal class TestMaterialsUsedRepository : MaterialsUsedRepository
    {
        public void SetDataTable(ITable<MaterialsUsed> dataTable)
        {
            _dataTable = dataTable;
        }
    }
}