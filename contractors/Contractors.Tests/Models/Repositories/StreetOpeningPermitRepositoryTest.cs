using System.Linq;
using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class StreetOpeningPermitRepositoryTest : ContractorsControllerTestBase<StreetOpeningPermit, StreetOpeningPermitRepository>
    {
        #region Private Members

        private OperatingCenter _currentOperatingCenter;
        private WorkOrder _workOrder;

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
            _currentOperatingCenter =
                GetFactory<OperatingCenterFactory>().Create();
            _workOrder =
                GetFactory<WorkOrderFactory>().Create(
                    new {AssignedContractor = _currentUser.Contractor});
            Repository = _container.GetInstance<StreetOpeningPermitRepository>();
        }

        #endregion

        #region Linq/Criteria Filtering

        private StreetOpeningPermit[] SetupLinqCriteriaTests()
        {
            var expectedOrders = GetFactory<WorkOrderFactory>().CreateArray(2,
                new {
                    AssignedContractor = _currentUser.Contractor,
                    OperatingCenter = _currentOperatingCenter
                });
            var extraOrders = GetFactory<WorkOrderFactory>().CreateArray(2,
                new {OperatingCenter = _currentOperatingCenter});
            var expected = new[] {
                GetFactory<StreetOpeningPermitFactory>().Create(
                    new {WorkOrder = expectedOrders[0]}),
                GetFactory<StreetOpeningPermitFactory>().Create(
                    new {WorkOrder = expectedOrders[1]})
            };
            var extra = new[] {
                GetFactory<StreetOpeningPermitFactory>().Create(
                    new {WorkOrder = extraOrders[0]}),
                GetFactory<StreetOpeningPermitFactory>().Create(
                    new {WorkOrder = extraOrders[1]})
            };
            return expected;
        }

        [TestMethod]
        public void
            TestLinqOnlyAllowsAccessToTheStreetOpeningPermitsBelongingToWorkOrdersAssignedToTheContractorThatTheCurrentUserBelongsTo
            ()
        {
            var expected = SetupLinqCriteriaTests();
            // .GetAll uses the Linq property (for now at least)
            var actual = Repository.GetAll().ToArray();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id,
                    actual[i].Id);
            }
        }

        [TestMethod]
        public void
            TestCriteriaOnlyAllowsAccessToTheStreetOpeningPermitsBelongingToWorkOrdersAssignedToTheContractorThatTheCurrentUserBelongsTo
            ()
        {
            var expected = SetupLinqCriteriaTests();
            // .Search uses the Criteria property (for now at least)
            var actual =
                Repository.Search(Restrictions.Conjunction()).List
                    <StreetOpeningPermit>();

            Assert.AreEqual(expected.Count(), actual.Count());
            for (var i = 0; i < actual.Count(); ++i)
            {
                Assert.AreEqual(expected[i].Id,
                    actual[i].Id);
            }
        }

        #endregion
    }
}
