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

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class RecurringProjectRepositoryTest : MapCallMvcSecuredRepositoryTestBase<RecurringProject,
        RecurringProjectRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.FieldServicesProjects;

        #endregion

        #region Private Members

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        private Mock<IDateTimeProvider> _dateProvider;

        #endregion

        #region Tests

        #region Linq

        [TestMethod]
        public void TestLinqDoesNotReturnRecurringProjectsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
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

            var rp1 = GetFactory<RecurringProjectFactory>().Create(new {OperatingCenter = opCntr1});
            var rp2 = GetFactory<RecurringProjectFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<RecurringProjectRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(rp1));
            Assert.IsFalse(result.Contains(rp2));
        }

        [TestMethod]
        public void TestLinqReturnsAllRecurringProjectsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
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

            var rp1 = GetFactory<RecurringProjectFactory>().Create(new {OperatingCenter = opCntr1});
            var rp2 = GetFactory<RecurringProjectFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<RecurringProjectRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(rp1));
            Assert.IsTrue(result.Contains(rp2));
        }

        #endregion

        #region Criteria

        [TestMethod]
        public void TestCriteriaDoesNotReturnRecurringProjectsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
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

            var rp1 = GetFactory<RecurringProjectFactory>().Create(new {OperatingCenter = opCntr1});
            var rp2 = GetFactory<RecurringProjectFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<RecurringProjectRepository>();
            var model = new EmptySearchSet<RecurringProject>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(rp1));
            Assert.IsFalse(result.Contains(rp2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllRecurringProjectsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
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

            var rp1 = GetFactory<RecurringProjectFactory>().Create(new {OperatingCenter = opCntr1});
            var rp2 = GetFactory<RecurringProjectFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<RecurringProjectRepository>();
            var model = new EmptySearchSet<RecurringProject>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(rp1));
            Assert.IsTrue(result.Contains(rp2));
        }

        [TestMethod]
        public void TestCriteriaReturnsRecurringProjectsThatHaveNullCoordinatesDueToLeftJoin()
        {
            var rpWithCoord = GetFactory<RecurringProjectFactory>()
               .Create(new {Coordinate = typeof(CoordinateFactory)});
            var rpWithoutCoord = GetFactory<RecurringProjectFactory>().Create();
            rpWithoutCoord.Coordinate = null;
            Assert.IsNotNull(rpWithCoord.Coordinate, "Sanity check");
            Assert.IsNull(rpWithoutCoord.Coordinate, "Sanity check");

            var model = new EmptySearchSet<RecurringProject>();
            var result = Repository.Search(model);
            Assert.IsTrue(result.Contains(rpWithCoord));
            Assert.IsTrue(result.Contains(rpWithoutCoord));
        }

        #endregion

        #endregion
    }
}
