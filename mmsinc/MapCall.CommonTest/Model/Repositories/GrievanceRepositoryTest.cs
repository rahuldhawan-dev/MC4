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
    public class GrievanceRepositoryTest : MapCallMvcSecuredRepositoryTestBase<Grievance, GrievanceRepository, User>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetEntityFactory<User>().Create(new {IsAdmin = true});
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        #endregion

        #region GetAll

        [TestMethod]
        public void TestGetAllOnlyReturnsGrievancesForOperatingCentersUserRoleBelongsToIfUserIsNotAdmin()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.HumanResources});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.HumanResourcesUnion});
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

            var valid = GetFactory<GrievanceFactory>().Create(new {OperatingCenter = opCntr1});
            var invalid = GetFactory<GrievanceFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<GrievanceRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestGetAllReturnsAllGrievancesIfUserIsAdmin()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = true, DefaultOperatingCenter = opCntr1});
            var valid = GetFactory<GrievanceFactory>().Create(new {OperatingCenter = opCntr1});
            var alsoValid = GetFactory<GrievanceFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<GrievanceRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsTrue(result.Contains(alsoValid));
        }

        #endregion

        #region Search

        [TestMethod]
        public void TestSearchOnlyReturnsGrievancesForOperatingCentersUserRoleBelongsToIfUserIsNotAdmin()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.HumanResources});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.HumanResourcesUnion});
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

            var valid = GetFactory<GrievanceFactory>().Create(new {OperatingCenter = opCntr1});
            var invalid = GetFactory<GrievanceFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<GrievanceRepository>();

            //var searchModel = new TestSearchModel();
            // var mapper = _container.With(searchModel).GetInstance<ViewModelToSearchMapper<TestSearchModel, Grievance>>();
            var result = Repository.Search(new EmptySearchSet<Grievance>());

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestSearchReturnsAllGrievancesIfUserIsAdmin()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = true, DefaultOperatingCenter = opCntr1});
            var valid = GetFactory<GrievanceFactory>().Create(new {OperatingCenter = opCntr1});
            var alsoValid = GetFactory<GrievanceFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<GrievanceRepository>();

            //  var searchModel = new TestSearchModel();
            // var mapper = _container.With(searchModel).GetInstance<ViewModelToSearchMapper<TestSearchModel, Grievance>>();
            var result = Repository.Search(new EmptySearchSet<Grievance>());

            Assert.IsTrue(result.Contains(valid));
            Assert.IsTrue(result.Contains(alsoValid));
        }

        #endregion

    }
}
