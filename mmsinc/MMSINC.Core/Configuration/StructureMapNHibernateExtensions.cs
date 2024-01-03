using System;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace MMSINC.Configuration
{
    public static class StructureMapNHibernateExtensions
    {
        #region Exposed Methods

        public static ConfigurationExpression RegisterSecuredRepository<TInterface, TRepository, TEntity, TUser>(
            this ConfigurationExpression i)
            where TInterface : IRepository<TEntity>
            where TRepository : SecuredRepositoryBase<TEntity, TUser>
            where TEntity : class
            where TUser : IAdministratedUser
        {
            return i.RegisterSecuredRepository(typeof(TInterface), typeof(TRepository), typeof(TEntity));
        }

        /// <summary>
        /// When using this method, the repository will be instantiated using a registration for
        /// typeof(YourUserType) which should represent the current user.  If unsure, you probably
        /// want to register i.For<YourUserType>().Use(ctx => ctx.GetInstance<IAuthenticationService<YourUserType>>().CurrentUser);
        /// </summary>
        public static ConfigurationExpression RegisterSecuredRepository(this ConfigurationExpression i, Type tInterface,
            Type tRepository, Type tEntity)
        {
            return i.RegisterRepository(tInterface, tRepository, tEntity);
        }

        /// <summary>
        /// For instances where the repository needs a specific interface/type.
        /// </summary>
        public static ConfigurationExpression RegisterRepository<TInterface, TRepository, TEntity>(
            this ConfigurationExpression i)
            where TInterface : IRepository<TEntity>
            where TRepository : RepositoryBase<TEntity>
            where TEntity : class
        {
            return i.RegisterRepository(typeof(TInterface), typeof(TRepository), typeof(TEntity));
        }

        public static ConfigurationExpression RegisterRepository(this ConfigurationExpression i, Type tInterface,
            Type tRepository, Type tEntity)
        {
            i.For(tInterface)
             .Use(tRepository);
            i.For(typeof(IRepository<>).MakeGenericType(tEntity))
             .Use(tRepository);

            return i;
        }

        public static IRegistry RegisterRepository(this IRegistry i, Type tInterface, Type tRepository, Type tEntity)
        {
            i.For(tInterface)
             .Use(tRepository);
            i.For(typeof(IRepository<>).MakeGenericType(tEntity))
             .Use(tRepository);

            return i;
        }

        public static IRegistry RegisterSecuredRepository(this IRegistry i, Type tInterface, Type tRepository,
            Type tEntity)
        {
            return i.RegisterRepository(tInterface, tRepository, tEntity);
        }

        #endregion
    }
}
