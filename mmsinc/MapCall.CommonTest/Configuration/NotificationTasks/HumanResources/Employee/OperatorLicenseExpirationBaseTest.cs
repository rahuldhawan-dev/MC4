using System;
using System.Collections.Generic;
using log4net;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Employee;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Configuration.NotificationTasks.HumanResources.Employee
{
    public abstract class OperatorLicenseExpirationBaseTest<T> : InMemoryDatabaseTest<OperatorLicense> where T : OperatorLicenseExpirationBase
    {
        #region Private Members

        protected Mock<ILog> _log;
        protected Mock<INotifier> _notifier;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected Mock<INotificationService> _notificationService;

        protected T _target;
        protected IOperatorLicenseRepository _operatorLicenseRepository;
        protected INotificationConfigurationRepository _notificationConfigurationRepository;
        protected DateTime _currentDateTime;
        protected Contact _contact;
        protected OperatingCenter _operatingCenter;
        protected NotificationPurpose _notificationPurpose;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _notifier = e.For<INotifier>().Mock();
            _notificationService = e.For<INotificationService>().Mock();
            _log = e.For<ILog>().Mock();

            e.For<INotificationConfigurationRepository>().Use<NotificationConfigurationRepository>();
            e.For<IOperatorLicenseRepository>().Use<OperatorLicenseRepository>();
            e.For<IAuthenticationService<User>>().Use(new Mock<IAuthenticationService<User>>().Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = _container.GetInstance<T>();
            _operatorLicenseRepository = _container.GetInstance<IOperatorLicenseRepository>();
            _notificationConfigurationRepository = _container.GetInstance<INotificationConfigurationRepository>();
            
            _currentDateTime = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(_currentDateTime);

            _operatingCenter = GetFactory<OperatingCenterFactory>().Create();

            var module = GetFactory<ModuleFactory>().Create(new {
                Id = (int)RoleModules.HumanResourcesEmployee,
                Name = "Employee",
                Application = GetFactory<ApplicationFactory>().Create(new {
                    Id = 1,
                    Name = "Human Resources"
                })
            });

            _notificationPurpose = GetFactory<NotificationPurposeFactory>().Create(new {
                Module = module,
                Purpose = "Operator License Expiration"
            });

            _contact = GetFactory<ContactFactory>().Create();

            var configuration = GetFactory<NotificationConfigurationFactory>().Create(new {
                OperatingCenter = _operatingCenter,
                Contact = _contact,
                NotificationPurposes = new List<NotificationPurpose> {
                    _notificationPurpose
                }
            });

            Session.Save(module);
            Session.Save(configuration);
            Session.Flush();
        }

        #endregion
    }
}
