using System;
using System.Configuration;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Common;
using MMSINC.Interface;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using NHibernate;
using RazorEngine.Templating;
using StructureMap;

namespace MapCall.CommonTest.Utility.Notifications
{
    [TestClass]
    public class NotifierBaseTest
    {
        // using RazorNotifier since it's already in use and works
        private RazorNotifier _target;
        private Mock<ISmtpClientFactory> _smtpClientFactory;
        private Mock<ISmtpClient> _smtpClient;
        private Mock<IAuditLogEntryRepository> _auditLogRepo;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authenticationService;
        private Mock<ISessionFactory> _sessionFactory;
        private Mock<ISession> _session;
        private IContainer _container;

        private TrainingRecord UglyDataSetup()
        {
            var dataType1 = new DataType {
                TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_ATTENDED
            };
            var dataType2 = new DataType {
                TableName = TrainingRecordMap.TABLE_NAME, Name = TrainingRecord.DataTypeNames.EMPLOYEES_SCHEDULED
            };
            var model = new TrainingRecord {
                ClassLocation = new ClassLocation {
                    Description = "Conference Room",
                    OperatingCenter = new OperatingCenter
                        {OperatingCenterCode = "HT", OperatingCenterName = "Happy Times"}
                },
                HeldOn = new DateTime(2003, 6, 27, 12, 30, 00),
                Instructor = new Employee {FirstName = "Delores", LastName = "Herbig", EmployeeId = "123"},
                SecondInstructor = new Employee {FirstName = "Georgia", LastName = "Lass", EmployeeId = "321"},
                TrainingModule = new TrainingModule {TotalHours = 15}
            };
            model.EmployeesAttended.Add(new TrainingRecordAttendedEmployee
                {Employee = new Employee {FirstName = "Daisy", LastName = "Adair", EmployeeId = "124"}});
            model.EmployeesAttended.Add(new TrainingRecordAttendedEmployee
                {Employee = new Employee {FirstName = "Georgia", LastName = "Lass", EmployeeId = "125"}});

            return model;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _smtpClient = new Mock<ISmtpClient>();
            _smtpClientFactory.Setup(x => x.Build()).Returns(_smtpClient.Object);
            _session = new Mock<ISession>();
            _sessionFactory.Setup(x => x.OpenSession()).Returns(_session.Object);

            _target = _container.GetInstance<RazorNotifier>();
        }

        private void InitializeContainer(ConfigurationExpression e)
        {
            _smtpClientFactory = e.For<ISmtpClientFactory>().Mock();
            _auditLogRepo = e.For<IAuditLogEntryRepository>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _authenticationService = e.For<IAuthenticationService<User>>().Mock();
            _sessionFactory = e.For<ISessionFactory>().Mock();
            e.For<ITemplateService>().Use(ctx => new TemplateService()).Singleton();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _smtpClient.VerifyAll();
            ConfigurationManager.AppSettings[NotifierBase.EMAIL_ADDRESS_OVERRIDE_KEY] = null;
        }

        [TestMethod]
        public void TestNotify()
        {
            var model = UglyDataSetup();

            _smtpClient.Setup(x => x.Send(It.Is<IMailMessage>(m => m.To
                                                                    .Any(a =>
                                                                         a.Address == "foo@bar.com") &&
                                                                   m.Subject ==
                                                                   "MapCall Notification - Training Record")));

            _target.Notify(RoleApplications.Operations, RoleModules.OperationsTrainingRecords,
                "Training Record", model, "foo@bar.com");
        }

        [TestMethod]
        public void TestNotifyOverridesPurposeForEmailBodyFilenameIngestionIfConfiguredToDoSo()
        {
            var model = UglyDataSetup();

            _smtpClient.Setup(x => x.Send(It.Is<IMailMessage>(m => m.Body.Contains("<h2>Canceled Training Course Notification</h2>"))));

            _target.Notify(RoleApplications.Operations, 
                RoleModules.OperationsTrainingRecords, 
                "Training Record", 
                model, 
                "foo@bar.com", 
                templateName: "Canceled Training");
        }

        [TestMethod]
        public void TestNotifyRecordsAuditLogEntry()
        {
            var model = UglyDataSetup();
            var user = new User {Id = 4};

            _smtpClient.Setup(x => x.Send(It.Is<IMailMessage>(m => m.To
                                                                    .Any(a =>
                                                                         a.Address == "foo@bar.com") &&
                                                                   m.Subject ==
                                                                   "MapCall Notification - Training Record")));
            _authenticationService.Setup(x => x.CurrentUser).Returns(user);
            _auditLogRepo.Setup(x => x.Save(It.Is<AuditLogEntry>(e => e.User == user)));

            _target.Notify(RoleApplications.Operations, RoleModules.OperationsTrainingRecords,
                "Training Record", model, "foo@bar.com");

            _auditLogRepo.VerifyAll();
        }

        [TestMethod]
        public void TestNotifyDoesNotFreakOutWhenThereIsNoAuthenticationService()
        {
            var model = UglyDataSetup();

            _smtpClient.Setup(x => x.Send(It.Is<IMailMessage>(m => m.To
                                                                    .Any(a =>
                                                                         a.Address == "foo@bar.com") &&
                                                                   m.Subject ==
                                                                   "MapCall Notification - Training Record")));

            _auditLogRepo.Setup(x => x.Save(It.Is<AuditLogEntry>(e => e.User == null)));

            _target.Notify(RoleApplications.Operations, RoleModules.OperationsTrainingRecords,
                "Training Record", model, "foo@bar.com");

            _auditLogRepo.VerifyAll();
        }
    }
}
