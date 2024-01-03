using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class BelowGroundHazardRepositoryTest : MapCallMvcSecuredRepositoryTestBase<BelowGroundHazard,
        BelowGroundHazardRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesAssets;

        #endregion

        #region Private Members

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new { IsAdmin = true });
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        #endregion

        #region Tests

        #region Linq

        [TestMethod]
        public void TestLinqDoesNotReturnBelowGroundHazardsFromOtherOperatingCentersForUser()
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

            var bg1 = GetFactory<BelowGroundHazardFactory>().Create(new { OperatingCenter = opCntr1 });
            var bg2 = GetFactory<BelowGroundHazardFactory>().Create(new { OperatingCenter = opCntr2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<BelowGroundHazardRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(bg1));
            Assert.IsFalse(result.Contains(bg2));
        }

        [TestMethod]
        public void TestLinqReturnsAllBelowGroundHazardsIfUserHasMatchingRoleWithWildcardOperatingCenter()
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

            var bg1 = GetFactory<BelowGroundHazardFactory>().Create(new { OperatingCenter = opCntr1 });
            var bg2 = GetFactory<BelowGroundHazardFactory>().Create(new { OperatingCenter = opCntr2 });

            Repository = _container.GetInstance<BelowGroundHazardRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(bg1));
            Assert.IsTrue(result.Contains(bg2));
        }

        #endregion

        #region Criteria

        [TestMethod]
        public void TestCriteriaDoesNotReturnBelowGroundHazardsFromOtherOperatingCentersForUser()
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

            var bg1 = GetFactory<BelowGroundHazardFactory>().Create(new { OperatingCenter = opCntr1 });
            var bg2 = GetFactory<BelowGroundHazardFactory>().Create(new { OperatingCenter = opCntr2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<BelowGroundHazardRepository>();
            var model = new EmptySearchSet<BelowGroundHazard>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(bg1));
            Assert.IsFalse(result.Contains(bg2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllBelowGroundHazardsIfUserHasMatchingRoleWithWildcardOperatingCenter()
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

            var bg1 = GetFactory<BelowGroundHazardFactory>().Create(new { OperatingCenter = opCntr1 });
            var bg2 = GetFactory<BelowGroundHazardFactory>().Create(new { OperatingCenter = opCntr2 });

            Repository = _container.GetInstance<BelowGroundHazardRepository>();
            var model = new EmptySearchSet<BelowGroundHazard>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(bg1));
            Assert.IsTrue(result.Contains(bg2));
        }

        [TestMethod]
        public void TestCriteriaReturnsBelowGroundHazardsThatHaveNullCoordinatesDueToLeftJoin()
        {
            var bgWithCoord = GetFactory<BelowGroundHazardFactory>()
               .Create(new { Coordinate = typeof(CoordinateFactory) });
            var bgWithoutCoord = GetFactory<BelowGroundHazardFactory>().Create();
            bgWithoutCoord.Coordinate = null;
            Assert.IsNotNull(bgWithCoord.Coordinate, "Sanity check");
            Assert.IsNull(bgWithoutCoord.Coordinate, "Sanity check");

            var model = new EmptySearchSet<BelowGroundHazard>();
            var result = Repository.Search(model);
            Assert.IsTrue(result.Contains(bgWithCoord));
            Assert.IsTrue(result.Contains(bgWithoutCoord));
        }

        #endregion

        #endregion
    }
}
