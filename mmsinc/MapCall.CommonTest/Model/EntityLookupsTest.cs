using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model
{
    [TestClass]
    public class EntityLookupsTest : InMemoryDatabaseTest<AsBuiltImage>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(OperatingCenterFactory).Assembly);
        }

        #endregion

        protected dynamic GetCorrectEntityFactory(Type type, MethodInfo getEntityFactory)
        {
            var factory = getEntityFactory.MakeGenericMethod(type).Invoke(this, null);
            var factoryType = factory.GetType();

            if (MMSINC.ClassExtensions.ReflectionExtensions.TypeExtensions.IsSubclassOfRawGeneric(factoryType,
                typeof(StaticListEntityLookupFactory<,>)))
            {
                factoryType = factoryType.Assembly.GetTypes().First(t => t.IsSubclassOf(factoryType) && !t.IsAbstract);
                return _container.With(Session).GetInstance(factoryType);
            }

            return factory;
        }

        [TestMethod]
        public void TestAllEntityLookupClassesCanBeSelectedCleanly()
        {
            var types = typeof(AsBuiltImage).Assembly.GetTypes()
                                            .Where(t =>
                                                 t.Namespace != null &&
                                                 t.Namespace.StartsWith("MapCall.Common.Model.Entities") && t.IsClass &&
                                                 !t.IsAbstract && t.Implements<IEntityLookup>())
                                            .OrderBy(t => t.Name);
            var getEntityFactory =
                GetType().GetMethods().Single(m => m.Name == "GetEntityFactory" && m.IsGenericMethod);

            var failedToConstruct = new Dictionary<string, Exception>();
            var failedToSelect = new Dictionary<string, Exception>();

            foreach (var type in types)
            {
                var entityFactory = GetCorrectEntityFactory(type, getEntityFactory);

                try
                {
                    if (typeof(ISAPLookup).IsAssignableFrom(type))
                    {
                        entityFactory.Create(new {SAPCode = "foo"});
                    }
                    else
                    {
                        entityFactory.Create();
                    }
                }
                catch (Exception e)
                {
                    failedToConstruct.Add(type.Name, e);
                    continue;
                }

                dynamic repo = _container.GetInstance(typeof(RepositoryBase<>).MakeGenericType(type));

                try
                {
                    var result = IQueryableExtensions.SelectDynamic(repo.Query(), "Id", "Description").Result;

                    Enumerable.First(result);
                }
                catch (Exception e)
                {
                    failedToSelect.Add(type.Name, e);
                }
            }

            if (failedToConstruct.Any())
            {
                Assert.Fail(
                    $@"{failedToConstruct.Count} types failed to construct from factories:{Environment.NewLine}{Describe(failedToConstruct)}");
            }

            if (failedToSelect.Any())
            {
                Assert.Fail(
                    $@"{failedToSelect.Count} types failed to select as Id/Description:{Environment.NewLine}{Describe(failedToSelect)}");
            }
        }

        private string Describe(Dictionary<string, Exception> issues)
        {
            var sb = new StringBuilder();

            foreach (var issue in issues)
            {
                sb.AppendLine($"  {issue.Key}: {issue.Value.Message}");
            }

            return sb.ToString();
        }
    }
}
