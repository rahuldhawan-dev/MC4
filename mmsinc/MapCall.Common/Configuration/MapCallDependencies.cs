using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Configuration;
using MMSINC.Data.NHibernate;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace MapCall.Common.Configuration
{
    public static class MapCallDependencies
    {
        private static IEnumerable<RepositoryRegistration> _securedRepositories;
        private static IEnumerable<RepositoryRegistration> _repositories;

        #region Constants

        public const string REPOSITORY_NAMESPACE = "MapCall.Common.Model.Repositories";

        #endregion

        #region Private Methods

        private static IEnumerable<Type> GetTypesInNamespace()
        {
            return typeof(MapCallDependencies).Assembly.GetTypes()
                                              .Where(t => t.Namespace != null &&
                                                          t.Namespace.StartsWith(REPOSITORY_NAMESPACE));
        }

        private static IEnumerable<RepositoryRegistration> GetRepositories(Func<Type, bool> filter)
        {
            return GetTypesInNamespace().Where(filter).Select(repoType => new RepositoryRegistration(repoType))
                                        .Where(registration => registration.InterfaceType != null);
        }

        private static IEnumerable<RepositoryRegistration> GetSecuredRepositories()
        {
            return _securedRepositories ?? (_securedRepositories = GetRepositories(IsSecuredRepositoryType));
        }

        private static IEnumerable<RepositoryRegistration> GetRepositories()
        {
            return _repositories ?? (_repositories = GetRepositories(IsRepositoryType));
        }

        private static Type FindEntityType(Type repoType)
        {
            var baseType = repoType;
            while (null != (baseType = baseType.BaseType))
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(RepositoryBase<>))
                {
                    return baseType.GetGenericArguments()[0];
                }
            }

            throw new InvalidOperationException($"Could not find TEntity for repository type '{repoType}'.");
        }

        private static Type FindInterfaceType(Type repoType)
        {
            var interfaces = repoType.GetInterfaces().Where(iType => IsRepositoryInterfaceType(repoType, iType))
                                     .ToArray();

            if (interfaces.Length > 1)
            {
                throw new InvalidOperationException(
                    $"Found more than one matching interface for repository type '{repoType}'.");
            }

            return interfaces.SingleOrDefault();
        }

        private static bool IsRepositoryInterfaceType(Type repoType, Type iType)
        {
            return !iType.IsGenericType &&
                   repoType.GetInterfaces().Contains(iType) &&
                   iType.ImplementsRawGeneric(typeof(IRepository<>)) &&
                   repoType.Name == iType.Name.Substring(1);
        }

        private static bool IsSecuredRepositoryType(Type type)
        {
            return !type.IsInterface && !type.IsAbstract &&
                   type.IsSubclassOfRawGeneric(typeof(SecuredRepositoryBase<,>));
        }

        private static bool IsRepositoryType(Type type)
        {
            return !type.IsInterface && !type.IsAbstract &&
                   !type.IsSubclassOfRawGeneric(typeof(SecuredRepositoryBase<,>)) &&
                   type.IsSubclassOfRawGeneric(typeof(RepositoryBase<>));
        }

        #endregion

        #region Exposed Methods

        public static void RegisterRepositories(ConfigurationExpression i)
        {
            RegisterRepositories(
                (iface, repo, entity) => i.RegisterRepository(iface, repo, entity),
                (iface, repo, entity) => i.RegisterSecuredRepository(iface, repo, entity),
                (iface, concrete) => i.For(iface).Use(concrete));
        }

        public static void RegisterRepositories(IRegistry i)
        {
            RegisterRepositories(
                (iface, repo, entity) => i.RegisterRepository(iface, repo, entity),
                (iface, repo, entity) => i.RegisterSecuredRepository(iface, repo, entity),
                (iface, concrete) => i.For(iface).Use(concrete));
        }

        public static void RegisterRepositories(Action<Type, Type, Type> registerFn,
            Action<Type, Type, Type> registerSecureFn, Action<Type, Type> registerSimpleFn)
        {
            foreach (var repo in GetRepositories())
            {
                var repoInterfaceTypeName = repo.InterfaceType.Name;
                registerFn(repo.InterfaceType, repo.RepositoryType, repo.EntityType);
            }

            foreach (var repo in GetSecuredRepositories())
            {
                var typeName = repo.EntityType.Name;
                registerSecureFn(repo.InterfaceType, repo.RepositoryType, repo.EntityType);
            }

            registerSimpleFn(typeof(IRepository<>), typeof(RepositoryBase<>));
        }

        #endregion

        #region Nested Type: RepositoryRegistration

        private class RepositoryRegistration
        {
            #region Properties

            public Type RepositoryType { get; }
            public Type InterfaceType { get; }
            public Type EntityType { get; }

            #endregion

            #region Constructors

            public RepositoryRegistration(Type repoType)
            {
                RepositoryType = repoType;
                InterfaceType = FindInterfaceType(repoType);
                EntityType = FindEntityType(repoType);
            }

            #endregion
        }

        #endregion
    }
}
