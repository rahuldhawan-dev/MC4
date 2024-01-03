using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class SewerMainCleaningRepositoryTest : MapCallMvcSecuredRepositoryTestBase<SewerMainCleaning,
        SewerMainCleaningRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesAssets;

        #endregion

        #region Fields

        private Mock<IDateTimeProvider> _dateProvider;

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateProvider = e.For<IDateTimeProvider>().Mock<IDateTimeProvider>();
        }

        #endregion

        #region Linq/Criteria

        [TestMethod]
        public void TestLinqDoesNotReturnValvesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = ROLE_MODULE });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opCntr1 });
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });
            Session.Save(user);
            var sewerOpening1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening3 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            var sewerOpening4 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            var sewerMainCleaning1 = GetEntityFactory<SewerMainCleaning>().Create(new {
                Opening1 = sewerOpening1, Opening2 = sewerOpening2, OperatingCenter = opCntr1
            });
            var sewerMainCleaning2 = GetEntityFactory<SewerMainCleaning>().Create(new {
                Opening1 = sewerOpening3, Opening2 = sewerOpening4, OperatingCenter = opCntr2
            });
            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<SewerMainCleaningRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(sewerMainCleaning1));
            Assert.IsFalse(result.Contains(sewerMainCleaning2));
        }

        [TestMethod]
        public void TestLinqReturnsAllSewerOpeningsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = ROLE_MODULE });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });
            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);
            var sewerOpening1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening3 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            var sewerOpening4 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            var sewerMainCleaning1 = GetEntityFactory<SewerMainCleaning>().Create(new {
                Opening1 = sewerOpening1,
                Opening2 = sewerOpening2,
                OperatingCenter = opCntr1
            });
            var sewerMainCleaning2 = GetEntityFactory<SewerMainCleaning>().Create(new {
                Opening1 = sewerOpening3,
                Opening2 = sewerOpening4,
                OperatingCenter = opCntr2
            });
            Repository = _container.GetInstance<SewerMainCleaningRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(sewerMainCleaning1));
            Assert.IsTrue(result.Contains(sewerMainCleaning2));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnValvesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = ROLE_MODULE });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opCntr1 });
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });
            Session.Save(user);
            var sewerOpening1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening3 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            var sewerOpening4 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            var sewerMainCleaning1 = GetEntityFactory<SewerMainCleaning>().Create(new {
                Opening1 = sewerOpening1,
                Opening2 = sewerOpening2,
                OperatingCenter = opCntr1
            });
            var sewerMainCleaning2 = GetEntityFactory<SewerMainCleaning>().Create(new {
                Opening1 = sewerOpening3,
                Opening2 = sewerOpening4,
                OperatingCenter = opCntr2
            });
            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<SewerMainCleaningRepository>();
            var model = new EmptySearchSet<SewerMainCleaning>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(sewerMainCleaning1));
            Assert.IsFalse(result.Contains(sewerMainCleaning2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllSewerOpeningsIfUerHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury" });
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = ROLE_MODULE });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });
            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);
            var sewerOpening1 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening2 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr1 });
            var sewerOpening3 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            var sewerOpening4 = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr2 });
            var sewerMainCleaning1 = GetEntityFactory<SewerMainCleaning>().Create(new {
                Opening1 = sewerOpening1,
                Opening2 = sewerOpening2,
                OperatingCenter = opCntr1
            });
            var sewerMainCleaning2 = GetEntityFactory<SewerMainCleaning>().Create(new {
                Opening1 = sewerOpening3,
                Opening2 = sewerOpening4,
                OperatingCenter = opCntr2
            });
            Repository = _container.GetInstance<SewerMainCleaningRepository>();

            var model = new EmptySearchSet<SewerMainCleaning>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(sewerMainCleaning1));
            Assert.IsTrue(result.Contains(sewerMainCleaning2));
        }

        #region GetSewerMainCleaningsWithSapIssues

        [TestMethod]
        public void TestGetSewerMainCleaningsWithSapIssuesReturnsSewerMainCleaningsWithSapIssues()
        {
            var sewerMainCleaningValid1 = GetFactory<SewerMainCleaningFactory>()
               .Create(new {SAPErrorCode = "RETRY::Something went wrong"});
            var sewerMainCleaningInvalid1 = GetFactory<SewerMainCleaningFactory>().Create(new {SAPErrorCode = ""});
            var sewerMainCleaningInvalid2 = GetFactory<SewerMainCleaningFactory>().Create();

            var result = Repository.GetSewerMainCleaningsWithSapRetryIssues();

            Assert.AreEqual(1, result.Count());
        }

        #endregion

        #endregion
    }
}
