using System;
using System.Linq;
using log4net;
using MapCall.Common.Configuration.NotificationTasks.Environmental;
using MapCall.Common.Configuration.NotificationTasks.FieldServices.Assets;
using MapCall.Common.Configuration.NotificationTasks.HealthandSafety;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Employee;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Environmental;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Facilities;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Facilities.InterconnectContractEnds;
using MapCall.Common.Configuration.NotificationTasks.Production;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallScheduler.JobHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers
{
    [TestClass]
    public class NotifierTaskServiceTest
    {
        #region Private Members

        private NotifierTaskService _target;
        private Mock<ILog> _log;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(c => {
                //c.For<IRepository<NotificationPurpose>>().Use<RepositoryBase<NotificationPurpose>>();
            });
            _log = new Mock<ILog>();
            _container.Inject(_log.Object);
            _container.Inject(new Mock<IRepository<NotificationPurpose>>().Object);
            _container.Inject(new Mock<IEmployeeRepository>().Object);
            _container.Inject(new Mock<IInterconnectionRepository>().Object);
            _container.Inject(new Mock<IRepository<EnvironmentalPermit>>().Object);
            _container.Inject(new Mock<IRepository<ArcFlashStudy>>().Object);
            _container.Inject(new Mock<IRepository<GasMonitor>>().Object);
            _container.Inject(new Mock<IOperatorLicenseRepository>().Object);
            _container.Inject(new Mock<INotificationConfigurationRepository>().Object);
            _container.Inject(new Mock<IRepository<SystemDeliveryFacilityEntryAdjustment>>().Object);
            _container.Inject(new Mock<IRepository<MapCall.Common.Model.Entities.SystemDeliveryEntry>>().Object);
            _container.Inject(new Mock<IRepository<Facility>>().Object);
            _container.Inject(new Mock<IRepository<NearMiss>>().Object);
            _container.Inject(new Mock<INotifier>().Object);
            _container.Inject(new Mock<INotificationService>().Object);
            _container.Inject(new Mock<IDateTimeProvider>().Object);
            _container.Inject(new Mock<IServiceFlushRepository>().Object);
            _container.Inject(new Mock<IEnvironmentalNonComplianceEventActionItemRepository>().Object);
            _target = _container.GetInstance<NotifierTaskService>();
        }

        #endregion

        [TestMethod]
        public void TestExceptionsGeneratedInstantiatingATaskAreRethrown()
        {
            var mockContainer = new Mock<IContainer>();
            _target = new NotifierTaskService(_log.Object, mockContainer.Object);

            // TODO: this will always need to be the first task class in order...
            mockContainer.Setup(c => c.GetInstance(typeof(DriversLicenseRenewalDue))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(OperatorLicenseExpirationIn15Days))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(OperatorLicenseExpirationIn30Days))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(OperatorLicenseExpirationIn60Days))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(OperatorLicenseExpirationIn90Days))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(FlushingResultsNotReceived))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(InterconnectionContractEndsIn30Days))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(InterconnectionContractEndsIn3Months))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(InterconnectionContractEndsIn6Months))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(InterconnectionContractEndsIn1Year))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(ArcFlashStudyExpiresIn1Year))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(GasMonitorCalibrationDue))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(SystemDeliveryEntryValidated))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(SystemDeliveryEntryDue))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(SystemDeliveryEntryAdjustmentMade))).Throws<ApplicationException>();
            mockContainer.Setup(c => c.GetInstance(typeof(EnvironmentalNonComplianceEventActionItem))).Throws<ApplicationException>();

            // If this test is failing due to a NullReferenceException, you need to include your task in the mockContainer.
            MyAssert.Throws<ApplicationException>(() => _target.GetAllTasks().First());
        }

        [TestMethod]
        public void TestGetAllTasksThrowsExceptionWhenTaskCannotBeInstantiated()
        {
            var iContainer = new Mock<IContainer>();
            _target = new NotifierTaskService(_log.Object, iContainer.Object);

            iContainer.Setup(c => c.GetInstance(typeof(DriversLicenseRenewalDue))).Returns(null);

            MyAssert.Throws<NullReferenceException>(() => _target.GetAllTasks().First());
        }

        [TestMethod]
        public void TestGetAllTasksGetsAllTasksFromNamespace()
        {
            var tasks = new[] {
                typeof(DriversLicenseRenewalDue),
                typeof(MedicalCertificateDueInOneMonth),
                typeof(MedicalCertificateDueInTwoMonths),
                typeof(MedicalCertificateDueInTwoWeeks),
                typeof(MedicalCertificateOverdue),
                typeof(EnvironmentalPermitRenewalExpiresIn1Day),
                typeof(EnvironmentalPermitRenewalExpiresIn4Months),
                typeof(EnvironmentalPermitRenewalExpiresIn6Months),
                typeof(OperatorLicenseExpirationIn15Days),
                typeof(OperatorLicenseExpirationIn30Days),
                typeof(OperatorLicenseExpirationIn60Days),
                typeof(OperatorLicenseExpirationIn90Days),
                typeof(InterconnectionContractEndsIn30Days),
                typeof(InterconnectionContractEndsIn3Months),
                typeof(InterconnectionContractEndsIn6Months),
                typeof(InterconnectionContractEndsIn1Year),
                typeof(ArcFlashStudyExpiresIn1Year),
                typeof(GasMonitorCalibrationDue),
                typeof(SystemDeliveryEntryValidated),
                typeof(SystemDeliveryEntryDue),
                typeof(SystemDeliveryEntryAdjustmentMade),
                typeof(SafetyNearMiss),
                typeof(EnvironmentalNearMiss),
                typeof(FlushingResultsNotReceived),
                typeof(EnvironmentalNonComplianceActionItemAssigned)
            };

            var allTasks = _target.GetAllTasks();

            Assert.AreEqual(tasks.Length, allTasks.Count());

            tasks.Each(taskType => Assert.IsTrue(allTasks.Any(t => t.GetType() == taskType)));
        }
    }
}
