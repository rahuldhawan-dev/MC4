using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class JobSiteCheckListRepositoryTest : MapCallMvcSecuredRepositoryTestBase<JobSiteCheckList,
        JobSiteCheckListRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override User CreateUser()
        {
            return GetEntityFactory<User>().Create(new {IsAdmin = true});
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestDeletingARecordDeletesItsChildExcavationRecords()
        {
            var entity = GetFactory<JobSiteCheckListFactory>().Create();
            var excavation = GetFactory<JobSiteExcavationFactory>().Create(new {
                JobSiteCheckList = entity,
                LocationType = GetFactory<JobSiteExcavationLocationTypeFactory>().Create(),
                SoilType = GetFactory<JobSiteExcavationSoilTypeFactory>().Create()
            });
            Session.Clear();

            Repository.Delete(entity);

            Assert.IsNull(Repository.Find(entity.Id));
            Assert.IsNull(Session.Query<JobSiteExcavation>().SingleOrDefault(x => x.Id == excavation.Id));
        }

        [TestMethod]
        public void TestSavingSavesNewChildExcavationRecordsWhenCheckListIsNew()
        {
            var entity = GetFactory<JobSiteCheckListFactory>().BuildWithConcreteDependencies();
            var excavation = GetFactory<JobSiteExcavationFactory>().Build(new {
                JobSiteCheckList = entity,
                LocationType = GetFactory<JobSiteExcavationLocationTypeFactory>().Create(),
                SoilType = GetFactory<JobSiteExcavationSoilTypeFactory>().Create()
            });
            Repository.Save(entity);
            Assert.AreNotEqual(0, excavation.Id);

            Session.Evict(entity);

            var entityAgain = Repository.Find(entity.Id);
            Assert.AreEqual(excavation.Id, entityAgain.Excavations.Single().Id);
        }

        [TestMethod]
        public void TestSavingSavesNewChildExcavationRecordsWhenCheckListIsUpdated()
        {
            var entity = GetFactory<JobSiteCheckListFactory>().Create();
            var excavation = GetFactory<JobSiteExcavationFactory>().Build(new {
                JobSiteCheckList = entity,
                LocationType = GetFactory<JobSiteExcavationLocationTypeFactory>().Create(),
                SoilType = GetFactory<JobSiteExcavationSoilTypeFactory>().Create()
            });
            Repository.Save(entity);
            Assert.AreNotEqual(0, excavation.Id);

            Session.Evict(entity);

            var entityAgain = Repository.Find(entity.Id);
            Assert.AreEqual(excavation.Id, entityAgain.Excavations.Single().Id);
        }

        [TestMethod]
        public void TestSavingDeletesRemovedChildExcavationRecords()
        {
            var entity = GetFactory<JobSiteCheckListFactory>().Create();
            var goodExcavation = GetFactory<JobSiteExcavationFactory>().Build(new {
                JobSiteCheckList = entity,
                LocationType = GetFactory<JobSiteExcavationLocationTypeFactory>().Create(),
                SoilType = GetFactory<JobSiteExcavationSoilTypeFactory>().Create()
            });
            var badExcavation = GetFactory<JobSiteExcavationFactory>().Build(new {
                JobSiteCheckList = entity,
                LocationType = GetFactory<JobSiteExcavationLocationTypeFactory>().Create(),
                SoilType = GetFactory<JobSiteExcavationSoilTypeFactory>().Create()
            });
            Repository.Save(entity);

            Session.Evict(entity);

            var entityAgain = Repository.Find(entity.Id);
            Assert.AreEqual(2, entityAgain.Excavations.Count, "Sanity check");

            entityAgain.Excavations.Remove(entityAgain.Excavations.Single(x => x.Id == badExcavation.Id));
            Repository.Save(entityAgain);
            Session.Evict(entityAgain);

            entityAgain = Repository.Find(entity.Id);
            Assert.AreEqual(1, entityAgain.Excavations.Count);
            Assert.AreEqual(goodExcavation.Id, entityAgain.Excavations.Single().Id);
        }

        [TestMethod]
        public void TestSavingNewRecordSavesAttachedProtectionTypes()
        {
            var entity = GetFactory<JobSiteCheckListFactory>().BuildWithConcreteDependencies();
            var protectionType = GetFactory<JobSiteExcavationProtectionTypeFactory>().Create();
            entity.ProtectionTypes.Add(protectionType);
            Repository.Save(entity);

            Session.Evict(entity);

            var entityAgain = Repository.Find(entity.Id);
            Assert.AreEqual(1, entityAgain.ProtectionTypes.Count);
            Assert.AreEqual(protectionType.Id, entityAgain.ProtectionTypes.Single().Id);
        }

        [TestMethod]
        public void TestSavingExistingRecordSavesAttachedProtectionTypes()
        {
            var entity = GetFactory<JobSiteCheckListFactory>().Create();
            var protectionType = GetFactory<JobSiteExcavationProtectionTypeFactory>().Create();
            entity.ProtectionTypes.Add(protectionType);
            Repository.Save(entity);

            Session.Evict(entity);

            var entityAgain = Repository.Find(entity.Id);
            Assert.AreEqual(1, entityAgain.ProtectionTypes.Count);
            Assert.AreEqual(protectionType.Id, entityAgain.ProtectionTypes.Single().Id);
        }

        [TestMethod]
        public void TestSavingDeletesRemovedProtectionTypes()
        {
            var goodType = GetFactory<JobSiteExcavationProtectionTypeFactory>().Create();
            var badType = GetFactory<JobSiteExcavationProtectionTypeFactory>().Create();
            var entity = GetFactory<JobSiteCheckListFactory>().Create();
            entity.ProtectionTypes.Add(goodType);
            entity.ProtectionTypes.Add(badType);

            // TestDataFactory doesn't flush. Flushing's needed to get this
            // many-to-many ref to save for some reason. That's also why this
            // is calling Repository.Save instead of Session.Save/Flush.
            Repository.Save(entity);

            Session.Evict(entity);

            var entityAgain = Repository.Find(entity.Id);
            Assert.AreEqual(2, entityAgain.ProtectionTypes.Count, "Sanity check");

            entityAgain.ProtectionTypes.Remove(entityAgain.ProtectionTypes.Single(x => x.Id == badType.Id));
            Repository.Save(entityAgain);

            Session.Evict(entityAgain);
            entityAgain = Repository.Find(entity.Id);
            Assert.AreEqual(1, entityAgain.ProtectionTypes.Count);
            Assert.AreEqual(goodType.Id, entityAgain.ProtectionTypes.Single().Id);
        }

        [TestMethod]
        public void TestLinqDoesNotReturnIncidentsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });
            Session.Save(user);

            var valid = GetFactory<JobSiteCheckListFactory>().Create(new {OperatingCenter = opCntr1});
            var invalid = GetFactory<JobSiteCheckListFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<JobSiteCheckListRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestLinqReturnsAllTheIncidentsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var valid = GetFactory<JobSiteCheckListFactory>().Create(new {OperatingCenter = opCntr1});
            var otherValid = GetFactory<JobSiteCheckListFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<JobSiteCheckListRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsTrue(result.Contains(otherValid));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnIncidentsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });
            Session.Save(user);

            var valid = GetFactory<JobSiteCheckListFactory>().Create(new {OperatingCenter = opCntr1});
            var invalid = GetFactory<JobSiteCheckListFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<JobSiteCheckListRepository>();
            var model = new EmptySearchSet<JobSiteCheckList>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllTheIncidentsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var valid = GetFactory<JobSiteCheckListFactory>().Create(new {OperatingCenter = opCntr1});
            var otherValid = GetFactory<JobSiteCheckListFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<JobSiteCheckListRepository>();

            var model = new EmptySearchSet<JobSiteCheckList>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(valid));
            Assert.IsTrue(result.Contains(otherValid));
        }

        #endregion
    }
}
