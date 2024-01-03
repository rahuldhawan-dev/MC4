using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class LocalRepositoryTest : MapCallMvcSecuredRepositoryTestBase<Local, LocalRepository, User>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetEntityFactory<User>().Create(new {IsAdmin = true});
        }

        #endregion

        [TestMethod]
        public void TestGetByUnionIdReturnsByUnionId()
        {
            var unions = GetFactory<UnionFactory>().CreateList(3);
            var locals = GetFactory<LocalFactory>().CreateList(3, new {Union = unions[0]});
            var bennys = GetFactory<LocalFactory>().CreateList(30, new {Union = unions[1]});

            var result = Repository.GetByUnionId(unions[0].Id);

            Assert.AreEqual(locals.Count, result.Count());
            Assert.IsTrue(result.Contains(locals[0]));
            Assert.IsTrue(result.Contains(locals[1]));
            Assert.IsTrue(result.Contains(locals[2]));
            Assert.IsFalse(result.Contains(bennys[2]));
        }

        // nj4 user can't access nj7 
        [TestMethod]
        public void TestLinqDoesNotReturnLocalsFromOtherOperatingCentersForUser()
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

            var validLocal = GetFactory<LocalFactory>().Create(new {OperatingCenter = opCntr1});
            var invalidLocal = GetFactory<LocalFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<LocalRepository>();

            var result = Repository.GetAll();

            Assert.IsTrue(result.Contains(validLocal));
            Assert.IsFalse(result.Contains(invalidLocal));
        }
    }
}
