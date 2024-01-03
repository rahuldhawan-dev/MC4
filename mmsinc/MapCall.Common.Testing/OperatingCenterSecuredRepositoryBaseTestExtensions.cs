using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCall.Common.Testing
{
    public static class OperatingCenterSecuredRepositoryBaseTestExtensions
    {
        private static (User userWithRoleInOperatingCenter, User userWithRoleInOtherOperatingCenter, User
            userWithWildcardRole, User userWithNoRole, User adminUser, TEntity entityInThisOperatingCenter, TEntity
            entityInOtherOperatingCenter, TEntity entityInNeitherOperatingCenter)
            SetupData<TEntity>(IContainer container, RoleModules role)
            where TEntity : class, new()
        {
            var factoryService = container.GetInstance<ITestDataFactoryService>();

            var thisOperatingCenter = factoryService.GetFactory<UniqueOperatingCenterFactory>().Create();
            var otherOperatingCenter = factoryService.GetFactory<UniqueOperatingCenterFactory>().Create();
            var neitherOperatingCenter = factoryService.GetFactory<UniqueOperatingCenterFactory>().Create();

            var userWithRoleInOperatingCenter = factoryService
                                               .GetEntityFactory<User>()
                                               .Create(new {UserName = "userWithRoleInOperatingCenter"});
            factoryService.GetFactory<RoleFactory>().Create(role, thisOperatingCenter, userWithRoleInOperatingCenter);
            var userWithRoleInOtherOperatingCenter = factoryService
                                                    .GetEntityFactory<User>()
                                                    .Create(new {UserName = "userWithRoleInOtherOperatingCenter"});
            factoryService.GetFactory<RoleFactory>()
                          .Create(role, otherOperatingCenter, userWithRoleInOtherOperatingCenter);
            var userWithWildcardRole =
                factoryService.GetEntityFactory<User>().Create(new {UserName = "userWithWildcardRole"});
            factoryService.GetFactory<WildcardOpCenterRoleFactory>().Create(role, null, userWithWildcardRole);
            var userWithNoRole = factoryService.GetEntityFactory<User>().Create();
            var adminUser = factoryService.GetFactory<Common.Testing.Data.AdminUserFactory>().Create();

            var entityInThisOperatingCenter = factoryService
                                             .GetEntityFactory<TEntity>()
                                             .Create(new {OperatingCenter = thisOperatingCenter});
            var entityInOtherOperatingCenter = factoryService
                                              .GetEntityFactory<TEntity>()
                                              .Create(new {OperatingCenter = otherOperatingCenter});
            var entityInNeitherOperatingCenter = factoryService
                                                .GetEntityFactory<TEntity>()
                                                .Create(new {OperatingCenter = neitherOperatingCenter});

            return (userWithRoleInOperatingCenter, userWithRoleInOtherOperatingCenter, userWithWildcardRole,
                userWithNoRole, adminUser, entityInThisOperatingCenter, entityInOtherOperatingCenter,
                entityInNeitherOperatingCenter);
        }

        private static void TestUserCanSeeExpectedEntities<TEntity, TRepository>(IContainer container, User user,
            params TEntity[] expectedEntities)
            where TEntity : class, IThingWithOperatingCenter, new()
            where TRepository : SecuredRepositoryBase<TEntity, User>
        {
            container.Inject<IAuthenticationService<User>>(new MockAuthenticationService(user));
            // NOTE: we need to get a new one of these each time because the cache role lookups
            var repository = container.GetInstance<TRepository>();

            var results = repository.Linq.ToList();

            if (expectedEntities.Any())
            {
                MyAssert.CollectionsAreSimilar(expectedEntities, results);
            }
            else
            {
                MyAssert.IsEmpty(results);
            }
        }

        public static void TestLinqPropertyAppliesRoleFilters<TEntity, TRepository>(
            this MapCallMvcSecuredRepositoryTestBase<TEntity, TRepository, User> that, RoleModules role)
            where TEntity : class, IThingWithOperatingCenter, new()
            where TRepository : SecuredRepositoryBase<TEntity, User>
        {
            var (userWithRoleInOperatingCenter, userWithRoleInOtherOperatingCenter, userWithWildcardRole,
                    userWithNoRole, adminUser, entityInThisOperatingCenter, entityInOtherOperatingCenter,
                    entityInNeitherOperatingCenter)
                = SetupData<TEntity>(that.Container, role);

            // userWithRoleInOperatingCenter

            TestUserCanSeeExpectedEntities<TEntity, TRepository>(that.Container, userWithRoleInOperatingCenter,
                entityInThisOperatingCenter);

            // userWithRoleInOtherOperatingCenter

            TestUserCanSeeExpectedEntities<TEntity, TRepository>(that.Container, userWithRoleInOtherOperatingCenter,
                entityInOtherOperatingCenter);

            // userWithWildCardRole

            TestUserCanSeeExpectedEntities<TEntity, TRepository>(that.Container, userWithWildcardRole,
                entityInThisOperatingCenter, entityInOtherOperatingCenter, entityInNeitherOperatingCenter);

            // userWithNoRole

            TestUserCanSeeExpectedEntities<TEntity, TRepository>(that.Container, userWithNoRole);

            // admin user

            TestUserCanSeeExpectedEntities<TEntity, TRepository>(that.Container, adminUser,
                entityInThisOperatingCenter, entityInOtherOperatingCenter, entityInNeitherOperatingCenter);
        }
    }
}
