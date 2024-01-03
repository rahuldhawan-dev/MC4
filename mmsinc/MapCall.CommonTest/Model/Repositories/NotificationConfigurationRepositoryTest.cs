using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class NotificationConfigurationRepositoryTest : 
        MapCallMvcInMemoryDatabaseTestBase<NotificationConfiguration, NotificationConfigurationRepository>
    {
        #region Private Fields

        private Contact _contactA;
        private Contact _contactB;

        private OperatingCenter _operatingCenterA;
        private OperatingCenter _operatingCenterB;

        private Module _waterQualityModule;
        private Module _productionModule;

        private NotificationPurpose _waterQualityNotificationPurposeA;
        private NotificationPurpose _waterQualityNotificationPurposeB;
        private NotificationPurpose _waterQualityNotificationPurposeC;
        private NotificationPurpose _productionNotificationPurpose;

        private NotificationConfiguration _waterQualityNotificationConfigurationA;
        private NotificationConfiguration _waterQualityNotificationConfigurationB;
        private NotificationConfiguration _productionNotificationConfiguration;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {

            _contactA = GetEntityFactory<Contact>().Create();
            _contactB = GetEntityFactory<Contact>().Create();
            _operatingCenterA = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ3"});
            _operatingCenterB = GetEntityFactory<OperatingCenter>().Create(new {OperatingCenterCode = "NJ4"});

            _waterQualityModule = GetEntityFactory<Module>().Create(new {
                Id = RoleModules.WaterQualityGeneral,
                Application = GetEntityFactory<Application>().Create(new {
                    Id = RoleApplications.WaterQuality
                })
            });

            _waterQualityNotificationPurposeA = GetEntityFactory<NotificationPurpose>().Create(new {
                Module = _waterQualityModule,
                Purpose = "This is purpose A for water quality module"
            });

            _waterQualityNotificationPurposeB = GetEntityFactory<NotificationPurpose>().Create(new {
                Module = _waterQualityModule,
                Purpose = "This is purpose B for water quality module"
            });

            _waterQualityNotificationPurposeC = GetEntityFactory<NotificationPurpose>().Create(new {
                Module = _waterQualityModule,
                Purpose = "This is purpose C for water quality module"
            });

            _waterQualityNotificationConfigurationA = GetEntityFactory<NotificationConfiguration>().Create(new {
                OperatingCenter = _operatingCenterA,
                Contact = _contactA,
            });

            _waterQualityNotificationConfigurationB = GetEntityFactory<NotificationConfiguration>().Create(new {
                OperatingCenter = _operatingCenterB,
                Contact = _contactA,
            });

            _waterQualityNotificationConfigurationA.NotificationPurposes.Add(_waterQualityNotificationPurposeA);
            _waterQualityNotificationConfigurationA.NotificationPurposes.Add(_waterQualityNotificationPurposeB);
            _waterQualityNotificationConfigurationA.NotificationPurposes.Add(_waterQualityNotificationPurposeC);
            _waterQualityNotificationConfigurationB.NotificationPurposes.Add(_waterQualityNotificationPurposeA);
            _waterQualityNotificationConfigurationB.NotificationPurposes.Add(_waterQualityNotificationPurposeB);
            _waterQualityNotificationConfigurationB.NotificationPurposes.Add(_waterQualityNotificationPurposeC);

            _productionModule = GetEntityFactory<Module>().Create(new {
                Id = RoleModules.ProductionEquipment,
                Application = GetEntityFactory<Application>().Create(new {
                    Id = RoleApplications.Production
                })
            });

            _productionNotificationPurpose = GetEntityFactory<NotificationPurpose>().Create(new {
                Module = _productionModule,
                Purpose = "This is a purpose for production module"
            });

            _productionNotificationConfiguration = GetEntityFactory<NotificationConfiguration>().Create(new {
                OperatingCenter = _operatingCenterB,
                Contact = _contactB,
            });

            _productionNotificationConfiguration.NotificationPurposes.Add(_productionNotificationPurpose);

            Session.Save(_waterQualityNotificationConfigurationA);
            Session.Save(_waterQualityNotificationConfigurationB);
            Session.Save(_productionNotificationConfiguration);
            Session.Flush();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestFindByModuleAndPurposeDoesWhatItSaysOnTheTin()
        {
            var notificationConfigurations = Repository.FindByModuleAndPurpose(
                                                            (RoleModules)_waterQualityModule.Id,
                                                            _waterQualityNotificationPurposeA.Purpose)
                                                       .ToList();

            Assert.AreEqual(2, notificationConfigurations.Count);
            Assert.AreEqual(1, notificationConfigurations.First().NotificationPurposes.Count(x => x.Purpose == _waterQualityNotificationPurposeA.Purpose));
            Assert.AreEqual(1, notificationConfigurations.Last().NotificationPurposes.Count(x => x.Purpose == _waterQualityNotificationPurposeA.Purpose));
            Assert.AreEqual(_contactA, notificationConfigurations.First().Contact);
            Assert.AreEqual(_contactA, notificationConfigurations.Last().Contact);
        }

        [TestMethod]
        public void TestFindByOperatingCenterModuleAndPurposeReturnsNotificationsThatExactlyMatchWhatItSaysOnTheTin()
        {
            var notificationConfigurations = 
                Repository.FindByOperatingCenterModuleAndPurpose(_operatingCenterB.Id, 
                               (RoleModules)_waterQualityModule.Id,
                               _waterQualityNotificationPurposeA.Purpose)
                          .ToList();

            Assert.AreEqual(1, notificationConfigurations.Count);
            Assert.AreEqual(1, notificationConfigurations.First().NotificationPurposes.Count(x => x.Purpose == _waterQualityNotificationPurposeA.Purpose));
            Assert.AreEqual(_contactA, notificationConfigurations.First().Contact);
            Assert.AreEqual(_operatingCenterB, notificationConfigurations.First().OperatingCenter);
        }

        [TestMethod]
        public void TestFindByOperatingCenterModuleAndPurposeReturnsConfigurationsThatMatchTheModuleAndPurposeAndHaveANullOperatingCenter()
        {
            _waterQualityNotificationConfigurationA.OperatingCenter = null;
            Session.Save(_waterQualityNotificationConfigurationA);
            Session.Flush();

            // The operatingCenterId value for this test does not matter.
            var notificationConfigurations = 
                Repository.FindByOperatingCenterModuleAndPurpose(-12345, 
                               (RoleModules)_waterQualityModule.Id,
                               _waterQualityNotificationPurposeA.Purpose)
                          .ToList();

            Assert.AreEqual(1, notificationConfigurations.Count);
            Assert.AreSame(_waterQualityNotificationConfigurationA, notificationConfigurations.Single());
        }

        [TestMethod]
        public void TestSearchNotificationConfigurationsShouldReturnCorrectNotificationConfigurationsBasedOffSearchCriteria()
        {
            var searchSet = new SearchNotificationConfigurationsSearchSet {
                OperatingCenter = _operatingCenterA.Id
            };

            var results = Repository.SearchNotificationConfigurations(searchSet)
                                    .ToList();

            Assert.AreEqual(3, results.Count);

            Assert.IsTrue(results.All(x => x.OperatingCenter == _operatingCenterA));
            Assert.IsTrue(results.All(x => x.ContactName == _contactA.ContactName));
            Assert.IsTrue(results.All(x => x.Application.Id == _waterQualityModule.Application.Id));
            Assert.IsTrue(results.All(x => x.Module.Id == _waterQualityModule.Id));
            
            Assert.AreEqual(1, results.Count(x => x.Purpose == _waterQualityNotificationPurposeA.Purpose));
            Assert.AreEqual(1, results.Count(x => x.Purpose == _waterQualityNotificationPurposeB.Purpose));
            Assert.AreEqual(1, results.Count(x => x.Purpose == _waterQualityNotificationPurposeC.Purpose));
            Assert.AreEqual(_waterQualityNotificationConfigurationA.Id, results.First().Id);
        }

        #endregion

        #region Helper class

        private class SearchNotificationConfigurationsSearchSet : SearchSet<NotificationConfiguration>
        {
            [SearchAlias("OperatingCenter", "criteriaOperatingCenter", "Id")]
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int? OperatingCenter { get; set; }
        }

        #endregion
    }
}
