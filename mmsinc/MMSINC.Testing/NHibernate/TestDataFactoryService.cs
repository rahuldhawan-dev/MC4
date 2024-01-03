using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Data;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StructureMap;

namespace MMSINC.Testing.NHibernate
{
    public class TestDataFactoryService : ITestDataFactoryService
    {
        private readonly IContainer _container;
        private readonly Assembly _callingAssembly;
        private readonly IDictionary<Type, TestDataFactory> _cachedFactories;

        public TestDataFactoryService(IContainer container, Assembly callingAssembly)
        {
            _container = container;
            _callingAssembly = callingAssembly;
            _cachedFactories = new Dictionary<Type, TestDataFactory>();
        }

        public TFactory GetFactory<TFactory>()
            where TFactory : TestDataFactory
        {
            return (TFactory)GetFactory(typeof(TFactory));
        }

        public TestDataFactory GetFactory(Type factoryType)
        {
            var factory = _cachedFactories.ContainsKey(factoryType)
                ? _cachedFactories[factoryType]
                : (TestDataFactory)_container.GetInstance(factoryType);
            _cachedFactories[factoryType] = factory;
            return factory;
        }

        /// <summary>
        /// This returns a factory based on the given entity type. If a specific
        /// factory does not exist, then a generic TestDataFactory will be returned.
        ///
        /// NOTE: If tests aren't working out as expected, double check that the
        /// factory being returned from this is is the factory you're looking for.
        /// Tests in different calling assemblies might not be aware of the factories.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TestDataFactory<TEntity> GetEntityFactory<TEntity>()
            where TEntity : class, new()
        {
            return (TestDataFactory<TEntity>)GetEntityFactory(typeof(TEntity));
        }

        public TestDataFactory GetEntityFactory(Type entityType)
        {
            Type genericType, factoryType;

            // sorry for the complexity, but .MakeGenericType for entities that
            // do not inherit from EntityLookup will cause errors.  feel free to
            // clean this up if your brain is caffinated enough to see how; mine
            // is not so caffinated just yet
            if (entityType.IsSubclassOf(typeof(ReadOnlyEntityLookup)))
            {
                genericType = typeof(EntityLookupTestDataFactory<>).MakeGenericType(entityType);
                var plainGenericType = typeof(TestDataFactory<>).MakeGenericType(entityType);
                factoryType = _callingAssembly
                             .GetClassesByCondition(
                                  t => t.IsSubclassOf(genericType) || t.IsSubclassOf(plainGenericType))
                             .FirstOrDefault();
            }
            else
            {
                genericType = typeof(TestDataFactory<>).MakeGenericType(entityType);
                factoryType = _callingAssembly.GetClassesByCondition(t => t.IsSubclassOf(genericType)).FirstOrDefault();
            }

            return GetFactory(factoryType ?? genericType);
        }
    }

    // TODO: Remove this interface. It's functionally useless. We don't need to mock it
    // out for test reasons, and we only have one implementation of it.
    public interface ITestDataFactoryService
    {
        TFactory GetFactory<TFactory>() where TFactory : TestDataFactory;
        TestDataFactory GetFactory(Type factoryType);
        TestDataFactory<TEntity> GetEntityFactory<TEntity>() where TEntity : class, new();
        TestDataFactory GetEntityFactory(Type entityType);
    }
}
