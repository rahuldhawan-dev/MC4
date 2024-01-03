using log4net;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Employee;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCall.CommonTest.Configuration.NotificationTasks.HumanResources.Employee
{
    [TestClass]
    public class DriversLicenseRenewalDueTest
    {
        private DriversLicenseRenewalDue _target;
        private Mock<IEmployeeRepository> _repository;
        private Mock<INotifier> _notifier;
        private Mock<INotificationService> _notificationService;
        private Mock<ILog> _log;

        [TestInitialize]
        public void TestInitialize()
        {
            _repository = new Mock<IEmployeeRepository>();
            _notifier = new Mock<INotifier>();
            _notificationService = new Mock<INotificationService>();
            _log = new Mock<ILog>();
            _target = new DriversLicenseRenewalDue(_repository.Object, _notifier.Object, _notificationService.Object,
                _log.Object);
        }
    }
}
