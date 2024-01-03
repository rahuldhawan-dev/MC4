using System.Linq;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Criterion;
using StructureMap;
using MainBreakRepository = Contractors.Data.Models.Repositories.MainBreakRepository;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class MainBreakRepositoryTest : ContractorsControllerTestBase<MainBreak, MainBreakRepository>
    {
        #region Private Members

        private OperatingCenter _currentOperatingCenter;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _currentOperatingCenter = GetFactory<OperatingCenterFactory>().Create();
            Repository = _container.GetInstance<MainBreakRepository>();
        }

        #endregion

        #region Tests

        #region Linq/Criteria

        [TestMethod]
        public void TestLinqOnlyAllowsAccessToTheMainBreaksBelongingToWorkOrdersAssignedToTheContractorThatTheCurrentUserBelongsTo()
        {
            var expectedOrders = GetFactory<WorkOrderFactory>().CreateArray(2, new {
                AssignedContractor = _currentUser.Contractor, OperatingCenter = _currentOperatingCenter});
            var extraOrders = GetFactory<WorkOrderFactory>().CreateArray(2,
                new { OperatingCenter = _currentOperatingCenter });
            var expected = new[] {
                GetFactory<MainBreakFactory>().Create(new {WorkOrder = expectedOrders[0]}),
                GetFactory<MainBreakFactory>().Create(new {WorkOrder = expectedOrders[1]})
            };
            var extra = new[] {
                GetFactory<MainBreakFactory>().Create(new {WorkOrder = extraOrders[0]}),
                GetFactory<MainBreakFactory>().Create(new {WorkOrder = extraOrders[1]})
            };

            var actual = Repository.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for(var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }

        [TestMethod]
        public void TestCriteriaOnlyAllowsAccessToTheMainBreaksBelongingToWorkOrdersAssignedToTheContractorThatTheCurrentUserBelongsTo()
        {
            var expectedOrders = GetFactory<WorkOrderFactory>().CreateArray(2, new {
                AssignedContractor = _currentUser.Contractor, OperatingCenter = _currentOperatingCenter});
            var extraOrders = GetFactory<WorkOrderFactory>().CreateArray(2,
                new { OperatingCenter = _currentOperatingCenter });
            var expected = new[] {
                GetFactory<MainBreakFactory>().Create(new {WorkOrder = expectedOrders[0]}),
                GetFactory<MainBreakFactory>().Create(new {WorkOrder = expectedOrders[1]})
            };
            var extra = new[] {
                GetFactory<MainBreakFactory>().Create(new {WorkOrder = extraOrders[0]}),
                GetFactory<MainBreakFactory>().Create(new {WorkOrder = extraOrders[1]})
            };

            var actual = Repository.Search(Restrictions.Conjunction()).List<MainBreak>().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id, actual[i].Id);
            }
        }
        
        #endregion

        #endregion
    }
}
