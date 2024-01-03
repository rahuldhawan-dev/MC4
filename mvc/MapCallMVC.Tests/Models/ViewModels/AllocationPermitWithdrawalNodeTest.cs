using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class AllocationPermitWithdrawalNodeTest : InMemoryDatabaseTest<AllocationPermitWithdrawalNode>
    {
        #region Fields

        private AllocationPermitWithdrawalNode _entity;
        private AllocationPermitWithdrawalNodeViewModel _target;
        private ViewModelTester<AllocationPermitWithdrawalNodeViewModel, AllocationPermitWithdrawalNode> _vmTester;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = _container.GetInstance<TestDataFactory<AllocationPermitWithdrawalNode>>().Create();
            _target = new AllocationPermitWithdrawalNodeViewModel(_container);
            _vmTester = new ViewModelTester<AllocationPermitWithdrawalNodeViewModel, AllocationPermitWithdrawalNode>(_target, _entity);
        }

        #endregion

        [TestMethod]
        public void TestMapping()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 626);
            _vmTester.DoesNotMapToEntity(x => x.Id, 262);

            _vmTester.CanMapBothWays(x => x.WellPermitNumber);
            _vmTester.CanMapBothWays(x => x.Description);
            _vmTester.CanMapBothWays(x => x.AllowableGpm);
            _vmTester.CanMapBothWays(x => x.AllowableGpd);
            _vmTester.CanMapBothWays(x => x.AllowableMgm);
            _vmTester.CanMapBothWays(x => x.CapableGpm);
            _vmTester.CanMapBothWays(x => x.WithdrawalConstraint);
            _vmTester.CanMapBothWays(x => x.HasStandByPower);
            _vmTester.CanMapBothWays(x => x.CapacityUnderStandbyPower);
        }

        [TestMethod]
        public void TestViewModelMapSetsPropertiesAndIds()
        {
            var allocationCategory = _container.GetInstance<TestDataFactory<AllocationCategory>>().Create(new { Description = "all cat"});
            var facility = GetFactory<FacilityFactory>().Create();
            var allocationPermitWithdrawalNode = _container.GetInstance<TestDataFactory<AllocationPermitWithdrawalNode>>().Create( new {
                AllocationCategory = allocationCategory,
                Facility = facility
            });

            var target = new AllocationPermitWithdrawalNodeViewModel(_container);
            target.Map(allocationPermitWithdrawalNode);

            Assert.AreEqual(allocationCategory.Id, target.AllocationCategory);
            Assert.AreEqual(facility.Id, target.Facility);
        }

        [TestMethod]
        public void TestViewModelMapToEntitySetsProperties()
        {
            var allocationCategory = _container.GetInstance<TestDataFactory<AllocationCategory>>().Create(new { Description = "all cat" });
            var facility = GetFactory<FacilityFactory>().Create();
            var target = new AllocationPermitWithdrawalNodeViewModel(_container) {
                AllocationCategory = allocationCategory.Id,
                Facility = facility.Id
            };

            var entity = new AllocationPermitWithdrawalNode();
            target.MapToEntity(entity);
            Assert.AreEqual(allocationCategory.Id, entity.AllocationCategory.Id);
            Assert.AreEqual(facility.Id, entity.Facility.Id);
        }
    }
}