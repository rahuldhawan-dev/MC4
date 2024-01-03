using log4net;
using MapCall.Common.Configuration.NotificationTasks.Environmental;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Linq;

namespace MapCall.CommonTest.Configuration.NotificationTasks.Environmental
{
    [TestClass]
    public class EnvironmentalNonComplianceActionItemAssignedTest : InMemoryDatabaseTest<EnvironmentalNonComplianceEventActionItem>
    {
        #region Private Members

        private TestDateTimeProvider _dateTimeProvider;
        private Mock<INotifier> _notifier;
        private Mock<INotificationService> _notificationService;
        private Mock<ILog> _log;
        private DateTime _now;
        private RepositoryBase<EnvironmentalNonComplianceEventActionItem> _repository;
        private EnvironmentalNonComplianceActionItemAssigned _target;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotifier>();
            _notificationService = new Mock<INotificationService>();
            _log = new Mock<ILog>();
            _repository = _container.GetInstance<RepositoryBase<EnvironmentalNonComplianceEventActionItem>>();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
        }

        [TestMethod]
        public void TestGetDataReturnsExpectedResults()
        {
            var intervalDate = _dateTimeProvider.GetCurrentDate().AddDays(30);
            var responsibleOwner = GetFactory<UserFactory>().Create();
            var modelEvent = GetFactory<EnvironmentalNonComplianceEventFactory>().Create();
            var actionItem = GetFactory<EnvironmentalNonComplianceEventActionItemFactory>().Create(new {
                EnvironmentalNonComplianceEvent = modelEvent,
                ResponsibleOwner = responsibleOwner,
                TargetedCompletionDate = intervalDate
            });

            var target = new EnvironmentalNonComplianceActionItemAssigned(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider);

            Assert.AreSame(actionItem, target.GetData().Single());
        }

        [TestMethod]
        public void TestSendNotificationSendsNotificationToResponsibleOwnerEmail()
        {
            var atInterval = _dateTimeProvider.GetCurrentDate().AddDays(30);
            var responsibleOwner = GetFactory<UserFactory>().Create();
            var modelEvent = GetFactory<EnvironmentalNonComplianceEventFactory>().Create();
            var actionItem = GetFactory<EnvironmentalNonComplianceEventActionItemFactory>().Create(new {
                EnvironmentalNonComplianceEvent = modelEvent,
                ResponsibleOwner = responsibleOwner,
                TargetedCompletionDate = atInterval
            });

            var target = new EnvironmentalNonComplianceActionItemAssigned(_repository, _notifier.Object, _notificationService.Object, _log.Object, _dateTimeProvider);

            // Act
            target.SendNotification(actionItem);

            // Assert
            _notifier.Verify(x => x.Notify(
                EnvironmentalNonComplianceActionItemAssigned.APPLICATION,
                EnvironmentalNonComplianceActionItemAssigned.MODULE,
                EnvironmentalNonComplianceActionItemAssigned.PURPOSE,
                It.Is<EnvironmentalNonComplianceActionItemAssignedNotification>(notification =>
                    notification.EnvironmentalNonComplianceEventActionItem.Id == actionItem.Id &&
                    notification.AssignedToFullName == actionItem.ResponsibleOwner.FullName),
                actionItem.ResponsibleOwner.Email,
                null,
                null,
                null
            ), Times.Once);
        }
    }
}