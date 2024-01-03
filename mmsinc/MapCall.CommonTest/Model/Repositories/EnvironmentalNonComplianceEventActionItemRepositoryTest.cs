using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;
using System;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class EnvironmentalNonComplianceEventActionItemRepositoryTest : MapCallMvcSecuredRepositoryTestBase<EnvironmentalNonComplianceEventActionItem,
        EnvironmentalNonComplianceEventActionItemRepository, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Fields

        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
        }

        #endregion

        #region Tests

        #region RepositoryExtensions

        [TestMethod]
        public void TestGetAllActionItemsEvery30DaysFromEstimatedCompletionReturnsRecordsWith30DayInterval()
        {
            //arrange
            var atInterval = _dateTimeProvider.GetCurrentDate().AddDays(10);
            var responsibleOwner = GetFactory<UserFactory>().Create();
            var modelEvent = GetFactory<EnvironmentalNonComplianceEventFactory>().Create();
            for (var i = 0; i < 10; i++)
            {
                var actionItem = GetFactory<EnvironmentalNonComplianceEventActionItemFactory>().Create(new {
                    EnvironmentalNonComplianceEvent = modelEvent,
                    ResponsibleOwner = responsibleOwner,
                    TargetedCompletionDate = atInterval
                });
                Session.Refresh(actionItem);
                Session.Save(actionItem);
                atInterval = atInterval.AddDays(10);
            }
            Session.Flush();
            var results = Repository.GetAllActionItemsEvery30DaysFromEstimatedCompletion(_dateTimeProvider);
            Assert.AreEqual(3, results.Count());
        }

        [TestMethod]
        public void TestGetAllActionItemsDoesNotReturnTodayIfNotEstimatedCompletionDate()
        {
            //arrange
            var atInterval = _dateTimeProvider.GetCurrentDate();
            var responsibleOwner = GetFactory<UserFactory>().Create();
            var modelEvent = GetFactory<EnvironmentalNonComplianceEventFactory>().Create();
            for (var i = 1; i < 2; i++)
            {
                var actionItem = GetFactory<EnvironmentalNonComplianceEventActionItemFactory>().Create(new {
                    EnvironmentalNonComplianceEvent = modelEvent,
                    ResponsibleOwner = responsibleOwner,
                    TargetedCompletionDate = atInterval
                });
                Session.Save(actionItem);
                atInterval = atInterval.AddDays(1);
            }
            Session.Flush();
            var results = Repository.GetAllActionItemsEvery30DaysFromEstimatedCompletion(_dateTimeProvider);
            Assert.AreEqual(1, results.Count());
        }

        #endregion

        #endregion
    }
}