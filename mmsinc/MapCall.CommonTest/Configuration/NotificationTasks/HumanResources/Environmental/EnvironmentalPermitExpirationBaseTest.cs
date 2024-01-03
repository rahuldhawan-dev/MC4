using System;
using System.Collections.Generic;
using log4net;
using MapCall.Common.Configuration.NotificationTasks.HumanResources.Environmental;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Configuration.NotificationTasks.HumanResources.Environmental
{
    public abstract class EnvironmentalPermitExpirationBaseTest<T> : InMemoryDatabaseTest<EnvironmentalPermit> where T : EnvironmentalPermitRenewalExpirationBase
    {
        #region Private Members

        protected Mock<INotifier> _notifier;
        protected Mock<INotificationService> _notificationService;
        protected Mock<ILog> _log;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected RepositoryBase<EnvironmentalPermit> _repository;
        protected RepositoryBase<NotificationPurpose> _notificationPurposeRepository;
        protected DateTime _currentDateTime;

        protected T _target;
        protected OperatingCenter _operatingCenter;
        protected NotificationPurpose _allocationPermitPurpose;
        protected NotificationPurpose _chemicalPermitPurpose;
        protected NotificationPurpose _tankInspectionPermitPurpose;
        protected EnvironmentalPermit _notExpiringChemicalPermit;
        protected EnvironmentalPermit _expiringAllocationPermit;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);

            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _notifier = e.For<INotifier>().Mock();
            _notificationService = e.For<INotificationService>().Mock();
            _log = e.For<ILog>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = _container.GetInstance<T>();
            _repository = _container.GetInstance<RepositoryBase<EnvironmentalPermit>>();
            _notificationPurposeRepository = _container.GetInstance<RepositoryBase<NotificationPurpose>>();
            
            _currentDateTime = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate())
                             .Returns(_currentDateTime);

            _operatingCenter = GetFactory<OperatingCenterFactory>().Create();

            var module = GetFactory<ModuleFactory>().Create(new {
                Id = (int)RoleModules.EnvironmentalPermitTypesExpiration,
                Name = "Permit Types",
                Application = GetFactory<ApplicationFactory>().Create(new {
                    Id = 1,
                    Name = "Environmental"
                })
            });

            _allocationPermitPurpose = GetFactory<NotificationPurposeFactory>().Create(new {
                Module = module,
                Purpose = "Allocation Permit"
            });

            _chemicalPermitPurpose = GetFactory<NotificationPurposeFactory>().Create(new {
                Module = module,
                Purpose = "Chemical"
            });

            _tankInspectionPermitPurpose = GetFactory<NotificationPurposeFactory>().Create(new {
                Module = module,
                Purpose = "Tank Inspection"
            });

            var configuration = GetFactory<NotificationConfigurationFactory>().Create(new {
                OperatingCenter = _operatingCenter,
                Contact = GetFactory<ContactFactory>().Create(),
                NotificationPurposes = new List<NotificationPurpose> {
                    _allocationPermitPurpose,
                    _chemicalPermitPurpose,
                    _tankInspectionPermitPurpose
                }
            });

            _notExpiringChemicalPermit = GetEntityFactory<EnvironmentalPermit>().Create(new {
                EnvironmentalPermitType = GetEntityFactory<EnvironmentalPermitType>().Create(new {
                    Description = _chemicalPermitPurpose.Purpose
                }),
                OperatingCenters = GetEntityFactory<OperatingCenter>().CreateList(1, _operatingCenter)
            });

            _expiringAllocationPermit = GetFactory<EnvironmentalExpiringPermitFactory>().Create(new {
                EnvironmentalPermitType = GetEntityFactory<EnvironmentalPermitType>().Create(new {
                    Description = _allocationPermitPurpose.Purpose
                }),
                OperatingCenters = GetEntityFactory<OperatingCenter>().CreateList(1, _operatingCenter)
            });

            Session.Save(configuration);
            Session.Save(_notExpiringChemicalPermit);
            Session.Save(_expiringAllocationPermit);
            Session.Flush();
        }

        #endregion
    }
}
