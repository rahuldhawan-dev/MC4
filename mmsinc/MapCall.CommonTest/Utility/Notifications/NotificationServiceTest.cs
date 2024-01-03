using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Utility.Notifications
{
    [TestClass]
    public class NotificationServiceTest : InMemoryDatabaseTest<NotificationConfiguration>
    {
        #region Private Members

        private NotificationService _target;
        private Mock<INotifier> _notifier;
        private Application _application;
        private Module _module;
        private NotificationPurpose _purpose;
        private NotificationConfiguration _config;
        private TailgateTalk _tailgateTalk;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For(typeof(IRepository<>)).Use(typeof(RepositoryBase<>));
            i.For<INotificationConfigurationRepository>().Use<NotificationConfigurationRepository>();
            i.For<INotifier>().Use((_notifier = new Mock<INotifier>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = _container.GetInstance<NotificationService>();

            _application = GetFactory<ApplicationFactory>().Create(new {
                Id = 1,
                Name = "Operations"
            });

            _module = GetFactory<ModuleFactory>().Create(new {
                Id = 1,
                Name = "Health And Safety",
                Application = _application
            });

            _purpose = GetFactory<NotificationPurposeFactory>().Create(new {
                Module = _module,
                Purpose = "Tailgate Talk"
            });

            _config = GetFactory<NotificationConfigurationFactory>().Create(new {
                NotificationPurposes = GetFactory<NotificationPurposeFactory>().CreateList(1, _purpose)
            });

            _tailgateTalk = new TailgateTalk {
                HeldOn = new DateTime(2003, 6, 27, 12, 30, 00),
                PresentedBy = new Employee {
                    FirstName = "Rube", 
                    LastName = "Sofer", 
                    EmployeeId = "123"
                },
                Topic = new TailgateTalkTopic {
                    Topic = "Extraction", 
                    OrmReferenceNumber = "1234"
                },
                TrainingTimeHours = 4,
                TailgateTalkEmployees = new List<TailgateTalkEmployee> {
                    new TailgateTalkEmployee {
                        Employee = new Employee {
                            FirstName = "Daisy",
                            LastName = "Adair",
                            EmployeeId = "124"
                        }
                    },
                    new TailgateTalkEmployee {
                        Employee = new Employee {
                            FirstName = "Georgia",
                            LastName = "Lass",
                            EmployeeId = "125"
                        }
                    }
                }
            };

            Session.Save(_config);
            Session.Flush();
        }

        #endregion

        [TestMethod]
        public void TestNotify()
        {
            _notifier.Setup(
                x =>
                    x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                        _config.NotificationPurposes.First().Module.Value,
                        _config.NotificationPurposes.First().Purpose, 
                        _tailgateTalk,
                        _config.Contact.Email, 
                        null, 
                        It.IsAny<IList<Attachment>>(),
                        _purpose.Purpose));

            _target.Notify(_config.OperatingCenter.Id, 
                (RoleModules)_purpose.Module.Id,
                _purpose.Purpose, 
                _tailgateTalk);

            _notifier.VerifyAll();
        }

        [TestMethod]
        public void TestNotifyIncludesAttachments()
        {
            var notifierArgs = new NotifierArgs {
                OperatingCenterId = _config.OperatingCenter.Id,
                Module = (RoleModules)_config.NotificationPurposes.First().Module.Id,
                Purpose = _config.NotificationPurposes.First().Purpose,
                Data = _tailgateTalk
            };

            var expectedBytes = new byte[] {1, 2, 3};
            var expectedFileName = "somefile.file";
            var attachment = new Attachment(expectedFileName, expectedBytes);

            notifierArgs.Attachments.Add(attachment);

            _notifier.Setup(
                x =>
                    x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                        _config.NotificationPurposes.First().Module.Value,
                        _config.NotificationPurposes.First().Purpose,
                        _tailgateTalk,
                        _config.Contact.Email,
                        null,
                        new[] {attachment},
                        null));

            _target.Notify(notifierArgs);
        }

        [TestMethod]
        public void TestNotifySendsToAllIfOperatingCenterIdNotProvided()
        {
            _notifier.Setup(
                x =>
                    x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                        _config.NotificationPurposes.First().Module.Value,
                        _config.NotificationPurposes.First().Purpose,
                        _tailgateTalk,
                        _config.Contact.Email,
                        null,
                        It.IsAny<IList<Attachment>>(),
                        _config.NotificationPurposes.First().Purpose));

            _target.Notify(0, 
                (RoleModules)_config.NotificationPurposes.First().Module.Id,
                _config.NotificationPurposes.First().Purpose, 
                _tailgateTalk);

            _notifier.VerifyAll();
        }

        [TestMethod]
        public void TestNotifierSendsToIndividualIfSpecified()
        {
            var address = "a@b.com";
            var subject = "subject";

            _notifier.Setup(x =>
                x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                    _config.NotificationPurposes.First().Module.Value,
                    _config.NotificationPurposes.First().Purpose,
                    _tailgateTalk,
                    address,
                    subject,
                    It.IsAny<IList<Attachment>>(),
                    _config.NotificationPurposes.First().Purpose));

            _target.Notify(_config.OperatingCenter.Id,
                (RoleModules)_config.NotificationPurposes.First().Module.Id,
                _config.NotificationPurposes.First().Purpose,
                _tailgateTalk,
                subject,
                address);

            _notifier.VerifyAll();
        }

        [TestMethod]
        public void TestNotifierSendsToIndividualIfSpecifiedIfNoConfigurationsExist()
        {
            var address = "a@b.com";
            var subject = "subject";
            _notifier.Setup(x =>
                x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                    _config.NotificationPurposes.First().Module.Value,
                    _config.NotificationPurposes.First().Purpose,
                    _tailgateTalk,
                    address,
                    subject,
                    It.IsAny<IList<Attachment>>(),
                    _config.NotificationPurposes.First().Purpose));

            _target.Notify(666,
                (RoleModules)_config.NotificationPurposes.First().Module.Id,
                _config.NotificationPurposes.First().Purpose,
                _tailgateTalk,
                subject,
                address);

            _notifier.VerifyAll();
        }

        [TestMethod]
        public void TestNotifierDoesNotSendWhenAddressIsNullOrWhiteSpace()
        {
            var whitespaceAddress = "";
            var validAddress = "x@x.com";
            string nullAddress = null;
            var subject = "Subject";

            // White Space Test

            _notifier.Setup(x =>
                x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                    _config.NotificationPurposes.First().Module.Value,
                    _config.NotificationPurposes.First().Purpose,
                    _tailgateTalk,
                    whitespaceAddress,
                    subject,
                    It.IsAny<IList<Attachment>>(),
                    _config.NotificationPurposes.First().Purpose));

            _target.Notify(666,
                (RoleModules)_config.NotificationPurposes.First().Module.Id,
                _config.NotificationPurposes.First().Purpose,
                _tailgateTalk,
                subject,
                whitespaceAddress);

            _notifier.Verify(x => x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                _config.NotificationPurposes.First().Module.Value,
                _config.NotificationPurposes.First().Purpose,
                _tailgateTalk,
                whitespaceAddress,
                subject,
                It.IsAny<IList<Attachment>>(),
                _config.NotificationPurposes.First().Purpose), Times.Never);

            // null test

            _notifier.Setup(x =>
                x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                    _config.NotificationPurposes.First().Module.Value,
                    _config.NotificationPurposes.First().Purpose,
                    _tailgateTalk,
                    nullAddress,
                    subject,
                    It.IsAny<IList<Attachment>>(),
                    _config.NotificationPurposes.First().Purpose));

            _target.Notify(666,
                (RoleModules)_config.NotificationPurposes.First().Module.Id,
                _config.NotificationPurposes.First().Purpose,
                _tailgateTalk,
                subject);

            _notifier.Verify(x => x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                _config.NotificationPurposes.First().Module.Value,
                _config.NotificationPurposes.First().Purpose,
                _tailgateTalk,
                nullAddress,
                subject,
                It.IsAny<IList<Attachment>>(),
                _config.NotificationPurposes.First().Purpose), Times.Never);

            // Runs with valid address

            _notifier.Setup(x =>
                x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                    _config.NotificationPurposes.First().Module.Value,
                    _config.NotificationPurposes.First().Purpose,
                    _tailgateTalk,
                    validAddress,
                    subject,
                    It.IsAny<IList<Attachment>>(),
                    _config.NotificationPurposes.First().Purpose));

            _target.Notify(666,
                (RoleModules)_config.NotificationPurposes.First().Module.Id,
                _config.NotificationPurposes.First().Purpose,
                _tailgateTalk,
                subject,
                validAddress);

            _notifier.Verify(x => x.Notify(_config.NotificationPurposes.First().Module.Application.Value,
                _config.NotificationPurposes.First().Module.Value,
                _config.NotificationPurposes.First().Purpose,
                _tailgateTalk,
                validAddress,
                subject,
                It.IsAny<IList<Attachment>>(),
                _config.NotificationPurposes.First().Purpose), Times.Once);
        }
    }
}
